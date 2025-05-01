using Shooting_Game.Model;
using Shooting_Game.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooting_Game.Presenter
{
    public class MainMenuPresenter
    {
        private readonly IMainMenu view;

        public MainMenuPresenter(IMainMenu view)
        {
            this.view = view;
        }

        public void SelectSinglePlayer()
        {
            GameManager.Instance.IsSinglePlayer = true;
        }

        public void SelectTwoPlayer()
        {
            GameManager.Instance.IsSinglePlayer = false;
        }

        public void SelectDifficulty(int level)
        {
            GameManager.Instance.Difficulty = level;
        }

        public void StartGame()
        {
            if (GameManager.Instance.IsSinglePlayer)
                view.ShowGameForm();
            else
                view.ShowTwoPlayerForm();
        }
    }
}
