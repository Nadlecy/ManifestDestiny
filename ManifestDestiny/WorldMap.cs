using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny
{
    internal class WorldMap
    {
        List<List<WorldTile>> _worldMapTiles = new List<List<WorldTile>>();
        Dictionary<string, WorldTile> _worldTiles = new Dictionary<string, WorldTile>();
        int[] _playerPosition = new int[2];

        public List<List<WorldTile>> WorldMapTiles { get => _worldMapTiles; }

        public void Init ()
        {
            WorldTile floor = new WorldTile("▒", ConsoleColor.Gray, ConsoleColor.DarkGray);
            _worldTiles.Add("floor", floor);

            WorldTile grass = new WorldTile("▓", ConsoleColor.Green, ConsoleColor.DarkGreen);
            _worldTiles.Add("grass", grass);

            WorldTile exterior = new WorldTile(" ", ConsoleColor.DarkGray, ConsoleColor.White);
            _worldTiles.Add("exterior", exterior);

            WorldTile player = new WorldTile("@", ConsoleColor.Black, ConsoleColor.Red);
            _worldTiles.Add("player", player);
        }

        public void SetMap(string textFile)
        {
            string path = "../../../";
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
                            row.Add(_worldTiles["floor"]);
                        }
                        else if (c == 'g')
                        {
                            row.Add(_worldTiles["grass"]);
                        }
                        else if (c == 'e')
                        {
                            row.Add(_worldTiles["exterior"]);
                        }
                    }
                    _worldMapTiles.Add(row);
                }
            }
            else
            {
                Console.WriteLine("File do not exist");
            }

        }
    }
}
