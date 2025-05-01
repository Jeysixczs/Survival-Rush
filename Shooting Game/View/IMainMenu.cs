using Shooting_Game.Presenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooting_Game.View
{
    public interface IMainMenu
    {
        void SetPresenter(MainMenuPresenter presenter);
        void ShowGameForm();
        void ShowTwoPlayerForm();


    }
}
