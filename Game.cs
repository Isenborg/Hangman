namespace Hangman
{
    class Game
    {
        public bool IsGameOver(int AvailableWords, int tries)
        {
            if (AvailableWords == 1)
            {
                return true;
            }
            else if (tries >= 5 || AvailableWords == 0)
            {
                return true;
            }
            return false;
        }
    }
}
