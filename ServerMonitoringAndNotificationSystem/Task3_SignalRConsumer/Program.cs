using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
class Program
{
    static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        string signalRUrl = config["SignalRConfig:SignalRUrl"];

        if (string.IsNullOrEmpty(signalRUrl))
        {
            Console.WriteLine("SignalR URL is not configured. Please check appsettings.json.");
            return;
        }

        var connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/signalrhub")
            .Build();

        connection.On<string>("ReceiveAlert", (message) =>
        {
            Console.WriteLine($"Received Alert: {message}");
        });

        try
        {
            Console.WriteLine("Connecting to SignalR hub...");
            await connection.StartAsync();
            Console.WriteLine("Connected to SignalR hub. Listening for events...");

            Console.ReadLine();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to SignalR hub: {ex.Message}");
        }
    }
}
