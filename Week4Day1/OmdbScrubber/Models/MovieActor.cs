using System;

namespace OmdbScrubber.Models
{
    public class MovieActor
    {

        public Guid ActorId { get; set; }
        public Actor Actor { get; set; }

        public Guid MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
