using Newtonsoft.Json;

namespace Boids.Models
{
    public class Settings
    {
        [JsonProperty("numberOfBoids")]
        public int NumberOfBoids { get; set; }

        [JsonProperty("numberOfObstacles")]
        public int NumberOfObstacles { get; set; }

        [JsonProperty("numberOfPredators")]
        public int NumberOfPredators { get; set; }

        [JsonProperty("boidRadius")]
        public int BoidRadius { get; set; }

        [JsonProperty("obstacleRadius")]
        public int ObstacleRadius { get; set; }

        [JsonProperty("predatorRadius")]
        public int PredatorRadius { get; set; }

        [JsonProperty("separationWeight")]
        public double SeparationWeight { get; set; }

        [JsonProperty("alignmentWeight")]
        public double AlignmentWeight { get; set; }

        [JsonProperty("cohesionWeight")]
        public double CohesionWeight { get; set; }

        [JsonProperty("maxVelocityBoid")]
        public double MaxVelocityBoid { get; set; }

        [JsonProperty("maxVelocityPredator")]
        public double MaxVelocityPredator { get; set; }

        [JsonProperty("neighbourRadiusBoid")]
        public double NeighbourRadiusBoid { get; set; }

        [JsonProperty("collisionRadiusBoid")]
        public double CollisionRadiusBoid { get; set; }

        [JsonProperty("threathDetectionRadius")]
        public double ThreathDetectionRadius { get; set; }

        [JsonProperty("interval")]
        public double Interval { get; set; }
    }
}