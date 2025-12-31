using EZInput;
using Game.Core;
using Game.Entities;
using Game.Interfaces;
using System.Drawing;

namespace Game.Movements
{
    public class KeyboardMovement : IMovement
    {
        public float Speed { get; set; } = 5f;

        public void Move(GameObject obj, GameTime gameTime)
        {
            // Left/Right movement
            if (Keyboard.IsKeyPressed(Key.LeftArrow))
                obj.Position = new PointF(obj.Position.X - Speed, obj.Position.Y);

            if (Keyboard.IsKeyPressed(Key.RightArrow))
                obj.Position = new PointF(obj.Position.X + Speed, obj.Position.Y);

            // Forward/Backward (UP/DOWN) without jumping
            if (Keyboard.IsKeyPressed(Key.UpArrow))
                obj.Position = new PointF(obj.Position.X, obj.Position.Y - Speed);

            if (Keyboard.IsKeyPressed(Key.DownArrow))
                obj.Position = new PointF(obj.Position.X, obj.Position.Y + Speed);
        }
    }
}
