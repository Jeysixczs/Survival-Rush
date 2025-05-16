using System.Windows.Forms;

namespace Shooting_Game.Model
{
    public abstract class GameEntity
    {
        public PictureBox PictureBox { get; set; }
        public abstract void OnInteract(Player player);
    }
}
