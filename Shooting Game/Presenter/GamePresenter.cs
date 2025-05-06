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
using static Shooting_Game.Model.Zombie;

using NAudio.Wave;

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
            //SpawnPotion();
            //SpawnAmmo();
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
                    Size = new Size(64, 64),
                    BackColor = Color.Transparent,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Image = Properties.Resources.idledownbluebird,
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



        public void SetSinglePlayerView(IGameView view)
        {
            singlePlayerView = view;
        }

        public void SetTwoPlayerView(ITwoPlayerGameView view)
        {
            twoPlayerView = view;
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

        //drop where the zombie dies
        private void SpawnPotion(Point location)
        {
            PotionFactory factory = new PotionFactory();
            GameEntity potion = factory.CreateEntity();
            potion.PictureBox = new PictureBox
            {
                Size = new Size(40, 40),
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = Properties.Resources.antidote_potion,
                Location = location
            };

            potionDrops.Add((Potion)potion);

            singlePlayerView?.SpawnEntity(potion);
            twoPlayerView?.SpawnEntity(potion);

            // Set up a timer to remove after 3 seconds if not picked up
            Timer removalTimer = new Timer { Interval = 3000 };
            removalTimer.Tick += (s, e) =>
            {
                removalTimer.Stop();
                if (potionDrops.Contains((Potion)potion))
                {
                    potionDrops.Remove((Potion)potion);
                    singlePlayerView?.RemoveEntity(potion);
                    twoPlayerView?.RemoveEntity(potion);
                }
            };
            removalTimer.Start();
        }


        private void SpawnAmmo(Point location)
        {
            AmmoFactory factory = new AmmoFactory();
            GameEntity ammo = factory.CreateEntity();
            ammo.PictureBox = new PictureBox
            {
                Size = new Size(40, 40),
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = Properties.Resources.ammo_rifle_alt_32px,
                Location = location
            };

            ammoDrops.Add((Ammo)ammo);

            singlePlayerView?.SpawnEntity(ammo);
            twoPlayerView?.SpawnEntity(ammo);

            // Set up a timer to remove after 3 seconds if not picked up
            Timer removalTimer = new Timer { Interval = 10000 };
            removalTimer.Tick += (s, e) =>
            {
                removalTimer.Stop();
                if (ammoDrops.Contains((Ammo)ammo))
                {
                    ammoDrops.Remove((Ammo)ammo);
                    singlePlayerView?.RemoveEntity(ammo);
                    twoPlayerView?.RemoveEntity(ammo);
                }
            };
            removalTimer.Start();
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


        private int animationElapsed = 0;
        private int animationSwitchInterval = 100;

        private bool isUpGif1 = true;
        private bool isDownGif1 = true;
        private bool isLeftGif1 = true;
        private bool isRightGif1 = true;

        private bool isUpGif2 = true;
        private bool isDownGif2 = true;
        private bool isLeftGif2 = true;
        private bool isRightGif2 = true;



        private void ProcessPlayerMovement()
        {
            int moveAmount = 5;
            bool player1Moved = false;
            animationElapsed += movementTimer.Interval;

            if (player1 != null)
            {
                // Movement and animation for W
                if (pressedKeys.Contains(Keys.W) && player1.PictureBox.Top > 0)
                {
                    Rectangle proposedPosition = new Rectangle(
                        player1.PictureBox.Left,
                        player1.PictureBox.Top - moveAmount,
                        player1.PictureBox.Width,
                        player1.PictureBox.Height
                    );

                    if (!IsCollidingWithWall(proposedPosition))
                    {
                        player1.PictureBox.Top -= moveAmount;
                        player1.SetLastDirection(Direction.Up);
                        player1Moved = true;

                        if (animationElapsed >= animationSwitchInterval)
                        {
                            isUpGif1 = !isUpGif1;
                            player1.PictureBox.Image = isUpGif1 ? Properties.Resources.walkingupbluebirds : Properties.Resources.walkingup2bluebird;
                        }
                    }
                }

                // Movement and animation for S
                if (pressedKeys.Contains(Keys.S) && player1.PictureBox.Bottom + moveAmount < FormHeight)
                {
                    Rectangle proposedPosition = new Rectangle(
                        player1.PictureBox.Left,
                        player1.PictureBox.Top + moveAmount,
                        player1.PictureBox.Width,
                        player1.PictureBox.Height
                    );

                    if (!IsCollidingWithWall(proposedPosition))
                    {
                        player1.PictureBox.Top += moveAmount;
                        player1.SetLastDirection(Direction.Down);
                        player1Moved = true;

                        if (animationElapsed >= animationSwitchInterval)
                        {
                            isDownGif1 = !isDownGif1;
                            player1.PictureBox.Image = isDownGif1 ? Properties.Resources.walkingdownbluebird : Properties.Resources.walkingdown2bluebird;
                        }
                    }
                }

                // Movement and animation for A (Left)
                if (pressedKeys.Contains(Keys.A) && player1.PictureBox.Left > 0)
                {
                    Rectangle proposedPosition = new Rectangle(
                        player1.PictureBox.Left - moveAmount,
                        player1.PictureBox.Top,
                        player1.PictureBox.Width,
                        player1.PictureBox.Height
                    );

                    if (!IsCollidingWithWall(proposedPosition))
                    {
                        player1.PictureBox.Left -= moveAmount;
                        player1.SetLastDirection(Direction.Left);
                        player1Moved = true;

                        if (animationElapsed >= animationSwitchInterval)
                        {
                            isLeftGif1 = !isLeftGif1;
                            player1.PictureBox.Image = isLeftGif1 ? Properties.Resources.walkingleftbluebird : Properties.Resources.walingleft2bluebird;
                        }
                    }
                }

                // Movement and animation for D (Right)
                if (pressedKeys.Contains(Keys.D) && player1.PictureBox.Right + moveAmount < FormWidth)
                {
                    Rectangle proposedPosition = new Rectangle(
                        player1.PictureBox.Left + moveAmount,
                        player1.PictureBox.Top,
                        player1.PictureBox.Width,
                        player1.PictureBox.Height
                    );

                    if (!IsCollidingWithWall(proposedPosition))
                    {
                        player1.PictureBox.Left += moveAmount;
                        player1.SetLastDirection(Direction.Right);
                        player1Moved = true;

                        if (animationElapsed >= animationSwitchInterval)
                        {
                            isRightGif1 = !isRightGif1;
                            player1.PictureBox.Image = isRightGif1 ? Properties.Resources.walkingrightbluebird : Properties.Resources.walkinright2bluebird;
                        }
                    }
                }

                // Handle Idle Animation
                if (!player1Moved)
                {
                    switch (player1.GetLastDirection())
                    {
                        case Direction.Up:
                            player1.PictureBox.Image = Properties.Resources.idleupbluebird;
                            break;
                        case Direction.Down:
                            player1.PictureBox.Image = Properties.Resources.idledownbluebird;
                            break;
                        case Direction.Left:
                            player1.PictureBox.Image = Properties.Resources.idleleftbluebird;
                            break;
                        case Direction.Right:
                            player1.PictureBox.Image = Properties.Resources.idlerightbluebird;
                            break;
                    }
                }
            }

            // plaeyr 2


            bool player2Moved = false;
            animationElapsed += movementTimer.Interval;


            if (pressedKeys.Contains(Keys.Up) && player2.PictureBox.Top > 0)
            {
                Rectangle proposedPosition = new Rectangle(
                    player2.PictureBox.Left,
                    player2.PictureBox.Top - moveAmount,
                    player2.PictureBox.Width,
                    player2.PictureBox.Height
                );

                if (!IsCollidingWithWall(proposedPosition))
                {
                    player2.PictureBox.Top -= moveAmount;
                    player2.SetLastDirection(Direction.Up);
                    player2Moved = true;

                    if (animationElapsed >= animationSwitchInterval)
                    {
                        isUpGif2 = !isUpGif2;
                        player2.PictureBox.Image = isUpGif2 ? Properties.Resources.walkingupwhitebird : Properties.Resources.walkingup2whitebird;
                    }
                }
            }

            // Movement and animation for Down
            if (pressedKeys.Contains(Keys.Down) && player2.PictureBox.Bottom + moveAmount < FormHeight)
            {
                Rectangle proposedPosition = new Rectangle(
                    player2.PictureBox.Left,
                    player2.PictureBox.Top + moveAmount,
                    player2.PictureBox.Width,
                    player2.PictureBox.Height
                );

                if (!IsCollidingWithWall(proposedPosition))
                {
                    player2.PictureBox.Top += moveAmount;
                    player2.SetLastDirection(Direction.Down);
                    player2Moved = true;

                    if (animationElapsed >= animationSwitchInterval)
                    {
                        isDownGif2 = !isDownGif2;
                        player2.PictureBox.Image = isDownGif2 ? Properties.Resources.walkingdownwhitebird : Properties.Resources.walkingdown2whitebird;
                    }
                }
            }

            // Movement and animation for Left
            if (pressedKeys.Contains(Keys.Left) && player2.PictureBox.Left > 0)
            {
                Rectangle proposedPosition = new Rectangle(
                    player2.PictureBox.Left - moveAmount,
                    player2.PictureBox.Top,
                    player2.PictureBox.Width,
                    player2.PictureBox.Height
                );

                if (!IsCollidingWithWall(proposedPosition))
                {
                    player2.PictureBox.Left -= moveAmount;
                    player2.SetLastDirection(Direction.Left);
                    player2Moved = true;

                    if (animationElapsed >= animationSwitchInterval)
                    {
                        isLeftGif2 = !isLeftGif2;
                        player2.PictureBox.Image = isLeftGif2 ? Properties.Resources.walkingleftwhitebird : Properties.Resources.walkingleft2whitebird;
                    }
                }
            }

            // Movement and animation for Right
            if (pressedKeys.Contains(Keys.Right) && player2.PictureBox.Right + moveAmount < FormWidth)
            {
                Rectangle proposedPosition = new Rectangle(
                    player2.PictureBox.Left + moveAmount,
                    player2.PictureBox.Top,
                    player2.PictureBox.Width,
                    player2.PictureBox.Height
                );

                if (!IsCollidingWithWall(proposedPosition))
                {
                    player2.PictureBox.Left += moveAmount;
                    player2.SetLastDirection(Direction.Right);
                    player2Moved = true;

                    if (animationElapsed >= animationSwitchInterval)
                    {
                        isRightGif2 = !isRightGif2;
                        player2.PictureBox.Image = isRightGif2 ? Properties.Resources.walkingrightwhitebird : Properties.Resources.walkingright2whitebird;
                    }
                }
            }

            // Handle Idle Animation for Player 2
            if (!player2Moved)
            {
                switch (player2.GetLastDirection())
                {
                    case Direction.Up:
                        player2.PictureBox.Image = Properties.Resources.idleupwhitebird;
                        break;
                    case Direction.Down:
                        player2.PictureBox.Image = Properties.Resources.idledownwhitebird;
                        break;
                    case Direction.Left:
                        player2.PictureBox.Image = Properties.Resources.idleleftwhitebird;
                        break;
                    case Direction.Right:
                        player2.PictureBox.Image = Properties.Resources.idlerightwhitebird;
                        break;
                }
            }
            if (animationElapsed >= animationSwitchInterval)
                animationElapsed = 0;

            // Update UI
            singlePlayerView?.UpdatePlayerStatus(player1?.Health ?? 0, player1?.Ammo ?? 0);
            twoPlayerView?.UpdatePlayer1Status(player1?.Health ?? 0, player1?.Ammo ?? 0);
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
            Image bulletImage = GetBulletImageForDirection(direction);

            var bullet = new Bullet(direction)
            {
                PictureBox = new PictureBox
                {
                    Size = new Size(64, 64),
                    BackColor = Color.Transparent,
                    Image = bulletImage,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Location = new Point(
                        player.PictureBox.Left + player.PictureBox.Width / 2 - 32,
                        player.PictureBox.Top + player.PictureBox.Height / 2 - 32
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

        private Image GetBulletImageForDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Properties.Resources.fireballup;
                case Direction.Down:
                    return Properties.Resources.fireballdown;
                case Direction.Left:
                    return Properties.Resources.fireballleft;
                case Direction.Right:
                    return Properties.Resources.fireball_side_medium;
                case Direction.UpLeft:
                    return Properties.Resources.fireupleft;
                case Direction.UpRight:
                    return Properties.Resources.fireupright;
                case Direction.DownLeft:
                    return Properties.Resources.firedownleft;
                case Direction.DownRight:
                    return Properties.Resources.firedownright;
                default:
                    return Properties.Resources.fireball_side_medium; // Default image
            }

        }

        //test

        //cooldown bullet attack 

        private bool player1CanShoot = true;
        private bool player2CanShoot = true;
        private const int NORMAL_SHOT_COOLDOWN_MS = 300;  // 0.3 second for normal shots
        private const int SPREAD_SHOT_COOLDOWN_MS = 1000; // 0.5 second cooldown
        private Timer player1CooldownTimer;
        private Timer player2CooldownTimer;

        private void ShootBullet(Player player, Direction direction)
        {
            // Check if player can shoot based on cooldown
            if ((player == player1 && !player1CanShoot) ||
                (player == player2 && !player2CanShoot))
            {
                return;
            }

            if (player.Ammo <= 0) return;

            int cooldownTime = NORMAL_SHOT_COOLDOWN_MS; // Default to normal shot cooldown
            bool attemptedSpreadShot = player.IsSpreadShotActive;

            // Check if trying to do spread shot but not enough ammo
            if (attemptedSpreadShot && player.Ammo < 5)
            {

                attemptedSpreadShot = false;
            }

            if (attemptedSpreadShot)
            {
                ShootSpreadBullets(player, direction);
                player.Ammo -= 5;
                cooldownTime = SPREAD_SHOT_COOLDOWN_MS; // Use longer cooldown for spread shot
            }
            else
            {
                CreateBullet(player, direction);
                player.Ammo--;
            }

            // Start cooldown
            if (player == player1)
            {
                player1CanShoot = false;
                player1CooldownTimer = new Timer { Interval = cooldownTime };
                player1CooldownTimer.Tick += (s, e) =>
                {
                    player1CanShoot = true;
                    player1CooldownTimer.Stop();
                    player1CooldownTimer.Dispose();
                };
                player1CooldownTimer.Start();
            }
            else if (player == player2)
            {
                player2CanShoot = false;
                player2CooldownTimer = new Timer { Interval = cooldownTime };
                player2CooldownTimer.Tick += (s, e) =>
                {
                    player2CanShoot = true;
                    player2CooldownTimer.Stop();
                    player2CooldownTimer.Dispose();
                };
                player2CooldownTimer.Start();
            }

            // Update UI
            singlePlayerView?.UpdatePlayerStatus(player1.Health, player1.Ammo);
            twoPlayerView?.UpdatePlayer1Status(player1.Health, player1.Ammo);
            if (player2 != null) twoPlayerView?.UpdatePlayer2Status(player2.Health, player2.Ammo);
        }

        //try dead annimation

        private async void StartZombieDeathAnimation(Zombie zombie)
        {
            zombie.State = ZombieState.Dying;

            await Task.Run(() =>
            {
                System.Media.SoundPlayer explosionSound = new System.Media.SoundPlayer(Properties.Resources.explosion);
                explosionSound.Play();
            });

            // Set the initial size for death animation (make it bigger than regular zombie)
            zombie.PictureBox.Size = new Size(200, 200); // Adjust these values as needed
            zombie.PictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            zombie.PictureBox.Location = new Point(zombie.PictureBox.Left - 64, zombie.PictureBox.Top - 50); // Center the explosion

            // Change to first frame of death animation
            zombie.PictureBox.Image = Properties.Resources.fireball_side_medium_explode;

            var deathTimer = new Timer { Interval = 50 };
            deathTimer.Tick += (s, e) =>
            {
                zombie.DeathAnimationFrame++;

                // Update animation frame
                switch (zombie.DeathAnimationFrame)
                {
                    case 1:
                        zombie.PictureBox.Image = Properties.Resources.fireball_side_medium_explode;
                        break;

                    case 2:
                        // Animation complete
                        deathTimer.Stop();
                        zombie.State = ZombieState.Dead;

                        // Get the location where the zombie died
                        Point dropLocation = zombie.PictureBox.Location;

                        // Remove the zombie
                        zombies.Remove(zombie);
                        singlePlayerView?.RemoveEntity(zombie);
                        twoPlayerView?.RemoveEntity(zombie);

                        // Random drop
                        Random rand = new Random();
                        if (rand.Next(100) < 50)
                        {
                            if (rand.Next(2) == 0)
                                SpawnPotion(dropLocation);
                            else
                                SpawnAmmo(dropLocation);
                        }

                        // Spawn a new zombie to maintain count
                        SpawnZombie();
                        break;
                }
            };
            deathTimer.Start();
        }

        private void StartZombieChase()
        {
            zombieMoveTimer = new Timer { Interval = 100 };
            zombieMoveTimer.Tick += (s, e) =>
            {
                foreach (var zombie in zombies.ToList())
                {
                    // Skip if zombie is dying or dead
                    if (zombie.State != ZombieState.Alive) continue;

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
                CheckPlayerPickupCollision();
            };
            zombieMoveTimer.Start();
        }

        private void CheckBulletZombieCollision()
        {
            foreach (var bullet in bullets.ToList())
            {
                foreach (var zombie in zombies.ToList())
                {
                    if (zombie.State == ZombieState.Alive &&
                        bullet.PictureBox.Bounds.IntersectsWith(zombie.PictureBox.Bounds))
                    {
                        bullets.Remove(bullet);
                        singlePlayerView?.RemoveEntity(bullet);
                        twoPlayerView?.RemoveEntity(bullet);

                        // Start dying animation instead of immediately removing
                        StartZombieDeathAnimation(zombie);
                        break;
                    }
                }
            }
        }



        // add wals
        private bool IsCollidingWithWall(Rectangle bounds)
        {
            // Get all PictureBox controls with "WALL" tag from the form
            var walls = singlePlayerView?.GetWalls() ?? twoPlayerView?.GetWalls();
            if (walls == null) return false;

            foreach (var wall in walls)
            {
                if (wall.Bounds.IntersectsWith(bounds))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
