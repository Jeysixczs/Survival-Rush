using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooting_Game.Model
{
    public class Zombie : GameEntity
    {
        public override void OnInteract(Player player)
        {
            player.Health -= 10;

        }
        public bool IsDying { get; set; } = false;

    }
}
