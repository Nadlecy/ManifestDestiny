﻿using ManifestDestiny.Helper.Position;
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
            _player = new WorldTile("♀", ConsoleColor.Black, ConsoleColor.DarkRed, true);
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

            WorldTile tile = _currentDisplay[playerX + x][playerY + y];

            if (playerX + x >= 0 && playerY + y >= 0 && _currentDisplay.Count() > playerX + x && _currentDisplay[playerX + x].Count() > playerY + y)
            {
                if (tile.Walkable == true)
                {
                    string apparence = _currentDisplay[playerX][playerY].Apparence;

                    if (tile.IsWarp == true)
                    {
                        _playerPosition = tile.Warp.DestinationPosition.Clone();
                        _worldMap.SetMap(tile.Warp.DestinationMap);
                        SetWorldDisplay(_worldMap.WorldMapTiles);
                        WorldDisplay();
                        x = 0;
                        y = 0;
                    }
                    else if (tile.AsObject == true)
                    {
                        _gameManager.Inventory.AddItem(tile.GiveObject());
                    }
                    else if (tile.EncounterChance != 0)
                    {
                        Random aleatoire = new Random();
                        int chance = aleatoire.Next(1, 100);
                        if (chance <= tile.EncounterChance)
                        {
                            int aleaSeraph = 0;
                            foreach (var sera in tile.Encounters)
                            {
                                aleaSeraph += sera.Value;
                            }

                            foreach (var sera in tile.Encounters)
                            {
                                int seraChoice = aleatoire.Next(1, aleaSeraph);
                                if (seraChoice <= sera.Value)
                                {
                                    //_gameManager.GameState = GameManager.GameStates.StartBattle;
                                    // FAUT LANCER LE COMBAT !!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                                    int level = aleatoire.Next(tile.LevelMin, tile.LevelMax);
                                    Seraph seraph = _gameManager.Data.Summon(sera.Key, level);
                                    List<Seraph> listSeraph = new List<Seraph>();
                                    listSeraph.Add(seraph);
                                    _gameManager.BattleHandler.StartBattle(listSeraph, tile.AILevel);
                                    _gameManager.GameState = GameStates.StartBattle;
                                    _gameManager.CurrentMenu = _gameManager.battleMenu;
                                    //MenuDisplay(_gameManager.battleMenu);
                                    break;
                                }
                                else
                                {
                                    aleaSeraph -= sera.Value;
                                }
                            }
                        }
                    }

                    if (_gameManager.GameState == GameStates.Exploration || _gameManager.GameState == GameStates.StartExploration)
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
                        Console.WriteLine("Player X Posittion: " + _playerPosition.X);
                        Console.WriteLine("Player Y Posittion: " + _playerPosition.Y);
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

                    if (tile.AsObject == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(tile.Item.Character);
                        Console.ForegroundColor = tile.ColorText;
                    }
                    else
                    {
                        Console.Write(tile.Apparence);
                    }
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
            int topPadding = 22;
            for (int i = 0; i < topPadding; i++)
            {
                if (i == 2)
                {
                    // Display enemy
                    StringBuilder newPadding = new StringBuilder();
                    Seraph enemy = battleManager.CurrentEnemy;
                    for (int j = 0; j < 64 - enemy.Name.Length - 2; j++)
                    {
                        newPadding.Append(" ");
                    }
                    Console.WriteLine(newPadding + enemy.Name + "  ");

                    StringBuilder hpPadding = new StringBuilder();
                    string hp = "HP:" + enemy.CurrentStats[Seraph.Stats.hp] + "/" + enemy.BaseStats[Seraph.Stats.hp];
                    for (int j = 0; j < 64 - hp.Length - 2; j++)
                    {
                        hpPadding.Append(" ");
                    }
                    Console.WriteLine(hpPadding.ToString() + hp + "  ");
                }
                else if (i == 15)
                {
                    // Display friendly
                    Seraph playerSeraph = battleManager.CurrentPlayer;
                    StringBuilder namePadding = new StringBuilder();
                    string name = playerSeraph.Name;
                    for (int j = 0; j < 64 - name.Length - 2; j++)
                    {
                        namePadding.Append(" ");
                    }
                    Console.WriteLine("  " + name + namePadding);
                    StringBuilder hpPadding = new StringBuilder();
                    string hp = "HP:" + playerSeraph.CurrentStats[Seraph.Stats.hp] + "/" + playerSeraph.BaseStats[Seraph.Stats.hp];
                    for (int j = 0; j < 64 - hp.Length - 2; j++)
                    {
                        hpPadding.Append(" ");
                    }
                    Console.WriteLine("  " + hp + hpPadding.ToString());
                }
                else
                {
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

        public void MenuDisplay(Menu menu)
        {
            Menu.MenuDisplayType displayType = menu.DisplayType;
            switch (displayType)
            {
                case Menu.MenuDisplayType.leftSide:
                    Console.SetCursorPosition(0, 0);
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    StringBuilder titlePadding;

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
                            titlePadding = new StringBuilder();
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
                                    Console.WriteLine(" ► " + menu.ItemStorage.Items[i].Name + " x" + menu.ItemStorage.Items[i].Count /*+ newPadding.ToString()*/);
                                }
                                else
                                {
                                    Console.WriteLine("   " + menu.ItemStorage.Items[i].Name + " x" + menu.ItemStorage.Items[i].Count /*+ newPadding.ToString()*/);
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

                        //SERAPH MENU
                        case Menu.LinesType.seraph:
                            Console.SetCursorPosition(0, 0);
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;

                            maxLength = 0;
                            for (int i = 0; i < menu.Seraphim.Count; i++)
                            {
                                if (menu.Seraphim[i].Name.Length > maxLength)
                                {
                                    maxLength = menu.Seraphim[i].Name.Length + 1;
                                }
                            }
                            titlePadding = new StringBuilder();
                            for (int i = 0; i < maxLength - menu.Name.Length + 2; i++) // Add 2 cause the menu is 2 characters to the left from the lines
                            {
                                titlePadding.Append(" ");
                            }
                            Console.WriteLine(" " + menu.Name + titlePadding.ToString());

                            for (int i = 0; i < menu.Seraphim.Count; i++)
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
                                    Console.WriteLine(" ► " + menu.Seraphim[i].Name /*+ newPadding.ToString()*/);
                                }
                                else
                                {
                                    Console.WriteLine("   " + menu.Seraphim[i].Name /*+ newPadding.ToString()*/);
                                }
                            }

                            if (menu.SelectedLine == menu.Seraphim.Count)
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
                case Menu.MenuDisplayType.battle:
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
                    }
                    break;
                case Menu.MenuDisplayType.abilities:

                    Seraph playerSeraph = _gameManager.BattleHandler.CurrentPlayer;

                    int topPadding2 = 23 - playerSeraph._abilities.Count;
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
                            Console.WriteLine(" ► " + playerSeraph._abilities[i].Name + newPadding.ToString());
                        }
                        else
                        {
                            Console.WriteLine("   " + playerSeraph._abilities[i].Name + newPadding.ToString());
                        }
                    }

                    // CLOSE button
                    // Padding
                    StringBuilder closePadding = new StringBuilder();
                    int closePaddingLength = 61 - "CLOSE".Length; // 64 = window width 24 = height
                    for (int j = 0; j < closePaddingLength; j++)
                    {
                        closePadding.Append(" ");
                    }

                    if (menu.SelectedLine == menu.Abilities.Count)
                    {
                        Console.WriteLine(" ► CLOSE"+ closePadding.ToString());
                    }
                    else
                    {
                        Console.WriteLine("   CLOSE"+ closePadding.ToString());
                    }

                    break;
                default:
                    throw new ArgumentException("Menu Display Type is not in list");
            }
        }
    }
}