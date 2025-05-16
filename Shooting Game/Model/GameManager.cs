namespace Shooting_Game.Model
{
    public class GameManager
    {
        private static GameManager _instance;
        public static GameManager Instance => _instance ?? (_instance = new GameManager());

        public int Difficulty { get; set; }
        public bool IsSinglePlayer { get; set; }

        private GameManager() { }
    }
}
