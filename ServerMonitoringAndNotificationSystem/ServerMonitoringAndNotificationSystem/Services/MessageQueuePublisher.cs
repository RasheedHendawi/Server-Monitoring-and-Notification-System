using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Shared.Models;

namespace Task1_ServerStatistics.Services
{
    public class MessageQueuePublisher
    {
        private readonly string _hostname;
        private readonly string _exchange;

        public MessageQueuePublisher(string hostname, string exchange)
        {
            _hostname = hostname;
            _exchange = exchange;
        }

        public void Publish(ServerStatistics stats, string topic)
        {
            var factory = new ConnectionFactory() { HostName = _hostname };

            using var connection =  factory.CreateConnection();
            using var channel =  connection.CreateModel();

            channel.ExchangeDeclare(_exchange, "topic");

            var message = JsonConvert.SerializeObject(stats);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: _exchange, routingKey: topic, body: body);
            Console.WriteLine($" [x] Sent: {message}");
        }
    }
}
