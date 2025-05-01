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
            InitializeComponent();
            this.KeyDown += (s, ev) => presenter.HandleKeyPress(ev.KeyCode);
            // this.KeyUp += (s, ev) => presenter.HandleKeyRelease(ev.KeyCode);
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

        public void UpdatePlayer1Status(int health, int ammo)
        {
            this.Text = $"P1 - Health: {health} | Ammo: {ammo} | " + this.Text;
        }

        public void UpdatePlayer2Status(int health, int ammo)
        {
            this.Text += $" | P2 - Health: {health} | Ammo: {ammo}";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            player1 = new Player { PictureBox = new PictureBox { Size = new Size(40, 40), BackColor = Color.Green } };
            player2 = new Player { PictureBox = new PictureBox { Size = new Size(40, 40), BackColor = Color.Red } };
            player1.PictureBox.Location = new Point(150, 150);
            player2.PictureBox.Location = new Point(300, 150);
            Controls.Add(player1.PictureBox);
            Controls.Add(player2.PictureBox);

            presenter.SetTwoPlayerView(this);
            presenter.StartTwoPlayerGame(player1, player2);


        }
    }
}
