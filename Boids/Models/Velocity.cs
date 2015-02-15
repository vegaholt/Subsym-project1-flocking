using Newtonsoft.Json;

namespace Boids.Models
{
    public class Velocity : Position
    {
        [JsonProperty("deg")]
        public double Deg { get; set; }
    }
}