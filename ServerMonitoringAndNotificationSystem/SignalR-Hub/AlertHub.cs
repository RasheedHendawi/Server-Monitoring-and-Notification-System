using Microsoft.AspNetCore.SignalR;

namespace SignalR_Hub
{
    public class AlertHub : Hub
    {
        public async Task SendAlert(string message)
        {
            await Clients.All.SendAsync("ReceiveAlert", message);
        }
    }
}
