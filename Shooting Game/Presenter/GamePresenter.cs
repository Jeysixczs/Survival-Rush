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

        private List<Zombie> zombies = new List<Zombie>();
        private Timer zombieMoveTimer;

        private List<Bullet> bullets = new List<Bullet>();
        private int zombieCount = 3; // Default for easy

        public void StartSinglePlayerGame(Player player)
        {
            player1 = player;
            singlePlayerView.UpdatePlayerStatus(player1.Health, player1.Ammo);
            SetDifficulty(GameManager.Instance.Difficulty);
            for (int i = 0; i < zombieCount; i++) SpawnZombie();
            SpawnPotion();
            SpawnAmmo();
            StartZombieChase();
        }

        public void StartTwoPlayerGame(Player p1, Player p2)
        {
            player1 = p1;
            player2 = p2;
            twoPlayerView.UpdatePlayer1Status(player1.Health, player1.Ammo);
            twoPlayerView.UpdatePlayer2Status(player2.Health, player2.Ammo);
            SetDifficulty(GameManager.Instance.Difficulty);
            for (int i = 0; i < zombieCount; i++) SpawnZombie();
            SpawnPotion();
            SpawnAmmo();
            StartZombieChase();
        }

        private void SetDifficulty(int difficulty)
        {
            switch (difficulty)
            {
                case 1: zombieCount = 3; break;   // Easy
                case 2: zombieCount = 5; break;   // Medium
                case 3: zombieCount = 10; break;  // Hard
                default: zombieCount = 3; break;
            }
        }

        private void SpawnZombie()
        {
            var zombie = new Zombie
            {
                PictureBox = new PictureBox
                {
                    Size = new Size(40, 40),
                    BackColor = Color.Black,
                    Location = new Point(new Random().Next(0, FormWidth - 40), new Random().Next(0, FormHeight - 40))
                }
            };

            zombies.Add(zombie);
            singlePlayerView?.SpawnEntity(zombie);
            twoPlayerView?.SpawnEntity(zombie);
        }

        private void StartZombieChase()
        {
            zombieMoveTimer = new Timer { Interval = 100 };
            zombieMoveTimer.Tick += (s, e) =>
            {
                foreach (var zombie in zombies.ToList())
                {
                    Player target = player1;
                    if (player2 != null)
                    {
                        double d1 = GetDistance(zombie.PictureBox.Location, player1.PictureBox.Location);
                        double d2 = GetDistance(zombie.PictureBox.Location, player2.PictureBox.Location);
                        target = d2 < d1 ? player2 : player1;
                    }

                    MoveZombieTowards(zombie, target);

                    if (zombie.PictureBox.Bounds.IntersectsWith(player1.PictureBox.Bounds))
                    {
                        player1.Health -= 1;
                        singlePlayerView?.UpdatePlayerStatus(player1.Health, player1.Ammo);
                        twoPlayerView?.UpdatePlayer1Status(player1.Health, player1.Ammo);
                    }

                    if (player2 != null && zombie.PictureBox.Bounds.IntersectsWith(player2.PictureBox.Bounds))
                    {
                        player2.Health -= 1;
                        twoPlayerView?.UpdatePlayer2Status(player2.Health, player2.Ammo);
                    }
                }

                CheckBulletZombieCollision();
            };
            zombieMoveTimer.Start();
        }

        private void CheckBulletZombieCollision()
        {
            foreach (var bullet in bullets.ToList())
            {
                foreach (var zombie in zombies.ToList())
                {
                    if (bullet.PictureBox.Bounds.IntersectsWith(zombie.PictureBox.Bounds))
                    {
                        bullets.Remove(bullet);
                        singlePlayerView?.RemoveEntity(bullet);
                        twoPlayerView?.RemoveEntity(bullet);

                        zombies.Remove(zombie);
                        singlePlayerView?.RemoveEntity(zombie);
                        twoPlayerView?.RemoveEntity(zombie);

                        Random rand = new Random();
                        if (rand.Next(100) < 50)
                        {
                            if (rand.Next(2) == 0)
                                SpawnPotion();
                            else
                                SpawnAmmo();
                        }

                        // Spawn a new zombie to maintain count
                        SpawnZombie();
                        break;
                    }
                }
            }
        }

        private void MoveZombieTowards(Zombie zombie, Player target)
        {
            int speed = 5;
            Point zPos = zombie.PictureBox.Location;
            Point tPos = target.PictureBox.Location;

            int dx = tPos.X - zPos.X;
            int dy = tPos.Y - zPos.Y;

            double length = Math.Sqrt(dx * dx + dy * dy);
            if (length == 0) return;

            int moveX = (int)(dx / length * speed);
            int moveY = (int)(dy / length * speed);

            zombie.PictureBox.Left += moveX;
            zombie.PictureBox.Top += moveY;
        }

        private double GetDistance(Point a, Point b)
        {
            int dx = a.X - b.X;
            int dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
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
    }
}
