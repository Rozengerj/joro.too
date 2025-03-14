using CloudinaryDotNet;
using joro.too.Entities;
using joro.too.Services.Services;
using joro.too.Services.Services.IServices;
using joro.too.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;

namespace joro.too.Web.Controllers;

public class AdminController : Controller
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
        var genres = await _genreService.GetGenres();
        AddMediaModel model = new AddMediaModel();
        model.Genres = new List<SelectListItem>();
        foreach (Genre item in genres)
        {
            model.Genres.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Type });
        }
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> AddMovie(string name, string desc, IFormFile img, string[] genres, AddMediaModel model, IFormFile vid)
    {
        List<int> genreIds = new List<int>();
        var list = await _genreService.GetGenres();
        foreach (Genre item in list)
        {
            model.Genres.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Type });
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
        var vidsrc = await _cloudinary.UploadVideoAsync(vid);

        var media = await _mediaService.AddMedia(name, imageUrl, false, genresreal, desc);
        await _mediaService.AddMovie(media, vidsrc);
        return RedirectToAction("SearchResult", "Media");
    }

    public async Task<IActionResult> AddShow()
    {
        var genres = await _genreService.GetGenres();
        AddMediaModel model = new AddMediaModel();
        model.Genres = new List<SelectListItem>();
        foreach (Genre item in genres)
        {
            model.Genres.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Type });
        }
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> AddShow(string name, string desc, IFormFile img, string[] genres, AddMediaModel model, string[] season, string[] episode, IFormFile[] episodevidsrc)
    {
        List<int> genreIds = new List<int>();
        var list = await _genreService.GetGenres();
        foreach (Genre item in list)
        {
            model.Genres.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Type });
        }
        if (genres is not null)
        {
            foreach (SelectListItem li in model.Genres)
            {
                if (genres.Contains(li.Value))
                {
                    li.Selected = true;
                    genreIds.Add(int.Parse(li.Value));
                }
            }
        }

        // this thing
        List<List<Tuple<string, string>>> episodesinfo = new List<List<Tuple<string, string>>>();
        int counter = 0;
        foreach (var item in episode)
        {
            
            var tempList = new List<Tuple<string, string>>();
            if (item != "_-_-_@_-_-_")
            {
                var vidurl = await _cloudinary.UploadVideoAsync(episodevidsrc[counter]);
                tempList.Add(new Tuple<string, string>(item, vidurl));
                counter++;
                continue;
            }
            episodesinfo.Add(tempList);

        }
        var imageUrl = await _cloudinary.UploadImageAsync(img);
        var genresreal = await _genreService.GetGenresById(genreIds);
        
        //Console.WriteLine("this is the top");
        //Console.WriteLine(string.Join(", ", episode));
        //Console.WriteLine(string.Join(", ", season));
        //Console.WriteLine("this is the bottom");
        var media = await _mediaService.AddMedia(name, imageUrl, true, genresreal, desc);
        await _mediaService.AddShow(media, episodesinfo, season.ToList());
        return RedirectToAction("SearchResult", "Media");
    }

}