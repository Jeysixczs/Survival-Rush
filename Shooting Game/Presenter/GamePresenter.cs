using Shooting_Game.Factory;
using Shooting_Game.Model;
using Shooting_Game.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shooting_Game.Presenter
{
    public class GamePresenter
    {
        private IGameView singlePlayerView;
        private ITwoPlayerGameView twoPlayerView;
        private Player player1;
        private Player player2;

        private const int FormWidth = 1463;
        private const int FormHeight = 868;

        public void StartSinglePlayerGame(Player player)
        {
            player1 = player;
            singlePlayerView.UpdatePlayerStatus(player1.Health, player1.Ammo);
            SpawnZombie();
            SpawnPotion();
            SpawnAmmo();
        }

        public void StartTwoPlayerGame(Player p1, Player p2)
        {
            player1 = p1;
            player2 = p2;
            twoPlayerView.UpdatePlayer1Status(player1.Health, player1.Ammo);
            twoPlayerView.UpdatePlayer2Status(player2.Health, player2.Ammo);
            SpawnZombie();
            SpawnPotion();
            SpawnAmmo();
        }

        private void SpawnZombie()
        {
            Zombie zombie = new Zombie { PictureBox = new PictureBox { Size = new Size(40, 40), BackColor = Color.Black } };
            zombie.PictureBox.Location = new Point(100, 100);

            if (singlePlayerView != null)
                singlePlayerView.SpawnEntity(zombie);
            if (twoPlayerView != null)
                twoPlayerView.SpawnEntity(zombie);
        }

        private void SpawnPotion()
        {
            PotionFactory factory = new PotionFactory();
            GameEntity potion = factory.CreateEntity();
            potion.PictureBox = new PictureBox { Size = new Size(40, 40), BackColor = Color.Purple };
            potion.PictureBox.Location = new Point(200, 150);
            singlePlayerView?.SpawnEntity(potion);
            twoPlayerView?.SpawnEntity(potion);
        }

        private void SpawnAmmo()
        {
            AmmoFactory factory = new AmmoFactory();
            GameEntity ammo = factory.CreateEntity();
            ammo.PictureBox = new PictureBox { Size = new Size(40, 40), BackColor = Color.Orange };
            ammo.PictureBox.Location = new Point(300, 200);
            singlePlayerView?.SpawnEntity(ammo);
            twoPlayerView?.SpawnEntity(ammo);
        }

        public void SetSinglePlayerView(IGameView view)
        {
            singlePlayerView = view;
        }

        public void SetTwoPlayerView(ITwoPlayerGameView view)
        {
            twoPlayerView = view;
        }

        private List<Bullet> bullets = new List<Bullet>();

        public void HandleKeyPress(Keys key)
        {
            if (GameManager.Instance.IsSinglePlayer)
            {
                HandleSinglePlayerControls(key);
            }
            else
            {
                HandleTwoPlayerControls(key);
            }
        }

        private void HandleSinglePlayerControls(Keys key)
        {
            if (player1 == null) return;

            switch (key)
            {
                case Keys.W:
                    if (player1.PictureBox.Top > 0)
                    {
                        player1.PictureBox.Top -= 10;
                        player1.SetLastDirection(Direction.Up);
                    }
                    break;
                case Keys.S:
                    if (player1.PictureBox.Bottom + 10 < FormHeight)
                    {
                        player1.PictureBox.Top += 10;
                        player1.SetLastDirection(Direction.Down);
                    }
                    break;
                case Keys.A:
                    if (player1.PictureBox.Left > 0)
                    {
                        player1.PictureBox.Left -= 10;
                        player1.SetLastDirection(Direction.Left);
                    }
                    break;
                case Keys.D:
                    if (player1.PictureBox.Right + 10 < FormWidth)
                    {
                        player1.PictureBox.Left += 10;
                        player1.SetLastDirection(Direction.Right);
                    }
                    break;
                case Keys.Q:
                    player1.Dash(player1.GetLastDirection());
                    break;
                case Keys.E:
                    player1.SpreadShot();
                    break;
                case Keys.Space:
                    ShootBullet(player1, Direction.Up);
                    break;
            }

            singlePlayerView.UpdatePlayerStatus(player1.Health, player1.Ammo);
        }

        private void HandleTwoPlayerControls(Keys key)
        {
            if (player1 == null || player2 == null) return;

            switch (key)
            {
                case Keys.W:
                    if (player1.PictureBox.Top > 0)
                    {
                        player1.PictureBox.Top -= 10;
                        player1.SetLastDirection(Direction.Up);
                    }
                    break;
                case Keys.S:
                    if (player1.PictureBox.Bottom + 10 < FormHeight)
                    {
                        player1.PictureBox.Top += 10;
                        player1.SetLastDirection(Direction.Down);
                    }
                    break;
                case Keys.A:
                    if (player1.PictureBox.Left > 0)
                    {
                        player1.PictureBox.Left -= 10;
                        player1.SetLastDirection(Direction.Left);
                    }
                    break;
                case Keys.D:
                    if (player1.PictureBox.Right + 10 < FormWidth)
                    {
                        player1.PictureBox.Left += 10;
                        player1.SetLastDirection(Direction.Right);
                    }
                    break;
                case Keys.Q:
                    player1.Dash(player1.GetLastDirection());
                    break;
                case Keys.E:
                    player1.SpreadShot();
                    break;
                case Keys.Space:
                    ShootBullet(player1, Direction.Up);
                    break;
            }

            switch (key)
            {
                case Keys.Up:
                    if (player2.PictureBox.Top > 0)
                    {
                        player2.PictureBox.Top -= 10;
                        player2.SetLastDirection(Direction.Up);
                    }
                    break;
                case Keys.Down:
                    if (player2.PictureBox.Bottom + 10 < FormHeight)
                    {
                        player2.PictureBox.Top += 10;
                        player2.SetLastDirection(Direction.Down);
                    }
                    break;
                case Keys.Left:
                    if (player2.PictureBox.Left > 0)
                    {
                        player2.PictureBox.Left -= 10;
                        player2.SetLastDirection(Direction.Left);
                    }
                    break;
                case Keys.Right:
                    if (player2.PictureBox.Right + 10 < FormWidth)
                    {
                        player2.PictureBox.Left += 10;
                        player2.SetLastDirection(Direction.Right);
                    }
                    break;
                case Keys.ShiftKey:
                    player2.Dash(player2.GetLastDirection());
                    break;
                case Keys.ControlKey:
                    player2.SpreadShot();
                    break;
                case Keys.Enter:
                    ShootBullet(player2, Direction.Up);
                    break;
            }

            twoPlayerView.UpdatePlayer1Status(player1.Health, player1.Ammo);
            twoPlayerView.UpdatePlayer2Status(player2.Health, player2.Ammo);
        }

        private void ShootBullet(Player player, Direction direction)
        {
            if (player.Ammo <= 0) return;
            player.Ammo--;

            var bullet = new Bullet(direction)
            {
                PictureBox = new PictureBox
                {
                    Size = new Size(10, 10),
                    BackColor = Color.Yellow,
                    Location = new Point(player.PictureBox.Left + player.PictureBox.Width / 2, player.PictureBox.Top)
                },
                Direction = player.GetLastDirection()
            };

            bullets.Add(bullet);

            if (singlePlayerView != null) singlePlayerView.SpawnEntity(bullet);
            if (twoPlayerView != null) twoPlayerView.SpawnEntity(bullet);

            var timer = new Timer { Interval = 50 };
            timer.Tick += (s, e) =>
            {
                bullet.Move();
                if (bullet.PictureBox.Top < 0 ||
                    bullet.PictureBox.Left < 0 ||
                    bullet.PictureBox.Right > FormWidth ||
                    bullet.PictureBox.Bottom > FormHeight)
                {
                    timer.Stop();
                    bullets.Remove(bullet);
                    singlePlayerView?.RemoveEntity(bullet);
                    twoPlayerView?.RemoveEntity(bullet);
                }
            };
            timer.Start();
        }
    }
}
