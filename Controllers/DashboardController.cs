﻿using Microsoft.AspNetCore.Identity;
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

        public async Task<IActionResult> Student()
        {
            var identityUser = await _signInManager.UserManager.GetUserAsync(User);

            var student = _context.Students.Where(e => e.UsersUsername.Equals(identityUser.UserName)).FirstOrDefault();

            var passed_lesson = _context.CourseHasStudents
                .Where(e => e.GradeCourseStudent >= 5 && e.StudentsRegistrationNumber == student.RegistrationNumber);

            var notPassedCourses = _context.CourseHasStudents
                .Where(e => e.GradeCourseStudent < 5 && e.StudentsRegistrationNumber == student.RegistrationNumber);

            var notPassedCoursesData = _context.Courses
                .Join(notPassedCourses,
                        course => course.IdCourse,
                        courseHasStudent => courseHasStudent.CourseIdCourse,
                        (course, courseHasStudent) => course);

            long? sum = 0;
            foreach (var i in passed_lesson)
            {
                sum += i.GradeCourseStudent;

            }

            var gpa = sum / (decimal)passed_lesson.Count();
            
            DashboardStudentData model = new DashboardStudentData();

            model.StudentName = student.Name;
            model.StudentSurname = student.Surname;
            if (gpa == null)
            {
                model.Gpa = 0;
                model.CompletedCourses = 0;
                model.Ects = 0;
            }
            else
            {
                model.Gpa = Math.Round((decimal)gpa, 2);
            }

            model.CompletedCourses = passed_lesson.Count();

            model.Ects = passed_lesson.Count() * 5;

            model.DashboardCardDataList = new List<DashboardCardData>();

            foreach (var course in await notPassedCoursesData.ToListAsync())
            {
                DashboardCardData dashboardCardData = new DashboardCardData();

                dashboardCardData.CourseTitle = course.CourseTitle;

                var professor = await _context.Professors.FindAsync(course.ProfessorsAfm);

                dashboardCardData.ProfessorName = professor.Name;
                dashboardCardData.ProfessorSurname = professor.Surname;

                dashboardCardData.CourseSemester = int.Parse(course.CourseSemester);

                model.DashboardCardDataList.Add(dashboardCardData);
            }

            return View(model);
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

            var courses = _context.Courses.Where(course => course.ProfessorsAfm == professor.Afm);

            professorDashboardData.Courses = courses.Count();

            professorDashboardData.DashboardCardDataList = new List<DashboardCardData>();
            professorDashboardData.Hours = 0;
            foreach (var course in courses)
            {
                DashboardCardData dashboardCardData = new DashboardCardData();

                dashboardCardData.CourseTitle = course.CourseTitle;
                dashboardCardData.CourseTotalHours = 25;
                dashboardCardData.ProfessorName = professorDashboardData.Name;
                dashboardCardData.CourseSemester = int.Parse(course.CourseSemester);
                
                professorDashboardData.Hours += dashboardCardData.CourseTotalHours;

                professorDashboardData.DashboardCardDataList.Add(dashboardCardData);
            }

            return View(professorDashboardData);
        }

        public async Task<IActionResult> Secretary()
        {

            var identityUser = await _signInManager.UserManager.GetUserAsync(User);

            var secretary = _context.Secretaries.Where( e => e.UsersUsername.Equals(identityUser.UserName)).FirstOrDefault();

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
