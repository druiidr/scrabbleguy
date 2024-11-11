using scrabbleguy;
using System;
using System.Collections.Generic;

public class ScrabbleBoard
{
    private Tile[,] board;
    private const int BoardSize = 15;
    private HashSet<string> playedWords = new HashSet<string>();
    private bool isEmpty = true;

    public ScrabbleBoard()
    {
        board = new Tile[BoardSize, BoardSize];
    }

    public Tile[,] GetBoard()
    {
        return board;
    }

    // Display the board
    public void PrintBoard()
    {
        Console.Write("   "); // Leading space for the row number column
        for (int i = 0; i < BoardSize; i++)
        {
            Console.Write((i < 10 ? " " : "") + i + " "); // Print column headers with extra space for single-digit numbers
        }
        Console.WriteLine();
        Console.WriteLine();

        for (int row = 0; row < BoardSize; row++)
        {
            Console.Write((row < 10 ? " " : "") + row + " "); // Print row number with extra space for single digits

            for (int col = 0; col < BoardSize; col++)
            {
                if (board[row, col] != null)
                {
                    Console.Write(" " + board[row, col].Letter + " ");
                }
                else
                {
                    Console.Write(" - ");
                }
            }
            Console.WriteLine();
        }
    }

    // CanPlaceWord: Validate that the word can be placed considering both rack and board tiles
    public bool CanPlaceWord(List<Tile> wordTiles, int wordRow, int wordCol, bool horizontal, Player player)
    {
        bool isAttached = false;
        if (isEmpty)
        {
            isEmpty = false;
            isAttached = true;
        }

        // Validate if the main word is a valid word
        if (!WordHandling.ValidWord(wordTiles))
            return false;

        for (int i = 0; i < wordTiles.Count; i++)
        {
            int tileRow = horizontal ? wordRow : wordRow + i;
            int tileCol = horizontal ? wordCol + i : wordCol;

            // Check if there’s already a tile on the board at this position
            if (board[tileRow, tileCol] != null)
            {
                isAttached = true;
                if(board[tileRow, tileCol].Letter!=wordTiles[i].Letter)
                {
                    Console.WriteLine("cant place a tile on an existing tile!!");
                    return false;
                }
            }

            // Check for perpendicular words
            if (horizontal)
            {
                if (HasAdjacentTilesVertically(tileRow, tileCol))
                {
                    List<Tile> newVerticalWord = FormVerticalWord(tileRow, tileCol);
                    if (!WordHandling.ValidWord(newVerticalWord))
                        return false; // Invalid perpendicular word

                    string verticalWordStr = WordHandling.TilesToWord(newVerticalWord);
                    if (!playedWords.Contains(verticalWordStr))
                    {
                        // Add points for the new vertical word and add it to playedWords
                        player.AddPoints(newVerticalWord, tileRow, tileCol, false, this);
                        playedWords.Add(verticalWordStr);
                    }
                }
            }
            else
            {
                if (HasAdjacentTilesHorizontally(tileRow, tileCol))
                {
                    List<Tile> newHorizontalWord = FormHorizontalWord(tileRow, tileCol);
                    if (!WordHandling.ValidWord(newHorizontalWord))
                        return false; // Invalid perpendicular word

                    string horizontalWordStr = WordHandling.TilesToWord(newHorizontalWord);
                    if (!playedWords.Contains(horizontalWordStr))
                    {
                        // Add points for the new horizontal word and add it to playedWords
                        player.AddPoints(newHorizontalWord, tileRow, tileCol, true, this);
                        playedWords.Add(horizontalWordStr);
                    }
                }
            }
        }

        return isAttached; // Return true only if it’s attached to existing tiles
    }


    // Place the word on the board (only if CanPlaceWord passed)
    public void PlaceWord(List<Tile> wordTiles, int startRow, int startCol, bool horizontal)
    {
        int row = startRow;
        int col = startCol;

        for (int i = 0; i < wordTiles.Count; i++)
        {
            if (board[row, col] == null) // Only place tile if the spot is empty
            {
                board[row, col] = wordTiles[i];
            }

            if (horizontal)
                col++;
            else
                row++;
        }

        playedWords.Add(WordHandling.TilesToWord(wordTiles)); // Add the main word to played words
    }

    public bool ValidateNewWordWithExistingTiles(int row, int col, bool horizontal, List<Tile> placedTiles)
    {
        // Validate main word
        if (!WordHandling.ValidWord(placedTiles))
            return false;

        foreach (Tile tile in placedTiles)
        {
            int tileRow = horizontal ? row : row++;
            int tileCol = horizontal ? col++ : col;

            if (horizontal && HasAdjacentTilesVertically(tileRow, tileCol))
            {
                List<Tile> newVerticalWord = FormVerticalWord(tileRow, tileCol);
                if (!WordHandling.ValidWord(newVerticalWord))
                {
                    Console.WriteLine("Invalid adjacent word: " + WordHandling.TilesToWord(newVerticalWord));
                    return false;
                }
            }
            else if (!horizontal && HasAdjacentTilesHorizontally(tileRow, tileCol))
            {
                List<Tile> newHorizontalWord = FormHorizontalWord(tileRow, tileCol);
                if (!WordHandling.ValidWord(newHorizontalWord))
                {
                    Console.WriteLine("Invalid adjacent word: " + WordHandling.TilesToWord(newHorizontalWord));
                    return false;
                }
            }

            if (horizontal)
                col++;
            else
                row++;
        }

        return true; // All words are valid
    }

    private bool HasAdjacentTilesVertically(int row, int col)
    {
        return (row > 0 && board[row - 1, col] != null) || (row < 14 && board[row + 1, col] != null);
    }

    private bool HasAdjacentTilesHorizontally(int row, int col)
    {
        return (col > 0 && board[row, col - 1] != null) || (col < 14 && board[row, col + 1] != null);
    }

    private List<Tile> FormVerticalWord(int row, int col)
    {
        List<Tile> word = new List<Tile>();

        // Move up to the start of the word
        int tempRow = row;
        while (tempRow > 0 && board[tempRow - 1, col] != null)
        {
            tempRow--;
        }

        // Collect tiles from the start of the word downward
        while (tempRow <= 14 && board[tempRow, col] != null)
        {
            word.Add(board[tempRow, col]);
            tempRow++;
        }

        return word;
    }

    private List<Tile> FormHorizontalWord(int row, int col)
    {
        List<Tile> word = new List<Tile>();

        // Move left to the start of the word
        int tempCol = col;
        while (tempCol > 0 && board[row, tempCol - 1] != null)
        {
            tempCol--;
        }

        // Collect tiles from the start of the word to the right
        while (tempCol <= 14 && board[row, tempCol] != null)
        {
            word.Add(board[row, tempCol]);
            tempCol++;
        }

        return word;
    }

    public void PrintPlayedWords()
    {
        foreach (string word in playedWords)
        {
            Console.Write(word + ", ");
        }
        Console.WriteLine();
    }
}
