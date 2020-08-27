using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OmdbScrubber.Extensions;
using OmdbScrubber.Models;
using OmdbScrubber.Models.ViewModels;
using OmdbScrubber.Repositories;
using RestSharp;

namespace OmdbScrubber.Controllers
{
    public class ImdbController : Controller
    {

        private readonly OmdbContext _omdbContext;
        private readonly IMapper _mapper;
        private readonly MovieRepository _movieRepository;

        public ImdbController(OmdbContext omdbContext, IMapper mapper)
        {
            _omdbContext = omdbContext;
            _mapper = mapper;
            _movieRepository = new MovieRepository(_omdbContext, _mapper);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(UploadViewModel uploadVm)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction();
            }

            List<Movie> movies = await _movieRepository.GetMovies(uploadVm.Input);
            if (movies.Count > 0)
            {
                HttpContext.Session.SetSession("movies", movies);

                return RedirectToAction(nameof(List));
            }

            return Json($"Not found movie with imdb id: {uploadVm.Input}.");
        }

        public IActionResult List(decimal? ratingAbove, int? runtimeMinsAbove, int? runtimeMinsBelow, string? hasActor)
        {
            List<Movie> movies = HttpContext.Session.GetSession<List<Movie>>("movies");

            FilterCriterials filterCriterials =new  FilterCriterials()
            {
               RatingAbove = ratingAbove,
               RuntimeMinsBelow = runtimeMinsBelow,
               RuntimeMinsAbove = runtimeMinsAbove,
               ActorName = hasActor
            };

            movies = _movieRepository.GetMoviesFiltered(movies, filterCriterials);

            return View(movies);
        }
    }
}
