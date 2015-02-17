using Boids.Models;
using System;

namespace Boids.Helpers
{
    public static class BoidHelper
    {
        public static Random RandomGenerator = new Random();

        public static double GetRandomNumber(double min, double max)
        {
            return Math.Round((min + RandomGenerator.NextDouble() * (max - min)), 10);
        }

        public static Velocity AdjustVelocity(Velocity velocity, double maxVelocity)
        {
            var oldVelocity = Math.Sqrt(Math.Pow(velocity.X, 2) + Math.Pow(velocity.Y, 2));
            if (oldVelocity <= maxVelocity) return velocity;

            var scale = maxVelocity / oldVelocity;

            var newVelocity = new Velocity()
            {
                X = Math.Round((velocity.X * scale), 5),
                Y = Math.Round((velocity.Y * scale), 5)
            };

            return newVelocity;
        }
    }
}