using System;
using System.Collections.Generic;
using System.Linq;

namespace Hangman
{
    public static class HangmanSolverInterface
    {
        public static void run()
        {
            var provider = new Provider();
            int occurances;
            int length;
            int fails;
            int[] positions;
            List<char> triedCharacters = new List<char>();
            List<string> availableWords = new List<string>();
            string choice;
            string newWord;
            string lang;
            string currentGuess;
            char characterAsked;
            bool wordContainsLetter;

            DisplayTitle();
            Console.ReadKey(true);
            Console.Clear();

            do
            {
                availableWords.Clear();
                triedCharacters.Clear();

                lang  = SelectLanguage(provider.GetAvailableLanguages());

                Console.Clear();
                DisplayTitle();
                length = GetWordLength();
                availableWords = provider.ReadList(length, lang);
                currentGuess = string.Concat(Enumerable.Repeat("_", length));
                fails = 0;
                Console.Clear();
                do
                {
                    DisplayTitle();
                    Console.WriteLine($"Amount of words left: {availableWords.Count}");
                    Console.WriteLine($"Current guess: {currentGuess}");
                    characterAsked = Guesser.GetMostRelevantCharacter(triedCharacters, availableWords);
                    triedCharacters.Add(characterAsked);
                    Console.WriteLine($"Amount of failures left: {5-fails} / 5");
                    Console.Write($"Is the letter \"{characterAsked}\" in your word? " +
                        $"\n(Y/N): ");
                    choice = Console.ReadLine();
                    
                    while (choice.ToUpper() != "Y" && choice.ToUpper() != "N")
                    {
                        Console.Write("Enter what was requested for: ");
                        choice = Console.ReadLine();
                    }

                    if (choice.ToUpper() == "Y")
                    {
                        wordContainsLetter = true;
                        Console.Write($"\nHow many times does the letter \"{characterAsked}\" occur in your word? " +
                            $"\nEnter how many occurances: ");
                        occurances = GetLetterAmount(length);
                        positions = GetWordPosition(length, occurances);
                        currentGuess = UpdateWord(currentGuess, positions, occurances, characterAsked);
                        availableWords = Guesser.FilterWords(characterAsked, availableWords, wordContainsLetter, positions);
                    }
                    
                    else if (choice.ToUpper() == "N")
                    {
                        wordContainsLetter = false;
                        availableWords = Guesser.FilterWords(characterAsked, availableWords, wordContainsLetter);
                        fails++;
                    }

                    Console.Clear();
                } while (!Game.IsOver(availableWords.Count, fails));

                if (fails >= 5 || availableWords.Count == 0)
                {
                    Console.Write("You WON! The computer could not guess your word." +
                        "\nWhat was your word? ");
                    newWord = Console.ReadLine();
                    provider.AddNewWord(newWord, lang);
                    Console.Write("Would you like to try again? (Y/N): ");
                    choice = Console.ReadLine();
                }
                else if(availableWords.Count == 1)
                {
                    Console.Write($"The computer thinks your word is {availableWords[0]}." +
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
                        newWord = Console.ReadLine();
                        provider.AddNewWord(newWord, lang);
                        Console.Write("Would you like to try again? (Y/N): ");
                        choice = Console.ReadLine();
                    }
                }
                Console.Clear();
            } while (choice == "Y" || choice == "y");
        }

        private static int GetWordLength()
        {
            int length = 0;
            Console.Write("Think of a word between 4-15 characters long" +
                "\nHow many characters does your word have?" +
                "\nEnter here: ");
            while (!int.TryParse(Console.ReadLine(), out length) || (length > 15 || length < 4))
            {
                Console.Write("Enter a valid numer instead: ");
            }
            return length;
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

        private static string SelectLanguage(List<string> Languages)
        {
            ConsoleKeyInfo keypressed;
            int ChosenNumber = 0;
            bool arrowHasMoved = true;
            string selectedLanguage = "";
            do
            {
                if (arrowHasMoved)
                {
                    DisplayTitle();
                    Console.WriteLine("Choose the language of your word:");
                    for (int i = 0; i < Languages.Count; i++)
                    {
                        if (i == ChosenNumber)
                        {
                            Console.WriteLine($"->{Languages[i]}");
                            selectedLanguage = Languages[i];
                        }
                        else
                        {
                            Console.WriteLine(Languages[i]);
                        }
                    }
                }
                keypressed = Console.ReadKey(true);
                if (keypressed.Key == ConsoleKey.DownArrow)
                {
                    if (ChosenNumber != Languages.Count - 1)
                    {
                        ChosenNumber++;
                        arrowHasMoved = true;
                        Console.Clear();
                    }
                    else arrowHasMoved = false;
                }
                else if (keypressed.Key == ConsoleKey.UpArrow)
                {
                    if (ChosenNumber != 0)
                    {
                        ChosenNumber--;
                        arrowHasMoved = true;
                        Console.Clear();
                    }
                    else arrowHasMoved = false;
                }

            } while (ConsoleKey.Enter != keypressed.Key);
            return selectedLanguage;
        }


        private static string UpdateWord(string word, int[] positions, int times, char character)
        {
            for (int i = 0; i < times; i++)
            {
                word = word.Insert(positions[i], character.ToString());
                word = word.Remove(positions[i] - 1, 1);
            }
            return word;
        }


        private static int GetLetterAmount(int length)
        {
            int position;
            while (!int.TryParse(Console.ReadLine(), out position) || (length < position || position <= 0))
            {
                Console.Write("Enter a valid numer instead: ");
            }
            return position;
        }

        private static int[] GetWordPosition(int length, int letterAmount)
        {
            int position;
            int[] positions = new int[letterAmount];
            for (int i = 0; i < letterAmount; i++)
            {
                Console.Write($"Enter position number {i + 1}: ");
                while (!int.TryParse(Console.ReadLine(), out position) || (length < position || position <= 0))
                {
                    Console.Write("Enter a valid numer instead: ");
                }
                positions[i] = position;
                position = 0;
            }
            return positions;
        }

    }
}
