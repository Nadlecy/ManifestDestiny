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
            _player = new WorldTile("@", ConsoleColor.Black, ConsoleColor.DarkRed, true);
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
                        if (tile.IsWarp == false)
                        {
                            Console.SetCursorPosition(playerY, playerX);
                            Console.BackgroundColor = _currentDisplay[playerX][playerY].ColorBackground;
                            Console.ForegroundColor = _currentDisplay[playerX][playerY].ColorText;
                            Console.Write(apparence);
                        }

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
            _gameManager.PlayerPosition = _playerPosition;
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
                Console.WriteLine("");
            }
        }

        public void HPBarCreate(int currentStats, int maxStats, ConsoleColor colorBar, ConsoleColor negaColorBar, int paddingLeft, int paddingRight)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            // Calculer le pourcentage de points de vie restants
            double percentage = (double)currentStats / maxStats;

            // Déterminer le nombre de caractères verts à afficher pour les points de vie actuels
            int Chars = (int)(percentage * 20);

            Console.Write(Padding(64,paddingLeft) + "[");

            // Afficher les points de vie actuels en vert
            Console.BackgroundColor = colorBar;
            for (int i = 0; i < Chars; i++)
            {
                Console.Write(" ");
            }

            // Afficher les points de vie manquants en rouge
            Console.BackgroundColor = negaColorBar;
            for (int i = Chars; i < 20; i++)
            {
                Console.Write(" ");
            }

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.WriteLine("]" + Padding(64,paddingRight)); 

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
                    Seraph enemy = battleManager.CurrentEnemy;
                    Console.WriteLine(Padding(64, -enemy.Name.Length - enemy.Level.ToString().Length - 9) + enemy.Name + "  " + "LVL: " + enemy.Level + "  ");

                    StringBuilder hpPadding = new StringBuilder();
                    string hp = "HP:" + enemy.CurrentStats[Seraph.Stats.hp] + "/" + enemy.BaseStats[Seraph.Stats.hp] + "  ";
                    string mp = "MP:" + enemy.CurrentStats[Seraph.Stats.mana] + "/" + enemy.BaseStats[Seraph.Stats.mana];

                    //Console.ForegroundColor = ConsoleColor.Red;
                    //Console.Write(Padding(64, -hp.Length - mp.Length - 2) + hp);
                    //Console.ForegroundColor = ConsoleColor.DarkBlue;
                    //Console.WriteLine(mp + "  ");
                    HPBarCreate(enemy.CurrentStats[Seraph.Stats.hp], enemy.BaseStats[Seraph.Stats.hp], ConsoleColor.Green, ConsoleColor.Red, -24, -62);
                    Console.WriteLine(Padding(64));
                    HPBarCreate(enemy.CurrentStats[Seraph.Stats.mana], enemy.BaseStats[Seraph.Stats.mana], ConsoleColor.Blue, ConsoleColor.DarkGray, -24, -62);
                    //Console.ForegroundColor = ConsoleColor.Black;
                }
                else if (i == 10)
                {
                    // Display friendly
                    StringBuilder namePadding = new StringBuilder();
                    string name = battleManager.CurrentPlayer.Name;
                    for (int j = 0; j < 64 - name.Length - battleManager.CurrentPlayer.Level.ToString().Length - 9; j++)
                    {
                        namePadding.Append(" ");
                    }
                    Console.WriteLine("  " + name + "  " + "LVL: " + battleManager.CurrentPlayer.Level + namePadding);
                    StringBuilder hpPadding = new StringBuilder();
                    string xp = "  EXP: " + battleManager.CurrentPlayer.Experience;
                    string hp = "  HP:" + battleManager.CurrentPlayer.CurrentStats[Seraph.Stats.hp] + "/" + battleManager.CurrentPlayer.BaseStats[Seraph.Stats.hp] ;
                    string mp = "  MP:" + battleManager.CurrentPlayer.CurrentStats[Seraph.Stats.mana] + "/" + battleManager.CurrentPlayer.BaseStats[Seraph.Stats.mana];
                    for (int j = 0; j < 64 - hp.Length - mp.Length - xp.Length; j++)
                    {
                        hpPadding.Append(" ");
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(hp);
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write(mp);
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(xp + hpPadding);
                    HPBarCreate(battleManager.CurrentPlayer.CurrentStats[Seraph.Stats.hp], battleManager.CurrentPlayer.BaseStats[Seraph.Stats.hp], ConsoleColor.Green, ConsoleColor.Red, -62, -24);
                    Console.WriteLine(Padding(64, 0));
                    HPBarCreate(battleManager.CurrentPlayer.CurrentStats[Seraph.Stats.mana], battleManager.CurrentPlayer.BaseStats[Seraph.Stats.mana], ConsoleColor.Blue, ConsoleColor.DarkGray, -62, -24);
                    Console.ForegroundColor = ConsoleColor.Black;
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
                            
                            Console.WriteLine(" " + menu.Name + Padding(menu._lines, -menu.Name.Length + 2));

                            for (int i = 0; i < menu._lines.Count; i++)
                            {
                                if (i == menu.SelectedLine)
                                {
                                    Console.WriteLine(" > " + menu._lines[i] + Padding(menu._lines, -menu._lines[i].Length));
                                }
                                else
                                {
                                    Console.WriteLine("   " + menu._lines[i] + Padding(menu._lines, -menu._lines[i].Length));
                                }
                            }
                            break;

                        // ---- ITEMS DISPLAY ---- //
                        case Menu.LinesType.items:
                            Console.SetCursorPosition(0, 0);
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.ForegroundColor = ConsoleColor.Black;

                            Console.WriteLine(" " + menu.Name + Padding(menu.ItemStorage.Items, -menu.Name.Length + 4));

                            for (int i = 0; i < menu.ItemStorage.Items.Count; i++)
                            {
                                if (i == menu.SelectedLine)
                                {
                                    Console.WriteLine(" > " + menu.ItemStorage.Items[i].Name + " x" + menu.ItemStorage.Items[i].Count + Padding(menu.ItemStorage.Items, -menu.ItemStorage.Items[i].Name.Length - 2));
                                }
                                else
                                {
                                    Console.WriteLine("   " + menu.ItemStorage.Items[i].Name + " x" + menu.ItemStorage.Items[i].Count + Padding(menu.ItemStorage.Items, -menu.ItemStorage.Items[i].Name.Length - 2));
                                }
                            }

                            if (menu.SelectedLine == menu.ItemStorage.Items.Count)
                            {
                                Console.WriteLine(" > CLOSE" + Padding(menu.ItemStorage.Items, - 3));
                            }
                            else
                            {
                                Console.WriteLine("   CLOSE" + Padding(menu.ItemStorage.Items, - 3));
                            }


                            break;

                        // SERAPH MENU
                        case Menu.LinesType.seraph:
                            Console.SetCursorPosition(0, 0);
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;

                            Console.WriteLine(" " + menu.Name + Padding(menu.Seraphim, -menu.Name.Length + 6));

                            for (int i = 0; i < menu.Seraphim.Count; i++)
                            {
                                if (i == menu.SelectedLine)
                                {
                                    Console.WriteLine(" > " + menu.Seraphim[i].Name + " LVL" + menu.Seraphim[i].Level.ToString() + Padding(menu.Seraphim, -menu.Seraphim[i].Name.Length - menu.Seraphim[i].Level.ToString().Length - 4));
                                }
                                else
                                {
                                    Console.WriteLine("   " + menu.Seraphim[i].Name + " LVL" + menu.Seraphim[i].Level.ToString() + Padding(menu.Seraphim, -menu.Seraphim[i].Name.Length - menu.Seraphim[i].Level.ToString().Length - 4));
                                }
                            }

                            if (menu.SelectedLine == menu.Seraphim.Count)
                            {
                                Console.WriteLine(" > CLOSE" + Padding(menu.Seraphim,-1));
                            }
                            else
                            {
                                Console.WriteLine("   CLOSE" + Padding(menu.Seraphim,-1));
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
                    switch (menu.LineType)
                    {
                        // -- Choice selection -- //
                        case Menu.LinesType.text:
                            int topPadding = 24 - menu._lines.Count;

                            Console.SetCursorPosition(0, topPadding - 1);
                            Console.BackgroundColor = ConsoleColor.Gray;
                            for (int i = 0; i < menu._lines.Count; i++)
                            {
                                if (i == menu.SelectedLine)
                                {
                                    Console.WriteLine(" > " + menu._lines[i] + Padding(61, -menu._lines[i].Length));
                                }
                                else
                                {
                                    Console.WriteLine("   " + menu._lines[i] + Padding(61, -menu._lines[i].Length));
                                }
                            }
                            Console.WriteLine(Padding(64));
                            break;
                    }
                    break;
                case Menu.MenuDisplayType.abilities:

                    int topPadding2 = 22 - _gameManager.BattleHandler.CurrentPlayer._abilities.Count;
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;

                    for (int i = 0; i < topPadding2; i++)
                    {
                        // Padding
                        Console.WriteLine("                                                                "); //64
                    }

                    // -- Abilities -- //
                    Console.SetCursorPosition(0, topPadding2 - 1);
                    if (_gameManager.BattleHandler.CurrentPlayer._abilities.Count == 0)
                    {
                        Console.WriteLine(_gameManager.BattleHandler.CurrentPlayer.Name + " does not have any available abilities.");
                    }
                    else
                    {
                        Console.WriteLine(_gameManager.BattleHandler.CurrentPlayer.Name + " has " + _gameManager.BattleHandler.CurrentPlayer._abilities.Count + " abilities:");
                    }

                    for (int i = 0; i < _gameManager.BattleHandler.CurrentPlayer._abilities.Count; i++)
                    {
                        if (i == menu.SelectedLine)
                        {
                            Console.Write(" > ");
                        }
                        else
                        {
                            Console.Write("   ");                         
                        }
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.Write(_gameManager.BattleHandler.CurrentPlayer._abilities[i].Cost + " MP  ");
                        Console.ForegroundColor = _gameManager.BattleHandler.CurrentPlayer._abilities[i].BattleType.Color;
                        Console.Write(_gameManager.BattleHandler.CurrentPlayer._abilities[i].BattleType.Name + "  " );
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write(_gameManager.BattleHandler.CurrentPlayer._abilities[i].Name );

                        Console.WriteLine(Padding(61, -(_gameManager.BattleHandler.CurrentPlayer._abilities[i].Name.Length + _gameManager.BattleHandler.CurrentPlayer._abilities[i].Cost.ToString().Length + _gameManager.BattleHandler.CurrentPlayer._abilities[i].BattleType.Name.Length + 7)));

                    }

                    // CLOSE button
                    if (menu.SelectedLine == menu.Abilities.Count)
                    {
                        Console.WriteLine(" > CLOSE" + Padding(64,-8));
                    }
                    else
                    {
                        Console.WriteLine("   CLOSE" + Padding(64, -8));
                    }
                    Console.WriteLine("                                                                ");

                    break;
                default:
                    throw new ArgumentException("Menu Display Type is not in list");
            }
        }
        
        public string Padding(List<string> input, int offset = 0)
        {
            int maxLength = 0;
            for (int i = 0; i < input.Count; i++)
            {
                if (input[i].Length > maxLength)
                {
                    maxLength = input[i].Length + 1;
                }
            }
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < maxLength + offset; i++)
            {
                stringBuilder.Append(" ");
            }
            return stringBuilder.ToString();
        }

        public string Padding(List<Item> input, int offset = 0)
        {
            int maxLength = 0;
            for (int i = 0; i < input.Count; i++)
            {
                if (input[i].Name.Length > maxLength)
                {
                    maxLength = input[i].Name.Length + 1;
                }
            }
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < maxLength + offset; i++)
            {
                stringBuilder.Append(" ");
            }
            return stringBuilder.ToString();
        }

        public string Padding(List<Seraph> input, int offset = 0)
        {
            int maxLength = 0;
            for (int i = 0; i < input.Count; i++)
            {
                if (input[i].Name.Length > maxLength)
                {
                    maxLength = input[i].Name.Length + 1;
                }
            }
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < maxLength + offset; i++)
            {
                stringBuilder.Append(" ");
            }
            return stringBuilder.ToString();
        }

        public string Padding(int length, int offset = 0)
        {
            int maxLength = length;
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < maxLength + offset; i++)
            {
                stringBuilder.Append(" ");
            }
            return stringBuilder.ToString();
        }

        public void BubbleDisplay(List<string> bubbleList)
        {
            Console.SetCursorPosition(0, 20);
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;

            string[] splitWords = bubbleList[0].Split(" ");

            string[] lines = { "    ", "    ", "    ", "    " };
            int lineCounter = 0;

            foreach (string word in splitWords)
            {
                if(lines[lineCounter].Length + word.Length + 1 < 60)
                {
                    lines[lineCounter] += word + " ";
                }
                else
                {
                    lineCounter++; 
                    lines[lineCounter] += word + " ";
                }
            }

            foreach(string line in lines)
            {
                Console.WriteLine(line + Padding(64, -line.Length));
            }
        }
    }
}