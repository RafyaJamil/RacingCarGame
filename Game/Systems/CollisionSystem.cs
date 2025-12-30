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

                    if (!a.Bounds.IntersectsWith(b.Bounds))
                        continue;

                    // 🔴 SPECIAL CASE: Player ↔ Enemy
                    // Only notify, NO position resolution
                    if (
                        (a is Player && b is Enemy) ||
                        (a is Enemy && b is Player)
                    )
                    {
                        a.OnCollision(b);
                        b.OnCollision(a);
                        continue; // 🚨 stop further processing
                    }

                    // ✅ Normal collision handling
                    RectangleF overlap = RectangleF.Intersect(a.Bounds, b.Bounds);

                    if (overlap.Width > 0 && overlap.Height > 0)
                    {
                        // Rigid body handling
                        if (a.IsRigidBody && !b.IsRigidBody)
                        {
                            ResolveRigid(a, b, overlap);
                        }
                        else if (b.IsRigidBody && !a.IsRigidBody)
                        {
                            ResolveRigid(b, a, overlap);
                        }
                        else
                        {
                            ResolveBoth(a, b, overlap);
                        }
                    }

                    // Notify both objects
                    a.OnCollision(b);
                    b.OnCollision(a);
                }
            }
        }

        // 🧱 One object is rigid, other is movable
        private void ResolveRigid(GameObject rigid, GameObject movable, RectangleF overlap)
        {
            if (overlap.Width < overlap.Height)
            {
                if (rigid.Position.X < movable.Position.X)
                    movable.Position = new PointF(movable.Position.X + overlap.Width, movable.Position.Y);
                else
                    movable.Position = new PointF(movable.Position.X - overlap.Width, movable.Position.Y);
            }
            else
            {
                if (rigid.Position.Y < movable.Position.Y)
                    movable.Position = new PointF(movable.Position.X, movable.Position.Y + overlap.Height);
                else
                    movable.Position = new PointF(movable.Position.X, movable.Position.Y - overlap.Height);
            }

            movable.Velocity = PointF.Empty;
        }

        // 🔄 Both movable objects
        private void ResolveBoth(GameObject a, GameObject b, RectangleF overlap)
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
