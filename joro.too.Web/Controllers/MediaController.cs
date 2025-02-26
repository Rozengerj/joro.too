using joro.too.DataAccess;
using joro.too.Services.Services;
using joro.too.Services.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace joro.too.Web.Controllers
{
    public class MediaController:Controller
    {
        private readonly IGenreService _genreService;
        private readonly IMediaService _mediaService;
        public MediaController(MovieDbContext context) 
        {
            _genreService = new GenreService(context);
            _mediaService = new MediaService(context);
        }    
        public async Task<IActionResult> SearchResult()
        {

            return View();
        }

        public async Task<IActionResult> AddMedia()
        {
            var genres = _genreService.GetGenres().Result;
            return View(genres);
        }
        [HttpPost]
        public async Task<IActionResult> AddMedia(string name, string desc, string imgSrc, bool isShow)
        {
            return RedirectToAction("SearchResult");
        }
    }
}
