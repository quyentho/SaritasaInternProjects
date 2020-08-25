using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OmdbScrubber.Models;
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
           
            var imdbIds = input.UserInput.Split(",", StringSplitOptions.RemoveEmptyEntries);

            List<Movie> movies = new List<Movie>();

            RestClient client = new RestClient("https://www.omdbapi.com/");
            for (int i = 0; i < imdbIds.Length; i++)
            {
                RestRequest request = new RestRequest();
                request.AddParameter("i", imdbIds[i]);
                request.AddParameter("apikey", "19e14e4");

                IRestResponse response = await client.ExecuteAsync(request);

                MovieResponse movieResponse = JsonConvert.DeserializeObject<MovieResponse>(response.Content);

                Movie movie = _mapper.Map<Movie>(movieResponse);
                movies.Add(movie);
            }

            TempData["movies"] = JsonConvert.SerializeObject(movies);

            return RedirectToAction(nameof(List));
        }
        
        public IActionResult List()
        {

            var movies = JsonConvert.DeserializeObject<List<Movie>>((string)TempData["movies"]);

            return View(movies);
        }
    }
}
