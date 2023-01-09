using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using university_project.Areas.Identity.Data;

namespace university_project.Controllers
{
    public class LogoutController : Controller
    {
        private readonly SignInManager<EntityUser> _signInManager;

        public LogoutController(SignInManager<EntityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> OnPost()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Login");
        }
    }
}
