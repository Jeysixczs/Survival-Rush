using Shooting_Game.Model;
using Shooting_Game.Presenter;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            this.DoubleBuffered = true;
            InitializeComponent();

            KeyDown += (s, e) =>
            {
                presenter.OnKeyDown(e.KeyCode);
            };

            KeyUp += (s, e) =>
            {
                presenter.OnKeyUp(e.KeyCode);
            };


            player = new Player
            {
                PictureBox = new PictureBox
                {
                    Size = new Size(64, 64),
                    BackColor = Color.Transparent,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Image = Properties.Resources.idledownbluebird // Must be a PNG with alpha
                }
            };

            player.PictureBox.Parent = this; // Assign parent first
            player.PictureBox.Location = new Point(200, 200);
            Controls.Add(player.PictureBox);
            player.PictureBox.BringToFront();
            player.PictureBox.Invalidate(); // Refresh
            AudioManager.PlayMusic(Properties.Resources.singlemusic, 0.5f);


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
                MainMenuForm mainMenu = new MainMenuForm();
                mainMenu.Show();
            }

            // Update ammo label
            label1.Text = $"Ammo: {ammo}";
        }


        protected override void OnLoad(EventArgs e)
        {

            presenter.SetSinglePlayerView(this);
            presenter.StartSinglePlayerGame(player);


        }

        private void SinglePlayerForm_Load(object sender, EventArgs e)
        {
            AudioManager.PlayMusic(Properties.Resources.singlemusic, 0.5f);
        }

        private void SinglePlayerForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void SinglePlayerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            AudioManager.StopMusic();

        }

        public List<PictureBox> GetWalls()
        {
            List<PictureBox> walls = new List<PictureBox>();
            foreach (Control control in this.Controls)
            {
                if (control is PictureBox pb && pb.Tag?.ToString() == "WALL")
                {
                    walls.Add(pb);
                }
            }
            return walls;
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        public Panel GetGamePanel()
        {
            throw new NotImplementedException();
        }

        public void ShowGameOver()
        {
            this.Enabled = false;

            // Show game over panel/message
            //  MessageBox.Show("Game Over!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }


    }
}
