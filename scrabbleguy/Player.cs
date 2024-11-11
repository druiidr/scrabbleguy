﻿using scrabbleguy;

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
    public void RefillRack(List<Tile> tiles)
    {
        foreach (Tile tile in tiles)
        {
            Rack.Add(tile);
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
    // Manage the score from a given word, considering multipliers
    public void AddPoints(List<Tile> wordTiles, int startRow, int startCol, bool horizontal, ScrabbleBoard board)
    {
        int roundPoints = 0;
        int wordMultiplier = 1; // This will hold the cumulative word multiplier
        if (wordTiles.Count ==7) //handles bingos(turns where a player uses all of his rack in a word)
        {
            Console.WriteLine("BINGO!!!!");
            roundPoints+=50;
        }
        for (int i = 0; i < wordTiles.Count; i++)
        {
            Tile tile = wordTiles[i];
            int row = horizontal ? startRow : startRow + i;
            int col = horizontal ? startCol + i : startCol;

            // Calculate the tile score
            roundPoints += tile.Score;

            // Get the multiplier for the current tile position
            

            // Apply multipliers
            switch (row,col)
            {
                case (0, 3):
                case (0, 11):               
                case (2, 6):
                case (2, 8):
                case (3, 0):
                case (3, 7):
                case (3, 14):
                case (6, 2):
                case (6, 6):
                case (6, 8):
                case (6, 12):
                case (7, 3):
                case (7, 11):
                case (8, 2):
                case (8, 6):
                case (8, 8):
                case (8, 12):
                case (11, 0):
                case (11, 7):
                case (11, 14):
                case (12, 6):
                case (12, 8):
                case (14, 3):
                case (14, 11):
                    Console.WriteLine("{0} double Letter!!", tile.Letter);
                    roundPoints += tile.Score; // Double the tile score
                    break;
                case (1, 5):
                case (1, 9):
                case (5, 1):
                case (5, 5):
                case (5, 9):
                case (5, 13):
                case (9, 1):
                case (9, 5):
                case (9, 9):
                case (9, 13):
                case (13, 5):
                case (13, 9):
                    roundPoints += tile.Score * 2; // Triple the tile score
                    Console.WriteLine("{0} triple Letter!!!",tile.Letter);
                    break;
        case (1, 1):
        case (2, 2):
        case (3, 3):
        case (4, 4):
        case (7, 7): // Center square, also a Double Word
        case (10, 10):
        case (11, 11):
        case (12, 12):
                    Console.WriteLine( "double word!!");
                    wordMultiplier *= 2; // Double the word score
                    break;
                case (0, 0):
                case (0, 7):
                case (0, 14):
                case (7, 0):
                case (7, 14):
                case (14, 0):
                case (14, 7):
                case (14, 14):
                    Console.WriteLine("tripple word!!!");
                    wordMultiplier *= 3; // Triple the word score
                    break;
            }
        }
        
        // Apply the word multipliers to the total score
        roundPoints *= wordMultiplier;

        // Add the points to the player's score
        score += roundPoints;
        Console.WriteLine($"{Name} scored {roundPoints} points this turn. Total Score: {score}");
    }


    // Player's turn: handle actions like placing a word and interacting with the board
    public void PlayerTurn(ScrabbleBoard board, TileBag tileBag)
    {
        int wordRow = 0;
        int rowCurr = 0;
        int wordCol = 0;
        int colCurr = 0;
        bool orientation = false; // true=horizontal, false=vertical;

        Console.WriteLine($"{Name}, it's your turn!");
        ShowRack();
        Console.WriteLine("play the round or draw from tile bag?()");
        if (bool.Parse(Console.ReadLine()))
        {
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
            if (board.CanPlaceWord(wordTiles, wordRow, wordCol, orientation, this))
            {
                board.PlaceWord(wordTiles, wordRow, wordCol, orientation);
                Console.WriteLine("Word placed successfully.");
                AddPoints(wordTiles, wordRow, wordCol, orientation, board);
                Console.WriteLine(score);
                board.PrintBoard();
                board.PrintPlayedWords();
            }
            else
            {
                Console.WriteLine("Invalid word placement. Try again.");
                RefillRack(wordTiles);
                PlayerTurn(board, tileBag);
            }
        }
        else
            {

            char inputLetter;
            do
            {
                Console.WriteLine("Enter letters from your rack to remove (or press '0' to finish):");
                inputLetter = char.Parse(Console.ReadLine().ToUpper());
                Tile tileFromRack = Rack.FirstOrDefault(t => t.Letter == inputLetter);

                if (tileFromRack != null)
                {
                    tileBag.AddTiles(tileFromRack.Letter,1,tileFromRack.Score);
                    RemoveTileFromRack(tileFromRack); // Remove the tile from player's rack once used
                }
                else
                {
                    Console.WriteLine("You don't have that tile in your rack.");
                }

            } while (inputLetter != '0');
            RefillRack(tileBag);
            PlayerTurn(board, tileBag);
        }

        RefillRack(tileBag); // Refill the player's rack after their turn

    }

}
