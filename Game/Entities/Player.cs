using Game.Core;
using Game.Interfaces;
using Game.Movements;
using System.Drawing;

namespace Game.Entities
{
    public class Player : GameObject, ICollidable
    {
        public IMovement? Movement { get; set; }
        public JumpingMovement? JumpMovement { get; set; }

        public int Fuel { get; set; } = 100;
        public int MaxFuel { get; private set; } = 100;
        public int Score { get; set; } = 0;
        public string Tag { get; set; } = "Player";

        public int framesWithoutBooster { get; set; } = 0;
        public float PushbackY { get; set; } = 0f;
        public string JumpMessage { get; set; } = "";

        public override void Update(GameTime gameTime)
        {
            // Keyboard movement
            Movement?.Move(this, gameTime);

            // Jump movement
            JumpMovement?.Move(this, gameTime);

            // Apply pushback
            Position = new PointF(Position.X + Velocity.X, Position.Y + Velocity.Y + PushbackY);
            PushbackY *= 0.7f;
            if (Math.Abs(PushbackY) < 0.1f) PushbackY = 0f;

            // Clamp vertical position
            if (Position.Y < 0) Position = new PointF(Position.X, 0);
            if (Position.Y > 500) Position = new PointF(Position.X, 500);

            base.Update(gameTime);
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Enemy enemy)
            {
                Fuel -= 20;
                if (Fuel < 0) Fuel = 0;

                this.PushbackY = 5;
                enemy.PushbackY = -10;

                enemy.Velocity = new PointF(enemy.Velocity.X, enemy.Velocity.Y * 0.6f);
            }

            if (other is EnergyBooster booster)
            {
                Score += 10;
                Fuel += 20;
                if (Fuel > MaxFuel) Fuel = MaxFuel;
                framesWithoutBooster = 0;
                booster.IsActive = false;
            }
        }

        public void TryJump()
        {
            if (Fuel >= MaxFuel)
            {
                JumpMovement?.StartJump();
            }
            else
            {
                JumpMessage = "You can only jump when fuel is full!";
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
