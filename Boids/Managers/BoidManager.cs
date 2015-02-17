using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using Boids.Helpers;
using Boids.Models;

namespace Boids.Managers
{
    public class BoidManager
    {
        public Boid CreateNewBoid(Settings settings)
        {
            var boid = new Boid
            {
                Position = new Position { X = BoidHelper.GetRandomNumber(0, 800), Y = BoidHelper.GetRandomNumber(0, 600) },
                Velocity = new Velocity { X = BoidHelper.GetRandomNumber(-15, 15), Y = BoidHelper.GetRandomNumber(-15, 15) }
            };

            boid.Velocity = BoidHelper.AdjustVelocity(boid.Velocity, settings.MaxVelocityBoid);

            //Out of bounce
            boid.Position.X = (boid.Position.X > 800) ? boid.Position.X - 800 : (boid.Position.X < 0) ? boid.Position.X + 800 : boid.Position.X;
            boid.Position.Y = (boid.Position.Y > 600) ? boid.Position.Y - 600 : (boid.Position.Y < 0) ? boid.Position.Y + 600 : boid.Position.Y;

            //Angle
            boid.Velocity.Deg = Math.Atan2(boid.Position.Y, boid.Position.X) * 180.0 / Math.PI;
            
            return boid;
        }

        #region Update Boid
        public void CalculateNewVelocityForBoids(List<Boid> boids, Settings settings)
        {
            foreach (var boid in boids)
            {
                var newVelocity = new Velocity{X = boid.Velocity.X, Y = boid.Velocity.Y};

                var neighbourBoids = GetNeighbourBoids(boids, boid, settings.NeighbourRadiusBoid);
                var collisionBoids = GetNeighbourBoids(boids, boid, settings.CollisionRadiusBoid);
                //var surroundingObstacles = GetNeighbourObstacles(new List<Obstacle>(), boid, settings.ThreathDetectionRadius);
                //var surroundingPredators = GetNeighbourPredators(new List<Predator>(), boid, settings.ThreathDetectionRadius);

                //Stick to the same vector if there are no neighbours, obstacles or predators within the boid's radius
                if (!(neighbourBoids.Any()))// && collisionBoids.Any() && surroundingObstacles.Any() && surroundingPredators.Any()))
                {
                    UpdateBoid(boid, boid.Velocity, settings);
                    continue;
                }

                //Adjust for alignment and cohesion
                var alignVelocity = CalculateAlignmentVelocity(neighbourBoids, boid);
                var cohesionVelocity = CalculateCohesionVelocity(neighbourBoids, boid);
                var separationVelocity = CalculateSeparationVelocity(collisionBoids, boid);

                newVelocity.X =
                    newVelocity.X +
                    cohesionVelocity.X*settings.CohesionWeight +
                    alignVelocity.X*settings.AlignmentWeight +
                    separationVelocity.X*settings.SeparationWeight;
                newVelocity.Y =
                    newVelocity.Y +
                    cohesionVelocity.Y * settings.CohesionWeight + 
                    alignVelocity.Y*settings.AlignmentWeight +
                    separationVelocity.Y * settings.SeparationWeight;
                
                /*
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
                 */

                UpdateBoid(boid, newVelocity, settings);
            }
        }

        private Velocity CalculateSeparationVelocity(List<Boid> neighbours, Boid boid)
        {
            Velocity newVelocity = new Velocity();

		    foreach(var neighbour in neighbours)
		    {
                newVelocity.X = newVelocity.X - (neighbour.Position.X - boid.Position.X);
                newVelocity.Y = newVelocity.X - (neighbour.Position.Y - boid.Position.Y);
            }

            return newVelocity;
/*
            double avgX = 0;
            double avgY = 0;

            foreach (var neighbour in neighbours)
            {
                avgX += neighbour.Position.X;
                avgY += neighbour.Position.Y;
            }

            avgX = avgX / neighbours.Count();
            avgY = avgY / neighbours.Count();

            return new Velocity
            {
                X = boid.Position.X - avgX,
                Y = boid.Position.Y- avgY
            };
 */
        }

        private Velocity CalculateCohesionVelocity(List<Boid> neighbours, Boid boid)
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
                X = avgX - boid.Position.X,
                Y = avgY - boid.Position.Y
            };
        }

        private Velocity CalculateAlignmentVelocity(List<Boid> neighbours, Boid boid)
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
                X = avgX - boid.Velocity.X,
                Y = avgY - boid.Velocity.Y
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
            boid.Position.X += boid.Velocity.X;
            boid.Position.Y += boid.Velocity.Y;

            //Out of bounce
            boid.Position.X = (boid.Position.X > 800) ? boid.Position.X - 800 : (boid.Position.X < 0) ? boid.Position.X + 800 : boid.Position.X;
            boid.Position.Y = (boid.Position.Y > 600) ? boid.Position.Y - 600 : (boid.Position.Y < 0) ? boid.Position.Y + 600 : boid.Position.Y;

            //Angle
            boid.Velocity.Deg = Math.Atan2(boid.Position.Y, boid.Position.X) * 180.0 / Math.PI;
        }
        #endregion

        #region Detect Neighbours
        private List<Boid> GetNeighbourBoids(List<Boid> boids, Boid boid, double radius)
        {
            var list = new List<Boid>();
            var neighbourBoids = boids.Where(x => GetDistance(x.Position, boid.Position) <= radius).ToList();
            list.AddRange(neighbourBoids);
            if (list.Contains(boid)) list.Remove(boid);
            return list;
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
            return Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
        }
        #endregion
    }
}