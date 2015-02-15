using Newtonsoft.Json;

namespace Boids.Models
{
    public class Boid
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("position")]
        public Position Position { get; set; }

        [JsonProperty("velocity")]
        public Velocity Velocity { get; set; }
    }
}