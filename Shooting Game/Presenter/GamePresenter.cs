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


        //test
        //HandleKeyPressTwoPlayer

        // GamePresenter.cs

        //// start


        //private Dictionary<Keys, bool> player1Keys = new Dictionary<Keys, bool>();
        //private Dictionary<Keys, bool> player2Keys = new Dictionary<Keys, bool>();


        //private List<Bullet> bullets = new List<Bullet>();

        //public void HandleKeyPress(Keys key)
        //{
        //    //if (key == Keys.W || key == Keys.A || key == Keys.S || key == Keys.D)
        //    //{
        //    //    player1Keys[key] = true;  // Player 1 is pressing WASD
        //    //}
        //    //else if (key == Keys.Up || key == Keys.Left || key == Keys.Right || key == Keys.Down)
        //    //{
        //    //    player2Keys[key] = true;  // Player 2 is pressing arrow keys
        //    //}

        //    //UpdatePlayerMovement();

        //    if (player1 == null) return;

        //    switch (key)
        //    {
        //        case Keys.W: player1.PictureBox.Top -= 10; break;
        //        case Keys.S: player1.PictureBox.Top += 10; break;
        //        case Keys.A: player1.PictureBox.Left -= 10; break;
        //        case Keys.D: player1.PictureBox.Left += 10; break;
        //        case Keys.Q:
        //            if (Control.ModifierKeys == Keys.W) player1.Dash(Direction.Up);
        //            else if (Control.ModifierKeys == Keys.S) player1.Dash(Direction.Down);
        //            else if (Control.ModifierKeys == Keys.A) player1.Dash(Direction.Left);
        //            else if (Control.ModifierKeys == Keys.D) player1.Dash(Direction.Right);
        //            break;
        //        case Keys.E:
        //            player1.SpreadShot();
        //            break;
        //        case Keys.Space:
        //            ShootBullet(player1);
        //            break;
        //    }

        //    singlePlayerView.UpdatePlayerStatus(player1.Health, player1.Ammo);

        //}

        //public void HandleKeyRelease(Keys key)
        //{
        //    //if (key == Keys.W || key == Keys.A || key == Keys.S || key == Keys.D)
        //    //{
        //    //    player1Keys[key] = false;  // Player 1 released WASD
        //    //}
        //    //else if (key == Keys.Up || key == Keys.Left || key == Keys.Right || key == Keys.Down)
        //    //{
        //    //    player2Keys[key] = false;  // Player 2 released arrow keys
        //    //}

        //    //UpdatePlayerMovement();

        //    if (player1 == null || player2 == null) return;

        //    // Player 1 controls: WASD + Q/E + Space
        //    switch (key)
        //    {
        //        case Keys.W: player1.PictureBox.Top -= 10; break;
        //        case Keys.S: player1.PictureBox.Top += 10; break;
        //        case Keys.A: player1.PictureBox.Left -= 10; break;
        //        case Keys.D: player1.PictureBox.Left += 10; break;
        //        case Keys.Q:
        //            if (Control.ModifierKeys == Keys.W) player1.Dash(Direction.Up);
        //            else if (Control.ModifierKeys == Keys.S) player1.Dash(Direction.Down);
        //            else if (Control.ModifierKeys == Keys.A) player1.Dash(Direction.Left);
        //            else if (Control.ModifierKeys == Keys.D) player1.Dash(Direction.Right);
        //            break;
        //        case Keys.E:
        //            player1.SpreadShot();
        //            break;
        //        case Keys.Space:
        //            ShootBullet(player1);
        //            break;
        //    }

        //    // Player 2 controls: Arrow keys + NumPad0 (Dash), NumPad1 (Spread), Enter (Shoot)
        //    switch (key)
        //    {
        //        case Keys.Up: player2.PictureBox.Top -= 10; break;
        //        case Keys.Down: player2.PictureBox.Top += 10; break;
        //        case Keys.Left: player2.PictureBox.Left -= 10; break;
        //        case Keys.Right: player2.PictureBox.Left += 10; break;
        //        case Keys.NumPad0:
        //            if (Control.ModifierKeys == Keys.Up) player2.Dash(Direction.Up);
        //            else if (Control.ModifierKeys == Keys.Down) player2.Dash(Direction.Down);
        //            else if (Control.ModifierKeys == Keys.Left) player2.Dash(Direction.Left);
        //            else if (Control.ModifierKeys == Keys.Right) player2.Dash(Direction.Right);
        //            break;
        //        case Keys.NumPad1:
        //            player2.SpreadShot();
        //            break;
        //        case Keys.Enter:
        //            ShootBullet(player2);
        //            break;
        //    }

        //    twoPlayerView.UpdatePlayer1Status(player1.Health, player1.Ammo);
        //    twoPlayerView.UpdatePlayer2Status(player2.Health, player2.Ammo);
        //    UpdatePlayerMovement();
        //}

        //private void UpdatePlayerMovement()
        //{
        //    // Player 1 movement based on WASD keys
        //    if (player1 != null)
        //    {
        //        if (player1Keys.ContainsKey(Keys.W) && player1Keys[Keys.W]) player1.PictureBox.Top -= 10;
        //        if (player1Keys.ContainsKey(Keys.S) && player1Keys[Keys.S]) player1.PictureBox.Top += 10;
        //        if (player1Keys.ContainsKey(Keys.A) && player1Keys[Keys.A]) player1.PictureBox.Left -= 10;
        //        if (player1Keys.ContainsKey(Keys.D) && player1Keys[Keys.D]) player1.PictureBox.Left += 10;
        //    }

        //    // Player 2 movement based on arrow keys
        //    if (player2 != null)
        //    {
        //        if (player2Keys.ContainsKey(Keys.Up) && player2Keys[Keys.Up]) player2.PictureBox.Top -= 10;
        //        if (player2Keys.ContainsKey(Keys.Down) && player2Keys[Keys.Down]) player2.PictureBox.Top += 10;
        //        if (player2Keys.ContainsKey(Keys.Left) && player2Keys[Keys.Left]) player2.PictureBox.Left -= 10;
        //        if (player2Keys.ContainsKey(Keys.Right) && player2Keys[Keys.Right]) player2.PictureBox.Left += 10;
        //    }

        //    // Update the player status in the view
        //    if (twoPlayerView != null)
        //    {
        //        twoPlayerView.UpdatePlayer1Status(player1.Health, player1.Ammo);
        //        twoPlayerView.UpdatePlayer2Status(player2.Health, player2.Ammo);
        //    }
        //}

        //private void ShootBullet(Player player)
        //{
        //    if (player.Ammo <= 0) return;
        //    player.Ammo--;

        //    var bullet = new Bullet(Direction.Up) // or whatever direction you're supporting
        //    {
        //        PictureBox = new PictureBox
        //        {
        //            Size = new Size(10, 10),
        //            BackColor = Color.Yellow,
        //            Location = new Point(player.PictureBox.Left + player.PictureBox.Width / 2, player.PictureBox.Top)
        //        }
        //    };


        //    bullets.Add(bullet);

        //    if (singlePlayerView != null) singlePlayerView.SpawnEntity(bullet);
        //    if (twoPlayerView != null) twoPlayerView.SpawnEntity(bullet);

        //    var timer = new Timer { Interval = 50 };
        //    timer.Tick += (s, e) =>
        //    {
        //        bullet.Move();
        //        if (bullet.PictureBox.Top < 0)
        //        {
        //            timer.Stop();
        //            bullets.Remove(bullet);
        //            if (singlePlayerView != null) singlePlayerView.RemoveEntity(bullet);
        //            if (twoPlayerView != null) twoPlayerView.RemoveEntity(bullet);
        //        }
        //    };
        //    timer.Start();
        //}


        //end

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
                    player1.PictureBox.Top -= 10;
                    player1.SetLastDirection(Direction.Up); // Update direction
                    break;
                case Keys.S:
                    player1.PictureBox.Top += 10;
                    player1.SetLastDirection(Direction.Down); // Update direction
                    break;
                case Keys.A:
                    player1.PictureBox.Left -= 10;
                    player1.SetLastDirection(Direction.Left); // Update direction
                    break;
                case Keys.D:
                    player1.PictureBox.Left += 10;
                    player1.SetLastDirection(Direction.Right); // Update direction
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

            // Player 1 controls: WASD + Q/E + Space
            switch (key)
            {
                case Keys.W:
                    player1.PictureBox.Top -= 10;
                    player1.SetLastDirection(Direction.Up); // Update direction
                    break;
                case Keys.S:
                    player1.PictureBox.Top += 10;
                    player1.SetLastDirection(Direction.Down); // Update direction
                    break;
                case Keys.A:
                    player1.PictureBox.Left -= 10;
                    player1.SetLastDirection(Direction.Left); // Update direction
                    break;
                case Keys.D:
                    player1.PictureBox.Left += 10;
                    player1.SetLastDirection(Direction.Right); // Update direction
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


            // Player 2 controls: Arrow keys + NumPad0 (Dash), NumPad1 (Spread), Enter (Shoot)

            switch (key)
            {
                case Keys.Up:
                    player2.PictureBox.Top -= 10;
                    player2.SetLastDirection(Direction.Up); // Update direction
                    break;
                case Keys.Down:
                    player2.PictureBox.Top += 10;
                    player2.SetLastDirection(Direction.Down); // Update direction
                    break;
                case Keys.Left:
                    player2.PictureBox.Left -= 10;
                    player2.SetLastDirection(Direction.Left); // Update direction
                    break;
                case Keys.Right:
                    player2.PictureBox.Left += 10;
                    player2.SetLastDirection(Direction.Right); // Update direction
                    break;
                case Keys.ShiftKey:
                    // Dash in the last direction the player moved
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
                if (bullet.PictureBox.Top < 0)
                {
                    timer.Stop();
                    bullets.Remove(bullet);
                    if (singlePlayerView != null) singlePlayerView.RemoveEntity(bullet);
                    if (twoPlayerView != null) twoPlayerView.RemoveEntity(bullet);
                }
            };
            timer.Start();
        }



    }
}
