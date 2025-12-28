using Game.Core;
using Game.Entities;
using Game.Movements;
using Game.Properties;
using Game.Systems;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Game
{
    public partial class RacingCar : Form
    {
        Game.Core.Game game = new Game.Core.Game();
        PhysicsSystem physics = new PhysicsSystem();
        CollisionSystem collisions = new CollisionSystem();
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        Image[] enemyCarImages;

        // Lane setup
        float laneWidth = 100f;   // width of one lane (fits one car)
        float[] lanes;
        Random random = new Random();

        // Enemy spawn
        int spawnCounter = 0;
        int spawnInterval = 60;

        // Energy booster spawn
        int boosterCounter = 0;
        int boosterInterval = 180;

        // Road
        Image roadImage;
        float roadX;
        float roadWidth;

        public RacingCar()
        {
            InitializeComponent();
            DoubleBuffered = true;

            // Load road image
            roadImage = Resources.Roadimage;

            // Road width = 3 lanes
            roadWidth = laneWidth * 3;

            // Center road in form
            roadX = (this.ClientSize.Width - roadWidth) / 2;

            // Lane centers
            lanes = new float[]
            {
                roadX + laneWidth / 2f,   // Left lane
                roadX + laneWidth * 1.5f, // Middle lane
                roadX + laneWidth * 2.5f  // Right lane
            };

            // Enemy images
            enemyCarImages = new Image[]
            {
                Resources.pinkCar,
                Resources.blueCar,
                Resources.greenCar,
                Resources.whiteTruck
            };

            Setting();
            SetupTimer();
        }

        private void Setting()
        {
            // Add player car (middle lane)
            game.AddObject(new Player
            {
                Position = new PointF(lanes[1], 400),
                Size = new Size(100, 100),
                Sprite = Resources.DriverCar,
                Movement = new KeyboardMovement() // standard keyboard movement
            });
        }

        private void SetupTimer()
        {
            timer.Interval = 16; // ~60 FPS
            timer.Tick += timer1_Tick;
            timer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Draw fixed road (height = form height)
            g.DrawImage(roadImage, roadX, 0, (int)roadWidth, this.ClientSize.Height);

            // Draw all game objects
            game.Draw(g);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Spawn enemies
            spawnCounter++;
            if (spawnCounter >= spawnInterval)
            {
                SpawnEnemy();
                spawnCounter = 0;
                spawnInterval = random.Next(40, 100);
            }

            // Spawn energy boosters
            boosterCounter++;
            if (boosterCounter >= boosterInterval)
            {
                SpawnEnergyBooster();
                boosterCounter = 0;
            }

            // Update game objects
            game.Update(new GameTime());
            physics.Apply(game.Objects.ToList());
            collisions.Check(game.Objects.ToList());
            game.Cleanup();

            // Keep player inside road
            Player player = game.Objects.OfType<Player>().FirstOrDefault();
            if (player != null)
            {
                float minX = roadX + player.Size.Width / 2;
                float maxX = roadX + roadWidth - player.Size.Width / 2;
                if (player.Position.X < minX) player.Position = new PointF(minX, player.Position.Y);
                if (player.Position.X > maxX) player.Position = new PointF(maxX, player.Position.Y);
            }

            Invalidate(); // refresh the form
        }

        private void SpawnEnemy()
        {
            int laneIndex = random.Next(lanes.Length);
            float laneX = lanes[laneIndex];

            Enemy enemy = new Enemy
            {
                Position = new PointF(laneX, -100),
                Size = new Size(100, 100),
                Velocity = new PointF(0, 5),
                Sprite = enemyCarImages[random.Next(enemyCarImages.Length)]
            };

            game.AddObject(enemy);
        }

        private void SpawnEnergyBooster()
        {
            int laneIndex = random.Next(lanes.Length);
            float laneX = lanes[laneIndex];

            EnergyBooster booster = new EnergyBooster
            {
                Position = new PointF(laneX, -50),
                Size = new Size(50, 50),
                Velocity = new PointF(0, 4),
                Sprite = Resources.energy
            };

            game.AddObject(booster);
        }
    }
}
