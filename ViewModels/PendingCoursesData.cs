using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using university_project.Models;

namespace university_project.ViewModels
{
    public partial class PendingCoursesData
    {
        public long IdCourse { get; set; }

        public string CourseTitle { get; set; }

        public string CourseSemester { get; set; }

        public long? Grade { get; set; }

        public long StudentRegistrationNumber { get; set; }

        public string StudentName { get; set; }
        
        public string StudentSurname { get; set; } 

    }
}
    