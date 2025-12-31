using Game.Core;
using Game.Entities;
using Game.Interfaces;
using System.Drawing;

namespace Game.Movements
{
    public class JumpingMovement : IMovement
    {
        public float JumpStrength { get; set; } = 15f;
        public float Gravity { get; set; } = 0.5f;

        private float verticalVelocity = 0f;
        private float groundY;
        public bool IsJumping { get; private set; } = false;

        public JumpingMovement(float groundY)
        {
            this.groundY = groundY;
        }

        public void StartJump()
        {
            if (!IsJumping)
            {
                verticalVelocity = -JumpStrength;
                IsJumping = true;
            }
        }

        public void Move(GameObject obj, GameTime gameTime)
        {
            if (!IsJumping) return;

            verticalVelocity += Gravity;
            obj.Position = new PointF(obj.Position.X, obj.Position.Y + verticalVelocity);

            if (obj.Position.Y >= groundY)
            {
                obj.Position = new PointF(obj.Position.X, groundY);
                verticalVelocity = 0f;
                IsJumping = false;
            }
        }
    }
}
