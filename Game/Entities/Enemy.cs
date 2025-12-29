using Game.Core;
using Game.Extensions;
using Game.Interfaces;
using System.Drawing;

namespace Game.Entities
{
    internal class Enemy : GameObject, ICollidable
    {
        public IMovement? Movement { get; set; }

        public string Tag { get; set; } = "Enemy"; // Add Tag for collision identification

        public Enemy()
        {
            Velocity = new PointF(-2, 0);
        }

        public override void Update(GameTime gameTime)
        {
            Movement?.Move(this, gameTime);
            base.Update(gameTime);
        }

        public override void Draw(Graphics g)
        {
            if (Sprite != null)
            {
                g.DrawImage(Sprite, Bounds);
            }
            else
            {
                g.FillRectangle(Brushes.Red, Bounds);
            }
        }

        public override void OnCollision(GameObject other)
        {
            // Enemy reaction: if hit by bullet, deactivate
            if (other is Bullet)
                IsActive = false;

            // Optional: collision with player handled by Player.OnCollision
        }
    }
}
