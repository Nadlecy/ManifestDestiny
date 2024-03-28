using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ManifestDestiny
{
    class Save
    {

        public void TeamJsonWriter(string jsonName, List<Seraph> seraphs)
        {

            TeamSaveContainer saveContainer = new TeamSaveContainer();
            saveContainer.Name = new List<string> { };
            saveContainer.Level = new List<int> { };
            saveContainer.Experience = new List<int> { };
            saveContainer.CurrentStats = new List<Dictionary<Seraph.Stats, int>> { };
            for (int i = 0; i < seraphs.Count; i++)
            {
                saveContainer.Name.Add(seraphs[i].Name);
                saveContainer.Level.Add(seraphs[i].Level);
                saveContainer.Experience.Add(seraphs[i].Experience);
                saveContainer.CurrentStats.Add(seraphs[i].CurrentStats);
            }

            string jsonString = JsonSerializer.Serialize(saveContainer);
            File.WriteAllText("../../../Data/Save/" + jsonName + ".json", jsonString);
        }
        public void PositionJsonWriter(string jsonName, Position position, ref string map)
        {

            PositionSaveContainer saveContainer = new PositionSaveContainer();
            saveContainer.Positioning = position;
            saveContainer.Map = map;
            
            string jsonString = JsonSerializer.Serialize(saveContainer);
            File.WriteAllText("../../../Data/Save/" + jsonName + ".json", jsonString);
        }

        public List<Seraph> TeamJsonLoader(string jsonName, List<Seraph> seraphs, GameData Data)
        {
            CustomJson<TeamSaveContainer> customJson = new CustomJson<TeamSaveContainer>("Save/" + jsonName + ".json");

            TeamSaveContainer saveContainer = customJson.Read();

            for (int i = 0; i < saveContainer.Name.Count; i++)
            {
                Seraph ser = Data.Summon(saveContainer.Name[i], saveContainer.Level[i]);

                ser.Experience = saveContainer.Experience[i];
                ser.CurrentStats = saveContainer.CurrentStats[i];

                seraphs.Add(ser);
            }

            return seraphs;
        }

        public void PositionJsonLoader(string jsonName, ref Position position, ref string map)
        {

            CustomJson<PositionSaveContainer> customJson = new CustomJson<PositionSaveContainer>("Save/" + jsonName + ".json");

            PositionSaveContainer saveContainer = customJson.Read();

            position = saveContainer.Positioning;
            map = saveContainer.Map;

        }
    }

    class TeamSaveContainer
    {
        public List<string> Name { get; set; }
        public List<int> Level { get; set; }
        public List<int> Experience { get; set; }
        public List<Dictionary<Seraph.Stats, int>> CurrentStats { get; set; }
    }

    class PositionSaveContainer
    {
        public Position Positioning { get; set; }
        public string Map { get; set; }

    }
}
