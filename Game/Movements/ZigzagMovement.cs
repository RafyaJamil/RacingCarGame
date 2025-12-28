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
    internal class ZigzagMovement : IMovement
    {
        public float SpeedX { get; set; } = 5f;
        public float SpeedY { get; set; } = 5f;
        private float topBound;
        private float bottomBound;
        private float leftBound;
        private float rightBound;

        public ZigzagMovement(float topBound, float bottomBound, float leftBound, float rightBound)
        {
            this.topBound = topBound;
            this.bottomBound = bottomBound;
            this.leftBound = leftBound;
            this.rightBound = rightBound;
        }

        public void Move(GameObject obj, GameTime gameTime)
        {
            obj.Position = new PointF(obj.Position.X + SpeedX, obj.Position.Y + SpeedY);
            if (obj.Position.Y < topBound)
            {
                obj.Position = new PointF(obj.Position.X, topBound);
                SpeedX = Math.Abs(SpeedX);
            }
            else if (obj.Position.Y > bottomBound)
            {
                obj.Position = new PointF(obj.Position.X, bottomBound);
                SpeedX = -Math.Abs(SpeedX);
            }
            if (obj.Position.X < leftBound)
            {
                obj.Position = new PointF(leftBound, obj.Position.Y);
                SpeedY = Math.Abs(SpeedY);
            }
            else if (obj.Position.X > rightBound)
            {
                obj.Position = new PointF(rightBound, obj.Position.Y);
                SpeedY = -Math.Abs(SpeedY);
            }
        }
    }
}
