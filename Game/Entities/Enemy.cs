using Game.Core;
using Game.Interfaces;
using System.Drawing;

namespace Game.Entities
{
    public class Enemy : GameObject
    {
        public IMovement? Movement { get; set; }
        public string Tag { get; set; } = "Enemy";

        // Pushback applied during collision
        public float PushbackY { get; set; } = 0f;

        public Enemy()
        {
            Velocity = new PointF(0, 5); // Normal downward speed
        }

        public override void Update(GameTime gameTime)
        {
            // Apply normal velocity + pushback
            Position = new PointF(Position.X, Position.Y + Velocity.Y + PushbackY);

            // Smoothly reduce pushback
            PushbackY *= 0.7f;
            if (Math.Abs(PushbackY) < 0.1f) PushbackY = 0f;

            // Clamp Y at bottom of screen only
            if (Position.Y > 600) IsActive = false; // Enemy naturally leaves screen here

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
            // Enemy reaction handled by Player collision
            // Do NOT deactivate enemy here
        }
    }
}
