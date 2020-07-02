using CoreDemo.Models;
using CoreDemo.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly ICinemaService _cinemaService;

        public MovieController(IMovieService movieService,
            ICinemaService cinemaService)
        {
            _movieService = movieService;
            _cinemaService = cinemaService;
        }
        public async Task<IActionResult> Index(int cinemaId)
        {
            var cinema = await _cinemaService.GetByIdAsync(cinemaId);
            ViewBag.Title = $"{cinema.Name}这个电影院上映的电影有：";
            ViewBag.CinemaId = cinemaId;
            return View(await _movieService.GetByCinemaAsync(cinemaId));
        }
        public IActionResult Add(int cinemaId)
        {
            ViewBag.Title = "添加电影";
            return View(new Movie { CinemaId = cinemaId });
        }

        [HttpPost]
        public async Task<IActionResult> Add(Movie model)
        {
            if (ModelState.IsValid)
            {
                await _movieService.AddAsync(model);
            }
            return RedirectToAction("Index", new { cinemaId = model.CinemaId });
        }
        public async Task<IActionResult> Edit(int movieId)
        {
            var movie = await _movieService.GetByIdAsync(movieId);
            return View(movie);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int movieId, Movie model)
        {
            if (ModelState.IsValid)
            {
                var exist = await _movieService.GetByIdAsync(movieId);
                if (exist == null)
                {
                    return NotFound();
                }
                exist.Name = model.Name;
                exist.Starring = model.Starring;
                exist.ReleaseDate = model.ReleaseDate;
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            await _movieService.DeleteByIdAsync(id);
            return RedirectToAction("Index");
        }
    }
}
