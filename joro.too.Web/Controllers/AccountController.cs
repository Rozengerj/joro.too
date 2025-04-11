using joro.too.Entities;
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

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
        RoleManager<IdentityRole> roleManager, IUserService userService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _userService = userService;
    }

    public IActionResult Login() => View();

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
            var user = new User { UserName = model.UserName, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            //Console.WriteLine(String.Join(", ", result.Errors.Select(e => e.Description)));
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

    public async Task<IActionResult> WriteComment(string text, int mediaId, bool isShow)
    {
        await _userService.WriteComment(text, await _userManager.GetUserAsync(User), mediaId, isShow);
        if (isShow)
        {
            return RedirectToAction("WatchShow", "Watch", new { showId = mediaId });
        }
        return RedirectToAction("WatchMovie","Watch", new {movieId = mediaId});
    }
    public IActionResult AccessDenied()
    {
        return View();
    }

    public async Task<IActionResult> EditAccountInfo()
    {
        var user = await _userManager.GetUserAsync(User);

        return View();
    }
}