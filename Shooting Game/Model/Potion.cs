namespace Shooting_Game.Model
{
    public class Potion : GameEntity
    {
        public override void OnInteract(Player player)
        {
            player.Health += 20;
        }
    }
}
