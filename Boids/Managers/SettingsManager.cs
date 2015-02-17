using Boids.Models;

namespace Boids.Managers
{
    public class SettingsManager
    {
        public Settings GetInitialSettings()
        {
            return new Settings
            {
                NumberOfBoids = 150,
                NumberOfObstacles = 0,
                NumberOfPredators = 0,
                BoidRadius = 5,
                ObstacleRadius = 40,
                PredatorRadius = 20,
                SeparationWeight = 0.8,
                AlignmentWeight = 0.7,
                CohesionWeight = 0.01,
                MaxVelocityBoid = 8,
                MaxVelocityPredator = 6,
                NeighbourRadiusBoid = 50,
                CollisionRadiusBoid = 20,
                ThreathDetectionRadius = 150,
                Interval = 50
            };
        }
    }
}