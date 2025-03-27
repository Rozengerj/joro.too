using CloudinaryDotNet;
using joro.too.Entities;
using joro.too.Services.Services;
using joro.too.Services.Services.IServices;
using joro.too.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

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
    public async Task<IActionResult> AddMovie(string name, string desc, IFormFile img, string[] genres, IFormFile vid,
        AddMediaModel model)
    {
        List<int> genreIds = new List<int>();
        var list = await _genreService.GetGenres();
        Console.WriteLine(name);
        Console.WriteLine(desc);
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
        await _mediaService.AddMovie(name, imageUrl, genresreal, desc, vidsrc);
        return RedirectToAction("SearchResult", "Media");
    }

    [HttpGet]
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
    [RequestSizeLimit(2147483647)] //unit is bytes => 2GB
    [RequestFormLimits(MultipartBodyLengthLimit = 2147483647)]
    public async Task<IActionResult> AddShow(string name, string desc, string[] genres, IFormFile img, string[] season,
        string[] episode, IFormFileCollection episodevidsrc, AddMediaModel model)
    {
        List<int> genreIds = new List<int>();
        var list = await _genreService.GetGenres();

        model.Genres = new List<SelectListItem>();
        foreach (Genre item in list)
        {
            model.Genres.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Type });
        }

        var imageUrl = await _cloudinary.UploadImageAsync(img);
        Console.WriteLine(imageUrl);
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
        int episodeCounter = 1;
        var tempList = new List<Tuple<string, string>>();
        foreach (var item in episode)
        {
            if (item != "_-_-_|_-_-_")
            {
                var vidurl = await _cloudinary.UploadVideoAsync(episodevidsrc[counter]);
                Console.WriteLine("uploaded video " + counter);
                if (item.Trim().IsNullOrEmpty())
                {
                    tempList.Add(new Tuple<string, string>("episode " + episodeCounter, vidurl));
                    counter++;
                    episodeCounter++;
                    continue;
                }

                tempList.Add(new Tuple<string, string>(item, vidurl));
                counter++;
                continue;
            }

            Console.WriteLine("Added Episodes For Season");
            episodesinfo.Add(tempList);
            episodeCounter = 1;
            tempList = new List<Tuple<string, string>>();
        }

        var genresreal = await _genreService.GetGenresById(genreIds);
        //Console.WriteLine("this is the top");
        //Console.WriteLine(string.Join(", ", episode));
        //Console.WriteLine(string.Join(", ", season));
        //Console.WriteLine("this is the bottom");
        await _mediaService.AddShow(name, imageUrl, genresreal, desc, episodesinfo, season.ToList());
        return RedirectToAction("SearchResult", "Media");
    }
    
    [Route("showiddd")]
    public async Task<IActionResult> EditShow(int showiddd)
    {
        var show = await _mediaService.FindShowById(showiddd);
        var Genres = new List<SelectListItem>();
        foreach (var item in await _genreService.GetGenres())
        {
            if (show.Genres.Select(x => x.Genre).Contains(item))
            {
                Genres.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Type, Selected = true});
                Console.WriteLine("do i go in here please");
                continue;
            }
            Genres.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Type});
        }
        List<List<VideoViewModel>> episodes = new List<List<VideoViewModel>>();
        episodes = show.Seasons.Select(x => x.Episodes).Select(x => x.Select(x => new VideoViewModel()
        {
            name = x.name,
            id = x.Id,
            vidsrc = x.vidsrc,
            comments = x.Comments.Select(y => new ViewCommentsModel()
                {
                    comment = y.Text,
                    id = y.Id,
                    pfpsrc = y.Commenter.Pfp,
                    username = y.Commenter.Name
                }
            ).ToList()
        }).ToList()).ToList();
        var editShowModel = new EditShowModel()
        {
            Name = show.Name,
            Description = show.Description,
            Genres = Genres,
            SeasonNames = show.Seasons.Select(x => x.Name).ToList(),
            SeasonIds = show.Seasons.Select(x => x.Id).ToList(),
            Episodes = episodes,
            MediaImgSrc = show.MediaImgSrc,
            Id = show.Id
        };
        return View(editShowModel);
    }
    [HttpPost]
    public async Task<IActionResult> EditShowFunc(EditShowModel model)
    {
        Console.WriteLine("if you actually go in here i will be so so pissed you would not believe your eyes");
        return RedirectToAction("ViewMedia", "Media",new { id = model.Id ,IsShow=true });
    }
}