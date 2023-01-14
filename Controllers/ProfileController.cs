using Microsoft.AspNetCore.Mvc;

namespace university_project.Controllers
{
    // TODO: Add authorization
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}