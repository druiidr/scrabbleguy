using System;
using System.Collections.Generic;
using System.Text;

namespace scrabbleguy
{
    internal class WordHandling
    {
        static ScrabbleDictionary scrabbleDictionary = new ScrabbleDictionary(@"C:\Users\gguyl\OneDrive\מסמכים\fullScrabbleLegalDictionary.txt");

        public static string TilesToWord(List<Tile> word)
        {
            StringBuilder strBuilder = new StringBuilder();
            foreach (Tile tile in word)
            {
                strBuilder.Append(tile.ToString());
            }

            return strBuilder.ToString();
        }

        public static bool ValidWord(List<Tile> word)
        {
            string str = TilesToWord(word);
            if (scrabbleDictionary.IsValidWord(str))
            {
                return true;
            }
            Console.WriteLine("Not a real word!");
            return false;
        }
    }
}
