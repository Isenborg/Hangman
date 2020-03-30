using System;
using System.Collections.Generic;
using System.Linq;

namespace Hangman
{
    public static class Guesser
    {
        public static List<string> FilterWords(char character, List<string> words, bool WordContainsLetter, int[] positions = null)
        {
            return WordContainsLetter
                ? GetWordsThatContainCharAtPositions(words, character, positions).ToList()
                : GetWordsThatDontContainChar(words, character).ToList();
        }

        private static IEnumerable<string> GetWordsThatContainCharAtPositions(IEnumerable<string> words, char character, IEnumerable<int> positions)
        {
            var filteredWords = words.Where(word =>
            {
                return StringContainsCharacterAtPositions(word, character, positions)
                && StringContainsSameAmountOfCharacters(word, character, positions.Count());
            });
            return filteredWords;
        }

        private static bool StringContainsCharacterAtPositions(string word, char character, IEnumerable<int> positions)
            => positions.All(pos => word[pos - 1].Equals(character));
            
        private static bool StringContainsSameAmountOfCharacters(string word, char character, int AmountOfSpecificCharacter)
            => word.Count(c => c == character) == AmountOfSpecificCharacter;

        private static IEnumerable<string> GetWordsThatDontContainChar(List<string> words, char character)
            => words.Where(w => !w.Contains(character));

        public static char GetMostRelevantCharacter(List<char> TriedCharacters, List<string> words)
        {
            SortedDictionary<Char, int> CharacterCount = new SortedDictionary<char, int>();
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
            int temp = 0;
            char MostRelevantChar = ' ';
            foreach(var info in CharacterCount)
            {
                if (!TriedCharacters.Contains(info.Key))
                {
                    if(info.Value > temp)
                    {
                        temp = info.Value;
                        MostRelevantChar = info.Key;
                    }
                }          
            }
            if (temp == 0)
            {
                //Does this if no characters are available
            }
            return MostRelevantChar; 
        }
    }
}
        

