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
using RestSharp;

namespace OmdbScrubber.Controllers
{
    public class ImdbController : Controller
    {

        OmdbContext _omdbContext;
        private readonly IMapper _mapper;
        public ImdbController(OmdbContext omdbContext, IMapper mapper)
        {
            _omdbContext = omdbContext;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upload()
        {
            // TODO: upgrade better view;
            return View(new InputVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(InputVM input)
        {
            // TODO: Validate input and return back the message.
            if (!ModelState.IsValid)
            {
                return RedirectToAction();
            }

            List<Movie> movies = await GetMovies(input);
            if(movies.Count > 0)
            {
                await SaveMovies(movies);

                TempData["movies"] = JsonConvert.SerializeObject(movies, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                return RedirectToAction(nameof(List));
            }

            return Json($"Not found movie with imdb id: {input.UserInput}.");
        }

        public IActionResult List()
        {
            var movies = JsonConvert.DeserializeObject<List<Movie>>((string)TempData["movies"]);
            var movieVM = new MovieVM() { Movies = movies };
            return View(movieVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult List(MovieVM movieVM)
        {
            var filtered = movieVM.Movies.Where(m => m.MovieActors.Where(ma => ma.Actor.Name == movieVM.HasActor).ToList() && m.ImdbRating >= movieVM.RatingAbove && (m.RuntimeMins >= movieVM.RuntimeMinsAbove || movieVM.RuntimeMinsBelow > m.RuntimeMins)).ToList();
            return RedirectToAction();
        }

        private async Task SaveMovies(List<Movie> movies)
        {
            foreach (var movie in movies)
            {
                if (!_omdbContext.Movies.Any(m => m.ImdbId == movie.ImdbId)) // Check if not duplicate.
                {
                    _omdbContext.Movies.Add(movie);
                }
            }

            await _omdbContext.SaveChangesAsync();
        }

        private async Task<List<Movie>> GetMovies(InputVM input)
        {

            var imdbIds = input.UserInput.Split(",", StringSplitOptions.RemoveEmptyEntries);
            

            RestClient client = new RestClient("https://www.omdbapi.com/");

            List<Movie> movies = new List<Movie>();

            for (int i = 0; i < imdbIds.Length; i++)
            {
                IRestResponse response = await GetResponse(client, imdbIds[i]);

                MovieResponse movieResponse = JsonConvert.DeserializeObject<MovieResponse>(response.Content);
                
                if (movieResponse.ImdbId != null)
                {
                    Movie movie = _mapper.Map<Movie>(movieResponse);
                    movies.Add(movie);
                }
            }

            return movies;
        }

        private static async Task<IRestResponse> GetResponse(RestClient client, string imdbId)
        {
            RestRequest request = new RestRequest();
            request.AddParameter("i", imdbId);
            request.AddParameter("apikey", "19e14e4");

            IRestResponse response = await client.ExecuteAsync(request);
            return response;
        }

       
    }
}
