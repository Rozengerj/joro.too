using System.Diagnostics;
using joro.too.Entities;
using joro.too.Services.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using joro.too.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace joro.too.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IMediaService mediaService;
    public HomeController(ILogger<HomeController> logger,IMediaService _mediaService)
    {
        _logger = logger;
        mediaService = _mediaService;
    }

    public async Task<IActionResult> Index()
    {
        var recommendedMedia = await mediaService.GetMediasWithGenres(null);
        recommendedMedia.Where(x=> x.Rating.Average() > (decimal)7.5).ToList();
        Random k = new Random();
        HashSet<SearchResultModel> thething = new HashSet<SearchResultModel>();
        if (recommendedMedia.IsNullOrEmpty())
        {
            return View();
        }
        for (int i = 0; i < 10; i++)
        {
            var currmedia = recommendedMedia[k.Next(0, recommendedMedia.Count)];
            thething.Add(new SearchResultModel()
            {
                name = currmedia.Name,
                id = currmedia.Id,
                desc = currmedia.Description,
                Genres = new List<SelectListItem>(),
                imgsrc = currmedia.MediaImgSrc
            });
        }
        return View(thething);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}