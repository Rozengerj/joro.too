using System.Collections;
using System.Diagnostics;
using joro.too.Entities;
using joro.too.Services.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using joro.too.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace joro.too.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IMediaService mediaService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public HomeController(ILogger<HomeController> logger, IMediaService _mediaService
        //,UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager
    )
    {
        _logger = logger;
        mediaService = _mediaService;

        //_userManager = userManager;
        //_signInManager = signInManager;
        //_roleManager = roleManager;
    }

    public async Task<IActionResult> Index()
    {
        var tempTuple = await mediaService.GetMediasWithGenres(null);
        var recommendedMedia = new List<IMedia>();
        recommendedMedia.AddRange(tempTuple.Item1);
        recommendedMedia.AddRange(tempTuple.Item2);
        recommendedMedia.Where(x => x.RatedCount > 0).Where(x => (x.RatingsSum / x.RatedCount) > 9).ToList();
        Random k = new Random();
        HashSet<string> thething = new HashSet<string>();
        List<SearchResultModel> model = new List<SearchResultModel>();
        if (recommendedMedia.IsNullOrEmpty())
        {
            return View();
        }

        for (int i = 0; i < 10; i++)
        {
            var currmedia = recommendedMedia[k.Next(0, recommendedMedia.Count)];
            if(thething.Add(currmedia.Name))
            {
                bool isShow = currmedia is Show;
                model.Add(new SearchResultModel()
                {
                    name = currmedia.Name,
                    id = currmedia.Id,
                    desc = currmedia.Description,
                    Genres = new List<SelectListItem>(),
                    imgsrc = currmedia.MediaImgSrc,
                    isShow = isShow
                });
            }
        }
        return View(model);
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