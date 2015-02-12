using Newtonsoft.Json;

namespace Boids.Models
{
    public class Velocity : Position
    {
        [JsonProperty("deg")]
        public float Deg { get; set; }
    }
}