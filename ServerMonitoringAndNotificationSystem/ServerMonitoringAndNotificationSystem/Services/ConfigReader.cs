using Microsoft.Extensions.Configuration;
namespace Task1_ServerStatistics.Services
{
    public class ConfigReader(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public string GetServerIdentifier()
            => _configuration["ServerStatisticsConfig:ServerIdentifier"];

        public int GetSamplingInterval()
            => int.Parse(_configuration["ServerStatisticsConfig:SamplingIntervalSeconds"]);

        public string GetRabbitMQHostName() => _configuration["RabbitMQConfig:HostName"];

        public string GetExchange() => _configuration["RabbitMQConfig:Exchange"];

        public string GetTopic() => _configuration["RabbitMQConfig:Topic"];

    }
}
