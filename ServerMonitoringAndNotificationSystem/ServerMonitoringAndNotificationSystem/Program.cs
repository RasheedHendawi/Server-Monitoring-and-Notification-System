using Microsoft.Extensions.Configuration;
using Task1_ServerStatistics.Services;

class Program
{
    static void Main(string[] args)
    {
        var configObj = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

        var configReader = new ConfigReader(configObj);
        var samplingInterval = configReader.GetSamplingInterval();
        var serverIdentifier = configReader.GetServerIdentifier();
        var rabbitHostName = configReader.GetRabbitMQHostName();
        var exchange = configReader.GetExchange();
        var topic = $"{configReader.GetTopic()}.{serverIdentifier}";

        var collector = new StatisticsCollector();
        var publisher = new MessageQueuePublisher(rabbitHostName, exchange);

        Console.WriteLine("Starting server statistics collection...");

        while (true)
        {
            try
            {
                var stats = collector.Collect(serverIdentifier);
                publisher.Publish(stats, topic);

                Console.WriteLine($"Published: MemoryUsage={stats.MemoryUsage} MB, " +
                                  $"AvailableMemory={stats.AvailableMemory} MB, " +
                                  $"CPU Usage={stats.CpuUsage}%, Timestamp={stats.Timestamp}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

             Task.Delay(samplingInterval);
        }
    }
}
