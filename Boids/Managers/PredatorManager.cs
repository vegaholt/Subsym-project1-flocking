using System;
using System.Collections.Generic;
using Boids.Helpers;
using Boids.Models;

namespace Boids
{
    public class PredatorManager
    {
        public Predator CreateNewPredator()
        {
            var predator = new Predator
            {
                Position = new Position { X = BoidHelper.GetRandomNumber(0, 100), Y = BoidHelper.GetRandomNumber(0, 100) },
                Velocity = new Velocity { X = BoidHelper.GetRandomNumber(-10, 10), Y = BoidHelper.GetRandomNumber(-10, 10) }
            };

            predator.Velocity = BoidHelper.AdjustVelocity(predator.Velocity, 5);

            //Out of bounce
            predator.Position.X = predator.Position.X % 800;
            predator.Position.Y = predator.Position.Y % 600;

            //Angle
            predator.Velocity.Deg = Math.Atan2(predator.Position.Y, predator.Position.X) * 180.0 / Math.PI;

            return predator;
        }

        public void CalculateNewVelocityForPredators(List<Predator> predators, Settings settings)
        {
            foreach (var predator in predators)
            {
                var newVelocity = new Velocity
                {
                    X = BoidHelper.GetRandomNumber(0, -1),
                    Y = -1
                };

                UpdatePredator(predator, newVelocity, settings);
            }
        }

        private void UpdatePredator(Predator predator, Velocity velocity, Settings settings)
        {
            predator.Velocity = BoidHelper.AdjustVelocity(velocity, settings.MaxVelocityBoid);
            predator.Position.X += velocity.X;
            predator.Position.Y += velocity.Y;

            //Out of bounce
            predator.Position.X = (predator.Position.X > 100) ? predator.Position.X - 100 : (predator.Position.X < 0) ? predator.Position.X + 100 : predator.Position.X;
            predator.Position.Y = (predator.Position.Y > 100) ? predator.Position.Y - 100 : (predator.Position.Y < 0) ? predator.Position.Y + 100 : predator.Position.Y;

            //Angle
            predator.Velocity.Deg = Math.Atan2(predator.Position.Y, predator.Position.X) * 180.0 / Math.PI;
        }
    }
}