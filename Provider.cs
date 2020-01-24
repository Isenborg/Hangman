using System;
using System.Collections.Generic;
using System.IO;

namespace Hangman
{
    public class Provider
    {
        public int GetWordLength()
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


        public List<string> GetAvailableLanguages(string path)
        {
            var words = Directory.GetFiles(path);
            string lang;
            List<string> languages = new List<string>();
            foreach(var word in words)
            {
                lang = word.Substring(5);
                lang = lang.Remove(lang.Length - 4);
                languages.Add(lang);
            }
            return languages;
        }

        public int GetLetterAmount(int length)
        {
            int position;
            while (!int.TryParse(Console.ReadLine(), out position) || (length < position || position <= 0))
            {
                Console.Write("Enter a valid numer instead: ");
            }
            return position;
        }

        public int[] GetWordPosition(int length, int times)
        {
            int position;
            int[] positions = new int[times];
            for (int i = 0; i < times; i++)
            {
                Console.Write($"Enter position number {i+1}: ");
                while (!int.TryParse(Console.ReadLine(), out position) || (length < position || position <= 0))
                {
                    Console.Write("Enter a valid numer instead: ");
                }
                positions[i] = position;
                position = 0;
            }
            return positions;
        }

        public string UpdateWord(string word, int[] positions, int times, char character)
        {
            for(int i = 0; i < times; i++)
            {
                word = word.Insert(positions[i], character.ToString());
                word = word.Remove(positions[i]-1, 1);
            }
            return word;
        }

    }
}
