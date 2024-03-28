using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny
{
    [Serializable]
    class ItemContainer
    {
        public Dictionary<string, Item> ItemList { get; set; }
    }
}
