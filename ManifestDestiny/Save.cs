using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ManifestDestiny.Helper.Json;

namespace ManifestDestiny
{
    class Save
    {
        
        public void TeamJsonWriter(string jsonName, List<Seraph> seraphs) 
        {

            SaveContainer saveContainer = new SaveContainer();
            saveContainer.Name = new List<string> {  };
            saveContainer.Level = new List<int> {  };
            saveContainer.Experience = new List<int> {  };
            saveContainer.CurrentStats = new List<Dictionary<Seraph.Stats, int>> {  };
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

        public List<Seraph> TeamJsonLoader(string jsonName, List<Seraph> seraphs, GameData Data)
        {
            CustomJson<SaveContainer> customJson = new CustomJson<SaveContainer>("Save/" + jsonName + ".json");

            SaveContainer saveContainer = customJson.Read();

            for (int i = 0; i < saveContainer.Name.Count; i++)
            {
                Seraph ser = Data.Summon(saveContainer.Name[i], saveContainer.Level[i]);

                ser.Experience = saveContainer.Experience[i];
                ser.CurrentStats = saveContainer.CurrentStats[i];

                seraphs.Add(ser);
            }

            return seraphs;
        }
    }

    class SaveContainer
    {
        public List<string> Name { get; set; }
        public List<int> Level { get; set; }
        public List<int> Experience { get; set; }
        public List<Dictionary<Seraph.Stats, int>> CurrentStats { get; set; }
    }
}
