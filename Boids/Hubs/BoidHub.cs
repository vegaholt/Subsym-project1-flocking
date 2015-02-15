using System.Threading.Tasks;
using Boids.Managers;
using Boids.Models;
using Microsoft.AspNet.SignalR;

namespace Boids.Hubs
{
    public class BoidHub : Hub
    {
        #region Initial Methods
        public Task InitializeWorld()
        {
            InitializerManager.GetInstance().SendSettingsToClient();
            InitializerManager.GetInstance().SendBoidListToClient();
            return null;
        }
        #endregion

        #region Runtime Methods
        public Task StartFlocking()
        {
            InitializerManager.GetInstance().StartFlocking();
            return null;
        }
        public Task StopFlocking()
        {
            InitializerManager.GetInstance().StopFlocking();
            return null;
        }
        #endregion

        public Task UpdateSettings(Settings settings)
        {
            InitializerManager.GetInstance().UpdateSettings(settings);
            return null;
        }
    }
}