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
    private readonly ISeasonService _seasonService;
    private readonly IEpisodeService _episodeService;
    private readonly IActorService _actorService;
    private CloudinaryService _cloudinary;

    public AdminController(IGenreService genreService, IMediaService mediaservice, CloudinaryService cloudinary, ISeasonService seasonService, IEpisodeService episodeService, IActorService actorService)
    {
        _genreService = genreService;
        _mediaService = mediaservice;
        _cloudinary = cloudinary;
        _seasonService = seasonService;
        _episodeService = episodeService;
        _actorService = actorService;
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
    [RequestSizeLimit(2147483647)] //unit is bytes => 2GB
    [RequestFormLimits(MultipartBodyLengthLimit = 2147483647)]
    public async Task<IActionResult> AddMovie(string name, string desc, IFormFile img, string[] genres, IFormFile vid,
        AddMediaModel model, string director)
    {
        List<int> genreIds = new List<int>();
        var list = await _genreService.GetGenres();
        foreach (Genre item in list)
        {
            model.Genres.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Type });
        }

        if (!genres.IsNullOrEmpty())
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
        var imageUrl = await _cloudinary.UploadImageAsync(img);
        var genresreal = await _genreService.GetGenresById(genreIds);
        var vidsrc = await _cloudinary.UploadVideoAsync(vid);
        await _mediaService.AddMovie(name, imageUrl, genresreal, desc, vidsrc, director);
        return RedirectToAction("SearchResult", "Search");
    }

    public async Task<IActionResult> EditMovie(int id)
    {
        
        var movie = await _mediaService.FindMovieById(id);
        var Genres = new List<SelectListItem>();
        foreach (var item in await _genreService.GetGenres())
        {
            if (movie.Genres.Select(x => x.Genre).Contains(item))
            {
                Genres.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Type, Selected = true });
                continue;
            }

            Genres.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Type });
        }

        var model = new EditMovieModel()
        {
            name = movie.Name,
            id = movie.Id,
            description = movie.Description,
            imgsrc = movie.MediaImgSrc,
            videourl = movie.VidSrc,
            Genres = Genres
        };
        return View(model);
    }

    [HttpPost]
    [RequestSizeLimit(2147483647)] //unit is bytes => 2GB
    [RequestFormLimits(MultipartBodyLengthLimit = 2147483647)]
    public async Task<IActionResult> EditMovie(string name, string desc, IFormFile? img, string[] genres, IFormFile? vid,
        AddMediaModel model, int id)
    {
        Movie movie = await _mediaService.FindMovieById(id);
        List<int> genreIds = new List<int>();
        var list = await _genreService.GetGenres();
        model.Genres = new List<SelectListItem>();
        foreach (Genre item in list)
        {
            model.Genres.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Type });
        }
        movie.Name = name;
        movie.Description = desc;
        if (!genres.IsNullOrEmpty())
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
        if (img is not null)
        {
            await _cloudinary.DeleteImageAsync(movie.MediaImgSrc);
            movie.MediaImgSrc = await _cloudinary.UploadImageAsync(img);
        }
        if (vid is not null)
        {
            await _cloudinary.DeleteVideoAsync(movie.VidSrc);
            movie.VidSrc = await _cloudinary.UploadVideoAsync(vid);
        }
        var genresReal = await _genreService.GetGenresById(genreIds);
        await _mediaService.UpdateMedia(movie, genresReal);
        return RedirectToAction("ViewMedia", "Search", new { id = id, IsShow = false });
    }

    public async Task<IActionResult> DeleteMovie(int id)
    {
        return RedirectToAction("SearchResult", "Search");
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
        string[] episode, IFormFileCollection episodevidsrc, AddMediaModel model, string director)
    {
        List<int> genreIds = new List<int>();
        var list = await _genreService.GetGenres();

        model.Genres = new List<SelectListItem>();
        foreach (Genre item in list)
        {
            model.Genres.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Type });
        }

        var imageUrl = await _cloudinary.UploadImageAsync(img);
        if (!genres.IsNullOrEmpty())
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
            episodesinfo.Add(tempList);
            episodeCounter = 1;
            tempList = new List<Tuple<string, string>>();
        }

        var genresreal = await _genreService.GetGenresById(genreIds);
        await _mediaService.AddShow(name, imageUrl, genresreal, desc, episodesinfo, season.ToList(), director);
        return RedirectToAction("SearchResult", "Search");
    }


    public async Task<IActionResult> EditShow(int showiddd)
    {
        var show = await _mediaService.FindShowById(showiddd);
        var Genres = new List<SelectListItem>();
        foreach (var item in await _genreService.GetGenres())
        {
            if (show.Genres.Select(x => x.Genre).Contains(item))
            {
                Genres.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Type, Selected = true });
                continue;
            }
            Genres.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Type });
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
                    username = y.Commenter.UserName
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
    [RequestSizeLimit(2147483647)] //unit is bytes => 2GB
    [RequestFormLimits(MultipartBodyLengthLimit = 2147483647)]
    public async Task<IActionResult> EditShow(int id, string name, string desc, string[] genres, IFormFile? img,
        string[]? season,
        string[]? episode,
        IFormFileCollection? episodevidsrc, AddMediaModel model)
    {
        List<int> genreIds = new List<int>();
        var list = await _genreService.GetGenres();

        model.Genres = new List<SelectListItem>();
        foreach (Genre item in list)
        {
            model.Genres.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.Type });
        }
        var media = await _mediaService.FindShowById(id);
        var imageUrl = media.MediaImgSrc;
        if (img is not null)
        {
            await _cloudinary.DeleteImageAsync(media.MediaImgSrc);
            imageUrl = await _cloudinary.UploadImageAsync(img);
        }

        if (!genres.IsNullOrEmpty())
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
        List<List<Tuple<string, string>>> episodesinfo = new List<List<Tuple<string, string>>>();
        if (season.IsNullOrEmpty())
        {
            int counter = 0;
            int episodeCounter = 1;
            var tempList = new List<Tuple<string, string>>();
            foreach (var item in episode)
            {
                if (item != "_-_-_|_-_-_")
                {
                    var vidurl = await _cloudinary.UploadVideoAsync(episodevidsrc[counter]);
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
                episodesinfo.Add(tempList);
                episodeCounter = 1;
                tempList = new List<Tuple<string, string>>();
            }
        }

        if (season.IsNullOrEmpty())
        {
            season = new string[1];
        }

        media.Name = name;
        media.Description = desc;
        media.MediaImgSrc = imageUrl;
        var genresReal = await _genreService.GetGenresById(genreIds);
        await _mediaService.UpdateMedia(media, episodesinfo, season.ToList(), genresReal);
        return RedirectToAction("ViewMedia", "Search", new { id = id, IsShow = true });
    }

    public async Task<IActionResult> RemoveSeason(int id, int showId)
    {
        var season = await _seasonService.FindSeasonById(id);
        foreach (var eps in season.Episodes)
        {
            await _cloudinary.DeleteVideoAsync(eps.vidsrc);
        }
        await _seasonService.RemoveSeason(id);
        return RedirectToAction("EditShow", "Admin", new{showiddd=showId});
    }
    public async Task<IActionResult> EditSeason(int sId)
    {
        Season s = await _seasonService.FindSeasonById(sId);
        EditSeasonModel model = new EditSeasonModel()
        {
            name = s.Name,
            id = s.Id,
            episodes = s.Episodes.Select(x => new VideoViewModel()
                {
                    id = x.Id,
                    name = x.name,
                    vidsrc = x.vidsrc
                }
            ).ToList()
        };
        return View(model);
    }

    [HttpPost]
    [RequestSizeLimit(2147483647)] //unit is bytes => 2GB
    [RequestFormLimits(MultipartBodyLengthLimit = 2147483647)]
    public async Task<IActionResult> EditSeason(int id, string name, IFormFileCollection episodeFiles, string[] episodeNames)
    {
        Season season = await _seasonService.FindSeasonById(id);
        List<string> episodeVidSrcs = new List<string>();
        foreach (var episode in episodeFiles)
        {
            episodeVidSrcs.Add(await _cloudinary.UploadVideoAsync(episode));
        }
        await _seasonService.AddEpisdesToSeason(season, episodeNames.ToList(), episodeVidSrcs);
        season.Name = name;
        await _seasonService.UpdateSeason(season);
        return RedirectToAction("EditShow", "Admin", new{showiddd=season.ShowId});
    }

    public async Task<IActionResult> RemoveEpisode(int id, int seasonId)
    {
        var ep = await _episodeService.FindEpisodeById(id);
        await _cloudinary.DeleteVideoAsync(ep.vidsrc);
        await _episodeService.RemoveEpisode(id);
        return RedirectToAction("EditSeason", "Admin", new{sId=seasonId});
    }

    public async Task<IActionResult> EditEpisode(int id)
    {
        Episode ep = await _episodeService.FindEpisodeById(id);
        VideoViewModel model = new VideoViewModel()
        {
            name = ep.name,
            id = ep.Id,
            vidsrc = ep.vidsrc
        };
        return View(model);
        
    }
    [HttpPost]
    [RequestSizeLimit(2147483647)] //unit is bytes => 2GB
    [RequestFormLimits(MultipartBodyLengthLimit = 2147483647)]
    public async Task<IActionResult> EditEpisode(int id, string name, string oldVidSrc, IFormFile? vid)
    {
        Episode ep = await _episodeService.FindEpisodeById(id);
        ep.name = name;
        if (vid is not null)
        {
            await _cloudinary.DeleteVideoAsync(oldVidSrc);
            ep.vidsrc = await _cloudinary.UploadVideoAsync(vid);
        }
        await _episodeService.UpdateEpisode(ep);
        
        return RedirectToAction("EditSeason", "Admin", new {sId = ep.SeasonId} );
    }
    public async Task<IActionResult> RemoveShow(int id)
    {
        var show = await _mediaService.FindShowById(id);
        foreach (var eps in show.Seasons.Select(x=>x.Episodes))
        {
            foreach (var ep in eps)
            {
                await _cloudinary.DeleteVideoAsync(ep.vidsrc);
            }
        }

        await _cloudinary.DeleteImageAsync(show.MediaImgSrc);
        await _seasonService.RemoveSeason(id);
        return RedirectToAction("SearchResult", "Search");
    }

    public async Task<IActionResult> AddActor()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddActor(string name, IFormFile img)
    {
        string imgsrc = await _cloudinary.UploadImageAsync(img);
        await _actorService.AddActor(name, imgsrc);
        return RedirectToAction("ViewActors", "Search");
    }

    public async Task<IActionResult> AddRoleToActor(int actorId)
    {
        var actor = await _actorService.FindActorById(actorId);
        var tempTuple = await _mediaService.GetMediasWithGenres(null);
        var allMedia = new List<IMedia>();
        allMedia.AddRange(tempTuple.Item1); allMedia.AddRange(tempTuple.Item2);
        AddRoleToActorModel model = new AddRoleToActorModel();
        model.actorId = actorId;
        model.allMedia = allMedia.Select(x => new AllMediaModel()
        {
            name = x.Name,
            id = x.Id,
            isShow = x is Show
        }).ToList();
        model.actorName = actor.Name;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddRoleToActor(string role, int actorId, int mediaId, bool isShow)
    {
        Console.WriteLine(isShow);
        if (isShow)
        {
            Show media = await _mediaService.FindShowById(mediaId);
            await _actorService.AddRoleToActor(actorId, role, media); 
        }
        else
        {
           Movie media = await _mediaService.FindMovieById(mediaId);
           await _actorService.AddRoleToActor(actorId, role, media); 
        }
        return RedirectToAction("ViewActors", "Search");
    }

    public async Task<IActionResult> EditActor(int actorId)
    {
        var actor = await  _actorService.FindActorById(actorId);
        var model = new EditActorModel()
        {
            Name = actor.Name,
            Id = actor.Id,
            Roles = new List<ActorRolesModel>(),
            oldImg = actor.imgsrc
        };
        foreach (var roles in actor.RolesInMovies)
        {
            foreach (var role in roles.Roles)
            {
                model.Roles.Add(new ActorRolesModel()
                {
                    MediaName = roles.Movie.Name,
                    Role = role,
                    MediaId = roles.MovieId,
                    isShow = false
                });
            } 
        }
        foreach (var roles in actor.RolesInShows)
        {
            foreach (var role in roles.Roles)
            {
                model.Roles.Add(new ActorRolesModel()
                {
                    MediaName = roles.Show.Name,
                    Role = role,
                    MediaId = roles.ShowId,
                    isShow = true
                });
            } 
        }
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> EditActor(string[] removedRoles, string name, int id, IFormFile? newImage)
    {
        var actor = await _actorService.FindActorById(id);
        await _actorService.RemoveRolesFromActor(removedRoles, id);
        if (newImage is not null)
        {
            await _cloudinary.DeleteImageAsync(actor.imgsrc);
            actor.imgsrc = await _cloudinary.UploadImageAsync(newImage);
        }
        actor.Name = name;
        await _actorService.UpdateActor(actor);
        return RedirectToAction("ViewActors", "Search");
    }

    public async Task<IActionResult> DeleteActor(int actorId)
    {
        await _actorService.RemoveActor(actorId);
        return RedirectToAction("ViewActors", "Search");
    }
    

    public async Task<IActionResult> AdminActions()
    {
        return View();
    }
}