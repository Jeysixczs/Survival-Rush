using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooting_Game.Model
{
    public class Ammo : GameEntity
    {
        public override void OnInteract(Player player)
        {
            player.Ammo += 10;
        }
    }

}
