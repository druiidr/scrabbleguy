using scrabbleguy;

public class Player
{
    public string Name { get; private set; }
    public int score { get; private set; }
    public List<Tile> Rack { get; private set; }
    private const int MaxTilesInRack = 7;

    public Player(string name)
    {
        Name = name;
        score = 0;
        Rack = new List<Tile>();
    }

    // Check if player has the tile in their rack
    public bool HasTileInRack(char letter)
    {
        return Rack.Any(t => t.Letter == letter);
    }

    // Remove a tile from the rack
    public void RemoveTileFromRack(Tile tile)
    {
        Tile match = Rack.FirstOrDefault(t => t.Letter == tile.Letter);
        if (match != null)
        {
            Rack.Remove(match);
        }
    }

    // Draw a tile and add it to the player's rack
    public void DrawTile(TileBag tileBag)
    {
        Tile tile = tileBag.DrawTile();
        if (tile != null)
        {
            Rack.Add(tile);
        }
    }

    // Refill the player's rack to 7 tiles
    public void RefillRack(TileBag tileBag)
    {
        while (Rack.Count < MaxTilesInRack && !tileBag.IsEmpty())
        {
            DrawTile(tileBag);
        }
    }

    // Show the player's rack (for testing purposes)
    public void ShowRack()
    {
        foreach (Tile tile in Rack)
        {
            Console.Write($"{tile.Letter}({tile.Score}) ");
        }
        Console.WriteLine();
    }
    //manage the score from a given word
    public void AddPoints (List<Tile> wordTiles)
    {
        int roundPoints = 0;
        foreach (Tile tile in wordTiles)
        {
            roundPoints+=tile.Score;
        }
        score+=roundPoints;

    }

    // Player's turn: handle actions like placing a word and interacting with the board
    public void PlayerTurn(ScrabbleBoard board, TileBag tileBag)
    {
        int wordRow=0;
        int rowCurr=0;
        int wordCol = 0;
        int colCurr = 0;
        bool orientation=false; // true=horizontal, false=vertical;

        Console.WriteLine($"{Name}, it's your turn!");
        ShowRack();
        try
        {
            Console.WriteLine("Pick a starting row (0-14):");
            wordRow = int.Parse(Console.ReadLine());
            rowCurr = wordRow;

            Console.WriteLine("Pick a starting column (0-14):");
            wordCol = int.Parse(Console.ReadLine());
            colCurr = wordCol;

            Console.WriteLine("Horizontal orientation? (true/false):");
            orientation = bool.Parse(Console.ReadLine());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        Console.WriteLine("Pick tiles to form a word (e.g., enter the letters one by one):");
        List<Tile> wordTiles = new List<Tile>();
        char inputLetter;

        do
        {
            Console.WriteLine("Enter a letter from your rack (or press '0' to finish):");
            inputLetter = char.Parse(Console.ReadLine().ToUpper());

            if (inputLetter != '0')
            {
                Tile boardTile = board.GetBoard()[rowCurr, colCurr];

                // Check if there's already a tile on the board
                if (boardTile != null && boardTile.Letter == inputLetter)
                {
                    wordTiles.Add(boardTile); // Use the board's tile
                }
                else
                {
                    // Look for the tile in the player's rack
                    Tile tileFromRack = Rack.FirstOrDefault(t => t.Letter == inputLetter);

                    if (tileFromRack != null)
                    {
                        wordTiles.Add(tileFromRack);
                        RemoveTileFromRack(tileFromRack); // Remove the tile from player's rack once used
                    }
                    else
                    {
                        Console.WriteLine("You don't have that tile in your rack.");
                    }
                }

                // Move to the next position
                if (orientation)
                    colCurr++;
                else
                    rowCurr++;
            }
        }
        while (inputLetter != '0');


        // Check if the word can be placed
        if (board.CanPlaceWord(wordTiles, wordRow, wordCol, orientation,this))
        {
            board.PlaceWord(wordTiles, wordRow, wordCol, orientation);
            Console.WriteLine("Word placed successfully.");
            AddPoints(wordTiles);
            Console.WriteLine(score);
            board.PrintBoard();
            board.PrintPlayedWords();
        }
        else
        {
            Console.WriteLine("Invalid word placement. Try again.");
        }

        RefillRack(tileBag); // Refill the player's rack after their turn
        ShowRack(); // Show updated rack
    }

}
