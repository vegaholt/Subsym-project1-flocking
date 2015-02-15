using System;
using System.Collections.Generic;
using System.Linq;
using Boids.Helpers;
using Boids.Models;

namespace Boids.Managers
{
    public class BoidManager
    {
        public Boid CreateNewBoid()
        {
            var boid = new Boid
            {
                Position = new Position { X = BoidHelper.GetRandomNumber(0, 100), Y = BoidHelper.GetRandomNumber(0, 100) },
                Velocity = new Velocity { X = BoidHelper.GetRandomNumber(-10, 10), Y = BoidHelper.GetRandomNumber(-10, 10) }
            };

            boid.Velocity = BoidHelper.AdjustVelocity(boid.Velocity, 5);

            //Out of bounce
            boid.Position.X = boid.Position.X % 800;
            boid.Position.Y = boid.Position.Y % 600;

            //Angle
            boid.Velocity.Deg = Math.Atan2(boid.Position.Y, boid.Position.X) * 180.0 / Math.PI;
            
            return boid;
        }

        #region Update Boid
        public List<Boid> CalculateNewVelocityForBoids(List<Boid> boids, Settings settings)
        {
            foreach (var boid in boids)
            {
                var newVelocity = new Velocity
                {
                    X = BoidHelper.GetRandomNumber(0, 1),
                    Y = 1
                };

                UpdateBoid(boid, newVelocity, settings);

                /*var newVelocity = new Velocity();

                var neighbourBoids = GetNeighbourBoids(new List<Boid>(), boid, settings.NeighbourRadiusBoid);
                var collisionBoids = GetNeighbourBoids(new List<Boid>(), boid, settings.CollisionRadiusBoid);
                var surroundingObstacles = GetNeighbourObstacles(new List<Obstacle>(), boid, settings.ThreathDetectionRadius);
                var surroundingPredators = GetNeighbourPredators(new List<Predator>(), boid, settings.ThreathDetectionRadius);

                //Stick to the same vector if there are no neighbours, obstacles or predators within the boid's radius
                if (!(neighbourBoids.Any() && collisionBoids.Any() && surroundingObstacles.Any() && surroundingPredators.Any()))
                {
                    UpdateBoid(boid, boid.Velocity);
                }

                //Adjust for alignment and cohesion
                if (neighbourBoids.Any())
                {
                    var alignVelocity = CalculateAlignmentVelocity(neighbourBoids, boid);
                    var cohesionVelocity = CalculateCohesionVelocity(neighbourBoids, boid);

                    newVelocity.X =
                        newVelocity.X +
                        alignVelocity.X * settings.AlignmentWeight +
                        cohesionVelocity.X * settings.CohesionWeight;
                    newVelocity.Y =
                        newVelocity.Y +
                        alignVelocity.Y * settings.AlignmentWeight +
                        cohesionVelocity.Y * settings.CohesionWeight;
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

                UpdateBoid(boid, newVelocity);*/
            }
            return boids;
        }

        private Velocity CalculateSeparationVelocity(List<Boid> neighbours, Boid boid)
        {
            return null;
        }

        private Velocity CalculateAlignmentVelocity(List<Boid> neighbours, Boid boid)
        {
            double avgX = 0;
            double avgY = 0;
            
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

        private Velocity CalculateCohesionVelocity(List<Boid> neighbours, Boid boid)
        {
            double avgX = 0;
            double avgY = 0;

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

        private Velocity GetVelocityToAvoidObstacle(List<Obstacle> obstacles, Boid boid)
        {
            return null;
        }

        private Velocity GetVelocityToAvoidPredator(List<Predator> obstacles, Boid boid)
        {
            return null;
        }
        
        private void UpdateBoid(Boid boid, Velocity velocity, Settings settings)
        {
            boid.Velocity = BoidHelper.AdjustVelocity(velocity, settings.MaxVelocityBoid);
            boid.Position.X += velocity.X;
            boid.Position.Y += velocity.Y;

            //Out of bounce
            boid.Position.X = (boid.Position.X > 100) ? boid.Position.X - 100 : (boid.Position.X < 0) ? boid.Position.X + 100 : boid.Position.X;
            boid.Position.Y = (boid.Position.Y > 100) ? boid.Position.Y - 100 : (boid.Position.Y < 0) ? boid.Position.Y + 100 : boid.Position.Y;
            
            //Angle
            boid.Velocity.Deg = Math.Atan2(boid.Position.Y, boid.Position.X) * 180.0 / Math.PI;
        }
        #endregion

        #region Detect Neighbours
        private List<Boid> GetNeighbourBoids(List<Boid> boids, Boid boid, double radius)
        {
            var neighbourBoids = boids.Where(x => GetDistance(x.Position, boid.Position) <= radius).ToList();
            if (neighbourBoids.Contains(boid)) neighbourBoids.Remove(boid);
            return neighbourBoids;
        }

        private List<Obstacle> GetNeighbourObstacles(List<Obstacle> obstacles, Boid boid, double radius)
        {
            return obstacles.Where(x => GetDistance(x.Position, boid.Position) <= radius).ToList();
        }

        private List<Predator> GetNeighbourPredators(List<Predator> predators, Boid boid, double radius)
        {
            return predators.Where(x => GetDistance(x.Position, boid.Position) <= radius).ToList();
        }

        private double GetDistance(Position position1, Position position2)
        {
            var a = position2.X - position1.X;
            var b = position2.Y - position1.Y;
            return (double)Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
        }
        #endregion
    }
}