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

        public void RemoveEntity(GameEntity entity)
        {
            Controls.Remove(entity.PictureBox);
        }



        public void SpawnEntity(GameEntity entity)
        {
            Controls.Add(entity.PictureBox);
        }

        private bool gameOver = false;

        public void UpdatePlayerStatus(int health, int ammo)
        {
            // Cap health between 0 and 100
            health = Math.Max(0, Math.Min(health, 100));

            // Update health bar
            progressBar1.Value = health;

            // Optional: Change bar color based on health (ForeColor doesn't affect default progress bar)
            // (If you're using a custom-rendered progress bar, this applies)

            // Show "Game Over" only once
            if (health == 0 && !gameOver)
            {
                gameOver = true; // prevent further game-over logic
                MessageBox.Show("Game Over!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // Optional: Disable game controls or stop the game loop here
                Close();// Close the application
            }

            // Update ammo label
            label1.Text = $"Ammo: {ammo}";
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            player = new Player
            {
                PictureBox = new PictureBox
                {
                    Size = new Size(64, 64),
                    BackColor = Color.Transparent,

                    SizeMode = PictureBoxSizeMode.Zoom
                }
            };
            player.PictureBox.Location = new Point(200, 200);
            Controls.Add(player.PictureBox);

            presenter.SetSinglePlayerView(this);
            presenter.StartSinglePlayerGame(player);


        }
    }
}
