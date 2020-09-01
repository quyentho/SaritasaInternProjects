using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealEstate.Models.Models
{
    public class FilterCriteria
    {
        public string Address { get; set; }

        public uint? MaxPrice { get; set; }

        public uint? MinPrice { get; set; }

        public uint? MaxAge { get; set; }

        public uint? Offset { get; set; }

        public uint? Limit { get; set; }

        public double? MinSize { get; set; }
    }
}
