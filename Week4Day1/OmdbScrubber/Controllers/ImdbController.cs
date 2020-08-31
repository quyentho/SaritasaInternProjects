using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OmdbScrubber.Extensions;
using OmdbScrubber.Models;
using OmdbScrubber.Models.ViewModels;
using ServicesLayer;

namespace OmdbScrubber.Controllers
{
    public class ImdbController : Controller
    {
        private readonly IMovieService _movieServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImdbController"/> class.
        /// </summary>
        /// <param name="movieServices">Movie service inject in constructor.</param>
        public ImdbController(IMovieService movieServices)
        {
            this._movieServices = movieServices;
        }

        /// <summary>
        /// Renders upload view.
        /// </summary>
        /// <returns>Upload view.</returns>
        public IActionResult Upload()
        {
            return View();
        }

        /// <summary>
        /// Handles http post to get movies by ids and redirect to list action.
        /// </summary>
        /// <param name="uploadVm">View model represents user input.</param>
        /// <returns>Redirect to List action if found movies, renders message as json if not found.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(UploadViewModel uploadVm)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction();
            }

            List<Movie> movies = await _movieServices.GetMovies(uploadVm.Input);
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

            FilterCriterial filterCriterials = new FilterCriterial()
            {
                RatingAbove = ratingAbove,
                RuntimeMinsBelow = runtimeMinsBelow,
                RuntimeMinsAbove = runtimeMinsAbove,
                ActorName = hasActor
            };

            movies = _movieServices.GetMoviesFiltered(movies, filterCriterials);

            return View(movies);
        }
    }
}
