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
        
        public void StateCheck()
        {
            switch (state)
            {
                case GameStates.Menu:
                    break;
                case GameStates.GameStart:
                    break;
                case GameStates.GameOver:
                    break;
            }
        }
    }
}