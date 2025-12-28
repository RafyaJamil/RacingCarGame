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
    internal class HorizontalMovement : IMovement
    {
        public float Speed { get; set; } = 5f;
        private float leftBound;
        private float rightBound;

        public HorizontalMovement(float leftBound, float rightBound)
        {
            this.leftBound = leftBound;
            this.rightBound = rightBound;
        }

        public void Move(GameObject obj, GameTime gameTime)
        {
            obj.Position = new PointF(obj.Position.X + Speed, obj.Position.Y);
            if (obj.Position.X < leftBound)
            {
                obj.Position = new PointF(leftBound, obj.Position.Y);
                Speed = Math.Abs(Speed);
            }
            else if (obj.Position.X > rightBound)
            {
                obj.Position = new PointF(rightBound, obj.Position.Y);
                Speed = -Math.Abs(Speed);
            }
        }
    }
}
