using Microsoft.AspNetCore.Mvc;

namespace university_project.Controllers
{
    // TODO: Add authorization
    public class SettingsController : Controller
    {
        public IActionResult Email()
        {
            return View();
        }

        public IActionResult Password()
        {
            return View();
        }

        [Route("settings/personal-data")]
        public IActionResult Personal()
        {
            return View();
        }

        public IActionResult Theme()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }
        
    }
}
