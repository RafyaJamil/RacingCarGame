using Game.Interfaces;
using System.Drawing;

namespace Game.Entities
{
    internal class EnergyBooster : GameObject
    {
        public string Tag { get; set; } = "Booster";

        public EnergyBooster()
        {
            IsActive = true;
        }

        public override void OnCollision(GameObject other)
        {
            if (other is Player player)
            {
                this.IsActive = false;
            }
        }
    }
}
