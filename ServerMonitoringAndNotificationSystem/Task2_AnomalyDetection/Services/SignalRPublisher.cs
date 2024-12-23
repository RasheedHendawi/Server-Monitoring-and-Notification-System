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

            _connection.Closed += async (error) =>
            {
                Console.WriteLine("SignalR connection closed. Attempting to reconnect...");
                await Task.Delay(2000);
                await StartConnectionAsync();
            };
        }

        public async Task SendAlert(string message)
        {
            try
            {
                if (_connection.State == HubConnectionState.Disconnected)
                {
                    Console.WriteLine("SignalR hub is disconnected. Reconnecting...");
                    await StartConnectionAsync();
                }

                await _connection.InvokeAsync("SendAlert", message);
                Console.WriteLine($"Alert sent: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending alert: {ex.Message}");
            }
        }

        private async Task StartConnectionAsync()
        {
            try
            {
                if (_connection.State == HubConnectionState.Disconnected)
                {
                    await _connection.StartAsync();
                    Console.WriteLine("SignalR connection started.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting SignalR connection: {ex.Message}");
            }
        }
    }
}
