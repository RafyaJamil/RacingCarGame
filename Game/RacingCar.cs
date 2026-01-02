using Game.Core;
using Game.Entities;
using Game.Movements;
using Game.Properties;
using Game.Systems;
using Game.Component;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Media;

namespace Game
{
    public partial class RacingCar : Form
    {
        Game.Core.Game game = new Game.Core.Game();
        PhysicsSystem physics = new PhysicsSystem();
        CollisionSystem collisions = new CollisionSystem();
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        Audio audio = new Audio();

        Image roadImage;
        Image grassImage;
        Image[] enemyCarImages;

        float laneWidth;
        float[] lanes;
        float roadX;
        float roadWidth;

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
        bool isPaused = false;
        bool isGameOver = false;
        bool isGameWin = false;

        Button pauseButton;
        Button restartButton;
        Button endGameButton;
        Button nextLevelButton;

        public RacingCar()
        {
            InitializeComponent();
            DoubleBuffered = true;

            SetupAudio();
            Player.AudioSystem = audio;// 👈 سب سے پہلے

            roadImage = Resources.Roadimage;
            grassImage = Resources.grass;

            enemyCarImages = new Image[]
            {
                Resources.pinkCar,
                Resources.blueCar,
                Resources.greenCar,
                Resources.whiteTruck
            };

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
            SetupPauseButton();
            SetupRestartButton();
            SetupEndGameButton();
            SetupNextLevelButton();
            SetupTimer();
            this.Shown += RacingCar_Shown;
        }

        private void RacingCar_Shown(object? sender, EventArgs e)
        {
            audio.PlaySound("bgm");
        }


        void SetupAudio()
        {
            audio.AddSound(new AudioTrack("bgm","Sounds/bgmusic.wav", true));
            audio.AddSound(new AudioTrack("jump", "Sounds/jump.wav", false));
            audio.AddSound(new AudioTrack("crash", "Sounds/crash.wav", false));
            audio.AddSound(new AudioTrack("win", "Sounds/win.wav", false));
            audio.AddSound(new AudioTrack("energyEater", "Sounds/energyEater.wav", false));
            audio.AddSound(new AudioTrack("collision", "Sounds/collision.wav", false));
        }


        void SetupGame()
        {
            var player = new Player
            {
                Position = new PointF(lanes[1], 420),
                Size = new Size(100, 100),
                Sprite = Resources.DriverCar
            };

            player.Movement = new KeyboardMovement();
            player.JumpMovement = new JumpingMovement(420);

            game.AddObject(player);
        }

        void SetupJumpButton()
        {
            jumpButton = new Button();
            jumpButton.Text = "JUMP";
            jumpButton.Size = new Size(120, 55);
            jumpButton.Location = new Point(ClientSize.Width - jumpButton.Width - 30,
                                            ClientSize.Height - jumpButton.Height - 40);
            jumpButton.BackColor = Color.Orange;
            jumpButton.ForeColor = Color.White;
            jumpButton.FlatStyle = FlatStyle.Flat;
            jumpButton.Font = new Font("Arial", 12, FontStyle.Bold);
            jumpButton.FlatAppearance.BorderSize = 0;
            jumpButton.Click += (s, e) =>
            {
                var player = game.Objects.OfType<Player>().FirstOrDefault();
                player?.TryJump();
                audio.PlaySound("jump");
            };
            Controls.Add(jumpButton);
        }

        void SetupPauseButton()
        {
            pauseButton = new Button();
            pauseButton.Text = "PAUSE";
            pauseButton.Size = new Size(120, 45);
            pauseButton.Location = new Point(30, 30);
            pauseButton.BackColor = Color.DarkSlateBlue;
            pauseButton.ForeColor = Color.White;
            pauseButton.FlatStyle = FlatStyle.Flat;
            pauseButton.Font = new Font("Arial", 10, FontStyle.Bold);
            pauseButton.FlatAppearance.BorderSize = 0;
            pauseButton.Click += (s, e) => TogglePause();
            Controls.Add(pauseButton);
        }

        void SetupRestartButton()
        {
            restartButton = new Button();
            restartButton.Text = "RESTART";
            restartButton.Size = new Size(110, 45);
            restartButton.BackColor = Color.DarkOrange;
            restartButton.ForeColor = Color.White;
            restartButton.FlatStyle = FlatStyle.Flat;
            restartButton.Location = new Point(30, 90);
            restartButton.Visible = false;
            restartButton.Click += (s, e) => RestartGame();
            Controls.Add(restartButton);
        }

        void SetupEndGameButton()
        {
            endGameButton = new Button();
            endGameButton.Text = "END GAME";
            endGameButton.Size = new Size(120, 45);
            endGameButton.BackColor = Color.Red;
            endGameButton.ForeColor = Color.White;
            endGameButton.FlatStyle = FlatStyle.Flat;
            endGameButton.Location = new Point(30, 150);
            endGameButton.Visible = false;
            endGameButton.Click += (s, e) => Application.Exit();
            Controls.Add(endGameButton);
        }

        void SetupNextLevelButton()
        {
            nextLevelButton = new Button();
            nextLevelButton.Text = "NEXT LEVEL";
            nextLevelButton.Size = new Size(120, 45);
            nextLevelButton.BackColor = Color.Green;
            nextLevelButton.ForeColor = Color.White;
            nextLevelButton.FlatStyle = FlatStyle.Flat;
            nextLevelButton.Location = new Point(30, 210);
            nextLevelButton.Visible = false;
            nextLevelButton.Click += (s, e) => RestartGame(); // next level logic
            Controls.Add(nextLevelButton);
        }

        void SetupTimer()
        {
            timer.Interval = 16;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        void TogglePause()
        {
            if (!isPaused)
            {
                timer.Stop();
                audio.Stop("bgm");
                isPaused = true;
                pauseButton.Text = "RESUME";
            }
            else
            {
                timer.Start();
                audio.PlaySound("bgm");
                isPaused = false;
                pauseButton.Text = "PAUSE";
            }
        }

        void RestartGame()
        {
            timer.Stop();

            game.Objects.Clear();
            enemyCounter = 0;
            boosterCounter = 0;
            grassOffsetY = 0;
            roadOffsetY = 0;

            isGameOver = false;
            isGameWin = false;
            audio.StopAll();
            audio.PlaySound("bgm");

            SetupGame();

            timer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            DrawScrollingGrass(g, 0, (int)roadX);
            DrawScrollingRoad(g, (int)roadX, (int)roadWidth);
            DrawScrollingGrass(g, (int)(roadX + roadWidth), ClientSize.Width - (int)(roadX + roadWidth));

            foreach (var obj in game.Objects)
            {
                if (obj.Sprite != null)
                    g.DrawImage(obj.Sprite, obj.Position.X - obj.Size.Width / 2,
                                       obj.Position.Y - obj.Size.Height / 2,
                                       obj.Size.Width, obj.Size.Height);
            }

            var player = game.Objects.OfType<Player>().FirstOrDefault();
            if (player != null && !string.IsNullOrEmpty(player.JumpMessage))
            {
                using (Font font = new Font("Arial", 16, FontStyle.Bold))
                using (Brush brush = new SolidBrush(Color.Yellow))
                {
                    g.DrawString(player.JumpMessage, font, brush, 50, 50);
                }
            }

            if (isGameOver)
                DrawEndScreen(g, "YOU FAILED");

            if (isGameWin)
                DrawEndScreen(g, "YOU WIN!");

            DrawHUD(g);
        }

        void DrawEndScreen(Graphics g, string message)
        {
            try
            {
                using (Brush bg = new SolidBrush(Color.FromArgb(180, 0, 0, 0)))
                using (Font font = new Font("Arial", 36, FontStyle.Bold))
                using (Brush textBrush = Brushes.White)
                {
                    g.FillRectangle(bg, 0, 0, ClientSize.Width, ClientSize.Height);
                    SizeF textSize = g.MeasureString(message, font);
                    float x = Math.Max(0, (ClientSize.Width - textSize.Width) / 2);
                    float y = Math.Max(0, (ClientSize.Height - textSize.Height) / 2 - 40);
                    g.DrawString(message, font, textBrush, x, y);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it gracefully
                Console.WriteLine("Error drawing end screen: " + ex.Message);
            }
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
                    isGameOver = true;
                    isPaused = true;
                    audio.Stop("bgm");
                    timer.Stop();
                }

                if (player.Score >= maxScore)
                {
                    isGameWin = true;
                    isPaused = true;
                    audio.Stop("bgm");
                    audio.PlaySound("win");
                    timer.Stop();
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
            collisions.Check(game.Objects.ToList());

            foreach (var booster in game.Objects.OfType<EnergyBooster>().ToList())
            {
                if (booster.Position.Y > 600) booster.IsActive = false;
            }

            // Buttons visibility
            if (restartButton != null) restartButton.Visible = isGameOver || isGameWin;
            if (endGameButton != null) endGameButton.Visible = isGameOver || isGameWin;
            if (nextLevelButton != null) nextLevelButton.Visible = isGameWin;

            game.Cleanup();
            Invalidate();
        }

        void DrawScrollingGrass(Graphics g, int x, int width)
        {
            int imgH = grassImage.Height;
            for (int y = -imgH; y < ClientSize.Height + imgH; y += imgH)
                g.DrawImage(grassImage, x, y + (int)grassOffsetY, width, imgH);
        }

        void DrawScrollingRoad(Graphics g, int x, int width)
        {
            int imgH = roadImage.Height;
            for (int y = -imgH; y < ClientSize.Height + imgH; y += imgH)
                g.DrawImage(roadImage, x, y + (int)roadOffsetY, width, imgH);
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
