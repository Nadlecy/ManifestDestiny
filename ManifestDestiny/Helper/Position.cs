using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManifestDestiny
{
    [System.Serializable]
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position Clone()
        {
            var pos = new Position();
            pos.X = X;
            pos.Y = Y;
            return pos;
        }
    }
}
