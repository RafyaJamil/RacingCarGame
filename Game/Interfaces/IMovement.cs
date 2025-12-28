using Game.Core;
using Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Interfaces
{
    public interface IMovement
    {
        void Move(GameObject obj, GameTime gameTime);
    }
}
