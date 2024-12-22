using Microsoft.AspNetCore.SignalR.Client;

namespace Task2_AnomalyDetection.Services
{
    public class SignalRPublisher
    {
        private readonly HubConnection _connection;

        public SignalRPublisher(string hubUrl)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();
        }

        public async Task SendAlert(string message)
        {
            await _connection.StartAsync();
            await _connection.InvokeAsync("SendAlert", message);
        }
        /*public async Task SendAlert(string message)
        {
            try
            {
                if (_connection.State != HubConnectionState.Connected)
                {
                    Console.WriteLine("Attempting to connect to SignalR hub...");
                    await _connection.StartAsync();
                    Console.WriteLine("Connected to SignalR hub.");
                }

                await _connection.InvokeAsync("SendAlert", message);
                Console.WriteLine($"Alert sent: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending alert: {ex.Message}");
                throw;
            }
        }*/
    }
}
