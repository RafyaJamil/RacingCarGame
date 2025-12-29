using Game.Core;
using Game.Entities;
using Game.Movements;
using Game.Properties;
using Game.Systems;
using System;
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

        Image roadImage;
        Image sideImage;
        Image[] enemyCarImages;

        float laneWidth = 100f;
        float[] lanes;
        float roadX;
        float roadWidth;

        Random random = new Random();

        int enemyCounter = 0;
        int enemyInterval = 60;

        int boosterCounter = 0;
        int boosterInterval = 180;

        const int framesFor5Seconds = 300;
        const int maxScore = 70;

        public RacingCar()
        {
            InitializeComponent();
            DoubleBuffered = true;

            roadImage = Resources.Roadimage;
            sideImage = Resources.roadsidebuildings;

            enemyCarImages = new Image[]
            {
                Resources.pinkCar,
                Resources.blueCar,
                Resources.greenCar,
                Resources.whiteTruck
            };

            // --- ALIGNMENT FIX START ---
            roadWidth = roadImage.Width; // original road width
            roadX = (ClientSize.Width - roadWidth) / 2; // center road
            laneWidth = roadWidth / 3f; // 3 lanes

            lanes = new float[]
            {
                roadX + laneWidth * 0.5f,
                roadX + laneWidth * 1.5f,
                roadX + laneWidth * 2.5f
            };
            // --- ALIGNMENT FIX END ---

            SetupGame();
            SetupTimer();
        }

        void SetupGame()
        {
            game.AddObject(new Player
            {
                Position = new PointF(lanes[1], 420),
                Size = new Size(100, 100),
                Sprite = Resources.DriverCar,
                Movement = new KeyboardMovement()
            });
        }

        void SetupTimer()
        {
            timer.Interval = 16; // ~60 FPS
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawImage(sideImage, 0, 0, (int)roadX, ClientSize.Height);
            g.DrawImage(roadImage, roadX, 0, (int)roadWidth, ClientSize.Height);

            game.Draw(g);
            DrawHUD(g);
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            Player player = game.Objects.OfType<Player>().FirstOrDefault();
            if (player != null)
            {
                player.framesWithoutBooster++;

                if (player.framesWithoutBooster >= framesFor5Seconds)
                {
                    player.Fuel -= 10;
                    if (player.Fuel < 0) player.Fuel = 0;
                    player.framesWithoutBooster = 0;
                }

                if (player.Position.X < lanes[0])
                    player.Position = new PointF(lanes[0], player.Position.Y);
                if (player.Position.X > lanes[2])
                    player.Position = new PointF(lanes[2], player.Position.Y);

                if (player.Fuel <= 0)
                {
                    timer.Stop();
                    MessageBox.Show("LEVEL FAILED!");
                }

                if (player.Score >= maxScore)
                {
                    timer.Stop();
                    MessageBox.Show("LEVEL COMPLETE!");
                }
            }

            enemyCounter++;
            if (enemyCounter >= enemyInterval)
            {
                SpawnEnemy();
                enemyCounter = 0;
            }

            boosterCounter++;
            if (boosterCounter >= boosterInterval)
            {
                SpawnBooster();
                boosterCounter = 0;
            }

            game.Update(new GameTime());
            physics.Apply(game.Objects.ToList());
            collisions.Check(game.Objects.ToList());
            game.Cleanup();

            Invalidate();
        }

        void SpawnEnemy()
        {
            int lane = random.Next(3);
            game.AddObject(new Enemy
            {
                Position = new PointF(lanes[lane], -120),
                Size = new Size(100, 100),
                Velocity = new PointF(0, 5),
                Sprite = enemyCarImages[random.Next(enemyCarImages.Length)]
            });
        }

        void SpawnBooster()
        {
            int lane = random.Next(3);
            game.AddObject(new EnergyBooster
            {
                Position = new PointF(lanes[lane], -60),
                Size = new Size(50, 50),
                Velocity = new PointF(0, 4),
                Sprite = Resources.energy
            });
        }

        void DrawHUD(Graphics g)
        {
            // original DrawHUD function as you sent
            float hudX = roadX + roadWidth + 30;
            int barWidth = 40;
            int barHeight = 260;
            int fuelY = 80;
            Font titleFont = new Font("Arial", 10, FontStyle.Bold);

            Player player = game.Objects.OfType<Player>().FirstOrDefault();
            if (player == null) return;

            Color labelColor = Color.Black; // label color
            Color hudBgColor = Color.FromArgb(180, 100, 50); // brown-orange

            // Fuel
            g.DrawString("FUEL", titleFont, new SolidBrush(labelColor), hudX, fuelY - 30);
            g.FillRectangle(new SolidBrush(hudBgColor), hudX, fuelY, barWidth, barHeight);
            float fuelFill = (player.Fuel / (float)player.MaxFuel) * barHeight;
            Brush fuelBrush = (player.Fuel <= 30) ? Brushes.Red : Brushes.Lime;
            g.FillRectangle(fuelBrush, hudX, fuelY + barHeight - fuelFill, barWidth, fuelFill);

            // Score
            int scoreX = (int)(hudX + barWidth + 25);
            int scoreY = fuelY;
            g.DrawString("SCORE", titleFont, new SolidBrush(labelColor), scoreX, scoreY - 30);
            g.FillRectangle(new SolidBrush(hudBgColor), scoreX, scoreY, barWidth, barHeight);

            int maxBlocks = 7;
            int filledBlocks = Math.Min(player.Score / 10, maxBlocks);
            for (int i = 0; i < maxBlocks; i++)
            {
                Brush block = (i < filledBlocks) ? Brushes.Lime : Brushes.DimGray;
                g.FillRectangle(block, scoreX, scoreY + (maxBlocks - 1 - i) * 32, barWidth, 26);
            }
        }
    }
}
