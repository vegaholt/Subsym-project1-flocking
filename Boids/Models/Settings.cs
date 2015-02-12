using Newtonsoft.Json;

namespace Boids.Models
{
    public class Settings
    {
        [JsonProperty("numberOfBoids")]
        public int NumberOfBoids { get; set; }

        [JsonProperty("numberOfPredators")]
        public int NumberOfPredators { get; set; }

        [JsonProperty("numberOfObstacles")]
        public int NumberOfObstacles { get; set; }

        [JsonProperty("boidRadius")]
        public int BoidRadius { get; set; }

        [JsonProperty("predatorRadius")]
        public int PredatorRadius { get; set; }

        [JsonProperty("obstacleRadius")]
        public int ObstacleRadius { get; set; }

        [JsonProperty("separationWeight")]
        public int SeparationWeight { get; set; }

        [JsonProperty("alignmentWeight")]
        public int AlignmentWeight { get; set; }

        [JsonProperty("cohesionWeight")]
        public int CohesionWeight { get; set; }

        [JsonProperty("maxVelocityBoid")]
        public float MaxVelocityBoid { get; set; }

        [JsonProperty("maxVelocityPredator")]
        public float MaxVelocityPredator { get; set; }

        [JsonProperty("neighbourRadiusBoid")]
        public float NeighbourRadiusBoid { get; set; }

        [JsonProperty("collisionRadiusBoid")]
        public float CollisionRadiusBoid { get; set; }

        [JsonProperty("threathDetectionRadius")]
        public float ThreathDetectionRadius { get; set; }
    }
}