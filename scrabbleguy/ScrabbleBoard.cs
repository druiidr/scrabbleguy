using scrabbleguy;

public class ScrabbleBoard
{
    private Tile[,] board;
    private const int BoardSize = 15;

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
    public bool CanPlaceWord(List<Tile> wordTiles, int startRow, int startCol, bool horizontal, Player player)
    {
        List<Tile> rackTilesToUse = new List<Tile>(); // Holds tiles that need to be placed from the rack
        int row = startRow;
        int col = startCol;
        if (WordHandling.ValidWord(wordTiles))
        {
            // Traverse the board and check each tile in the word
            for (int i = 0; i < wordTiles.Count; i++)
            {
                if (row >= BoardSize || col >= BoardSize) // Out of bounds
                {
                    Console.WriteLine("Word placement out of board bounds.");
                    return false;
                }

                Tile currentBoardTile = board[row, col];
                Tile playerTile = wordTiles[i];

                if (currentBoardTile != null)
                {
                    // If there is already a tile on the board, it must match the letter in the word
                    if (currentBoardTile.Letter != playerTile.Letter)
                    {
                        Console.WriteLine($"Conflict with existing tile '{currentBoardTile.Letter}' at position ({row}, {col}).");
                        return false;
                    }
                }
                else
                {

                    rackTilesToUse.Add(playerTile);
                }

                if (horizontal)
                    col++;
                else
                    row++;
            }

            // If the word can be placed, remove the tiles from the player's rack that were used
            foreach (var tile in rackTilesToUse)
            {
                player.RemoveTileFromRack(tile);
            }

            return true;
        }
        return false;
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
    }
}
