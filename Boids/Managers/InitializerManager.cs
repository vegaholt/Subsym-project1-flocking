using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;
using Boids.Hubs;
using Boids.Models;
using Microsoft.AspNet.SignalR;

namespace Boids.Managers
{
    public class InitializerManager
    {
        #region Initialize Methods
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

        private static int _numberOfBoids;
        private static int _numberOfObstacles;
        private static int _numberOfPredators;

        private static int _collisionsWithObstacle;
        private static int _collisionsWithPredator;

        private static Timer _timer;

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

            //Start with 100 boids and default settings
            SetInitialSettings();
            SetInitialBoids();

            _numberOfBoids = _settings.NumberOfBoids;
            _numberOfObstacles = _settings.NumberOfObstacles;
            _numberOfPredators = _settings.NumberOfPredators;

            _timer = new Timer(_settings.Interval);
            _timer.Elapsed += OnTimedEvent;
        }

        private void SetInitialBoids()
        {
            for (var i = 0; i < _settings.NumberOfBoids; i++)
            {
                var boid = _boidManager.CreateNewBoid(_settings);
                boid.Id = _boidIdCounter++;
                _boids.Add(boid);
            }
        }

        private void SetInitialSettings()
        {
            _settings = _settingsManager.GetInitialSettings();
        }
        #endregion

        #region Runtime Methods
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            //Main loop
            _boidManager.CalculateNewVelocityForBoids(_boids, _settings);
            SendBoidListToClient();

            SendObstacleListToClient();

            _predatorManager.CalculateNewVelocityForPredators(_predators, _settings);
            SendPredatorListToClient();
        }

        public void StartFlocking()
        {
            _timer.Enabled = true;
            
        }

        public void StopFlocking()
        {
            _timer.Enabled = false;
        }
        #endregion

        #region Update Methods
        public void UpdateSettings(Settings settings)
        {
            _settings = settings;
            _timer.Interval = _settings.Interval;

            //If different number of boids
            if (settings.NumberOfBoids > _numberOfBoids)
            {
                //Add boids
                var startIndex = _numberOfBoids;
                var endIndex = _settings.NumberOfBoids;

                _numberOfBoids = _settings.NumberOfBoids;
                
                for (var i = startIndex; i < endIndex; i++)
                {
                    var boid = _boidManager.CreateNewBoid(_settings);
                    boid.Id = _boidIdCounter++;
                    _boids.Add(boid);
                }
            }
            else if(settings.NumberOfBoids < _numberOfBoids)
            {
                //Remove boids
                var startIndex = _settings.NumberOfBoids;
                var endIndex = _numberOfBoids;

                _boidIdCounter = _settings.NumberOfBoids;
                _numberOfBoids = _settings.NumberOfBoids;

                for (var i = startIndex; i < endIndex; i++)
                {
                    var boid = _boids.FirstOrDefault(x => x.Id == i);
                    _boids.Remove(boid);
                }
            }

            //If different number of obstacles
            if (settings.NumberOfObstacles > _numberOfObstacles)
            {
                //Add obstacle
                var startIndex = _numberOfObstacles;
                var endIndex = _settings.NumberOfObstacles;

                _numberOfObstacles = _settings.NumberOfObstacles;

                for (var i = startIndex; i < endIndex; i++)
                {
                    var obstacle = _obstacleManager.CreateNewObstacle();
                    obstacle.Id = _obstacleIdCounter++;
                    _obstacles.Add(obstacle);
                }
            }
            else if (settings.NumberOfObstacles < _numberOfObstacles)
            {
                //Remove obstacle
                var startIndex = _settings.NumberOfObstacles;
                var endIndex = _numberOfObstacles;

                _obstacleIdCounter = _settings.NumberOfObstacles;
                _numberOfObstacles = _settings.NumberOfObstacles;

                for (var i = startIndex; i < endIndex; i++)
                {
                    var obstacle = _obstacles.FirstOrDefault(x => x.Id == i);
                    _obstacles.Remove(obstacle);
                }
            }

            //If different number of predators
            if (settings.NumberOfPredators > _numberOfPredators)
            {
                //Add predator
                var startIndex = _numberOfPredators;
                var endIndex = _settings.NumberOfPredators;

                _numberOfPredators = _settings.NumberOfPredators;

                for (var i = startIndex; i < endIndex; i++)
                {
                    var predator = _predatorManager.CreateNewPredator();
                    predator.Id = _predatorIdCounter++;
                    _predators.Add(predator);
                }
            }
            else if (settings.NumberOfPredators < _numberOfPredators)
            {
                //Remove predator
                var startIndex = _settings.NumberOfPredators;
                var endIndex = _numberOfPredators;

                _predatorIdCounter = _settings.NumberOfPredators;
                _numberOfPredators = _settings.NumberOfPredators;

                for (var i = startIndex; i < endIndex; i++)
                {
                    var predator = _predators.FirstOrDefault(x => x.Id == i);
                    _predators.Remove(predator);
                }
            }
        }
        #endregion

        #region Client methods
        public void SendSettingsToClient()
        {
            _hub.Clients.All.hubSetSettings(_settings);
        }

        public void SendBoidListToClient()
        {
            _hub.Clients.All.hubDrawBoidList(_boids);
        }

        public void SendObstacleListToClient()
        {
            _hub.Clients.All.hubDrawObstacleList(_obstacles);
        }

        public void SendPredatorListToClient()
        {
            _hub.Clients.All.hubDrawPredatorList(_predators);
        }
        #endregion
    }
}