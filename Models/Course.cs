using System;
using System.Collections.Generic;

namespace university_project.Models
{
    public partial class Course
    {
        public Course()
        {
            CourseHasStudents = new HashSet<CourseHasStudent>();
        }

        public long IdCourse { get; set; }
        public string CourseTitle { get; set; } = null!;
        public string CourseSemester { get; set; } = null!;
        public long ProfessorsAfm { get; set; }

        public virtual Professor ProfessorsAfmNavigation { get; set; } = null!;
        public virtual ICollection<CourseHasStudent> CourseHasStudents { get; set; }
    }
}
