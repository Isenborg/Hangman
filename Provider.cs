using System;
using System.Collections.Generic;
using System.IO;

namespace Hangman
{
    public class Provider
    {
        private readonly string path = "Dictionaries/";

        public List<string> ReadList(int length, string language)
        {
            if (!File.Exists(path + language + ".txt"))
            {
                Console.WriteLine("Word file has not been found.");
                return null;
            }

            var RetrievedWords = new List<string>();
            string[] words = File.ReadAllLines(path + language + ".txt");
            foreach (var word in words)
            {
                if (word.Length == length)
                {
                    RetrievedWords.Add(word.ToUpper());
                }
            }
            return RetrievedWords;
        }

        public List<string> GetAvailableLanguages()
        {
            var words = Directory.GetFiles(path);
            string lang;
            List<string> languages = new List<string>();
            foreach(var word in words)
            {
                lang = word.Substring(13);
                lang = lang.Remove(lang.Length - 4);
                languages.Add(lang);
            }
            return languages;
        }

        public void AddNewWord(string NewWord, string lang)
        {
            File.AppendAllText(path + lang + ".txt", "\n" + NewWord);
        }
    }
}
