using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hangman
{
    class Guesser
    {
        private string path = "Data/";

        public List<string> RetrieveWords(int length, string language)
        {
            List<string> RetrievedWords = new List<string>();
            if(!File.Exists(path + language + ".txt"))
            {
                Console.WriteLine("Word file has not been found.");
                return null;
            }

            string[] words = File.ReadAllLines(path + language + ".txt");
            
            foreach(var word in words)
            {
                if(word.Length == length)
                {
                    RetrievedWords.Add(word);
                }
            }
            return RetrievedWords;
        }

        public List<string> FilterWords(char character, List<string> words, bool state, int[] positions = null)
        {
            int x = 1;
            List<string> FilteredWords = new List<string>();
            if (state == true)
            {
                for (int i = 0; i < positions.Length; i++)
                {
                    if (i == 0)
                    {
                        foreach (var word in words)
                        {
                            x = 1;
                            foreach (var letter in word)
                            {
                                if (x == positions[i])
                                {
                                    if (letter.Equals(character))
                                    {
                                        FilteredWords.Add(word);
                                    }
                                }
                                x++;
                            }
                        }
                    }
                    else
                    {
                        List<string> TemporaryList = new List<string>();
                        foreach (var word in FilteredWords)
                        {
                            x = 1;
                            foreach (var letter in word)
                            {
                                if (x == positions[i])
                                {
                                    if (letter.Equals(character))
                                    {
                                        TemporaryList.Add(word);
                                    }
                                }
                                x++;
                            }
                        }
                        FilteredWords = TemporaryList;
                    }
                }
            }
            else
            {
                foreach (var word in words)
                {
                    if (!word.Contains(character))
                    {
                        FilteredWords.Add(word);
                    }
                }
            }
            return FilteredWords;
        }

        public char GetAvaiableCharacter(List<char> TriedCharacters, List<string> words)
        {
            SortedDictionary<Char, ulong> CharacterCount = new SortedDictionary<char, ulong>();
            foreach(var word in words)
            {
                foreach(var character in word)
                {
                    if (!CharacterCount.ContainsKey(character))
                    {
                        CharacterCount.Add(character, 1);
                    }
                    else
                    {
                        CharacterCount[character]++;
                    }
                }
            }
            ulong temp = 0;
            char AvailableCharacter = ' ';
            foreach(var info in CharacterCount)
            {
                if (!TriedCharacters.Contains(info.Key))
                {
                    if(info.Value > temp)
                    {
                        temp = info.Value;
                        AvailableCharacter = info.Key;
                    }
                }          
            }
            if (temp == 0)
            {
                //Does this if no characters are available
            }
            return AvailableCharacter; 
        }

        public void AddNewWord(string NewWord, string lang)
        {
            File.AppendAllText(path + lang + ".txt", "\n" + NewWord);
        }
    }
}
