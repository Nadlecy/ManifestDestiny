using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny.View
{
    internal class EncountersChanceContainer
    {
        public Dictionary<string, int> Encounter { get; set; }
        public int Chance { get; set; }
        
        public EncountersChanceContainer()
        {
        }
    }
}