using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny
{
    [System.Serializable]
    class AbilityContainer
    {

        public List<AbilityData> Ability { get; set; }
    }

    class AbilityData
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int Accuracy { get; set; }
        public int ManaCost { get; set; }
        public List<string> Attributes { get; set; } = new();

        //AttributeAttack fields
        public int? Power { get; set; }
        public int? CritChance { get; set; }

        //AttributStatAlteration
    }
}
