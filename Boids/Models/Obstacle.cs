using Newtonsoft.Json;

namespace Boids.Models
{
    public class Obstacle
    {
        [JsonProperty("position")]
        public Position Position { get; set; }
    }
}