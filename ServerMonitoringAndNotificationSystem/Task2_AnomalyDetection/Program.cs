using Task2_AnomalyDetection.Hubs;
using Task2_AnomalyDetection.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSignalR();


builder.Services.AddSingleton<ConfigReader>();


builder.Services.AddSingleton<MongoDBService>(sp =>
{
    var config = sp.GetRequiredService<ConfigReader>();
    return new MongoDBService(
        config.GetMongoConnectionString(),
        config.GetMongoDatabaseName(),
        config.GetMongoCollectionName()
    );
});


builder.Services.AddSingleton<MessageQueueSubscriber>(sp =>
{
    var config = sp.GetRequiredService<ConfigReader>();
    return new MessageQueueSubscriber(
        config.GetRabbitMQHostName(),
        config.GetExchange()
    );
});


builder.Services.AddSingleton<AnomalyDetector>(sp =>
{
    var config = sp.GetRequiredService<ConfigReader>();
    return new AnomalyDetector(
        config.GetMemoryAnomalyThreshold(),
        config.GetCpuAnomalyThreshold(),
        config.GetMemoryUsageThreshold(),
        config.GetCpuUsageThreshold()
    );
});


builder.Services.AddSingleton<SignalRPublisher>(sp =>
{
    var config = sp.GetRequiredService<ConfigReader>();
    return new SignalRPublisher(config.GetSignalRUrl());
});

var app = builder.Build();


app.MapHub<AlertHub>("/signalrhub");


var mongoService = app.Services.GetRequiredService<MongoDBService>();
var subscriber = app.Services.GetRequiredService<MessageQueueSubscriber>();
var anomalyDetector = app.Services.GetRequiredService<AnomalyDetector>();
var signalRPublisher = app.Services.GetRequiredService<SignalRPublisher>();
bool flagTesting = false;
subscriber.Subscribe("ServerStatistics.*", async stats =>
{
    try
    {
        await mongoService.InsertAsync(stats);
        if (!flagTesting)
        {
            await signalRPublisher.SendAlert($"High usage detected: Rasheed.Testing");
            flagTesting = true;
        }
        var recentStats = await mongoService.GetRecentAsync(stats.ServerIdentifier, 2);
        if (recentStats.Count > 1)
        {
            var previous = recentStats[1];

            if (anomalyDetector.IsMemoryAnomaly(stats.MemoryUsage, previous.MemoryUsage) ||
                anomalyDetector.IsCpuAnomaly(stats.CpuUsage, previous.CpuUsage))
            {
                await signalRPublisher.SendAlert($"Anomaly detected: {stats.ServerIdentifier}");
            }

            if (anomalyDetector.IsMemoryHighUsage(stats.MemoryUsage, stats.AvailableMemory) ||
                anomalyDetector.IsCpuHighUsage(stats.CpuUsage))
            {
                await signalRPublisher.SendAlert($"High usage detected: {stats.ServerIdentifier}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in subscriber: {ex.Message}");
    }
});


app.Run();