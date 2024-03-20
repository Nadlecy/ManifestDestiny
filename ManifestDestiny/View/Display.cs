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
        WorldTile _player = new WorldTile("@", ConsoleColor.Black, ConsoleColor.Black, true);
        int[] _playerPosition = { 0, 0 };


        public void SetWorldDisplay(List<List<WorldTile>> worldMap)
        {
            _currentDisplay = worldMap;
        }
        public void SetPlayerPosition(int x, int y)
        {
            _playerPosition[0] = x;
            _playerPosition[1] = y;
            PlayerWorldDisplay(0, 0);
        }

        public void PlayerWorldDisplay(int x, int y)
        {
            int playerX = _playerPosition[0];
            int playerY = _playerPosition[1];

            if (playerX + x >= 0 && playerY + y >= 0 && _currentDisplay.Count() > playerX + x && _currentDisplay[playerX + x].Count() > playerY + y)
            {
                if (_currentDisplay[playerX + x][playerY + y].Walkable == true)
                {
                    string apparence = _currentDisplay[playerX][playerY].Apparence;

                    Console.SetCursorPosition(playerY, playerX);
                    Console.BackgroundColor = _currentDisplay[playerX][playerY].ColorBackground;
                    Console.ForegroundColor = _currentDisplay[playerX][playerY].ColorText;
                    Console.Write(apparence);

                    _playerPosition[0] += x;
                    _playerPosition[1] += y;

                    playerX = _playerPosition[0];
                    playerY = _playerPosition[1];

                    Console.SetCursorPosition(playerY, playerX);
                    Console.BackgroundColor = _currentDisplay[playerX][playerY].ColorBackground;
                    Console.ForegroundColor = _player.ColorText;
                    Console.Write(_player.Apparence);

                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(0, Console.WindowHeight - 1);

                }
            }
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
