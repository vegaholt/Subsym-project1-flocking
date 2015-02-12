using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Boids.Hubs;
using Boids.Models;
using Microsoft.AspNet.SignalR;

namespace Boids.Managers
{
    public class InitializerManager
    {
        private static Settings _settings;
        private static List<Boid> _boids;
        private static List<Obstacle> _obstacles;
        private static List<Predator> _predators;
        private static int _boidIdCounter;
        private static int _ticks;
        private static int _collisionsWithObstacle;
        private static int _collisionsWithPredator;
        private static IHubContext _hub;
        private static InitializerManager _main;
        private static BoidManager _boidManager;

         public static InitializerManager GetInstance()
        {

            return _main ?? (

                _main = new InitializerManager()
                );
        }

         private InitializerManager()
        {
           _hub = GlobalHost.ConnectionManager.GetHubContext<BoidHub>();
             _boidIdCounter = 0;
             _boidManager = new BoidManager();
             _boids = new List<Boid>();
        }

        public void StartNewRound(Settings settings)
        {
            
        }


        public void AddNewBoid()
        {
            var newBoid = _boidManager.InitBoid();
            newBoid.Id = _boidIdCounter++;
            _boids.Add(newBoid);
            _hub.Clients.All.addNewBoid(newBoid);
        }

        public void GetBoidList()
        {
            _hub.Clients.All.logBoidList(_boids);
        }



        public void SpawnNewPredator()
        {
            
        }
    }
}