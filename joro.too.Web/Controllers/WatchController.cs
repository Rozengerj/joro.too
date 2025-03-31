using joro.too.Services.Services.IServices;
using joro.too.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace joro.too.Web.Controllers;

public class WatchController:Controller
{
    private readonly IGenreService _genreService;
    private readonly IMediaService _mediaService;
    public WatchController(IGenreService genreService, IMediaService mediaservice) 
    {
        _genreService = genreService;
        _mediaService = mediaservice;
    }
    public async Task<IActionResult> WatchMovie(int movieId)
    {
        var movie = await _mediaService.FindMovieById(movieId);
        WatchMovieModel model = new WatchMovieModel()
        {
            name = movie.Name,
            vidSrc = movie.vidsrc,
            Comments = movie.Comments.Select(y => new ViewCommentsModel()
            {
                username = y.Commenter.Name,
                comment = y.Text,
                id = y.Commenter.Id,
                pfpsrc = y.Commenter.Pfp
            }).ToList()
        };
        return View(model);
    }
    public async Task<IActionResult> WatchShow(int showId)
    {
        var show = await _mediaService.FindShowById(showId);
        WatchShowModel model = new WatchShowModel()
        {
            name = show.Name,
            id = show.Id,
        };
        model.seasonsNames = new List<string>();
        model.episodesInSeasons = new List<List<VideoViewModel>>();
        foreach (var season in show.Seasons)
        {
            model.episodesInSeasons.Add(season.Episodes.Select(x =>
                new VideoViewModel()
                {
                    name = x.name,
                    vidsrc = x.vidsrc,
                    comments = x.Comments.Select(y =>
                        new ViewCommentsModel()
                        {
                            username = y.Commenter.Name,
                            comment = y.Text,
                            id = y.Commenter.Id,
                            pfpsrc = y.Commenter.Pfp
                        }).ToList()
                }).ToList());
            model.seasonsNames.Add(season.Name);
        }
        return View(model);
    }
}