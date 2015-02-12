using Microsoft.AspNet.SignalR;

namespace Boids.Hubs
{
    public class PredatorHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}