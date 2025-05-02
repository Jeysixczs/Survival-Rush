using Shooting_Game.Model;
using Shooting_Game.Presenter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shooting_Game.View
{
    public interface ITwoPlayerGameView
    {
        Size ClientSize { get; }

        void SetPresenter(GamePresenter presenter);
        void UpdatePlayer1Status(int health, int ammo);
        void UpdatePlayer2Status(int health, int ammo);
        void SpawnEntity(GameEntity entity);
        void RemoveEntity(GameEntity entity);

    }
}
