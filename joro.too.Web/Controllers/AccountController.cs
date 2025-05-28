using System.Runtime.CompilerServices;
using joro.too.Entities;
using joro.too.Services.Services;
using joro.too.Services.Services.IServices;
using joro.too.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace joro.too.Web.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUserService _userService;
    private CloudinaryService _cloudinary;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
        RoleManager<IdentityRole> roleManager, IUserService userService, CloudinaryService cloudinary)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _userService = userService;
        _cloudinary = cloudinary;
    }

    public IActionResult Login()
    {
        var model = new LoginViewModel();
        model.IncorrectData=string.Empty;
        return View(model);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        else
        {
            // dolu tuka 
            var user = await _userManager.FindByEmailAsync(model.Email);
            
            if (user is null)
            {
                TempData["Error"] = "User not found";
                model.IncorrectData = "There is no such user! Please try again.";
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        ModelState.AddModelError("", "Invalid login");
        return View(model);
    }

    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new User { UserName = model.UserName, Email = model.Email, Pfp = "https://res.cloudinary.com/djubwo5uq/image/upload/v1744467542/n9kfa5wcfkpmnzti1quv.webp", RatedShows = new List<Show>(), RatedMovies = new List<Movie>() };
            Console.WriteLine(user.UserName);
            Console.WriteLine(user.Pfp);
            Console.WriteLine(user.Email);
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User"); // По подразбиране новите потребители са "User" 
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> WriteCommentMovie(string text, int mediaId, bool isShow)
    {
        await _userService.WriteComment(text, await _userManager.GetUserAsync(User), mediaId, isShow);
        return RedirectToAction("WatchMovie","Watch", new {movieId = mediaId});
    }
    public async Task<IActionResult> WriteCommentShow(string text, int episodeId, bool isShow, int mediaId)
    {
        await _userService.WriteComment(text, await _userManager.GetUserAsync(User), episodeId, isShow);
       
            return RedirectToAction("WatchShow", "Watch", new { showId = mediaId });
    }
    public IActionResult AccessDenied()
    {
        return View();
    }

    public async Task<IActionResult> EditAccountInfo()
    {
        var user = await _userManager.GetUserAsync(User);
        var model = new EditAccountModel()
        {
            PfpSource = user.Pfp,
            Email = user.Email,
            Pfp = null,
            Username = user.UserName
        };
        return View("ViewProfile", model);
    }
    [HttpPost]
    public async Task<IActionResult> EditAccountInfo(string username, string email, IFormFile? newimg)
    {
        var user = await _userManager.GetUserAsync(User);
        user.UserName = username;
        user.Email = email;
        if (newimg is not null)
        {
            if (!user.Pfp.Equals("https://res.cloudinary.com/djubwo5uq/image/upload/v1744467542/n9kfa5wcfkpmnzti1quv.webp"))
            {
                await _cloudinary.DeleteImageAsync(user.Pfp);
            }
            user.Pfp = await _cloudinary.UploadPfpAsync(newimg);
        }
        await _userManager.UpdateAsync(user);
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> DeleteUser()
    {
        var user = await _userManager.GetUserAsync(User);
        _userManager.DeleteAsync(user);
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> DeleteComment(int id, int mediaId, bool isShow)
    {
        await _userService.DeleteComment(id);
        if (isShow)
        {
            return RedirectToAction("WatchShow", "Watch", new { showId = mediaId });
        }
        return RedirectToAction("WatchMovie","Watch", new {movieId = mediaId});
    }

    public async Task<IActionResult> RateMedia(int mediaId, bool isShow, float rating)
    {
        Console.WriteLine(mediaId);
        Console.WriteLine(isShow);
        Console.WriteLine(rating);
        
        await _userService.RateMedia(await _userManager.GetUserAsync(User), rating, mediaId, isShow);
        return RedirectToAction("ViewMedia", "Search", new { id = mediaId, isShow = isShow });
    }
}