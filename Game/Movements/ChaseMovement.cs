using Game.Core;
using Game.Entities;
using Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Movements
{
    internal class ChaseMovement : IMovement
    {
        public float Speed { get; set; } = 5f;
        private GameObject target;

        public ChaseMovement(GameObject target)
        {
            this.target = target;
        }

        public void Move(GameObject obj, GameTime gameTime)
        {
            float dx = target.Position.X - obj.Position.X;
            float dy = target.Position.Y - obj.Position.Y;
            float distance = (float)Math.Sqrt(dx * dx + dy * dy);
            if (distance > 0)
            {
                float dx1 = dx / distance;
                float dy1 = dy / distance;

                obj.Position = new PointF(obj.Position.X + dx1 * Speed, obj.Position.Y + dy1 * Speed);
            }
        }
    }
}
