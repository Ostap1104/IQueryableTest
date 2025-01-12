using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class BuildingsRequestData
    {
        public BuildingsFiltersData Filters { get; set; } = new();
        public SortOrder SortOrder { get; set; } = SortOrder.None;
        public BuildingOrderBy OrderBy { get; set; } = BuildingOrderBy.None;
    }
}
