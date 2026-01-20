using Fitness.Models;
using Fitness.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fitness.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<AppUser> _signInManager { get; }
        private RoleManager<IdentityRole> _roleManager { get; }
        private UserManager<AppUser> _userManager { get; }
        public AccountController(SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager,
             UserManager<AppUser> userManager)
        {
            _signInManager=signInManager;
            _userManager=userManager;
            _roleManager=roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM user) 
        {
            if (!ModelState.IsValid) return View(user);
            AppUser appUser = new AppUser
            {
                Email=user.Email,
                Name=user.Fullname,
                UserName=user.Username
            };
            var identityResult = await _userManager.CreateAsync(appUser, user.Password);
            if (!identityResult.Succeeded) 
            {
                foreach (var err in identityResult.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                    return View(user);
                };
            }
            await _userManager.AddToRoleAsync(appUser, "Member");
            await _signInManager.SignInAsync(appUser, true);
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> CreateRoles() 
        {
            if (!await _roleManager.RoleExistsAsync("SuperAdmin")) 
            {
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await _roleManager.RoleExistsAsync("Member"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Member"));
            }
            return Content("Roles Created");
        }
        public async Task<IActionResult> Logout() 
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM user) 
        {
            if (!ModelState.IsValid) return View(user);
            AppUser? appUser = await _userManager.FindByEmailAsync(user.Email);
            if (appUser==null) 
            {
                ModelState.AddModelError("", "Your password or email is incorrect.");
                return View(user); 
            }
            var signInResult = await _signInManager.PasswordSignInAsync(appUser, user.Password, true, true);
            if (signInResult.IsLockedOut) 
            {
                ModelState.AddModelError("", "Please try again later");
                return View(user);
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Your password or email is incorrect.");
                return View(user);
            }
            await _signInManager.SignInAsync(appUser, true);
            await _userManager.UpdateAsync(appUser);
            return RedirectToAction("Index", "Home");
        }
    }
}
