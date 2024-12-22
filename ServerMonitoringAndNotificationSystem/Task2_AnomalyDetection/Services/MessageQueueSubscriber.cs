using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using Shared.Models;

namespace Task2_AnomalyDetection.Services
{
    public class MessageQueueSubscriber
    {
        private readonly string _hostname;
        private readonly string _exchange;

        public MessageQueueSubscriber(string hostname, string exchange)
        {
            _hostname = hostname;
            _exchange = exchange;
        }

        public void Subscribe(string topic, Func<ServerStatistics, Task> onMessageReceived)
        {
            var factory = new ConnectionFactory() { HostName = _hostname };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: _exchange, type: "topic");
            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName, exchange: _exchange, routingKey: topic);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var stats = JsonConvert.DeserializeObject<ServerStatistics>(message);

                if (stats != null)
                {
                    await onMessageReceived(stats);
                }
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            Console.WriteLine($"Subscribed to topic: {topic}");
            //Console.ReadLine();
        }
    }
}
