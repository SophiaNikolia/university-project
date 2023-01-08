using Microsoft.AspNetCore.Mvc;

namespace university_project.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
