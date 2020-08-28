namespace OmdbScrubber.Models
{
    public class FilterCriterial
    {
        public decimal? RatingAbove { get; set; }

        public int? RuntimeMinsAbove { get; set; }

        public int? RuntimeMinsBelow { get; set; }

        public string? ActorName { get; set; }
    }
}
