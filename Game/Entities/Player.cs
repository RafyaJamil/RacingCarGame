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

        // 5-second no booster counter
        public int framesWithoutBooster { get; set; } = 0;

        public override void Update(GameTime gameTime)
        {
            Movement?.Move(this, gameTime);
            base.Update(gameTime);
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Enemy enemy)
            {
                Fuel -= 30;
                if (Fuel < 0) Fuel = 0;

                // Important: prevent multiple collisions
                enemy.IsActive = false;
            }

            if (other is EnergyBooster booster)
            {
                Score += 10;
                Fuel += 20;
                if (Fuel > MaxFuel) Fuel = MaxFuel;

                // Reset framesWithoutBooster
                framesWithoutBooster = 0;

                // Prevent double collection
                booster.IsActive = false;
            }
        }

    }
}
