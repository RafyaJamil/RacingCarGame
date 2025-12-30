using Game.Entities;
using Game.Interfaces;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace Game.Systems
{
    internal class PhysicsSystem
    {
        public float Gravity { get; set; } = 0.5f;

        public void Apply(List<GameObject> objects)
        {
            foreach (var obj in objects.OfType<IMovable>().Where(o => o.HasPhysics))
            {
                // Apply gravity to vertical velocity
                obj.Velocity = new PointF(obj.Velocity.X, obj.Velocity.Y + Gravity);

                // Update position
                if (obj is GameObject go)
                {
                    go.Position = new PointF(
                        go.Position.X + obj.Velocity.X,
                        go.Position.Y + obj.Velocity.Y
                    );
                }
            }
        }
    }
}
