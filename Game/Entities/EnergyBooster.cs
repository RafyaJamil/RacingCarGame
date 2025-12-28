using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Entities
{
    internal class EnergyBooster : GameObject
    {
        public EnergyBooster()
        {
            IsActive = true;
        }
        public override void OnCollision(GameObject other)
        {
            // If player touches energy booster → disappear
            if (other is Player)
            {
                this.IsActive = false;
            }
        }
    }
}
