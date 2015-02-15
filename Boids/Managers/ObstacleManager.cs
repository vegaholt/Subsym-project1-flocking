using System;
using Boids.Helpers;
using Boids.Models;

namespace Boids.Managers
{
    public class ObstacleManager
    {
        public Obstacle CreateNewObstacle()
        {
            var obstacle = new Obstacle
            {
                Position = new Position { X = BoidHelper.GetRandomNumber(0, 100), Y = BoidHelper.GetRandomNumber(0, 100) },
            };

            return obstacle;
        }
    }
}