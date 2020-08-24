using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OmdbScrubber.Models
{
    public class Actor
    {
        [Key]
        public Guid MyProperty { get; set; }

        public string Name { get; set; }

        public IList<MovieActor> MovieActors { get; set; }
    }
}
