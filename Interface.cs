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
            bool WordContainsLetter;
            bool IsGameOver = false;
            ConsoleKeyInfo keypressed;
            DisplayTitle();
            Console.ReadKey(true);
            Languages = provider.GetAvailableLanguages();
            Console.Clear();
            do
            {
                AvailableWords.Clear();
                TriedCharacters.Clear();
                ChosenNumber = 0;
                lang  = "";
                do
                {
                    DisplayTitle();
                    Console.WriteLine("Choose the language of your word:");
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
                DisplayTitle();
                length = provider.GetWordLength();
                AvailableWords = guesser.RetrieveWords(length, lang);
                CurrentGuess = string.Concat(Enumerable.Repeat("_", length));
                IsGameOver = false;
                fails = 0;
                Console.Clear();
                do
                {
                    DisplayTitle();
                    Console.WriteLine($"Amount of words left: {AvailableWords.Count}");
                    Console.WriteLine($"Current guess: {CurrentGuess}");
                    CharacterAsked = guesser.GetAvaiableCharacter(TriedCharacters, AvailableWords);
                    TriedCharacters.Add(CharacterAsked);
                    Console.WriteLine($"Amount of failures left: {5-fails} / 5");
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
                        WordContainsLetter = true;
                        Console.Write($"\nHow many times does the letter \"{CharacterAsked}\" occur in your word? " +
                            $"\nEnter how many occurances: ");
                        occurances = provider.GetLetterAmount(length);
                        positions = provider.GetWordPosition(length, occurances);
                        CurrentGuess = provider.UpdateWord(CurrentGuess, positions, occurances, CharacterAsked);
                        AvailableWords = guesser.FilterWords(CharacterAsked, AvailableWords, WordContainsLetter, positions);
                        IsGameOver = game.IsGameOver(AvailableWords.Count, fails);
                    }
                    else
                    {
                        WordContainsLetter = false;
                        AvailableWords = guesser.FilterWords(CharacterAsked, AvailableWords, WordContainsLetter);
                        fails++;
                        IsGameOver = game.IsGameOver(AvailableWords.Count, fails);
                    }
                    Console.Clear();
                } while (IsGameOver == false);
                if (fails >= 5 || AvailableWords.Count == 0)
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

        private static void DisplayTitle()
        {
            var arr = new string[]
            {
                    @" _                                             ",
                    @"| |                                            ",
                    @"| |__   __ _ _ __   __ _ _ __ ___   __ _ _ __  ",
                    @"| '_ \ / _` | '_ \ / _` | '_ ` _ \ / _` | '_ \ ",
                    @"| | | | (_| | | | | (_| | | | | | | (_| | | | |",
                    @"|_| |_|\__,_|_| |_|\__, |_| |_| |_|\__,_|_| |_|",
                    @"                    __/ |                      ",
                    @"___________________|___/_______________________",
            };
            foreach (var line in arr)
            {
                Console.WriteLine(line);
            }
        }
    }
}
