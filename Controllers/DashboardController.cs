using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using university_project.Areas.Identity.Data;
using university_project.Models;

namespace university_project.Controllers
{

    public class DashboardController : Controller
    {
        private readonly SignInManager<EntityUser> _signInManager;
        private readonly UserManager<EntityUser> _userManager;
        private readonly IUserStore<EntityUser> _userStore;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly universityContext _context;


        public DashboardController(SignInManager<EntityUser> signInManager, universityContext context)
        {
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Student()
        {
            return View();
        }

        public IActionResult Professor()
        {
            return View();
        }

        public async Task<IActionResult> Secretary()
        {

            var identityUser = await _signInManager.UserManager.GetUserAsync(User);

            var secretary = _context.Secretaties.Where( e => e.UsersUsername.Equals(identityUser.UserName)).FirstOrDefault();

            return View(secretary);
            
        }
    }
}
