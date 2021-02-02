using Zenject;
namespace DefaultNamespace
{
    public class GameSystem
    {
        public enum GameStates
        {
            Menu,
            GameStart,
            GameOver
        }

        public GameStates state = GameStates.Menu;
        
    }
}