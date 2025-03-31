using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;

namespace scrabbleguy
{
    class Program
    {
        static void Main(string[] args)
        {
            ScrabbleBoard board = new ScrabbleBoard();
            TileBag tileBag = new TileBag();
            ScrabbleDictionary scrabbleDictionary = new ScrabbleDictionary(@"C:\Users\ASUS\source\repos\druiidr\scrabbleguy\scrabbleguy\fullScrabbleLegalDictionary.txt");
            //modify filepath when downloading
            Player player1 = new Player("player1");
            Console.WriteLine("play against ai?(true/false)");
            bool isPlayer2Ai = bool.Parse(Console.ReadLine());
            Player player2;
            if (isPlayer2Ai)
            {
                player2 = new AIPlayer("player2", scrabbleDictionary);
            }
            else
            {
                player2 = new Player("player2");
            }
            // Player draws 7 tiles
            for (int i = 0; i < 7; i++)
            {
                player1.DrawTile(tileBag);
                player2.DrawTile(tileBag);
            }

            // Display the board and player's rack
            board.PrintBoard();

            // Game loop until the tile bag is empty
            while (!tileBag.IsEmpty())
            {
                // Player's turn
               
                    player1.PlayerTurn(board, tileBag);
                
                if (isPlayer2Ai)
                {
                    ((AIPlayer)player2).ExecuteBestMove(board, tileBag);
                }
                else
                {
                    player2.PlayerTurn(board, tileBag);
                }

            }

                Player winner = player1.score > player2.score ? player1 : player2;
                Console.WriteLine("Sack cleared. {0} won", winner.Name);
        }
    }

}
