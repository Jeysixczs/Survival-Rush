using Shooting_Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooting_Game.Factory
{
    public abstract class EntityFactory
    {
        public abstract GameEntity CreateEntity();
    }
}
