using System;
using System.Collections.Generic;
using System.Linq;
using Boids.Helpers;
using Boids.Models;

namespace Boids.Managers
{
    public class BoidManager
    {
        public void InitBoid()
        {
            var boid = new Boid
            {
                Position = new Position { X = BoidHelper.GetRandomNumber(0, 100), Y = BoidHelper.GetRandomNumber(0, 100) },
                Velocity = new Velocity { X = BoidHelper.GetRandomNumber(-10, 10), Y = BoidHelper.GetRandomNumber(-10, 10) }
            };
            
            boid.Velocity = BoidHelper.AdjustVelocity(new Velocity(), 5);
        }

        #region Detect Neighbours
        public List<Boid> GetNeighbourBoids(List<Boid> boids, Boid boid, float radius)
        {
            var neighbourBoids = boids.Where(x => GetDistance(x.Position, boid.Position) <= radius).ToList();
            if(neighbourBoids.Contains(boid)) neighbourBoids.Remove(boid);
            return neighbourBoids;
        }

        public List<Obstacle> GetNeighbourObstacles(List<Obstacle> obstacles, Boid boid, float radius)
        {
            return obstacles.Where(x => GetDistance(x.Position, boid.Position) <= radius).ToList();
        }
 
        public List<Predator> GetNeighbourPredators(List<Predator> predators, Boid boid, float radius)
        {
            return predators.Where(x => GetDistance(x.Position, boid.Position) <= radius).ToList();
        }

        private static float GetDistance(Position position1, Position position2)
        {
            var a = position2.X - position1.X;
            var b = position2.Y - position1.Y;
            return (float) Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
        }
        #endregion

        #region Update Boid
        public void CalculateNewVelocityForBoid(Boid boid, Settings settings)
        {
            var newVelocity = new Velocity();

            var neighbourBoids = GetNeighbourBoids(new List<Boid>(), boid, settings.NeighbourRadiusBoid);
            var collisionBoids = GetNeighbourBoids(new List<Boid>(), boid, settings.CollisionRadiusBoid);
            var surroundingObstacles = GetNeighbourObstacles(new List<Obstacle>(), boid, settings.ThreathDetectionRadius);
            var surroundingPredators = GetNeighbourPredators(new List<Predator>(), boid, settings.ThreathDetectionRadius);

            //Stick to the same vector if there are no neighbours, obstacles or predators within the boid's radius
            if (!(neighbourBoids.Any() && collisionBoids.Any() && surroundingObstacles.Any() && surroundingPredators.Any()))
            {
                UpdateBoid(boid, boid.Velocity);
                return;
            }

            //Adjust for alignment and cohesion
            if (neighbourBoids.Any())
            { 
                var alignVelocity = CalculateAlignmentVelocity(neighbourBoids, boid);
                var cohesionVelocity = CalculateCohesionVelocity(neighbourBoids, boid);

                newVelocity.X =
                    newVelocity.X + 
                    alignVelocity.X * settings.AlignmentWeight +
                    cohesionVelocity.X*settings.CohesionWeight;
                newVelocity.Y =
                    newVelocity.Y +
                    alignVelocity.Y*settings.AlignmentWeight +
                    cohesionVelocity.Y*settings.CohesionWeight;
            }

            //Adjust for separation
            if (collisionBoids.Any())
            {
                var separationVelocity = CalculateSeparationVelocity(collisionBoids, boid);

                newVelocity.X =
                    newVelocity.X + separationVelocity.X * settings.SeparationWeight;
                newVelocity.Y =
                    newVelocity.Y + separationVelocity.Y * settings.SeparationWeight;
            }

            //Adjust for obstacle avoidance
            if (surroundingObstacles.Any())
            {
                var obstacleVelocity = GetVelocityToAvoidObstacle(surroundingObstacles, boid);

                newVelocity.X =
                    newVelocity.X + obstacleVelocity.X;
                newVelocity.Y =
                    newVelocity.Y + obstacleVelocity.Y;
            }

            //Adjust for predator avoidance
            if (surroundingPredators.Any())
            {
                var predatorVelocity = GetVelocityToAvoidPredator(surroundingPredators, boid);
          
                newVelocity.X =
                    newVelocity.X + predatorVelocity.X;
                newVelocity.Y =
                    newVelocity.Y + predatorVelocity.Y;
            }

            UpdateBoid(boid, newVelocity);
        }

        private static void UpdateBoid(Boid boid, Velocity velocity)
        {
            boid.Velocity = BoidHelper.AdjustVelocity(velocity, 5);
            boid.Position.X += velocity.X;
            boid.Position.Y += velocity.Y;
        }

        private static Velocity CalculateSeparationVelocity(List<Boid> neighbours, Boid boid)
        {
            return null;
        }

        private static Velocity CalculateAlignmentVelocity(List<Boid> neighbours, Boid boid)
        {
            float avgX = 0;
            float avgY = 0;
            
            foreach (var neighbour in neighbours)
            {
                avgX += neighbour.Position.X;
                avgY += neighbour.Position.Y;
            }

            avgX = avgX/neighbours.Count();
            avgY = avgY/neighbours.Count();

            return new Velocity
            {
                X = boid.Position.X - avgX,
                Y = boid.Position.Y - avgY
            };
        }

        private static Velocity CalculateCohesionVelocity(List<Boid> neighbours, Boid boid)
        {
            float avgX = 0;
            float avgY = 0;

            foreach (var neighbour in neighbours)
            {
                avgX += neighbour.Velocity.X;
                avgY += neighbour.Velocity.Y;
            }

            avgX = avgX / neighbours.Count();
            avgY = avgY / neighbours.Count();

            return new Velocity
            {
                X = avgX,
                Y = avgY
            };
        }

        public Velocity GetVelocityToAvoidObstacle(List<Obstacle> obstacles, Boid boid)
        {
            return null;
        }

        public Velocity GetVelocityToAvoidPredator(List<Predator> obstacles, Boid boid)
        {
            return null;
        }
        #endregion
    }
}