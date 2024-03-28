using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny
{
    [Serializable]
    class ItemTilesContainer
    {
        public List<ItemTiles> ItemTiles { get; set; }
    }

    class ItemTiles
    {
        public Position Position { get; set; }
        public string ItemName { get; set; }
    }
}
