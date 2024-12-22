
namespace Task2_AnomalyDetection.Services
{
    public class ConfigReader
    {
        private readonly IConfiguration _configuration;

        public ConfigReader()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }


        public string GetMongoConnectionString()
            => _configuration["MongoDBConfig:ConnectionString"];

        public string GetMongoDatabaseName()
            => _configuration["MongoDBConfig:DatabaseName"];

        public string GetMongoCollectionName()
            => _configuration["MongoDBConfig:CollectionName"];


        public string GetRabbitMQHostName()
            => _configuration["RabbitMQConfig:HostName"];

        public string GetExchange()
            => _configuration["RabbitMQConfig:Exchange"];

        public string GetTopic()
            => _configuration["RabbitMQConfig:Topic"];

        public double GetMemoryAnomalyThreshold()
            => double.Parse(_configuration["AnomalyDetectionConfig:MemoryUsageAnomalyThresholdPercentage"]);

        public double GetCpuAnomalyThreshold()
            => double.Parse(_configuration["AnomalyDetectionConfig:CpuUsageAnomalyThresholdPercentage"]);

        public double GetMemoryUsageThreshold()
            => double.Parse(_configuration["AnomalyDetectionConfig:MemoryUsageThresholdPercentage"]);

        public double GetCpuUsageThreshold()
            => double.Parse(_configuration["AnomalyDetectionConfig:CpuUsageThresholdPercentage"]);

        public string GetSignalRUrl()
            => _configuration["SignalRConfig:SignalRUrl"];
    }
}
