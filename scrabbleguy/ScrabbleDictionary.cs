using System;
using System.Collections.Generic;
using System.IO;

public class ScrabbleDictionary
{
    private HashSet<string> validWords;

    public ScrabbleDictionary(string filePath)
    {
        validWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase); // For case-insensitive matching
        LoadWordsFromFile(filePath);
    }

    private void LoadWordsFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Dictionary file not found!");
            return;
        }

        foreach (var line in File.ReadLines(filePath))
        {
            validWords.Add(line.Trim());
        }
    }

    public bool IsValidWord(string word)
    {
        return validWords.Contains(word);
    }
}
