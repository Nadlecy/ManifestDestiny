using ManifestDestiny.Helper.Position;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using static GameManager;

namespace ManifestDestiny
{
    internal class Display
    {
        public enum MenuDisplayType
        {
            rightSide, // Pokemon style menu
            leftSide, //Pokemon style menu but on the left side
            bottom,
            battle, // choice of action at start of round
            abilities // choice of attacks during battle
        }

        List<List<WorldTile>> _currentDisplay;
        WorldTile _player;
        Position _playerPosition;
        WorldMap _worldMap;
        GameManager _gameManager;

        public Display(WorldMap worldMap, GameManager gameManager)
        {
            _playerPosition = new Position();
            _playerPosition.X = 0;
            _playerPosition.Y = 0;
            _player = new WorldTile("@", ConsoleColor.Black, ConsoleColor.Black, true);
            _currentDisplay = new List<List<WorldTile>>();
            _worldMap = worldMap;
            _gameManager = gameManager;
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
                        _playerPosition = _currentDisplay[playerX + x][playerY + y].Warp.DestinationPosition.Clone();
                        _worldMap.SetMap(_currentDisplay[playerX + x][playerY + y].Warp.DestinationMap);
                        SetWorldDisplay(_worldMap.WorldMapTiles);
                        WorldDisplay();
                        x = 0;
                        y = 0;
                    }
                    else if (_currentDisplay[playerX + x][playerY + y].EncounterChance != 0)
                    {
                        Random aleatoire = new Random();
                        int chance = aleatoire.Next(1, 100);
                        if (chance <= _currentDisplay[playerX + x][playerY + y].EncounterChance)
                        {
                            int aleaSeraph = 0;
                            foreach (var sera in _currentDisplay[playerX + x][playerY + y].Encounters)
                            {
                                aleaSeraph += sera.Value;
                            }


                            foreach (var sera in _currentDisplay[playerX + x][playerY + y].Encounters)
                            {
                                int seraChoice = aleatoire.Next(1, aleaSeraph);
                                if (seraChoice <= sera.Value)
                                {
                                    //_gameManager.GameState = GameManager.GameStates.StartBattle;
                                    // FAUT LANCER LE COMBAT !!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                                    int level = aleatoire.Next(_currentDisplay[playerX + x][playerY + y].LevelMin, _currentDisplay[playerX + x][playerY + y].LevelMax);

                                    Seraph seraph = _gameManager.Data.Summon(sera.Key, level);

                                    List<Seraph> listSeraph = new List<Seraph>();
                                    listSeraph.Add(seraph);

                                    _gameManager.BattleHandler.StartBattle(listSeraph, _currentDisplay[playerX + x][playerY + y].AILevel);

                                    _gameManager.GameState = GameStates.StartBattle;
                                    MenuDisplay(_gameManager.battleMenu, Display.MenuDisplayType.battle);
                                    break;
                                }
                                else
                                {
                                    aleaSeraph -= sera.Value;
                                }
                            }
                        }
                    }

                    if (_gameManager.GameState == GameStates.Exploration)
                    {
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
                        //Console.WriteLine(_playerPosition.X);
                        //Console.WriteLine(_playerPosition.Y);
                    }
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

        public void BattleDisplay(BattleManager battleManager)
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, 0);
            int topPadding = 23;
            for (int i = 0; i < topPadding; i++)
            {
                if (i == 2)
                {
                    // Display enemy
                    StringBuilder newPadding = new StringBuilder();
                    string name = "DANY LE CAILLOU";
                    for (int j = 0; j < 64 - name.Length - 2; j++)
                    {
                        newPadding.Append(" ");
                    }
                    Console.WriteLine(newPadding + name + "  ");
                }
                else if (i == 15)
                {
                    // Display friendly
                    Seraph playerSeraph = _gameManager.BattleHandler.CurrentPlayer;
                    StringBuilder namePadding = new StringBuilder();
                    string name = playerSeraph.Name;
                    for (int j = 0; j < 64 - name.Length - 2; j++)
                    {
                        namePadding.Append(" ");
                    }
                    Console.WriteLine("  " + name + namePadding);
                    StringBuilder hpPadding = new StringBuilder();
                    string hp = playerSeraph.CurrentStats[Seraph.Stats.hp] + "/" + playerSeraph.BaseStats[Seraph.Stats.hp];
                    for (int j = 0; j < 64 - hp.Length - 2; j++)
                    {
                        hpPadding.Append(" ");
                    }
                    Console.WriteLine("  " + hp + hpPadding.ToString());
                }
                else
                {
                    if (i == topPadding - 1)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                    }
                    StringBuilder newPadding = new StringBuilder();
                    for (int j = 0; j < 64; j++)
                    {
                        newPadding.Append(" ");
                    }
                    Console.WriteLine(newPadding.ToString());
                }
            }
            Console.SetCursorPosition(0, 0);
        }

        public void BattleDisplayUpdate()
        {
            Console.SetCursorPosition(0, 0);
        }

        public void MenuDisplay(Menu menu, MenuDisplayType displayType = MenuDisplayType.leftSide)
        {
            switch (displayType)
            {
                case MenuDisplayType.leftSide:
                    Console.SetCursorPosition(0, 0);
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;

                    switch (menu.LineType)
                    {
                        // ---- TEXT DISPLAY ---- //
                        case Menu.LinesType.text:
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
                                StringBuilder newPadding = new StringBuilder();
                                int paddingLength = maxLength - menu._lines[i].Length;
                                for (int j = 0; j < paddingLength; j++)
                                {
                                    newPadding.Append(" ");
                                }

                                if (i == menu.SelectedLine)
                                {
                                    Console.WriteLine(" ► " + menu._lines[i] + newPadding.ToString());
                                }
                                else
                                {
                                    Console.WriteLine("   " + menu._lines[i] + newPadding.ToString());
                                }
                            }
                            break;

                        // ---- ITEMS DISPLAY ---- //
                        case Menu.LinesType.items:
                            Console.SetCursorPosition(0, 0);
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.ForegroundColor = ConsoleColor.Black;

                            maxLength = 0;
                            for (int i = 0; i < menu.ItemStorage.Items.Count; i++)
                            {
                                if (menu.ItemStorage.Items[i].Name.Length > maxLength)
                                {
                                    maxLength = menu.ItemStorage.Items[i].Name.Length;
                                }
                            }
                            StringBuilder titlePadding = new StringBuilder();
                            for (int i = 0; i < maxLength - menu.Name.Length + 2; i++) // Add 2 cause the menu is 2 characters to the left from the lines
                            {
                                titlePadding.Append(" ");
                            }
                            Console.WriteLine(" " + menu.Name + titlePadding.ToString());

                            for (int i = 0; i < menu.ItemStorage.Items.Count; i++)
                            {
                                // Padding
                                //StringBuilder newPadding = new StringBuilder();
                                //int paddingLength = maxLength - menu.ItemStorage.Items[i].Name.Length;
                                //for (int j = 0; j < paddingLength; j++)
                                //{
                                //    newPadding.Append(" ");
                                //}

                                if (i == menu.SelectedLine)
                                {
                                    Console.WriteLine(" ► " + menu.ItemStorage.Items[i].Name /*+ newPadding.ToString()*/);
                                }
                                else
                                {
                                    Console.WriteLine("   " + menu.ItemStorage.Items[i].Name /*+ newPadding.ToString()*/);
                                }
                            }

                            if (menu.SelectedLine == menu.ItemStorage.Items.Count)
                            {
                                Console.WriteLine(" ► CLOSE");
                            }
                            else
                            {
                                Console.WriteLine("   CLOSE");
                            }


                            break;
                        default:
                            break;
                    }
                    break;
                case MenuDisplayType.battle:
                    Console.SetCursorPosition(0, 0);
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    switch (menu.LineType) {
                        // -- Choice selection -- //
                        case Menu.LinesType.text:
                            int topPadding = 24 - menu._lines.Count;
                            
                            Console.SetCursorPosition(0, topPadding-1);
                            Console.BackgroundColor = ConsoleColor.Gray;
                            for (int i = 0; i < menu._lines.Count; i++)
                            {
                                // Padding
                                StringBuilder newPadding = new StringBuilder();
                                int paddingLength = 61 - menu._lines[i].Length; // 64 = window width 24 = height

                                for (int j = 0; j < paddingLength; j++)
                                {
                                    newPadding.Append(" ");
                                }

                                if (i == menu.SelectedLine)
                                {
                                    Console.WriteLine(" ► " + menu._lines[i] + newPadding.ToString());
                                }
                                else
                                {
                                    Console.WriteLine("   " + menu._lines[i] + newPadding.ToString());
                                }
                            }
                            break;
                        // -- Attack Selection -- //
                        case Menu.LinesType.ability:
                            
                            break;
                    }
                    break;
                case MenuDisplayType.abilities:

                    Seraph playerSeraph = _gameManager.BattleHandler.CurrentPlayer;

                    int topPadding2 = 24 - playerSeraph._abilities.Count;
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.ForegroundColor = ConsoleColor.White;

                    for (int i = 0; i < topPadding2; i++)
                    {
                        // Padding
                        Console.WriteLine("                                                                "); //64
                    }

                    // -- Abilities -- //
                    Console.SetCursorPosition(0, topPadding2 - 1);
                    if(playerSeraph._abilities.Count == 0)
                    {
                        Console.WriteLine(playerSeraph.Name + " doesn't have any abilities. What a loser.");
                    } else
                    {
                        Console.WriteLine(playerSeraph.Name + " has " + playerSeraph._abilities.Count + " abilities:");
                    }


                    for (int i = 0; i < playerSeraph._abilities.Count; i++)
                    {
                        // Padding
                        StringBuilder newPadding = new StringBuilder();
                        int paddingLength = 61 - playerSeraph._abilities[i].Name.Length; // 64 = window width 24 = height

                        for (int j = 0; j < paddingLength; j++)
                        {
                            newPadding.Append(" ");
                        }

                        if (i == menu.SelectedLine)
                        {
                            Console.WriteLine(" ► " + playerSeraph._abilities[i] + newPadding.ToString());
                        }
                        else
                        {
                            Console.WriteLine("   " + playerSeraph._abilities[i] + newPadding.ToString());
                        }
                    }

                    break;
                default:
                    throw new ArgumentException("Menu Display Type is not in list");
            }
        }
    }
}
