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

        Button jumpButton;

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

            // ROAD & LANES
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
            SetupJumpButton();
            SetupTimer();
        }

        void SetupGame()
        {
            var player = new Player
            {
                Position = new PointF(lanes[1], 420),
                Size = new Size(100, 100),
                Sprite = Resources.DriverCar
            };

            // Keyboard movement (Left/Right/Up/Down)
            player.Movement = new KeyboardMovement();
            // Jump only via button
            player.JumpMovement = new JumpingMovement(420);

            game.AddObject(player);
        }

        void SetupJumpButton()
        {
            jumpButton = new Button();
            jumpButton.Text = "JUMP";
            jumpButton.Size = new Size(100, 50);
            jumpButton.Location = new Point(50, ClientSize.Height - 80);
            jumpButton.Click += (s, e) =>
            {
                var player = game.Objects.OfType<Player>().FirstOrDefault();
                player?.TryJump(); // Jump only via button
            };
            Controls.Add(jumpButton);
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

            DrawScrollingGrass(g, 0, (int)roadX);
            DrawScrollingRoad(g, (int)roadX, (int)roadWidth);
            DrawScrollingGrass(g, (int)(roadX + roadWidth), ClientSize.Width - (int)(roadX + roadWidth));

            game.Draw(g);

            var player = game.Objects.OfType<Player>().FirstOrDefault();
            if (player != null && !string.IsNullOrEmpty(player.JumpMessage))
            {
                using (Font font = new Font("Arial", 16, FontStyle.Bold))
                using (Brush brush = new SolidBrush(Color.Yellow))
                {
                    g.DrawString(player.JumpMessage, font, brush, 50, 50);
                }
            }

            DrawHUD(g);
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            grassOffsetY += grassSpeed;
            roadOffsetY += roadSpeed;

            if (grassOffsetY >= grassImage.Height) grassOffsetY = 0;
            if (roadOffsetY >= roadImage.Height) roadOffsetY = 0;

            var player = game.Objects.OfType<Player>().FirstOrDefault();
            if (player != null)
            {
                player.framesWithoutBooster++;

                if (player.framesWithoutBooster >= framesFor5Seconds)
                {
                    player.Fuel -= 10;
                    if (player.Fuel < 0) player.Fuel = 0;
                    player.framesWithoutBooster = 0;
                }

                if (player.Position.X < lanes[0]) player.Position = new PointF(lanes[0], player.Position.Y);
                if (player.Position.X > lanes[2]) player.Position = new PointF(lanes[2], player.Position.Y);

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

            // Update all objects
            game.Update(new GameTime());
            collisions.Check(game.Objects.ToList()); // Collisions skip player if jumping

            foreach (var booster in game.Objects.OfType<EnergyBooster>().ToList())
            {
                if (booster.Position.Y > 600) booster.IsActive = false;
            }

            game.Cleanup();
            Invalidate();
        }

        void DrawScrollingGrass(Graphics g, int x, int width)
        {
            int imgH = grassImage.Height;
            for (int y = -imgH; y < ClientSize.Height + imgH; y += imgH)
            {
                g.DrawImage(grassImage, x, y + (int)grassOffsetY, width, imgH);
            }
        }

        void DrawScrollingRoad(Graphics g, int x, int width)
        {
            int imgH = roadImage.Height;
            for (int y = -imgH; y < ClientSize.Height + imgH; y += imgH)
            {
                g.DrawImage(roadImage, x, y + (int)roadOffsetY, width, imgH);
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

            var player = game.Objects.OfType<Player>().FirstOrDefault();
            if (player == null) return;

            Color labelColor = Color.Black;
            Color hudBgColor = Color.FromArgb(180, 100, 50);

            g.DrawString("FUEL", titleFont, new SolidBrush(labelColor), hudX, fuelY - 30);
            g.FillRectangle(new SolidBrush(hudBgColor), hudX, fuelY, barWidth, barHeight);
            float fuelFill = (player.Fuel / (float)player.MaxFuel) * barHeight;
            Brush fuelBrush = (player.Fuel <= 30) ? Brushes.Red : Brushes.Lime;
            g.FillRectangle(fuelBrush, hudX, fuelY + barHeight - fuelFill, barWidth, fuelFill);

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
