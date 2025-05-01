using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shooting_Game.Model
{
    public class Player
    {
        public int Health { get; set; } = 100;
        public int Ammo { get; set; } = 30;
        public PictureBox PictureBox { get; set; }

        private bool canDash = true;
        private bool canSpread = true;

        private Direction lastDirection;



        public void Dash(Direction direction)
        {
            if (!canDash) return;

            // Perform the flicker effect (move quickly in the given direction)
            switch (direction)
            {
                case Direction.Up:
                    PictureBox.Top -= 50;
                    break;
                case Direction.Down:
                    PictureBox.Top += 50;
                    break;
                case Direction.Left:
                    PictureBox.Left -= 50;
                    break;
                case Direction.Right:
                    PictureBox.Left += 50;
                    break;
            }

            canDash = false;
            Task.Delay(200).ContinueWith(_ => canDash = true); // Reset dash cooldown
        }

        // Method to update the last direction based on movement
        public void SetLastDirection(Direction direction)
        {
            lastDirection = direction;
        }

        // Get the last direction
        public Direction GetLastDirection()
        {
            return lastDirection;
        }
        public void SpreadShot()
        {
            if (!canSpread) return;
            canSpread = false;
            // Shoot multiple bullets for 5 seconds
            Task.Delay(5000).ContinueWith(_ =>
            {
                // End spread shot
                Task.Delay(10000).ContinueWith(__ => canSpread = true);
            });
        }
    }
    public enum Direction { Up, Down, Left, Right }
}
