using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace university_project.Models
{
    public partial class Professor
    {
        public Professor()
        {
            Courses = new HashSet<Course>();
        }

        [StringLength(9, ErrorMessage = "AFM must be 9 characters long")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Only numbers are allowed.")]
        public string Afm { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string UsersUsername { get; set; } = null!;

        public virtual User UsersUsernameNavigation { get; set; } = null!;
        public virtual ICollection<Course> Courses { get; set; }
    }
}
