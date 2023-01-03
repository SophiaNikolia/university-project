using System;
using System.Collections.Generic;

namespace university_project.Models
{
    public partial class CourseHasStudent
    {
        public long CourseIdCourse { get; set; }
        public long StudentsRegistrationNumber { get; set; }
        public long GradeCourseStudent { get; set; }

        public virtual Course CourseIdCourseNavigation { get; set; } = null!;
        public virtual Student StudentsRegistrationNumberNavigation { get; set; } = null!;
    }
}
