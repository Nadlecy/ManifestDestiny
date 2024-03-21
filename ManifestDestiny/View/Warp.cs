using ManifestDestiny.Helper.Position;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny.View
{
    [System.Serializable]
    public class Warp
    {
        public Position StartPosition { get; set; }
        public Position DestinationPosition { get; set; }
        public string DestinationMap { get; set; }
        public string StartMap { get; set; }
    }
}
