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
    internal class VerticalMovement : IMovement
    {
        public float Speed { get; set; } = 5f;
        private float topBound;
        private float bottomBound;

        public VerticalMovement(float topBound, float bottomBound)
        {
            this.topBound = topBound;
            this.bottomBound = bottomBound;
        }

        public void Move(GameObject obj, GameTime gameTime)
        {
            obj.Position = new PointF(obj.Position.X, obj.Position.Y + Speed);
            if (obj.Position.Y < topBound)
            {
                obj.Position = new PointF(obj.Position.X, topBound);
                Speed = Math.Abs(Speed);
            }
            else if (obj.Position.Y > bottomBound)
            {
                obj.Position = new PointF(obj.Position.X, bottomBound);
                Speed = -Math.Abs(Speed);
            }
        }
    }
}
