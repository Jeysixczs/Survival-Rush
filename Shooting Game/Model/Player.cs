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



        public void Dash(Direction direction, int formWidth, int formHeight)
        {
            if (!canDash) return;

            int dashDistance = 50;

            int newX = PictureBox.Left;
            int newY = PictureBox.Top;

            switch (direction)
            {
                case Direction.Up:
                    newY -= dashDistance;
                    break;
                case Direction.Down:
                    newY += dashDistance;
                    break;
                case Direction.Left:
                    newX -= dashDistance;
                    break;
                case Direction.Right:
                    newX += dashDistance;
                    break;
            }

            // Clamp the position to stay inside the form bounds
            newX = Math.Max(0, Math.Min(newX, formWidth - PictureBox.Width));
            newY = Math.Max(0, Math.Min(newY, formHeight - PictureBox.Height));

            PictureBox.Location = new System.Drawing.Point(newX, newY);

            canDash = false;
            Task.Delay(200).ContinueWith(_ => canDash = true); // Cooldown
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


        private bool isSpreadShotActive = false;
        private DateTime spreadShotEndTime;

        public bool IsSpreadShotActive => isSpreadShotActive && DateTime.Now < spreadShotEndTime;

        public void SpreadShot()
        {
            if (!canSpread) return;

            isSpreadShotActive = true;
            spreadShotEndTime = DateTime.Now.AddSeconds(5);
            canSpread = false;

            Task.Delay(5000).ContinueWith(_ =>
            {
                isSpreadShotActive = false;
                Task.Delay(10000).ContinueWith(__ => canSpread = true); // 10 second cooldown
            });
        }
    }
    public enum Direction { Up, Down, Left, Right, UpLeft, UpRight, DownLeft, DownRight }
}
