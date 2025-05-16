using Shooting_Game.Presenter;

namespace Shooting_Game.View
{
    public interface IMainMenu
    {
        void SetPresenter(MainMenuPresenter presenter);
        void ShowGameForm();
        void ShowTwoPlayerForm();


    }
}
