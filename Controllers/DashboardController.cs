using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using university_project.Areas.Identity.Data;
using university_project.Models;
using university_project.ViewModels;

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

        public async Task<IActionResult> Professor()
        {
            var identityUser = await _signInManager.UserManager.GetUserAsync(User);

            var professor = await _context.Professors.Where( e => e.UsersUsername.Equals(identityUser.UserName)).FirstOrDefaultAsync();

            ProfessorDashboardData professorDashboardData = new ProfessorDashboardData();

            professorDashboardData.Name = professor.Name;
            professorDashboardData.Surname = professor.Surname;

            professorDashboardData.Students = _context.Courses.ToList().Join( _context.CourseHasStudents.ToList(),
                                                course => course.IdCourse,
                                                courseHasStudent => courseHasStudent.CourseIdCourse,
                                                (course, courseHasStudent) => {

                                                    if(course.ProfessorsAfm == professor.Afm && courseHasStudent.GradeCourseStudent <= 4)
                                                    {
                                                        return true;
                                                    }

                                                    return false;
                                                }).Count( item => item == true );

            professorDashboardData.Courses = _context.Courses.Where(course => course.ProfessorsAfm == professor.Afm).Count();

            return View(professorDashboardData);
        }

        public async Task<IActionResult> Secretary()
        {

            var identityUser = await _signInManager.UserManager.GetUserAsync(User);

            var secretary = _context.Secretaties.Where( e => e.UsersUsername.Equals(identityUser.UserName)).FirstOrDefault();

            SecretaryDashboardData secretaryDashboardData = new SecretaryDashboardData();

            secretaryDashboardData.Name = secretary.Name;
            secretaryDashboardData.Surname = secretary.Surname;

            secretaryDashboardData.ListedCourses = _context.Courses.ToList().Count;
            secretaryDashboardData.ListedProfessors = _context.Professors.ToList().Count;
            secretaryDashboardData.ListedStudents = _context.Students.ToList().Count;

            return View(secretaryDashboardData);
            
        }
    }
}
