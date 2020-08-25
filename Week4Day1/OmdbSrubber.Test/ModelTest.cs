using AutoMapper;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using OmdbScrubber;
using OmdbScrubber.Models;
using System.Collections.Generic;
using Xunit;

namespace OmdbSrubber.Test
{
    public class ModelTest
    {
        [Fact]
        public void AutoMappingTest()
        {
            var movieResponse = new MovieResponse()
            {
                Title = "Test",
                MovieActors = "actor1, actor2, actor3",
                CreateAt = "1999–",
                Genre = "none",
                ImdbId = "tt0120735",
                ImdbRating = 9.5m,
                ReleaseDate = new System.DateTime(2020, 2, 12),
                RuntimeMins = "107 min"
            };
            var expected = new Movie()
            {
                Title = "Test",
                MovieActors = new List<MovieActor>() {
                    new MovieActor() { Actor = new Actor() { Name = "actor1" } },
                    new MovieActor() { Actor = new Actor() { Name = "actor2" } },
                    new MovieActor() { Actor = new Actor() { Name = "actor3" } },
                },
                CreateAt = new System.DateTime(1999, 1, 1),
                Genre = "none",
                ImdbId = "tt0120735",
                ImdbRating = 9.5m,
                ReleaseDate = new System.DateTime(2020, 2, 12),
                RuntimeMins = 107
            };

            Mapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new AutomapperProfile())));

            Movie result = mapper.Map<Movie>(movieResponse);

            result.Should().BeEquivalentTo(expected);
        }
    }
}