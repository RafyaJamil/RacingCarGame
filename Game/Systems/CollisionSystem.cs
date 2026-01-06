using Game.Entities;
using Game.Interfaces;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Game.Systems
{
    internal class CollisionSystem
    {
        public void Check(List<GameObject> objects)
        {
            var collidables = objects.OfType<ICollidable>().ToList();

            for (int i = 0; i < collidables.Count; i++)
            {
                for (int j = i + 1; j < collidables.Count; j++)
                {
                    var a = (GameObject)collidables[i];
                    var b = (GameObject)collidables[j];

                    if ((a is Player p1 && p1.IsJumping) || (b is Player p2 && p2.IsJumping))
                        continue;

                    if (!a.Bounds.IntersectsWith(b.Bounds))
                        continue;

                    a.OnCollision(b);
                    b.OnCollision(a);
                    RectangleF overlap = RectangleF.Intersect(a.Bounds, b.Bounds);
                    if (overlap.Width > 0 && overlap.Height > 0)
                        ResolveOverlap(a, b, overlap);
                }
            }
        }

        private void ResolveOverlap(GameObject a, GameObject b, RectangleF overlap)
        {
            if (overlap.Width < overlap.Height)
            {
                float sep = overlap.Width / 2f;
                if (a.Position.X < b.Position.X)
                {
                    a.Position = new PointF(a.Position.X - sep, a.Position.Y);
                    b.Position = new PointF(b.Position.X + sep, b.Position.Y);
                }
                else
                {
                    a.Position = new PointF(a.Position.X + sep, a.Position.Y);
                    b.Position = new PointF(b.Position.X - sep, b.Position.Y);
                }
            }
            else
            {
                float sep = overlap.Height / 2f;
                if (a.Position.Y < b.Position.Y)
                {
                    a.Position = new PointF(a.Position.X, a.Position.Y - sep);
                    b.Position = new PointF(b.Position.X, b.Position.Y + sep);
                }
                else
                {
                    a.Position = new PointF(a.Position.X, a.Position.Y + sep);
                    b.Position = new PointF(b.Position.X, b.Position.Y - sep);
                }
            }
        }
    }
}
