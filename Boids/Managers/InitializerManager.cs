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
        private static IHubContext _hub;
        private static InitializerManager _main;
        
        private static BoidManager _boidManager;
        private static ObstacleManager _obstacleManager;
        private static PredatorManager _predatorManager;
        private static SettingsManager _settingsManager;

        private static List<Boid> _boids;
        private static List<Obstacle> _obstacles;
        private static List<Predator> _predators;
        private static Settings _settings;

        private static int _boidIdCounter;
        private static int _obstacleIdCounter;
        private static int _predatorIdCounter;

        private static int _collisionsWithObstacle;
        private static int _collisionsWithPredator;

        private static int _ticks;

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

            _boidManager = new BoidManager();
            _obstacleManager = new ObstacleManager();
            _predatorManager = new PredatorManager();
            _settingsManager = new SettingsManager();

            _boids = new List<Boid>();
            _obstacles = new List<Obstacle>();
            _predators = new List<Predator>();
            _settings = new Settings();

            _boidIdCounter = 0;
            _obstacleIdCounter = 0;
            _predatorIdCounter = 0;

            _collisionsWithObstacle = 0;
            _collisionsWithPredator = 0;

            _ticks = 0;
        }

        public void StartNewRound(Settings settings)
        {
            //Timer
            //interval 10
            while (true)
            {
                //update all boids
            }
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