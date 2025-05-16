using Shooting_Game.Presenter;
using Shooting_Game.View;
using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace Shooting_Game
{
    public partial class MainMenuForm : Form, IMainMenu
    {
        private MainMenuPresenter presenter;
        public MainMenuForm()
        {
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            InitializeComponent();
            presenter = new MainMenuPresenter(this);
            SetPresenter(presenter);

        }

        public void SetPresenter(MainMenuPresenter presenter)
        {
            this.presenter = presenter;
        }





        public void ShowGameForm()
        {

            var form2 = new SinglePlayerForm();
            var gamePresenter = new GamePresenter();
            form2.SetPresenter(gamePresenter);
            form2.Show();
        }

        public void ShowTwoPlayerForm()
        {
            var form3 = new TwoPlayerForm();
            var gamePresenter = new GamePresenter();
            form3.SetPresenter(gamePresenter);
            form3.Show();
        }



        private void btnSinglePlayer_Click(object sender, EventArgs e)
        {
            clicksound();
            presenter?.SelectSinglePlayer();
            btnSinglePlayer.Enabled = false;
            btnTwoPlayer.Enabled = true;

            using (Graphics g = btnSinglePlayer.CreateGraphics())
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(30, Color.Gray))) // 50% opacity
                {
                    g.FillRectangle(brush, btnSinglePlayer.ClientRectangle);
                }
            }

        }


        private void btnTwoPlayer_Click(object sender, EventArgs e)
        {
            clicksound();
            presenter?.SelectTwoPlayer();
            btnTwoPlayer.Enabled = false;
            btnSinglePlayer.Enabled = true;


            using (Graphics g = btnTwoPlayer.CreateGraphics())
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(30, Color.Gray))) // 50% opacity
                {
                    g.FillRectangle(brush, btnSinglePlayer.ClientRectangle);
                }
            }
        }

        private void btnDifficultyEasy_Click(object sender, EventArgs e)
        {
            clicksound();
            presenter?.SelectDifficulty(1);
            btnDifficultyEasy.Enabled = false;
            btnDifficultyMedium.Enabled = true;
            btnDifficultyHard.Enabled = true;
            using (Graphics g = btnDifficultyEasy.CreateGraphics())
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(80, Color.Gray))) // 50% opacity
                {
                    g.FillRectangle(brush, btnSinglePlayer.ClientRectangle);
                }
            }

        }

        private void btnDifficultyMedium_Click(object sender, EventArgs e)
        {
            clicksound();
            presenter?.SelectDifficulty(2);
            btnDifficultyEasy.Enabled = true;
            btnDifficultyMedium.Enabled = false;
            btnDifficultyHard.Enabled = true;

            using (Graphics g = btnDifficultyMedium.CreateGraphics())
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(80, Color.Gray))) // 50% opacity
                {
                    g.FillRectangle(brush, btnSinglePlayer.ClientRectangle);
                }
            }
        }

        private void btnDifficultyHard_Click(object sender, EventArgs e)
        {
            clicksound();
            presenter?.SelectDifficulty(3);
            btnDifficultyEasy.Enabled = true;
            btnDifficultyMedium.Enabled = true;
            btnDifficultyHard.Enabled = false;
            using (Graphics g = btnDifficultyHard.CreateGraphics())
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(80, Color.Gray))) // 50% opacity
                {
                    g.FillRectangle(brush, btnSinglePlayer.ClientRectangle);
                }
            }

        }

        private void pictureBoxStart_Click(object sender, EventArgs e)
        {
            clicksound();
            AudioManager.StopMusic();
            presenter.StartGame();
            this.Hide();

            using (Graphics g = pictureBoxStart.CreateGraphics())
            {
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(30, Color.Gray))) // 50% opacity
                {
                    g.FillRectangle(brush, btnSinglePlayer.ClientRectangle);
                }
            }



        }

        private void MainMenuForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();

        }

        private void MainMenuForm_Load(object sender, EventArgs e)
        {

            AudioManager.PlayMusic(Properties.Resources.mainmenumusic, 0.5f);
        }



        private void clicksound()
        {
            SoundPlayer clickSound = new SoundPlayer(Properties.Resources.selectsound);
            clickSound.Play();
        }
    }
}
