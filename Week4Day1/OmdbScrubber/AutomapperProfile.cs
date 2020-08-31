using AutoMapper;
using OmdbScrubber.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace OmdbScrubber
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<MovieResponse, Movie>()
                .ForMember(dest => dest.RuntimeMins,
                opt => opt.ConvertUsing(new RunTimeFormater()))
                .ForMember(dest => dest.CreateAt,
                opt => opt.ConvertUsing(new DateTimeFormater()))
                .ForMember(dest => dest.MovieActors,
                opt => opt.ConvertUsing(new ActorFormater()));
        }
    }

    public class DateTimeFormater : IValueConverter<string, DateTime>
    {

        public DateTime Convert(string sourceMember, ResolutionContext context)
        {
            var year = sourceMember.Split('–', StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            return DateTime.ParseExact(year, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
        }
    }

    public class RunTimeFormater : IValueConverter<string, int>
    {
        public int Convert(string sourceMember, ResolutionContext context)
        {
            return int.Parse(sourceMember.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0].Trim());
        }
    }

    public class ActorFormater : IValueConverter<string, IList<MovieActor>>
    {
        public IList<MovieActor> Convert(string sourceMember, ResolutionContext context)
        {
            var actorNames = sourceMember.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(a => a.Trim());

            var movieActors = new List<MovieActor>();

            foreach (var actorName in actorNames)
            {
                movieActors.Add(new MovieActor() { Actor = new Actor() { Name = actorName } });
            }

            return movieActors;

        }
    }

}
