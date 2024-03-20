using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny
{
    internal class Display
    {
        List<List<WorldTile>> _currentWorldDisplay = new List<List<WorldTile>>();

        public void SetWorldDisplay(List<List<WorldTile>> worldMap)
        {
            _currentWorldDisplay = worldMap;
        }

        public void PlayerWorldDisplay()
        {

        }

        public void ShowDisplay()
        {
            foreach (List<WorldTile> line in _currentWorldDisplay)
            {
                foreach (WorldTile tile in line)
                {
                    Console.BackgroundColor = tile.ColorBackground;
                    Console.ForegroundColor = tile.ColorText;
                    Console.Write(tile.Apparence);
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\n");
            }
        }
    }
}
