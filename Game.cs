namespace Hangman
{
    public static class Game
    {
        public static bool IsOver(int AvailableWords, int tries)
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
