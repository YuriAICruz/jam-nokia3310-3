namespace DefaultNamespace
{
    public class ScoreManager
    {
        public int Score=0;
        public int HighScore=0;

        public void AddPoints(int points)
        {
            Score += points;
        }

        public void CheckHigScore()
        {
            if (Score>HighScore)
            {
                HighScore = Score;
                Score = 0;
            }
        }
    }
}