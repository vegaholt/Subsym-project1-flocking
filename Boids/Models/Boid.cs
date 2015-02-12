using Newtonsoft.Json;

namespace Boids.Models
{
    public class Boid
    {
        [JsonProperty("position")]
        public Position Position { get; set; }

        [JsonProperty("velocity")]
        public Velocity Velocity { get; set; }
    }
}