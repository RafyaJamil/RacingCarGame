using EZInput;
using Game.Core;
using Game.Entities;
using Game.Interfaces;
using System.Drawing;

namespace Game.Movements
{
    internal class KeyboardMovement : IMovement
    {
        public float Speed { get; set; } = 5f;

        public void Move(GameObject obj, GameTime gameTime)
        {
            float vx = 0;

            if (Keyboard.IsKeyPressed(Key.LeftArrow)) vx = -Speed;
            if (Keyboard.IsKeyPressed(Key.RightArrow)) vx = Speed;

            // Horizontal velocity from keyboard, vertical preserve (knockback)
            obj.Velocity = new PointF(vx, obj.Velocity.Y);
        }
    }
}
