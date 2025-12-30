using Game.Core;
using Game.Entities;
using Game.Interfaces;
using System.Security.Policy;

namespace Game.Entities
{
    public class Enemy : GameObject, ICollidable
    {
        public IMovement? Movement { get; set; }
        public string Tag { get; set; } = "Enemy";

        // Temporary pushback applied during collision
        public float PushbackY { get; set; } = 0f;

        public Enemy()
        {
            Velocity = new PointF(0, 5); // normal downward speed
        }

        public override void Update(GameTime gameTime)
        {
            // Apply normal velocity + pushback
            Position = new PointF(Position.X, Position.Y + Velocity.Y + PushbackY);

            // Slowly reduce pushback to zero
            PushbackY *= 0.7f;
            if (Math.Abs(PushbackY) < 0.1f) PushbackY = 0f;

            // Clamp Y within road
            if (Position.Y < 0) Position = new PointF(Position.X, 0);
            if (Position.Y > 520) Position = new PointF(Position.X, 520);

            base.Update(gameTime);
        }

        public override void Draw(Graphics g)
        {
            if (Sprite != null)
                g.DrawImage(Sprite, Bounds);
            else
                g.FillRectangle(Brushes.Red, Bounds);
        }

        public override void OnCollision(GameObject other)
        {
            // Enemy itself does nothing; push handled in Player collision
        }
    }
}

