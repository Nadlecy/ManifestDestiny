using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny
{
    [System.Serializable]
    class SeraphContainer
    {
        public List<SeraphData> Seraph { get; set; }

    }

    class SeraphData
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Speed { get; set; }
        public int Mana { get; set; }
        public int Magic { get; set; }

        public int HP100 { get; set; }
        public int Attack100 { get; set; }
        public int Defense100 { get; set; }
        public int Speed100 { get; set; }
        public int Mana100 { get; set; }
        public int Magic100 { get; set; }
        public int ExpReward { get; set; }
        public Dictionary<int, string> Ability { get; set; }
        public string Description { get; set; }

    }
}
