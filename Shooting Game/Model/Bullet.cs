using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shooting_Game.Model
{
    public class Bullet : GameEntity
    {
        // public PictureBox PictureBox { get; set; }
        public Direction Direction { get; set; }
        public bool IsActive { get; set; }

        public Bullet(Direction direction)
        {
            PictureBox = new PictureBox { Size = new Size(10, 10), BackColor = Color.Yellow };
            IsActive = true;
            Direction = direction;
        }

        public void Move()
        {
            if (!IsActive) return;

            switch (Direction)
            {
                case Direction.Up: PictureBox.Top -= 10; break;
                case Direction.Down: PictureBox.Top += 10; break;
                case Direction.Left: PictureBox.Left -= 10; break;
                case Direction.Right: PictureBox.Left += 10; break;
            }
            if (PictureBox.Top < 0 || PictureBox.Left < 0 || PictureBox.Top > 868 || PictureBox.Left > 1463)
            {
                IsActive = false; // Bullet goes off-screen and is deactivated
            }


        }

        public override void OnInteract(Player player) { }
    }
}
