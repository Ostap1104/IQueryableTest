using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class BuildingsFiltersData
    {
        public List<string> Names { get; set; } = new();
        public List<string> Addresses { get; set; } = new();
        public int? MinFloors { get; set; }
        public int? MaxFloors { get; set; }
        public int? MinBuiltYear { get; set; }
        public int? MaxBuiltYear { get; set; }
    }
}
