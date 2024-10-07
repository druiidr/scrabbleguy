using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scrabbleguy
{
    public class Player
    {
        public string Name { get; private set; }
        public List<Tile> Rack { get; private set; }

        public Player(string name)
        {
            Name = name;
            Rack = new List<Tile>();
        }

        // Add a tile to the player's rack
        public void DrawTile(TileBag tileBag)
        {
            Tile tile = tileBag.DrawTile();
            if (tile != null)
            {
                Rack.Add(tile);
            }
        }

        // Display player's rack (for testing purposes)
        public void ShowRack()
        {
            foreach (Tile tile in Rack)
            {
                Console.Write(tile.Letter + " ");
            }
            Console.WriteLine();
        }
    }

}
