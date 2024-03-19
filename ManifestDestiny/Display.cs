using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny
{
    internal class Display
    {
        List<List<WorldTile>> _currentDisplay;

        public void SetDiplay(List<List<WorldTile>> worldMap)
        {
            _currentDisplay = worldMap;
        }

        public void ShowDisplay()
        {
            foreach (List<WorldTile> line in _currentDisplay)
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
