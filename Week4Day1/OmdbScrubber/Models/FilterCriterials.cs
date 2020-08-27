using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmdbScrubber.Models
{
    public class FilterCriterials
    {
        public decimal? RatingAbove { get; set; }

        public int? RuntimeMinsAbove { get; set; }

        public int? RuntimeMinsBelow { get; set; }

        public string? ActorName { get; set; }
    }
}
