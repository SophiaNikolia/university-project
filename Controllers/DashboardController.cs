using Microsoft.AspNetCore.Mvc;

namespace university_project.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Student()
        {
            return View();
        }

        public IActionResult Professor()
        {
            return View();
        }
    }
}
