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

        public ZombieState State { get; set; } = ZombieState.Alive;
        public int DeathAnimationFrame { get; set; } = 0;
        public int ZombieType { get; internal set; }

        public enum ZombieState
        {
            Alive,
            Dying,
            Dead
        }
    }
}
