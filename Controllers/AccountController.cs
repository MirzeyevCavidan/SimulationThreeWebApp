using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimulationThreeWebApp.Models;
using SimulationThreeWebApp.Utilities.Enums;
using SimulationThreeWebApp.ViewModels.Account;

namespace SimulationThreeWebApp.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM registerVM)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        AppUser user = new AppUser()
        {
            Name = registerVM.Name,
            UserName = registerVM.UserName,
            SurName = registerVM.SurName,
            Email = registerVM.Email,
        };

        IdentityResult result = await _userManager.CreateAsync(user, registerVM.Password);

        if (!result.Succeeded)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        await _userManager.AddToRoleAsync(user, UserRole.Member.ToString());

        await _signInManager.SignInAsync(user, false);

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM loginVM)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        AppUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginVM.UserNameOrEmail || u.Email == loginVM.UserNameOrEmail);

        if (user == null)
        {
            ModelState.AddModelError("", "Username or email invalid");
            return View();
        }

        var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.IsPersisted, true);

        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Password invalid");
            return View();
        }

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    public async Task<IActionResult> CreateRole()
    {
        foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
        {
            if (!await _roleManager.RoleExistsAsync(role.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
            }
        }

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }
    public IActionResult AccessDenied()
    {
        return View();
    }
}