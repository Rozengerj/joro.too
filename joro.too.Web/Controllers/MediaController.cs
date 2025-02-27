using joro.too.DataAccess;
using joro.too.Entities;
using joro.too.Services.Services;
using joro.too.Services.Services.IServices;
using joro.too.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace joro.too.Web.Controllers
{
    public class MediaController:Controller
    {
        private readonly IGenreService _genreService;
        private readonly IMediaService _mediaService;
        public MediaController(IGenreService genreService, IMediaService mediaservice) 
        {
            _genreService = genreService;
            _mediaService = mediaservice;
        }    
        public async Task<IActionResult> SearchResult()
        {

            return View();
        }

        public async Task<IActionResult> AddMedia()
        {
            var genres = _genreService.GetGenres().Result;
            AddMediaModel model = new AddMediaModel();
            model.Genres = new List<SelectListItem>();
            foreach (Genre item in genres)
            {
                model.Genres.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Type });}
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddMedia(string name, string desc, string imgSrc, bool isShow, string[] genres, AddMediaModel model)
        {
            foreach (Genre item in _genreService.GetGenres().Result)
            {
                model.Genres.Add(new SelectListItem(){Value = item.Id.ToString(), Text = item.Type});
            }
            foreach (SelectListItem li in model.Genres)
            {
                if (genres.Contains(li.Value))
                {
                    li.Selected = true;
                    
                }
            }
            return RedirectToAction("SearchResult");
        }
    }
}
