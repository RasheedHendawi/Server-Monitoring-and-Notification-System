using Microsoft.AspNetCore.SignalR;

namespace Task2_AnomalyDetection.Hubs
{
    public class AlertHub : Hub
    {
        public async Task SendAlert(string message)
        {
            await Clients.All.SendAsync("ReceiveAlert", message);
            //Console.WriteLine($"Rasheed.{message}");
        }
    }
}
