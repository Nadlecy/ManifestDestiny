using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny.Container
{
    [Serializable]
    class AbilityContainer
    {

        public List<AbilityData> Ability { get; set; }
    }

    class AbilityData
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int Power { get; set; }
        public int Accuracy { get; set; }
        public int ManaCost { get; set; }
        public int CritChance { get; set; }
    }
}
