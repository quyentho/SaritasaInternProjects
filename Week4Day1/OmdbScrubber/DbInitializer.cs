using OmdbScrubber.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmdbScrubber
{
    public static class DbInitializer
    {
        public static void Initialize(OmdbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
