using GymManagement.BLL.ViewModels.AccountViewModel;
using GymManagement.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace GymManagement.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager , ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model,CancellationToken ct)
        {
            if(!ModelState.IsValid) return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Email Or Password");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                _logger.LogInformation($"User {user.UserName} Is Signed In");
                return RedirectToAction(nameof(Index), "Home");
            }
            else if (result.IsLockedOut)
            {
                _logger.LogInformation($"User {user.UserName} Is LockedOut");
                ModelState.AddModelError("InvalidLogin", "This Account Is Locked , Try Again Later");
                return View(model);
            }
            else
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Email Or Password");
                return View(model);
            }
            
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}