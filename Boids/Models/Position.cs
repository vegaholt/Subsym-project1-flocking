using Newtonsoft.Json;

namespace Boids.Models
{
    public class Position
    {
        [JsonProperty("x")]
        public float X { get; set; }

        [JsonProperty("y")]
        public float Y { get; set; }
    }
}