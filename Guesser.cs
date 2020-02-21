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
            return state
                ? GetWordsThatContainCharAtPositions(words, character, positions).ToList()
                : GetWordsThatDontContainChar(words, character).ToList();
        }

        private static IEnumerable<string> GetWordsThatContainCharAtPositions(IEnumerable<string> words, char character, IEnumerable<int> positions)
        {
            return words.Where(word => {
                return StringHasCharsAt(word, positions)
                && StringContainsCharacterAtPositions(word, character, positions) 
                && StringContainsSameAmountOfCharacters(word, character, positions.Count());
            });
        }

        private static bool StringHasCharsAt(string text, IEnumerable<int> positions)
            => text.Length >= positions.Max();

        private static bool StringContainsCharacterAtPositions(string word, char character, IEnumerable<int> positions)
            => positions.All(pos => word[pos - 1].Equals(character));

        private static IEnumerable<string> GetWordsThatDontContainChar(List<string> words, char character)
            => words.Where(w => !w.Contains(character));

        private static bool StringContainsSameAmountOfCharacters(string word, char character, int AmountOfSpecificCharacter)
        {
            int Amount = 0;
            foreach(var c in word)
            {
                if(c == character) Amount++;
            }
            return Amount == AmountOfSpecificCharacter;
        }


        public char GetAvaiableCharacter(List<char> TriedCharacters, List<string> words)
        {
            SortedDictionary<Char, ulong> CharacterCount = new SortedDictionary<char, ulong>();
            foreach(var word in words)
            {
                foreach(var character in word.ToUpper())
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
