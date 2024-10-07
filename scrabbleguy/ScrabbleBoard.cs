using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrabbleguy
{
    public class ScrabbleBoard
    {
        public char[,] Board { get; private set; }

        public ScrabbleBoard()
        {
            Board = new char[15, 15]; // Initialize the board with a 15x15 grid
            InitializeBoard();
        }

        // Initialize board with empty spaces
        private void InitializeBoard()
        {
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    Board[i, j] = ' '; // Represent empty space as ' '
                }
            }
        }

        // Print board to the console (for testing purposes)
        public void PrintBoard()
        {
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    Console.Write(Board[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }

}
