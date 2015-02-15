using Newtonsoft.Json;

namespace Boids.Models
{
    public class Obstacle
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("position")]
        public Position Position { get; set; }
    }
}