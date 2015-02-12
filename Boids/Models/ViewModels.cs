using System.Collections.Generic;
using Newtonsoft.Json;

namespace Boids.Models
{
    public class BoidViewModel
    {
        [JsonProperty("boids")]
        public List<Boid> Boids { get; set; }

        [JsonProperty("predators")]
        public List<Predator> Predators { get; set; }

        [JsonProperty("obstacles")]
        public List<Obstacle> Obstacles { get; set; }
    }

    public class SettingsViewModel
    {
        [JsonProperty("settings")]
        public Settings Settings { get; set; }
    }
}