using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
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
            // TODO: upgrade better view;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(string input)
        {
            // TODO: Validate input and return back the message.
            if (!ModelState.IsValid)
            {
                return RedirectToAction();
            }

            List<Movie> movies = await _movieRepository.GetMovies(input);
            if(movies.Count > 0)
            {
                await _movieRepository.SaveMovies(movies);

                return RedirectToAction(nameof(List));
            }

            return Json($"Not found movie with imdb id: {input}.");
        }

        public IActionResult List(decimal? ratingAbove,int? runtimeMinsAbove,int? runtimeMinsBelow,string? hasActor)
        {
            var movieVM = new MovieVM() { Movies = movies };
            return View(movieVM);
        }
    }
}
