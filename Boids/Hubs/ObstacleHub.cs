using Microsoft.AspNet.SignalR;

namespace Boids.Hubs
{
    public class ObstacleHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}