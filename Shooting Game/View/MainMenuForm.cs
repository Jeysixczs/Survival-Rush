using Shooting_Game.Model;
using Shooting_Game.Presenter;
using Shooting_Game.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            presenter?.SelectSinglePlayer();
            MessageBox.Show("Single Player Selected");
        }

        private void btnTwoPlayer_Click(object sender, EventArgs e)
        {
            presenter?.SelectTwoPlayer();
        }

        private void btnDifficultyEasy_Click(object sender, EventArgs e)
        {
            presenter?.SelectDifficulty(1);
            MessageBox.Show("Easy Difficulty Selected");
        }

        private void btnDifficultyMedium_Click(object sender, EventArgs e)
        {
            presenter?.SelectDifficulty(2);
        }

        private void btnDifficultyHard_Click(object sender, EventArgs e)
        {
            presenter?.SelectDifficulty(3);
        }

        private void pictureBoxStart_Click(object sender, EventArgs e)
        {
            AudioManager.StopMusic();
            presenter.StartGame();
            this.Hide();
            if (GameManager.Instance.IsSinglePlayer)
            {
                MessageBox.Show("Starting Single Player Game");
            }
            else
            {
                MessageBox.Show("Starting Two Player Game");
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
    }
}
