using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManifestDestiny.View;

namespace ManifestDestiny.Container
{
    [Serializable]
    public class WarpContainer
    {
        public List<Warp> warps { get; set; }
    }
}
