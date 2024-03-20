using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny
{
    internal class Display
    {
        List<List<WorldTile>> _currentDisplay = new List<List<WorldTile>>();
        WorldTile _player = new WorldTile("@", ConsoleColor.Black, ConsoleColor.Red);
        int[] _playerPosition = new int[2];


        public void SetWorldDisplay(List<List<WorldTile>> worldMap)
        {
            _currentDisplay = worldMap;
        }

        public void PlayerWorldDisplay(int x, int y)
        {
            string apparence = _currentDisplay[_playerPosition[0]][_playerPosition[1]].Apparence;

            Console.SetCursorPosition(_playerPosition[0], _playerPosition[1]);
            Console.BackgroundColor = _currentDisplay[_playerPosition[0]][_playerPosition[1]].ColorBackground;
            Console.ForegroundColor = _currentDisplay[_playerPosition[0]][_playerPosition[1]].ColorText;
            Console.Write(apparence);

            _playerPosition[0] = x;
            _playerPosition[1] = y;


            Console.SetCursorPosition(x, y);
            Console.BackgroundColor = _currentDisplay[_playerPosition[0]][_playerPosition[1]].ColorBackground;
            Console.ForegroundColor = _player.ColorText;
            Console.Write(_player.Apparence);

        }


        public void WorldDisplay()
        {
            Console.SetCursorPosition(0, 0);
            foreach (List<WorldTile> line in _currentDisplay)
            {
                foreach (WorldTile tile in line)
                {
                    if (Console.BackgroundColor != tile.ColorBackground)
                    {
                        Console.BackgroundColor = tile.ColorBackground;
                    }
                    if (Console.ForegroundColor != tile.ColorText)
                    {
                        Console.ForegroundColor = tile.ColorText;
                    }
                    Console.Write(tile.Apparence);
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\n");
            }
        }
    }
}
