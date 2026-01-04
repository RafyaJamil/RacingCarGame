using Game.Core;
using Game.Entities;
using Game.Movements;
using Game.Properties;
using Game.Systems;
using Game.Component;
using Game.Audios;
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
        Button backToMenuButton;


        bool showEndMessage = false;
        string endMessageText = "";
        bool endMessageActive = false;
        Label levelLabel;


        public RacingCar()
        {
            InitializeComponent();
            DoubleBuffered = true;
            SetupLevelLabel();

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
            SetupBackToMenuButton();
            SetupTimer();
            AudioManager.StopAll();   // pehle sab band
            AudioManager.Play("bgm"); // level 2 ka music start
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
                AudioManager.Play("jump");
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
            nextLevelButton.Click += (s, e) =>
            {

                this.Hide();   // Level 1 form hide
                Level2Form level2 = new Level2Form();
                level2.Show();


            };

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
                isPaused = true;
                pauseButton.Text = "RESUME";
            }
            else
            {
                timer.Start();
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
            showEndMessage = false;   // ⭐⭐⭐ IMPORTANT
            endMessageText = "";
            endMessageActive = false;

            restartButton.Visible = false;
            endGameButton.Visible = false;
            nextLevelButton.Visible = false;
            backToMenuButton.Visible = false;



            SetupGame();
            AudioManager.StopAll();     // pehle sab band
            AudioManager.Play("bgm");
            timer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            DrawScrollingGrass(g, 0, (int)roadX);
            DrawScrollingRoad(g, (int)roadX, (int)roadWidth);
            DrawScrollingGrass(g, (int)(roadX + roadWidth), ClientSize.Width - (int)(roadX + roadWidth));
            // 🔹 pehle saare objects EXCEPT player
            foreach (var obj in game.Objects)
            {
                if (obj is Player) continue;

                if (obj.Sprite != null)
                    e.Graphics.DrawImage(obj.Sprite,
                        obj.Position.X - obj.Size.Width / 2,
                        obj.Position.Y - obj.Size.Height / 2,
                        obj.Size.Width,
                        obj.Size.Height);
            }

            // 🔹 phir player LAST mein (jump ke upar dikhane ke liye)
            var player = game.Objects.OfType<Player>().FirstOrDefault();
            if (player != null && player.Sprite != null)
            {
                e.Graphics.DrawImage(player.Sprite,
                    player.Position.X - player.Size.Width / 2,
                    player.Position.Y - player.Size.Height / 2,
                    player.Size.Width,
                    player.Size.Height);
            }

            var playerMsg = game.Objects.OfType<Player>().FirstOrDefault();
            if (playerMsg != null && !string.IsNullOrEmpty(playerMsg.JumpMessage))
            {
                string msg = playerMsg.JumpMessage;

                Font msgFont = new Font("Segoe UI", 22, FontStyle.Bold);

                Size textSize = TextRenderer.MeasureText(msg, msgFont);

                int x = (ClientSize.Width - textSize.Width) / 2;
                int y = (ClientSize.Height / 2) - 100; // thora upar center se

                TextRenderer.DrawText(
                    e.Graphics,
                    msg,
                    msgFont,
                    new Point(x, y),
                    Color.Red
                );
            }



            DrawHUD(g);

            if (showEndMessage && endMessageActive)
            {
                DrawEndScreen(g, endMessageText);
            }


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

            if (showEndMessage && endMessageActive)
            {
                Invalidate();   // 🔥 har frame redraw
                return;         // game logic stop
            }

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

                if (player.Fuel <= 0 && !endMessageActive)
                {
                    isGameOver = true;
                    showEndMessage = true;
                    endMessageText = "YOU FAILED";

                    endMessageActive = true; // 
                    timer.Stop();
                    AudioManager.Stop("bgm");
                    AudioManager.Play("crash");
                    restartButton.Visible = true;
                    endGameButton.Visible = true;
                    nextLevelButton.Visible = false;
                    backToMenuButton.Visible = true;
                    Invalidate();
                }

                if (player.Score >= maxScore && !endMessageActive)
                {
                    isGameWin = true;
                    showEndMessage = true;
                    endMessageText = "YOU WIN!";
                    endMessageActive = true;
                    timer.Stop();
                    AudioManager.Stop("bgm");
                    AudioManager.Play("win");
                    restartButton.Visible = true;
                    endGameButton.Visible = true;
                    nextLevelButton.Visible = true;
                    backToMenuButton.Visible = true;
                    Invalidate();
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

        void SetupLevelLabel()
        {
            levelLabel = new Label();
            levelLabel.Text = "LEVEL 1";
            levelLabel.Font = new Font("Segoe UI", 28, FontStyle.Bold); // stylish aur badi font
            levelLabel.ForeColor = Color.Gold; // bright aur visible color
            levelLabel.BackColor = Color.Transparent; // background transparent
            levelLabel.AutoSize = true;

            // top center position
            levelLabel.Location = new Point(
                (ClientSize.Width - levelLabel.PreferredWidth) / 2,
                20 // upar thoda space
            );

            Controls.Add(levelLabel);
        }

        private void RacingCar_Load(object sender, EventArgs e)
        {

        }

        void SetupBackToMenuButton()
        {
            backToMenuButton = new Button();
            backToMenuButton.Text = "MAIN MENU";
            backToMenuButton.Size = new Size(140, 45);
            backToMenuButton.BackColor = Color.MediumPurple;
            backToMenuButton.ForeColor = Color.White;
            backToMenuButton.FlatStyle = FlatStyle.Flat;
            backToMenuButton.Location = new Point(30, 270); // End Game کے نیچے
            backToMenuButton.Visible = false;

            backToMenuButton.Click += (s, e) =>
            {
                AudioManager.StopAll();

                SelectForm menu = new SelectForm();
                menu.Show();

                this.Close(); // current level بند
            };

            Controls.Add(backToMenuButton);
        }

    }
}
