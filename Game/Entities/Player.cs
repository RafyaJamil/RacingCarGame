using Game.Core;
using Game.Component;
using Game.Audios;
using Game.Entities;
using Game.Interfaces;
using Game.Movements;
using System.Drawing;
using System.Windows.Forms;

namespace Game.Entities
{
    public class Player : GameObject
    {
        public IMovement? Movement { get; set; }
        public JumpingMovement? JumpMovement { get; set; }

        public int Fuel { get; set; } = 100;
        public int MaxFuel { get; private set; } = 100;
        public int Score { get; set; } = 0;
        public string Tag { get; set; } = "Player";

        private Image? originalSprite;

        public int framesWithoutBooster { get; set; } = 0;
        public float PushbackY { get; set; } = 0f;
        public string JumpMessage { get; set; } = "";

        public bool IsJumping => JumpMovement != null && JumpMovement.IsJumping;

        private const int JumpFrameDelay = 6;
        private Image[] jumpFrames =
{
           Properties.Resources.lifting_removebg_preview,
           Properties.Resources.inair_removebg_preview,
           Properties.Resources.lifting_removebg_preview
};

        private int jumpFrameIndex = 0;
        private int jumpFrameCounter = 0;

        private void UpdateJumpAnimation()
        {
            jumpFrameCounter++;

            if (jumpFrameCounter >= 6) 
            {
                jumpFrameIndex++;
                jumpFrameCounter = 0;

                if (jumpFrameIndex >= jumpFrames.Length)
                    jumpFrameIndex = jumpFrames.Length - 1;

                Sprite = jumpFrames[jumpFrameIndex];
            }
        }

        private void RestoreOriginalSprite()
        {
            if (originalSprite != null)
            {
                Sprite = originalSprite;
                jumpFrameIndex = 0;
                jumpFrameCounter = 0;
            }
        }

        public override void Update(GameTime gameTime)
        {
            Movement?.Move(this, gameTime);
            JumpMovement?.Move(this, gameTime);
            if (IsJumping)
            {
                UpdateJumpAnimation();  
            }
            else
            {
                RestoreOriginalSprite(); 
            }

            
            Position = new PointF(Position.X + Velocity.X, Position.Y + Velocity.Y + PushbackY);
            PushbackY *= 0.7f;
            if (System.Math.Abs(PushbackY) < 0.1f) PushbackY = 0f;

            
            if (Position.Y < 0) Position = new PointF(Position.X, 0);
            if (Position.Y > 500) Position = new PointF(Position.X, 500);

            base.Update(gameTime);
        }

        public override void OnCollision(GameObject other)
        {
            
            if (IsJumping) return;

            if (other is Enemy enemy)
            {
                Fuel -= 15;
                if (Fuel < 0)
                {
                    Fuel = 0;
                }

                    this.PushbackY = 5;
                    enemy.PushbackY = -18;

                    enemy.Velocity = new PointF(enemy.Velocity.X, enemy.Velocity.Y * 0.6f);

                    AudioManager.Play("collision");

                
                if (Fuel <= 0)
                {
                    AudioManager.Stop("bgm");
                    AudioManager.Play("crash");

                }
            }

            if (other is EnergyBooster booster)
            {
                Score += 10;
                Fuel += 20;
                if (Fuel > MaxFuel) Fuel = MaxFuel;

                framesWithoutBooster = 0;
                booster.IsActive = false;

                AudioManager.Play("energyEater");
            }
        }

        
        public void TryJump()
        {
            if (Fuel >= 50)
            {
                originalSprite ??= Sprite;
                JumpMovement?.StartJump();
                jumpFrameIndex = 0;
                jumpFrameCounter = 0;
            }
            else
            {
                JumpMessage = "Need more fuel to jump!";
                var t = new System.Windows.Forms.Timer();
                t.Interval = 1000;
                t.Tick += (s, e) =>
                {
                    JumpMessage = "";
                    t.Stop();
                };
                t.Start();
            }
        }
    }
}
