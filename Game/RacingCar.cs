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
        Image grassImage;
        Image[] enemyCarImages;

        float laneWidth;
        float[] lanes;
        float roadX;
        float roadWidth;

        // scrolling
        float grassOffsetY = 0f;
        float roadOffsetY = 0f;

        float grassSpeed = 4f;
        float roadSpeed = 6f;

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
            grassImage = Resources.grass;

            enemyCarImages = new Image[]
            {
                Resources.pinkCar,
                Resources.blueCar,
                Resources.greenCar,
                Resources.whiteTruck
            };

            // --- ROAD & LANES ALIGNMENT ---
            roadWidth = roadImage.Width;
            roadX = (ClientSize.Width - roadWidth) / 2f;
            laneWidth = roadWidth / 3f;

            lanes = new float[]
            {
                roadX + laneWidth * 0.5f,
                roadX + laneWidth * 1.5f,
                roadX + laneWidth * 2.5f
            };

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

            // LEFT GRASS
            DrawScrollingGrass(g, 0, (int)roadX);

            // ROAD (animated)
            DrawScrollingRoad(g);

            // RIGHT GRASS
            DrawScrollingGrass(
                g,
                (int)(roadX + roadWidth),
                ClientSize.Width - (int)(roadX + roadWidth)
            );

            game.Draw(g);
            DrawHUD(g);
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            // scrolling updates
            grassOffsetY += grassSpeed;
            roadOffsetY += roadSpeed;

            if (grassOffsetY >= grassImage.Height)
                grassOffsetY = 0;

            if (roadOffsetY >= roadImage.Height)
                roadOffsetY = 0;

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

        void DrawScrollingGrass(Graphics g, int x, int width)
        {
            int imgH = grassImage.Height;

            for (int y = -imgH; y < ClientSize.Height + imgH; y += imgH)
            {
                g.DrawImage(
                    grassImage,
                    x,
                    y + (int)grassOffsetY,
                    width,
                    imgH
                );
            }
        }

        void DrawScrollingRoad(Graphics g)
        {
            int imgH = roadImage.Height;

            for (int y = -imgH; y < ClientSize.Height + imgH; y += imgH)
            {
                g.DrawImage(
                    roadImage,
                    roadX,
                    y + (int)roadOffsetY,
                    (int)roadWidth,
                    imgH
                );
            }
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
            float hudX = roadX + roadWidth + 30;
            int barWidth = 40;
            int barHeight = 260;
            int fuelY = 80;
            Font titleFont = new Font("Arial", 10, FontStyle.Bold);

            Player player = game.Objects.OfType<Player>().FirstOrDefault();
            if (player == null) return;

            Color labelColor = Color.Black;
            Color hudBgColor = Color.FromArgb(180, 100, 50);

            // Fuel
            g.DrawString("FUEL", titleFont, new SolidBrush(labelColor), hudX, fuelY - 30);
            g.FillRectangle(new SolidBrush(hudBgColor), hudX, fuelY, barWidth, barHeight);

            float fuelFill = (player.Fuel / (float)player.MaxFuel) * barHeight;
            Brush fuelBrush = (player.Fuel <= 30) ? Brushes.Red : Brushes.Lime;
            g.FillRectangle(fuelBrush, hudX, fuelY + barHeight - fuelFill, barWidth, fuelFill);

            // Score
            int scoreX = (int)(hudX + barWidth + 25);
            g.DrawString("SCORE", titleFont, new SolidBrush(labelColor), scoreX, fuelY - 30);
            g.FillRectangle(new SolidBrush(hudBgColor), scoreX, fuelY, barWidth, barHeight);

            int maxBlocks = 7;
            int filledBlocks = Math.Min(player.Score / 10, maxBlocks);

            for (int i = 0; i < maxBlocks; i++)
            {
                Brush block = (i < filledBlocks) ? Brushes.Lime : Brushes.DimGray;
                g.FillRectangle(block, scoreX, fuelY + (maxBlocks - 1 - i) * 32, barWidth, 26);
            }
        }
    }
}
