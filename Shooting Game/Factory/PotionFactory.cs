using Shooting_Game.Model;

namespace Shooting_Game.Factory
{
    public class PotionFactory : EntityFactory
    {
        public override GameEntity CreateEntity() => new Potion();
    }
}
