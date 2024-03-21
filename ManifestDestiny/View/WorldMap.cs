using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManifestDestiny.Helper.Json;
using ManifestDestiny.Helper.Position;
using ManifestDestiny.View;

namespace ManifestDestiny
{
    internal class WorldMap
    {
        List<List<WorldTile>> _worldMapTiles;
        Dictionary<string, WorldTile> _worldTiles;

        Dictionary<Position, bool> _warpTiles;

        public List<List<WorldTile>> WorldMapTiles { get => _worldMapTiles; }

        public WorldMap()
        {
            _worldTiles = new Dictionary<string, WorldTile>();

            _warpTiles = new Dictionary<Position, bool>();

            _worldMapTiles = new List<List<WorldTile>>();

            WorldTile floor = new WorldTile("▒", ConsoleColor.DarkGreen, ConsoleColor.Green, true);
            _worldTiles.Add("floor", floor);

            WorldTile grass = new WorldTile("▒", ConsoleColor.DarkYellow, ConsoleColor.Yellow, true);
            _worldTiles.Add("grass", grass);

            WorldTile exterior = new WorldTile(" ", ConsoleColor.DarkGray, ConsoleColor.White, false);
            _worldTiles.Add("exterior", exterior);
        }


        public void SetMap(string textFile)
        {
            string path = "../../../Map/";
            if (File.Exists(path + textFile))
            { 
                string[] lines = File.ReadAllLines(path + textFile); 

                foreach (string line in lines)
                {
                    List<WorldTile> row = new List<WorldTile>();

                    foreach (char c in line)
                    {
                        if (c == 'f')
                        {
                            row.Add(_worldTiles["floor"].Clone());
                        }
                        else if (c == 'g')
                        {
                            row.Add(_worldTiles["grass"].Clone());
                        }
                        else if (c == 'e')
                        {
                            row.Add(_worldTiles["exterior"].Clone());
                        }
                    }
                    _worldMapTiles.Add(row);
                }
            }
            else
            {
                Console.WriteLine("File do not exist");
            }


            // Créer une instance de JsonReader pour désérialiser une liste de warps
            CustomJson<WarpContainer> jsonReader = new CustomJson<WarpContainer>("Warp.json");

            // Lire les données JSON et obtenir la liste de warps
            WarpContainer warps = jsonReader.Read();

            // Utiliser les données des warps dans votre jeu
            foreach (var warp in warps.warps)
            {
                if (warp.StartMap == textFile)
                {
                    _worldMapTiles[warp.StartPosition.X][warp.StartPosition.Y].SetWarp(warp);
                }
            }
        }

    }
}
