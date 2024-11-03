using scrabbleguy;   

class Program
{
    static void Main(string[] args)
    {
        
        ScrabbleBoard board = new ScrabbleBoard();
        TileBag tileBag = new TileBag();
        Player player1 = new Player("Alice");
        Player player2 = new Player("steve");

        // Player draws 7 tiles
        for (int i = 0; i < 7; i++)
        {
            player1.DrawTile(tileBag);
            player2.DrawTile(tileBag);
        }

        // Display the board and player's rack
        board.PrintBoard();
        player1.ShowRack();

        // Game loop until the tile bag is empty
        while (!tileBag.IsEmpty())
        {
            // Player's turn
            player1.PlayerTurn(board, tileBag);
            player2.PlayerTurn(board, tileBag);
        }
        Player winner;
        if (player1.score < player2.score) { winner=player1; }
        else { winner=player2; }
        Console.WriteLine("Sack cleared. {0}  won",winner.Name);
    }
}
