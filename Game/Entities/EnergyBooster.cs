using Game.Interfaces;
using System.Drawing;

namespace Game.Entities
{
    internal class EnergyBooster : GameObject, ICollidable
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
                // Remove Score increment here
                // Fuel boost handled in Player.OnCollision
                this.IsActive = false; // Booster disappears
            }
        }
    }
}
