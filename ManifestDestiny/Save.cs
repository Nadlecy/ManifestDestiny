using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ManifestDestiny
{
    class Save
    {
        
        public void JsonWriter(string jsonName, List<Seraph> seraphs) {

            var saveContainer = new SaveContainer
            {
                SeraphList = seraphs
            };

            string jsonString = JsonSerializer.Serialize(saveContainer);
            File.WriteAllText("../../../Data/Save/" + jsonName + ".json", jsonString);
        }
    }

    class SaveContainer
    {
        public List<Seraph> SeraphList { get; set; }
    }
}
