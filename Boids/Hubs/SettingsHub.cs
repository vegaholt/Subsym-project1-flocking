using Microsoft.AspNet.SignalR;

namespace Boids.Hubs
{
    public class SettingsHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}