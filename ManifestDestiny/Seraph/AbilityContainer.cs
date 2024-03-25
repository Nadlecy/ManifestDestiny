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
        Dictionary<string, AbilityData> _ability;

        public Dictionary<string, AbilityData> Ability { get => _ability; }
    }

    class AbilityData
    {
        string _name;
        string _type;
        string _description;
        int _power;
        int _accuracy;
        int _manaCost;
        int _critChance;
    }
}
