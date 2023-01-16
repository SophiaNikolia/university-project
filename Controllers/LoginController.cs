using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using university_project.Areas.Identity.Data;
using university_project.Models;

namespace university_project.Controllers
{
    public class LoginController : Controller
    {

        private readonly SignInManager<EntityUser> _signInManager;
        private readonly UserManager<EntityUser> _userManager;
        private readonly IUserStore<EntityUser> _userStore;
        private readonly ILogger<Admin> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public LoginController(RoleManager<IdentityRole> roleManager, ILogger<Admin> logger, IUserStore<EntityUser> userStore, UserManager<EntityUser> userManager, SignInManager<EntityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userStore = userStore;
            _roleManager = roleManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Login login)
        {

            if (ModelState.IsValid)
            {
                // find user by email
                var usr = await _signInManager.UserManager.FindByEmailAsync(login.Email);

                if (usr == null)
                {
                    ModelState.AddModelError(string.Empty, "Email not found");
                    return View();
                }

                var role = await _signInManager.UserManager.GetRolesAsync(usr);

                var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return RedirectToAction("Student", "Dashboard");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View();
            }

            return View();
        }
    }
}
