using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using university_project.Areas.Identity.Data;
using university_project.Models;
using university_project.ViewModels;

namespace university_project.Controllers
{
    public class StudentController : Controller
    {
        private readonly SignInManager<EntityUser> _signInManager;
        private readonly universityContext _context;

        public StudentController(universityContext context, SignInManager<EntityUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        [Route("/student/grades")]
        public async Task<IActionResult> Grades()
        {
            var identityUser = await _signInManager.UserManager.GetUserAsync(User);

            var student = _context.Students.Where(e => e.UsersUsername.Equals(identityUser.UserName)).FirstOrDefault();

            var courseGrades = CourseGradesAsync(student);

            return View(await courseGrades);
        }


        private async Task<IEnumerable<GradesData>> CourseGradesAsync(Student student)
        {
            var courseList = await _context.Courses.ToListAsync();

            var courseHasStudents = await _context.CourseHasStudents
                .Where( course => course.StudentsRegistrationNumber == student.RegistrationNumber )
                .ToListAsync();
            var courseGrades = courseList.Join(courseHasStudents,
                                                course => course.IdCourse,
                                                courseHasStudent => courseHasStudent.CourseIdCourse,
                                                (course, courseHasStudent) => {

                                                    GradesData gradesData = new GradesData();

                                                    gradesData.CourseTitle = course.CourseTitle;
                                                    gradesData.Semester = course.CourseSemester;
                                                    gradesData.CourseGrade = courseHasStudent.GradeCourseStudent;

                                                    return gradesData;
                                                });

            return courseGrades;
        }
    }
}
