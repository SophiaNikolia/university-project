using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using university_project.Areas.Identity.Data;
using university_project.Models;

namespace university_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<EntityUser> _signInManager;
        private readonly UserManager<EntityUser> _userManager;

        private readonly ILogger<HomeController> _logger;

        public HomeController(SignInManager<EntityUser> signInManager, UserManager<EntityUser> userManager, ILogger<HomeController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            if (_signInManager.IsSignedIn(User) && !User.IsInRole("Admin"))
            {
                
                var user = await _userManager.GetUserAsync(User);

                var role = await _userManager.GetRolesAsync(user);
                
                // redirect to Action based on user's role
                return RedirectToAction(role[0], "Dashboard");

            }
            return RedirectToAction("Index", "Login");
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
}