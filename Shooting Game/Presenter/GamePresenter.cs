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
            StartMovementTimer();
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
            StartMovementTimer();
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
            potion.PictureBox = new PictureBox
            {
                Size = new Size(40, 40),
                BackColor = Color.Purple,
                Location = new Point(new Random().Next(0, FormWidth - 40), new Random().Next(0, FormHeight - 40))
            };

            potionDrops.Add((Potion)potion);

            singlePlayerView?.SpawnEntity(potion);
            twoPlayerView?.SpawnEntity(potion);
        }

        private void SpawnAmmo()
        {
            AmmoFactory factory = new AmmoFactory();
            GameEntity ammo = factory.CreateEntity();
            ammo.PictureBox = new PictureBox
            {
                Size = new Size(40, 40),
                BackColor = Color.Orange,
                Location = new Point(new Random().Next(0, FormWidth - 40), new Random().Next(0, FormHeight - 40))
            };

            ammoDrops.Add((Ammo)ammo);

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

                        // Get the location where the zombie died
                        Point dropLocation = zombie.PictureBox.Location;

                        Random rand = new Random();
                        if (rand.Next(100) < 50)
                        {
                            if (rand.Next(2) == 0)
                                SpawnPotion(dropLocation); // Pass the zombie's location
                            else
                                SpawnAmmo(dropLocation); // Pass the zombie's location
                        }

                        // Spawn a new zombie to maintain count
                        SpawnZombie();
                        break;
                    }
                }
            }
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
                CheckPlayerPickupCollision(); // Check for player-item collisions
            };
            zombieMoveTimer.Start();
        }

        private void CheckPlayerPickupCollision()
        {
            // Player 1 - Potion Pickup
            foreach (var potion in GetPotionDrops().ToList())
            {
                if (player1.PictureBox.Bounds.IntersectsWith(potion.PictureBox.Bounds))
                {
                    if (player1.Health < 100) // Only pick up if health is not full
                    {
                        player1.Health = Math.Min(player1.Health + 20, 100); // Cap at 100
                        singlePlayerView?.UpdatePlayerStatus(player1.Health, player1.Ammo);
                        twoPlayerView?.UpdatePlayer1Status(player1.Health, player1.Ammo);

                        singlePlayerView?.RemoveEntity(potion);
                        twoPlayerView?.RemoveEntity(potion);
                        GetPotionDrops().Remove(potion);
                    }
                }
            }

            // Player 1 - Ammo Pickup
            foreach (var ammo in GetAmmoDrops().ToList())
            {
                if (player1.PictureBox.Bounds.IntersectsWith(ammo.PictureBox.Bounds))
                {
                    player1.Ammo += 10;
                    singlePlayerView?.UpdatePlayerStatus(player1.Health, player1.Ammo);
                    twoPlayerView?.UpdatePlayer1Status(player1.Health, player1.Ammo);

                    singlePlayerView?.RemoveEntity(ammo);
                    twoPlayerView?.RemoveEntity(ammo);
                    GetAmmoDrops().Remove(ammo);
                }
            }

            if (player2 != null)
            {
                // Player 2 - Potion Pickup
                foreach (var potion in GetPotionDrops().ToList())
                {
                    if (player2.PictureBox.Bounds.IntersectsWith(potion.PictureBox.Bounds))
                    {
                        if (player2.Health < 100)
                        {
                            player2.Health = Math.Min(player2.Health + 20, 100);
                            twoPlayerView?.UpdatePlayer2Status(player2.Health, player2.Ammo);

                            singlePlayerView?.RemoveEntity(potion);
                            twoPlayerView?.RemoveEntity(potion);
                            GetPotionDrops().Remove(potion);
                        }
                    }
                }

                // Player 2 - Ammo Pickup
                foreach (var ammo in GetAmmoDrops().ToList())
                {
                    if (player2.PictureBox.Bounds.IntersectsWith(ammo.PictureBox.Bounds))
                    {
                        player2.Ammo += 10;
                        twoPlayerView?.UpdatePlayer2Status(player2.Health, player2.Ammo);

                        singlePlayerView?.RemoveEntity(ammo);
                        twoPlayerView?.RemoveEntity(ammo);
                        GetAmmoDrops().Remove(ammo);
                    }
                }
            }
        }

        private List<Potion> potionDrops = new List<Potion>();
        private List<Ammo> ammoDrops = new List<Ammo>();

        private List<Potion> GetPotionDrops() => potionDrops;
        private List<Ammo> GetAmmoDrops() => ammoDrops;

        private void SpawnPotion(Point location)
        {
            PotionFactory factory = new PotionFactory();
            GameEntity potion = factory.CreateEntity();
            potion.PictureBox = new PictureBox { Size = new Size(40, 40), BackColor = Color.Purple };
            potion.PictureBox.Location = location;

            // Add potion to drop list
            potionDrops.Add((Potion)potion);

            singlePlayerView?.SpawnEntity(potion);
            twoPlayerView?.SpawnEntity(potion);
        }

        private void SpawnAmmo(Point location)
        {
            AmmoFactory factory = new AmmoFactory();
            GameEntity ammo = factory.CreateEntity();
            ammo.PictureBox = new PictureBox { Size = new Size(40, 40), BackColor = Color.Orange };
            ammo.PictureBox.Location = location;

            // Add ammo to drop list
            ammoDrops.Add((Ammo)ammo);

            singlePlayerView?.SpawnEntity(ammo);
            twoPlayerView?.SpawnEntity(ammo);
        }

        // trying to fix the moving player

        private HashSet<Keys> pressedKeys = new HashSet<Keys>();

        public void OnKeyDown(Keys key)
        {
            if (!pressedKeys.Contains(key))
            {
                pressedKeys.Add(key);

                // Handle dash and shooting on key press (not held)
                if (GameManager.Instance.IsSinglePlayer)
                {
                    HandleSinglePlayerActions(key);
                }
                else
                {
                    HandleTwoPlayerActions(key);
                }
            }
        }

        public void OnKeyUp(Keys key)
        {
            pressedKeys.Remove(key);
        }

        private Timer movementTimer;

        private void StartMovementTimer()
        {
            movementTimer = new Timer { Interval = 20 };
            movementTimer.Tick += (s, e) =>
            {
                ProcessPlayerMovement();
            };
            movementTimer.Start();
        }

        private void ProcessPlayerMovement()
        {
            int moveAmount = 5;

            if (player1 != null)
            {
                if (pressedKeys.Contains(Keys.W) && player1.PictureBox.Top > 0)
                {
                    player1.PictureBox.Top -= moveAmount;
                    player1.SetLastDirection(Direction.Up);
                }
                if (pressedKeys.Contains(Keys.S) && player1.PictureBox.Bottom + moveAmount < FormHeight)
                {
                    player1.PictureBox.Top += moveAmount;
                    player1.SetLastDirection(Direction.Down);
                }
                if (pressedKeys.Contains(Keys.A) && player1.PictureBox.Left > 0)
                {
                    player1.PictureBox.Left -= moveAmount;
                    player1.SetLastDirection(Direction.Left);
                }
                if (pressedKeys.Contains(Keys.D) && player1.PictureBox.Right + moveAmount < FormWidth)
                {
                    player1.PictureBox.Left += moveAmount;
                    player1.SetLastDirection(Direction.Right);
                }
            }

            if (player2 != null)
            {
                if (pressedKeys.Contains(Keys.Up) && player2.PictureBox.Top > 0)
                {
                    player2.PictureBox.Top -= moveAmount;
                    player2.SetLastDirection(Direction.Up);
                }
                if (pressedKeys.Contains(Keys.Down) && player2.PictureBox.Bottom + moveAmount < FormHeight)
                {
                    player2.PictureBox.Top += moveAmount;
                    player2.SetLastDirection(Direction.Down);
                }
                if (pressedKeys.Contains(Keys.Left) && player2.PictureBox.Left > 0)
                {
                    player2.PictureBox.Left -= moveAmount;
                    player2.SetLastDirection(Direction.Left);
                }
                if (pressedKeys.Contains(Keys.Right) && player2.PictureBox.Right + moveAmount < FormWidth)
                {
                    player2.PictureBox.Left += moveAmount;
                    player2.SetLastDirection(Direction.Right);
                }
            }

            // Keep UI updated
            singlePlayerView?.UpdatePlayerStatus(player1.Health, player1.Ammo);
            twoPlayerView?.UpdatePlayer1Status(player1.Health, player1.Ammo);
            if (player2 != null)
                twoPlayerView?.UpdatePlayer2Status(player2.Health, player2.Ammo);
        }

        private void HandleSinglePlayerActions(Keys key)
        {
            if (player1 == null) return;

            switch (key)
            {
                case Keys.Q:
                    player1.Dash(player1.GetLastDirection(), FormWidth, FormHeight);
                    break;
                case Keys.E:
                    player1.SpreadShot();
                    break;
                case Keys.Space:
                    ShootBullet(player1, player1.GetLastDirection());
                    break;
            }
        }

        private void HandleTwoPlayerActions(Keys key)
        {
            if (player1 == null || player2 == null) return;

            switch (key)
            {
                // Player 1
                case Keys.Q:
                    player1.Dash(player1.GetLastDirection(), FormWidth, FormHeight);
                    break;
                case Keys.E:
                    player1.SpreadShot();
                    break;
                case Keys.Space:
                    ShootBullet(player1, player1.GetLastDirection());
                    break;

                // Player 2
                case Keys.ShiftKey:
                    player2.Dash(player2.GetLastDirection(), FormWidth, FormHeight);

                    break;
                case Keys.ControlKey:
                    player2.SpreadShot();
                    break;
                case Keys.Enter:
                    ShootBullet(player2, player2.GetLastDirection());
                    break;
            }
        }

        //try


        private void ShootBullet(Player player, Direction direction)
        {
            if (player.Ammo <= 0) return;

            if (player.IsSpreadShotActive)
            {
                // Shoot 5 bullets in a spread pattern
                ShootSpreadBullets(player, direction);
                player.Ammo -= 5; // Deduct 5 ammo for the spread shot
            }
            else
            {
                // Normal single bullet
                CreateBullet(player, direction);
                player.Ammo--;
            }

            // Update UI
            singlePlayerView?.UpdatePlayerStatus(player1.Health, player1.Ammo);
            twoPlayerView?.UpdatePlayer1Status(player1.Health, player1.Ammo);
            if (player2 != null) twoPlayerView?.UpdatePlayer2Status(player2.Health, player2.Ammo);
        }

        private void ShootSpreadBullets(Player player, Direction mainDirection)
        {
            // Center bullet
            CreateBullet(player, mainDirection);

            // Calculate angles for spread bullets based on direction
            switch (mainDirection)
            {
                case Direction.Up:
                case Direction.Down:
                    // Left and right spread for vertical shots
                    CreateBullet(player, Direction.Left);
                    CreateBullet(player, Direction.Right);
                    // More angled bullets
                    CreateBullet(player, mainDirection == Direction.Up ? Direction.UpLeft : Direction.DownLeft);
                    CreateBullet(player, mainDirection == Direction.Up ? Direction.UpRight : Direction.DownRight);
                    break;
                case Direction.Left:
                case Direction.Right:
                    // Up and down spread for horizontal shots
                    CreateBullet(player, Direction.Up);
                    CreateBullet(player, Direction.Down);
                    // More angled bullets
                    CreateBullet(player, mainDirection == Direction.Left ? Direction.UpLeft : Direction.UpRight);
                    CreateBullet(player, mainDirection == Direction.Left ? Direction.DownLeft : Direction.DownRight);
                    break;
            }
        }

        private void CreateBullet(Player player, Direction direction)
        {
            var bullet = new Bullet(direction)
            {
                PictureBox = new PictureBox
                {
                    Size = new Size(10, 10),
                    BackColor = Color.Yellow,
                    Location = new Point(
                        player.PictureBox.Left + player.PictureBox.Width / 2 - 5,
                        player.PictureBox.Top + player.PictureBox.Height / 2 - 5
                    )
                },
                Direction = direction
            };

            bullets.Add(bullet);
            singlePlayerView?.SpawnEntity(bullet);
            twoPlayerView?.SpawnEntity(bullet);

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
