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
