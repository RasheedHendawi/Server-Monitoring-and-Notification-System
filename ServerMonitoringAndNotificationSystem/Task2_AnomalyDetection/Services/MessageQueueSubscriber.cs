using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using Shared.Models;

namespace Task2_AnomalyDetection.Services
{
    public class MessageQueueSubscriber : IDisposable
    {
        private readonly string _hostname;
        private readonly string _exchange;
        private IConnection _connection;
        private IModel _channel;

        public MessageQueueSubscriber(string hostname, string exchange)
        {
            _hostname = hostname;
            _exchange = exchange;
        }

        public void Subscribe(string topic, Func<ServerStatistics, Task> onMessageReceived)
        {

            if (_connection == null || !_connection.IsOpen)
            {
                var factory = new ConnectionFactory() { HostName = _hostname };
                _connection = factory.CreateConnection();
            }

            if (_channel == null || !_channel.IsOpen)
            {
                _channel = _connection.CreateModel();
            }

            _channel.ExchangeDeclare(exchange: _exchange, type: "topic");
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName, exchange: _exchange, routingKey: topic);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var stats = JsonConvert.DeserializeObject<ServerStatistics>(message);

                    if (stats != null)
                    {
                        //Console.WriteLine($"Message received for topic {topic}: {message}");
                        await onMessageReceived(stats);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                }
            };

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            Console.WriteLine($"Subscribed to topic: {topic}");
        }

        public void Dispose()
        {

            if (_channel != null && _channel.IsOpen)
            {
                _channel.Close();
                _channel.Dispose();
            }

            if (_connection != null && _connection.IsOpen)
            {
                _connection.Close();
                _connection.Dispose();
            }

            Console.WriteLine("RabbitMQ connection and channel disposed.");
        }
    }
}
