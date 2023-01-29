using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [StringLength(11, ErrorMessage = "AFM must be 11 characters long")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Only numbers are allowed.")]
        public string ProfessorsAfm { get; set; }

        public virtual Professor ProfessorsAfmNavigation { get; set; } = null!;
        public virtual ICollection<CourseHasStudent> CourseHasStudents { get; set; }
    }
}
