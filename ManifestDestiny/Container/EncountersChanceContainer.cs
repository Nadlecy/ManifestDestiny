using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny
{
    internal class EncountersChanceContainer
    {
        public Dictionary<string, int> Encounter { get; set; }
        public int Chance { get; set; }
        public int LevelMin { get; set; }
        public int LevelMax { get; set; }
        public int AILevel { get; set; }

        public EncountersChanceContainer()
        {
        }
    }
}