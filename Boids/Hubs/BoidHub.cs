using System.Threading.Tasks;
using Boids.Managers;
using Boids.Models;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace Boids.Hubs
{
    public class BoidHub : Hub
    {
        public void UpdateModel(ShapeModel clientModel)
        {
            clientModel.LastUpdatedBy = Context.ConnectionId;
            // Update the shape model within our broadcaster
            Clients.AllExcept(clientModel.LastUpdatedBy).updateShape(clientModel);
        }

        public void UpdateWorld(FlockModel clientModel)
        {

        }

        public Task OnStartConnect(Settings model)
        {
            InitializerManager.GetInstance().StartNewRound(model);
            return null;
        }

        public Task GiveMeBoid()
        {
            InitializerManager.GetInstance().AddNewBoid();
            return null;
        }
        public Task GetBoidList()
        {
            InitializerManager.GetInstance().GetBoidList();
            return null;
        }


        public Task Stop()
        {
            return null;
        }

        public Task UpdateSettings()
        {
            return null;
        }
       
        
    }

    public class ShapeModel
    {
        // We declare Left and Top as lowercase with 
        // JsonProperty to sync the client and server models
        [JsonProperty("left")]
        public double Left { get; set; }
        [JsonProperty("top")]
        public double Top { get; set; }
        // We don't want the client to get the "LastUpdatedBy" property
        [JsonIgnore]
        public string LastUpdatedBy { get; set; }
    }

    public class FlockModel
    {

    }
}