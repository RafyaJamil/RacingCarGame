using Game.Core;
using Game.Entities;
using Game.Interfaces;
using System.Drawing;

namespace Game.Movements
{
    public class JumpingMovement : IMovement
    {
        public float JumpStrength { get; set; } = 10f;
        public float Gravity { get; set; } = 0.5f;

        private float verticalVelocity = 0f;
        private float groundY;
        private bool isJumping = false;

        public JumpingMovement(float groundY)
        {
            this.groundY = groundY;
        }

        public void StartJump()
        {
            if (!isJumping)
            {
                verticalVelocity = -JumpStrength;
                isJumping = true;
            }
        }

        public void Move(GameObject obj, GameTime gameTime)
        {
            verticalVelocity += Gravity;
            obj.Position = new PointF(obj.Position.X, obj.Position.Y + verticalVelocity);

            if (obj.Position.Y >= groundY)
            {
                obj.Position = new PointF(obj.Position.X, groundY);
                verticalVelocity = 0f;
                isJumping = false;
            }
        }
    }
}
