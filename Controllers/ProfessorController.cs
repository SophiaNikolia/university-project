
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using university_project.Areas.Identity.Data;
using university_project.Models;
using university_project.ViewModels;

namespace university_project.Controllers
{
    public class ProfessorController : Controller
    {
        private readonly SignInManager<EntityUser> _signInManager;
        private readonly universityContext _context;

        public ProfessorController(SignInManager<EntityUser> signInManager, universityContext context)
        {
            _signInManager = signInManager;
            _context = context;
        }


        public async Task<IActionResult> Done()
        {
            return View();
        }

        public async Task<IActionResult> Pending()
        {
            var identityUser = await _signInManager.UserManager.GetUserAsync(User);

            var professor = _context.Professors.Where( e => e.UsersUsername.Equals(identityUser.UserName)).FirstOrDefault();

            var pendingCourses = PendingCoursesAsync(professor);

            return View(await pendingCourses);
        }

        [Route("/professor/grade/student")]
        [HttpGet]
        public async Task<IActionResult> GradeStudent(long? StudentRegistrationNumber, long? IdCourse)
        {
            var courseHasStudent = await _context.CourseHasStudents
                .FirstOrDefaultAsync(course => course.StudentsRegistrationNumber == StudentRegistrationNumber && course.CourseIdCourse == IdCourse);
                
            GradeStudentData gradeStudentData = new GradeStudentData();

            gradeStudentData.CourseHasStudent = new CourseHasStudent();

            gradeStudentData.CourseHasStudent.CourseIdCourse = courseHasStudent.CourseIdCourse;
            gradeStudentData.CourseHasStudent.StudentsRegistrationNumber = courseHasStudent.StudentsRegistrationNumber;

            return View(gradeStudentData);
        }

        [Route("/professor/grade/student")]
        [HttpPost]
        public async Task<IActionResult> GradeStudent(GradeStudentData gradeStudentData)
        {
            ModelState.Remove("CourseHasStudent.CourseIdCourseNavigation");
            ModelState.Remove("CourseHasStudent.StudentsRegistrationNumberNavigation");

            if (ModelState.IsValid)
            {
                _context.CourseHasStudents.Update(gradeStudentData.CourseHasStudent);
                await _context.SaveChangesAsync();

                return RedirectToAction("Pending", "Professor");
            }

            return View(gradeStudentData);
        }


        private async Task<IEnumerable<PendingCoursesData>> PendingCoursesAsync(Professor professor)
        {
            var courseList = await _context.Courses.Where( course => course.ProfessorsAfm == professor.Afm ).ToListAsync();

            var courseHasStudents = await _context.CourseHasStudents.ToListAsync();

            var pendingCourses = courseList.Join( courseHasStudents,
                                                course => course.IdCourse,
                                                courseHasStudent => courseHasStudent.CourseIdCourse,
                                                (course, courseHasStudent) => {
                                                    
                                                    PendingCoursesData pendingCourse = new PendingCoursesData();

                                                    if(courseHasStudent.GradeCourseStudent == null)
                                                    {
                                                        pendingCourse.IdCourse = course.IdCourse;
                                                        pendingCourse.CourseTitle = course.CourseTitle;
                                                        pendingCourse.CourseSemester = course.CourseSemester;
                                                        pendingCourse.Grade = courseHasStudent.GradeCourseStudent;
                                                        pendingCourse.StudentRegistrationNumber = courseHasStudent.StudentsRegistrationNumber;
                                                    }

                                                    return pendingCourse;
                                                });
            return pendingCourses;
        }
    }
}
