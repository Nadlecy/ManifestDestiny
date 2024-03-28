using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny
{
    internal class WorldMap
    {
        List<List<WorldTile>> _worldMapTiles;
        Dictionary<string, WorldTile> _worldTiles;

        Dictionary<Position, bool> _warpTiles;
        GameManager _gameManager;

        public List<List<WorldTile>> WorldMapTiles { get => _worldMapTiles; }

        public WorldMap(GameManager gameManager)
        {
            _worldTiles = new Dictionary<string, WorldTile>();

            _warpTiles = new Dictionary<Position, bool>();

            _worldMapTiles = new List<List<WorldTile>>();

            WorldTile floor = new WorldTile("▒", ConsoleColor.DarkGreen, ConsoleColor.Green, true);
            _worldTiles.Add("grass", floor);

            WorldTile grass = new WorldTile("▒", ConsoleColor.DarkYellow, ConsoleColor.Yellow, true);
            _worldTiles.Add("floor", grass);

            WorldTile exterior = new WorldTile(" ", ConsoleColor.DarkGray, ConsoleColor.White, false);
            _worldTiles.Add("exterior", exterior);

            WorldTile empty = new WorldTile(" ", ConsoleColor.Black, ConsoleColor.White, false);
            _worldTiles.Add("empty", empty);

            WorldTile water = new WorldTile("~", ConsoleColor.Blue, ConsoleColor.DarkBlue, false);
            _worldTiles.Add("water", water);

            WorldTile heal = new WorldTile("+", ConsoleColor.Red, ConsoleColor.Red, true);
            _worldTiles.Add("heal", heal);

            _gameManager = gameManager;
        }

        public void SetMap(string fileName)
        {
            _gameManager.Map = fileName + ".txt";
            _worldMapTiles.Clear();
            string path = "../../../Data/Map/";
            if (File.Exists(path + fileName + ".txt"))
            {
                string pathGrass = "TallGrass/Grass" + fileName + ".json";

                if (File.Exists("../../../Data/" + pathGrass))
                {
                    CustomJson<EncountersChanceContainer> jsonReaderGrass = new CustomJson<EncountersChanceContainer>(pathGrass);

                    // Lire les données JSON et obtenir la liste de warps
                    EncountersChanceContainer encountersChance = jsonReaderGrass.Read();

                    string[] lines = File.ReadAllLines(path + fileName + ".txt");

                    foreach (string line in lines)
                    {
                        List<WorldTile> row = new List<WorldTile>();

                        foreach (char c in line)
                        {
                            if (c == 'V')
                            {
                                row.Add(_worldTiles["empty"].Clone());
                            }
                            if (c == '/')
                            {
                                row.Add(_worldTiles["floor"].Clone());
                            }
                            else if (c == '#')
                            {
                                row.Add(_worldTiles["grass"].Clone());
                                row[row.Count - 1].EncounterChance = encountersChance.Chance;
                                row[row.Count - 1].Encounters = encountersChance.Encounter;
                                row[row.Count - 1].LevelMin = encountersChance.LevelMin;
                                row[row.Count - 1].LevelMax = encountersChance.LevelMax;
                                row[row.Count - 1].AILevel = encountersChance.AILevel;
                            }
                            else if (c == 'X')
                            {
                                row.Add(_worldTiles["exterior"].Clone());
                            }
                            else if (c == '~')
                            {
                                row.Add(_worldTiles["water"].Clone());
                            }
                            else if (c == '+')
                            {
                                row.Add(_worldTiles["heal"].Clone());
                                row[row.Count - 1].IsHealer = true;
                            }
                        }
                        _worldMapTiles.Add(row);
                    }
                }else
                {
                    string[] lines = File.ReadAllLines(path + fileName + ".txt");

                    foreach (string line in lines)
                    {
                        List<WorldTile> row = new List<WorldTile>();

                        foreach (char c in line)
                        {
                            if (c == 'V')
                            {
                                row.Add(_worldTiles["empty"].Clone());
                            }
                            if (c == '/')
                            {
                                row.Add(_worldTiles["floor"].Clone());
                            }
                            else if (c == '#')
                            {
                                row.Add(_worldTiles["grass"].Clone());
                            }
                            else if (c == 'X')
                            {
                                row.Add(_worldTiles["exterior"].Clone());
                            }
                            else if (c == '~')
                            {
                                row.Add(_worldTiles["water"].Clone());
                            }
                            else if (c == '+')
                            {
                                row.Add(_worldTiles["heal"].Clone());
                                row[row.Count - 1].IsHealer = true;
                            }
                        }
                        _worldMapTiles.Add(row);
                    }
                }

                
            }
            else
            {
                throw new FileNotFoundException("Le fichier JSON n'existe pas.", path + fileName + ".txt");
            }
            if (File.Exists("../../../Data/Warps/Warp" + fileName + ".json"))
            {
                string pathWarp = "Warps/Warp" + fileName + ".json";
                CustomJson<WarpContainer> jsonReader = new CustomJson<WarpContainer>(pathWarp);
                WarpContainer warps = jsonReader.Read();
                foreach (var warp in warps.warps)
                {
                    if (warp.StartMap == fileName)
                    {
                        _worldMapTiles[warp.StartPosition.X][warp.StartPosition.Y].SetWarp(warp);
                    }
                }
            }
            if (File.Exists("../../../Data/Item/Items" + fileName + ".json"))
            {
                string itemWarp = "Item/Items" + fileName + ".json";

                // Créer une instance de JsonReader pour désérialiser une liste de warps
                CustomJson<ItemTilesContainer> jsonReaderItem = new CustomJson<ItemTilesContainer>(itemWarp);

                // Lire les données JSON et obtenir la liste de warps
                ItemTilesContainer itemTiles = jsonReaderItem.Read();

                // Utiliser les données des warps dans votre jeu


                foreach (var itemT in itemTiles.ItemTiles)
                {
                    _worldMapTiles[itemT.Position.X][itemT.Position.Y].AsObject = true;
                    Item it = _gameManager.Items.ItemList[itemT.ItemName].Clone();
                    _worldMapTiles[itemT.Position.X][itemT.Position.Y].Item = it;

                }
            }

        }

    }
}
