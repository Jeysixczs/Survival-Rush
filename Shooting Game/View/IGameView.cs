using Shooting_Game.Model;
using Shooting_Game.Presenter;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Shooting_Game.View
{
    public interface IGameView
    {
        void SetPresenter(GamePresenter presenter);
        void UpdatePlayerStatus(int health, int ammo);
        void SpawnEntity(GameEntity entity);
        void RemoveEntity(GameEntity entity);
        List<PictureBox> GetWalls();

        Panel GetGamePanel();
        void ShowGameOver();

    }
}
