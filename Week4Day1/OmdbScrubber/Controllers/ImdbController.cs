using Microsoft.AspNetCore.Mvc;
using OmdbScrubber.Extensions;
using OmdbScrubber.Models;
using OmdbScrubber.Models.ViewModels;
using ServicesLayer;
using System.Collections.Generic;
using System.Threading.Tasks;

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

            List<Movie> movies = await _movieServices.GetMovies(uploadVm.Input).ConfigureAwait(false);
            if (movies.Count > 0)
            {
                HttpContext.Session.SetSession("movies", movies);

                return RedirectToAction(nameof(List));
            }

            return Json($"Not found movie with imdb id: {uploadVm.Input}.");
        }

        /// <summary>
        /// Renders view to display movies.
        /// </summary>
        /// <param name="ratingAbove">Filter movies by rating above.</param>
        /// <param name="runtimeMinsAbove">Filter movies by runtime minute above.</param>
        /// <param name="runtimeMinsBelow">Filter movies by runtime minute below.</param>
        /// <param name="hasActor">Filter movies by actor.</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult List(decimal? ratingAbove, int? runtimeMinsAbove, int? runtimeMinsBelow, string hasActor)
        {
            var filterCriteria = new FilterCriteria()
            {
                RatingAbove = ratingAbove,
                RuntimeMinsBelow = runtimeMinsBelow,
                RuntimeMinsAbove = runtimeMinsAbove,
                ActorName = hasActor
            };

            List<Movie> movies = HttpContext.Session.GetSession<List<Movie>>("movies");
            movies = movies.Filter(filterCriteria);

            return View(movies);
        }
    }
}
