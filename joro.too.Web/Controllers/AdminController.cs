using CloudinaryDotNet;
using joro.too.Entities;
using joro.too.Services.Services;
using joro.too.Services.Services.IServices;
using joro.too.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace joro.too.Web.Controllers;

public class AdminController:Controller
{
    
    private readonly IGenreService _genreService;
    private readonly IMediaService _mediaService;
    private CloudinaryService _cloudinary;
    public AdminController(IGenreService genreService, IMediaService mediaservice, CloudinaryService cloudinary) 
    {
        _genreService = genreService;
        _mediaService = mediaservice;
        _cloudinary = cloudinary;
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
    public async Task<IActionResult> AddMovie(string name, string desc, IFormFile img,  string[] genres, AddMediaModel model, IFormFile vid)
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
        // this thing
        var imageUrl = await _cloudinary.UploadImageAsync(img);
        var genresreal = await _genreService.GetGenresById(genreIds);
        //var vidsrc = await _cloudinary.UploadVideoAsync(vid);
        
        var media = await _mediaService.AddMedia(name, imageUrl, true, genresreal, desc);
        //await _mediaService.AddMovie(media, vidsrc);
        return RedirectToAction("SearchResult","Media");
    }

    public async Task<IActionResult> AddShow()
    {
        var genres =  await _genreService.GetGenres();
        AddMediaModel model = new AddMediaModel();
        model.Genres = new List<SelectListItem>();
        foreach (Genre item in genres)
        {
            model.Genres.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Type });}
        return View(model);
    }
    
}