using System;
using System.Collections.Generic;
using System.Linq;

namespace Hangman
{
    class Interface
    {
        public static void run()
        {
            var guesser = new Guesser();
            var provider = new Provider();
            var game = new Game();
            int length = 0;
            int occurances;
            int fails = 0;
            int[] positions;
            List<char> TriedCharacters = new List<char>();
            List<string> AvailableWords = new List<string>();
            List<string> Languages = new List<string>();
            string choice;
            string NewWord;
            string lang;
            int ChosenNumber;
            char CharacterAsked;
            string CurrentGuess;
            bool state;
            bool IsGameOver = false;
            ConsoleKeyInfo keypressed;
            Console.WriteLine("Welcome to the Hangman solver!" +
                "\nIn this program, You will think of a word while the program tries to solve it as fast as possible." +
                "\nGood luck..");
            Console.ReadKey(true);
            Languages = provider.GetAvailableLanguages("Data");
            Console.Clear();
            do
            {
                AvailableWords.Clear();
                TriedCharacters.Clear();
                ChosenNumber = 0;
                lang = "";
                do
                {
                    for (int i = 0; i < Languages.Count; i++)
                    {
                        if (i == ChosenNumber)
                        {
                            Console.WriteLine($"->{Languages[i]}");
                            lang = Languages[i];
                        }
                        else
                        {
                            Console.WriteLine(Languages[i]);
                        }
                    }
                    keypressed = Console.ReadKey(true);
                    if (keypressed.Key == ConsoleKey.DownArrow)
                    {
                        if(ChosenNumber != Languages.Count-1) ChosenNumber++;
                    }
                    else if (keypressed.Key == ConsoleKey.UpArrow)
                    {
                        if (ChosenNumber != 0) ChosenNumber--;
                    }
                    Console.Clear();
                } while (ConsoleKey.Enter != keypressed.Key);
                length = provider.GetWordLength();
                AvailableWords = guesser.RetrieveWords(length, lang);
                CurrentGuess = string.Concat(Enumerable.Repeat("_", length));
                IsGameOver = false;
                fails = 0;
                
                do
                {
                    Console.WriteLine($"Current guess: {CurrentGuess}");
                    CharacterAsked = guesser.GetAvaiableCharacter(TriedCharacters, AvailableWords);
                    TriedCharacters.Add(CharacterAsked);
                    Console.WriteLine($"Amount of failures left: {10-fails} / 10");
                    Console.Write($"Is the letter \"{CharacterAsked}\" in your word? " +
                        $"\n(Y/N): ");
                    choice = Console.ReadLine();
                    while(choice != "y" && choice != "Y" && choice != "N" && choice != "n")
                    {
                        Console.Write("Enter what was requested for: ");
                        choice = Console.ReadLine();
                    }
                    if (choice == "Y" || choice == "y")
                    {
                        state = true;
                        Console.Write($"\nHow many times does the letter \"{CharacterAsked}\" occur in your word? " +
                            $"\nEnter how many occurances: ");
                        occurances = provider.GetLetterAmount(length);
                        positions = provider.GetWordPosition(length, occurances);
                        CurrentGuess = provider.UpdateWord(CurrentGuess, positions, occurances, CharacterAsked);
                        AvailableWords = guesser.FilterWords(CharacterAsked, AvailableWords, state, positions);
                        IsGameOver = game.IsGameOver(AvailableWords.Count, fails);
                    }
                    else
                    {
                        state = false;
                        AvailableWords = guesser.FilterWords(CharacterAsked, AvailableWords, state);
                        fails++;
                        IsGameOver = game.IsGameOver(AvailableWords.Count, fails);
                    }
                } while (IsGameOver == false);
                if (fails >= 10 || AvailableWords.Count == 0)
                {
                    Console.Write("You WON! The computer could not guess your word." +
                        "\nWhat was your word? ");
                    NewWord = Console.ReadLine();
                    guesser.AddNewWord(NewWord, lang);
                    Console.Write("Would you like to try again? (Y/N): ");
                    choice = Console.ReadLine();
                }
                else if(AvailableWords.Count == 1)
                {
                    Console.Write($"The computer thinks your word is {AvailableWords[0]}." +
                        $"\nIs that correct? (Y/N): ");
                    choice = Console.ReadLine();
                    Console.WriteLine("");
                    if (choice == "Y" || choice == "y")
                    {
                        Console.Write("Thank you for playing." +
                            "\nWould you like to try again? (Y/N): ");
                        choice = Console.ReadLine();
                    }
                    else
                    {
                        Console.Write("Congratulations, you beat the computer." +
                            "\nWhat was your word? ");
                        NewWord = Console.ReadLine();
                        guesser.AddNewWord(NewWord, lang);
                        Console.Write("Would you like to try again? (Y/N): ");
                        choice = Console.ReadLine();
                    }
                }
                Console.Clear();
            } while (choice == "Y" || choice == "y");
        }
    }
}
