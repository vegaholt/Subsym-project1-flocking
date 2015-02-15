using Newtonsoft.Json;

namespace Boids.Models
{
    public class Position
    {
        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }
    }
}