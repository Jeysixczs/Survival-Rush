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
    public partial class SinglePlayerForm : Form, IGameView
    {
        private GamePresenter presenter;
        private Player player;


        public void SetPresenter(GamePresenter presenter) => this.presenter = presenter;

        public SinglePlayerForm()
        {
            InitializeComponent();
            this.KeyDown += (s, ev) => presenter.HandleKeyPress(ev.KeyCode);
            // this.KeyUp += (s, ev) => presenter.HandleKeyRelease(ev.KeyCode);

            //test


        }

        public void RemoveEntity(GameEntity entity)
        {
            Controls.Remove(entity.PictureBox);
        }



        public void SpawnEntity(GameEntity entity)
        {
            Controls.Add(entity.PictureBox);
        }

        public void UpdatePlayerStatus(int health, int ammo)
        {
            this.Text = $"Health: {health} | Ammo: {ammo}";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            player = new Player { PictureBox = new PictureBox { Size = new Size(40, 40), BackColor = Color.Blue } };
            player.PictureBox.Location = new Point(200, 200);
            Controls.Add(player.PictureBox);

            presenter.SetSinglePlayerView(this);
            presenter.StartSinglePlayerGame(player);

            this.KeyDown += (s, ev) => presenter.HandleKeyPress(ev.KeyCode);
        }
    }
}
