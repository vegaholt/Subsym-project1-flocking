using Boids.Models;

namespace Boids.Managers
{
    public class SettingsManager
    {
        public Settings GetInitialSettings()
        {
            return new Settings
            {
                NumberOfBoids = 2,
                NumberOfObstacles = 0,
                NumberOfPredators = 0,
                BoidRadius = 10,
                ObstacleRadius = 40,
                PredatorRadius = 20,
                SeparationWeight = 0.5,
                AlignmentWeight = 0.5,
                CohesionWeight = 0.5,
                MaxVelocityBoid = 2,
                MaxVelocityPredator = 1.5,
                NeighbourRadiusBoid = 100,
                CollisionRadiusBoid = 50,
                ThreathDetectionRadius = 150,
                Interval = 500
            };
        }
    }
}