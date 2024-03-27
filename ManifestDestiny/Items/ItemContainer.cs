using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny.Items
{
    [System.Serializable]
    class ItemContainer
    {
        public Dictionary<string, Item> ItemList { get; set; }
    }
}
