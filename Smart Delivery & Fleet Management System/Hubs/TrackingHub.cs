using Microsoft.AspNetCore.SignalR;
namespace Smart_Delivery___Fleet_Management_System.Hubs
{
    public class TrackingHub : Hub
    {
        public async Task SendLocation(int driverId, double lat, double lng)
        {
            await Clients.All.SendAsync("ReceiveLocation", driverId, lat, lng);
        }
    }
}
