using Shooting_Game.Model;

namespace Shooting_Game.Factory
{
    public class AmmoFactory : EntityFactory
    {
        public override GameEntity CreateEntity() => new Ammo();
    }

}
