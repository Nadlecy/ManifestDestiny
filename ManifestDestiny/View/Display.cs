using ManifestDestiny.Helper.Position;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny
{
    internal class Display
    {
        public enum MenuDisplayType
        {
            rightSide, // Pokemon style menu
            leftSide, //Pokemon style menu but on the left side
            bottom,
            battle,
            list
        }

        List<List<WorldTile>> _currentDisplay;
        WorldTile _player;
        Position _playerPosition;

        public Display()
        {
            _playerPosition = new Position();
            _playerPosition.X = 0;
            _playerPosition.Y = 0;
            _player = new WorldTile("@", ConsoleColor.Black, ConsoleColor.Black, true);
            _currentDisplay = new List<List<WorldTile>>();
        }

        public void SetWorldDisplay(List<List<WorldTile>> worldMap)
        {
            _currentDisplay = worldMap;
        }
        public void SetPlayerPosition(int x, int y)
        {
            _playerPosition.X = x;
            _playerPosition.Y = y;
            PlayerWorldDisplay(0, 0);
        }

        public void PlayerWorldDisplay(int x, int y)
        {
            int playerX = _playerPosition.X;
            int playerY = _playerPosition.Y;

            if (playerX + x >= 0 && playerY + y >= 0 && _currentDisplay.Count() > playerX + x && _currentDisplay[playerX + x].Count() > playerY + y)
            {
                if (_currentDisplay[playerX + x][playerY + y].Walkable == true)
                {
                    string apparence = _currentDisplay[playerX][playerY].Apparence;

                    if (_currentDisplay[playerX + x][playerY + y].IsWarp == true)
                    {
                        
                    }

                    Console.SetCursorPosition(playerY, playerX);
                    Console.BackgroundColor = _currentDisplay[playerX][playerY].ColorBackground;
                    Console.ForegroundColor = _currentDisplay[playerX][playerY].ColorText;
                    Console.Write(apparence);

                    _playerPosition.X += x;
                    _playerPosition.Y += y;

                    playerX = _playerPosition.X;
                    playerY = _playerPosition.Y;

                    Console.SetCursorPosition(playerY, playerX);
                    Console.BackgroundColor = _currentDisplay[playerX][playerY].ColorBackground;
                    Console.ForegroundColor = _player.ColorText;
                    Console.Write(_player.Apparence);

                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(0, Console.WindowHeight - 5);
                    Console.WriteLine(_playerPosition.X);
                    Console.WriteLine(_playerPosition.Y);
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

        public void MenuDisplay(Menu menu, MenuDisplayType displayType = MenuDisplayType.leftSide)
        {
            switch (displayType)
            {
                case MenuDisplayType.leftSide:
                    Console.SetCursorPosition(0, 0);
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;

                    int maxLength = 0;
                    for (int i = 0; i < menu._lines.Count; i++)
                    {
                        if (menu._lines[i].Length > maxLength)
                        {
                            maxLength = menu._lines[i].Length;
                        }
                    }
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < maxLength - menu.Name.Length + 2; i++) // Add 2 cause the menu is 2 characters to the left from the lines
                    {
                        sb.Append(" ");
                    }
                    Console.WriteLine(" " + menu.Name + sb.ToString());

                    for (int i = 0; i < menu._lines.Count; i++)
                    {
                        // Padding
                        StringBuilder padding = new StringBuilder();
                        int paddingLength = maxLength - menu._lines[i].Length;
                        for (int j = 0; j < paddingLength; j++)
                        {
                            padding.Append(" ");
                        }

                        if (i == menu.SelectedLine)
                        {
                            Console.WriteLine(" ► " + menu._lines[i] + padding.ToString());
                        }
                        else
                        {
                            Console.WriteLine("   " + menu._lines[i] + padding.ToString());
                        }
                    }
                    break;
                default: Console.Write("Menu Display Type is not in list ?\n"); break;
            }
        }

        public void BagDisplay(Menu bagMenu, ItemStorage inventory)
        {
            int selectedItem = 0;
            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            StringBuilder padding = new StringBuilder();

            int maxLength = 0;
            /*for (int i = 0; i < bag._lines.Count; i++)
            {
                if (menu._lines[i].Length > maxLength)
                {
                    maxLength = menu._lines[i].Length;
                }
            }*/

            Console.WriteLine(" BAG  " + padding.ToString());

            List<string> lines = new List<string>();

            //foreach (KeyValuePair<Item, int> entry in inventory.Items)
            //{
            //    ////Padding
            //    //int paddingLength = maxLength - lines._lines[i].Length;
            //    //for (int j = 0; j < paddingLength; j++)
            //    //{
            //    //    padding.Append(" ");
            //    //}
            //    lines.Add(entry.Key.Name + " x" + entry.Value);
            //   //Console.WriteLine("   " + entry.Value + "x" + entry.Key.Name);
            //}

            for (int i = 0; i < lines.Count; i++)
            {
                if (i == selectedItem)
                {
                    Console.WriteLine(" ► " + lines[i].ToString());
                } else
                {
                    Console.WriteLine("   " + lines[i].ToString());
                }
            }
        }
    }
}
