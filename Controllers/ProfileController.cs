using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using university_project.Areas.Identity.Data;
using university_project.Models;
using university_project.ViewModels;

namespace university_project.Controllers
{
    // TODO: Add authorization
    public class ProfileController : Controller
    {
        private readonly SignInManager<EntityUser> _signInManager;
        private readonly universityContext _context;

        public ProfileController(SignInManager<EntityUser> signInManager, universityContext context)
        {
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<IActionResult> Student()
        {
            var identityUser = await _signInManager.UserManager.GetUserAsync(User);
            
            var student = _context.Students.Where(e => e.UsersUsername.Equals(identityUser.UserName)).FirstOrDefault();

            ProfileData profileData = new ProfileData();

            profileData.Name = student.Name;
            profileData.Surname = student.Surname;
            profileData.Department = student.Department;
            profileData.Email = student.UsersUsername + "@unipi.gr";
            profileData.Role = "Student";


            return View(profileData);
        }

        public async Task<IActionResult> Professor()
        {
            var identityUser = await _signInManager.UserManager.GetUserAsync(User);

            var professor = _context.Professors
                .Where(e => e.UsersUsername.Equals(identityUser.UserName))
                .FirstOrDefault();

            ProfileData profileData = new ProfileData();

            profileData.Name = professor.Name;
            profileData.Surname = professor.Surname;
            profileData.Department = professor.Department;
            profileData.Email = professor.UsersUsername + "@unipi.gr";
            profileData.Role = "Professor";

            return View(profileData);
        }

        public async Task<IActionResult> Secretary()
        {
            var identityUser = await _signInManager.UserManager.GetUserAsync(User);

            var secretary = _context.Secretaties.Where(e => e.UsersUsername.Equals(identityUser.UserName)).FirstOrDefault();
            
            ProfileData profileData = new ProfileData();

            profileData.Name = secretary.Name;
            profileData.Surname = secretary.Surname;
            profileData.Department = secretary.Department;
            profileData.Email = secretary.UsersUsername + "@sec.unipi.gr";
            profileData.Role = "Secretary";

            return View(profileData); ;
        }
    }
}