using scrabbleguy;
using System.Text;

public class ScrabbleBoard
{
    private Tile[,] board;
    private const int BoardSize = 15;
    private HashSet<string> playedwords=new HashSet<string>();

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
        for (int row = 0; row < BoardSize; row++)
        {
            for (int col = 0; col < BoardSize; col++)
            {
                if (board[row, col] != null)
                {
                    Console.Write(board[row, col].Letter + " ");
                }
                else
                {
                    Console.Write("- ");
                }
            }
            Console.WriteLine();
        }
    }

    // CanPlaceWord: Validate that the word can be placed considering both rack and board tiles
    public bool CanPlaceWord(List<Tile> wordTiles, int wordRow, int wordCol, bool horizontal, Player player)
    {
        bool isAttached=false;
        // Validate if the main word can be placed as usual.
        if (!WordHandling.ValidWord(wordTiles))
            return false;


        // Check for new words formed with pre-existing tiles
        for (int i = 0; i < wordTiles.Count; i++)
        {
            if (wordTiles[i] != null)
                isAttached = true;
            int tileRow = horizontal ? wordRow : wordRow + i;
            int tileCol = horizontal ? wordCol + i : wordCol;

            // Check if placing horizontally
            if (horizontal)
            {
                if (HasAdjacentTilesVertically(tileRow, tileCol))
                {
                    List<Tile> newVerticalWord = FormVerticalWord(tileRow, tileCol);
                    if (!WordHandling.ValidWord(newVerticalWord))
                        return false; // Invalid perpendicular word
                    if (!playedwords.Contains(WordHandling.TilesToWord(newVerticalWord)))
                    {
                        player.AddPoints(newVerticalWord,tileRow,tileCol,false,this); ; // Calculate points for new words
                        playedwords.Add(WordHandling.TilesToWord(newVerticalWord)); // Add to played words
                    }
                }
            }
            // Check if placing vertically
            else
            {
                if (HasAdjacentTilesHorizontally(tileRow, tileCol))
                {
                    List<Tile> newHorizontalWord = FormHorizontalWord(tileRow, tileCol);
                    if (!WordHandling.ValidWord(newHorizontalWord))
                        return false; // Invalid perpendicular word
                    if (!playedwords.Contains(WordHandling.TilesToWord(newHorizontalWord)))
                    {
                        player.AddPoints(newHorizontalWord,tileRow,tileCol,true,this); ; // Calculate points for new words
                        playedwords.Add(WordHandling.TilesToWord(newHorizontalWord)); // Add to played words
                    }
                }
            }
        }
        return isAttached; // If all validations pass
    }



    // Place the word on the board (only if CanPlaceWord passed)
    public void PlaceWord(List<Tile> wordTiles, int startRow, int startCol, bool horizontal)
    {
        int row = startRow;
        int col = startCol;
        int tileMultiplyer = 1;
        int wordmultiplyer = 1;

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
        playedwords.Add(WordHandling.TilesToWord(wordTiles));
    }
    
    public bool ValidateNewWordWithExistingTiles(int row, int col, bool horizontal, List<Tile> placedTiles)
    {
        // Validate main word first (already implemented)
        if (!WordHandling.ValidWord(placedTiles))
            return false;

        // Check for perpendicular words
        foreach (Tile tile in placedTiles)
        {
            int tileRow = horizontal ? row : row++;
            int tileCol = horizontal ? col++ : col;

            // Check vertically if the word is placed horizontally
            if (horizontal)
            {
                if (HasAdjacentTilesVertically(tileRow, tileCol))
                {
                    List<Tile> newVerticalWord = FormVerticalWord(tileRow, tileCol);
                    if (!WordHandling.ValidWord(newVerticalWord))
                        Console.WriteLine("invalid adjacent word: "+newVerticalWord);
                    return false;  // Invalid word
                }
            }
            // Check horizontally if the word is placed vertically
            else
            {
                if (HasAdjacentTilesHorizontally(tileRow, tileCol))
                {
                    List<Tile> newHorizontalWord = FormHorizontalWord(tileRow, tileCol);
                    if (!WordHandling.ValidWord(newHorizontalWord))
                        Console.WriteLine("invalid adjacent word: "+newHorizontalWord);
                        return false;  // Invalid word
                }
            }
        }
        return true; // All words valid
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

        // Move upwards
        int tempRow = row;
        while (tempRow >= 0 && board[tempRow, col] != null)
        {
            word.Insert(0, board[tempRow, col]); // Insert at the beginning
            tempRow--;
        }

        // Move downwards
        tempRow = row + 1;
        while (tempRow <= 14 && board[tempRow, col] != null)
        {
            word.Add(board[tempRow, col]);  // Append to the end
            tempRow++;
        }

        return word;
    }

    private List<Tile> FormHorizontalWord(int row, int col)
    {
        List<Tile> word = new List<Tile>();

        // Move leftwards
        int tempCol = col;
        while (tempCol >= 0 && board[row, tempCol] != null)
        {
            word.Insert(0, board[row, tempCol]); // Insert at the beginning
            tempCol--;
        }

        // Move rightwards
        tempCol = col + 1;
        while (tempCol <= 14 && board[row, tempCol] != null)
        {
            word.Add(board[row, tempCol]);  // Append to the end
            tempCol++;
        }

        return word;
    }

    public void PrintPlayedWords()
    {
        foreach (string str in playedwords)
        {
            Console.Write(str+", ");
            }
    }

}
