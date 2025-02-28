using joro.too.Entities;
using joro.too.Services.Services.IServices;
using joro.too.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace joro.too.Web.Controllers;

public class AdminController:Controller
{
    
    private readonly IGenreService _genreService;
    private readonly IMediaService _mediaService;
    public AdminController(IGenreService genreService, IMediaService mediaservice) 
    {
        _genreService = genreService;
        _mediaService = mediaservice;
    }    
    public async Task<IActionResult> AddMovie()
    {
        var genres =  await _genreService.GetGenres();
        AddMediaModel model = new AddMediaModel();
        model.Genres = new List<SelectListItem>();
        foreach (Genre item in genres)
        {
            model.Genres.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Type });}
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> AddMovie(string name, string desc, string imgSrc,  string[] genres, AddMediaModel model)
    {
        List<int> genreIds = new List<int>();
        var list = await _genreService.GetGenres();
        foreach (Genre item in list )
        {
            model.Genres.Add(new SelectListItem(){Value = item.Id.ToString(), Text = item.Type});
        }
        foreach (SelectListItem li in model.Genres)
        {
            if (genres.Contains(li.Value))
            {
                li.Selected = true;
                genreIds.Add(int.Parse(li.Value));
            }
        }
        var genresreal = await _genreService.GetGenresById(genreIds);
        await _mediaService.AddMedia(name, imgSrc, true, genresreal, desc);
        return RedirectToAction("SearchResult","Media");
    }
    
}