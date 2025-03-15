using joro.too.Services.Services.IServices;
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

    public async Task<IActionResult> WatchMovie()
    {
        return View();
    }
}