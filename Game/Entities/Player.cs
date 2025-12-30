using Game.Core;
using Game.Interfaces;
using System.Drawing;

namespace Game.Entities
{
    public class Player : GameObject, ICollidable
    {
        public IMovement? Movement { get; set; }

        public int Fuel { get; set; } = 100;
        public int Score { get; set; } = 0;
        public int MaxFuel { get; private set; } = 100;

        public string Tag { get; set; } = "Player";

        public int framesWithoutBooster { get; set; } = 0;

        // Temporary pushback
        public float PushbackY { get; set; } = 0f;

        public override void Update(GameTime gameTime)
        {
            Movement?.Move(this, gameTime);

            // Apply velocity + pushback
            Position = new PointF(Position.X + Velocity.X, Position.Y + Velocity.Y + PushbackY);

            // Slowly reduce pushback to zero
            PushbackY *= 0.7f;
            if (Math.Abs(PushbackY) < 0.1f) PushbackY = 0f;

            // Clamp position within road
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

                // Apply smooth pushback
                this.PushbackY = 5;        // player moves slightly down
                enemy.PushbackY = -10;     // enemy moves slightly up
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
    }

}
