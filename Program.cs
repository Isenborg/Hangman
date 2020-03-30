using System;

namespace Hangman
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Hangman Guesser";
            HangmanSolverInterface.run();
        }
    }
}
