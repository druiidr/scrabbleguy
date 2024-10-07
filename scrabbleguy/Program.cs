using scrabbleguy;

class Program
{
    static void Main(string[] args)
    {
        ScrabbleBoard board = new ScrabbleBoard();
        TileBag tileBag = new TileBag();
        Player player1 = new Player("Alice");

        // Player draws 7 tiles
        for (int i = 0; i < 7; i++)
        {
            player1.DrawTile(tileBag);
        }

        // Display the board and player's rack
        board.PrintBoard();
        player1.ShowRack();
    }
}
