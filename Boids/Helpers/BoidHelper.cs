using Boids.Models;
using System;

namespace Boids.Helpers
{
    public static class BoidHelper
    {
        public static Random RandomGenerator = new Random();

        public static float GetRandomNumber(float min, float max)
        {
            return (float) (min + RandomGenerator.NextDouble() * (max - min));
        }

        public static Velocity AdjustVelocity(Velocity velocity, float maxVelocity)
        {
            var oldVelocity = Math.Sqrt(Math.Pow(velocity.X, 2) + Math.Pow(velocity.Y, 2));
            if (oldVelocity <= maxVelocity) return velocity;

            var scale = maxVelocity / oldVelocity;

            var newVelocity = new Velocity()
            {
                X = (float)(velocity.X * scale),
                Y = (float)(velocity.Y * scale)
            };

            return newVelocity;
        }
    }
}