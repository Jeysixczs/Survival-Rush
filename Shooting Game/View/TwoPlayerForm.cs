using Shooting_Game.Model;
using Shooting_Game.Presenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shooting_Game.View
{
    public partial class TwoPlayerForm : Form, ITwoPlayerGameView
    {
        public TwoPlayerForm()
        {
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            InitializeComponent();


            KeyDown += (s, e) =>
            {
                presenter.OnKeyDown(e.KeyCode);
            };

            KeyUp += (s, e) =>
            {
                presenter.OnKeyUp(e.KeyCode);
            };





        }

        private GamePresenter presenter;
        private Player player1, player2;


        public void SetPresenter(GamePresenter presenter) => this.presenter = presenter;


        public void RemoveEntity(GameEntity entity)
        {
            Controls.Remove(entity.PictureBox);
        }



        public void SpawnEntity(GameEntity entity)
        {
            Controls.Add(entity.PictureBox);
        }



        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            player1 = new Player { PictureBox = new PictureBox { Size = new Size(40, 40), BackColor = Color.Black } };
            player2 = new Player { PictureBox = new PictureBox { Size = new Size(40, 40), BackColor = Color.Red } };
            player1.PictureBox.Location = new Point(150, 150);
            player2.PictureBox.Location = new Point(300, 150);
            Controls.Add(player1.PictureBox);
            Controls.Add(player2.PictureBox);

            presenter.SetTwoPlayerView(this);
            presenter.StartTwoPlayerGame(player1, player2);


        }
        //test
        private bool player1Dead = false;
        private bool player2Dead = false;

        public void UpdatePlayer1Status(int health, int ammo)
        {
            health = Math.Max(0, Math.Min(health, 100));
            progressBar1.Value = health;
            label1.Text = $"P1 Ammo: {ammo}";

            if (health == 0 && !player1Dead)
            {
                player1Dead = true;
                MessageBox.Show("Player 1 is dead!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CheckBothPlayersDead();
            }
        }

        public void UpdatePlayer2Status(int health, int ammo)
        {
            health = Math.Max(0, Math.Min(health, 100));
            progressBar2.Value = health;
            label2.Text = $"P2 Ammo: {ammo}";

            if (health == 0 && !player2Dead)
            {
                player2Dead = true;
                MessageBox.Show("Player 2 is dead!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CheckBothPlayersDead();
            }
        }

        private void CheckBothPlayersDead()
        {
            if (player1Dead && player2Dead)
            {
                MessageBox.Show("Both players are dead! Game Over.", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Closes the game window
            }
        }

    }
}
