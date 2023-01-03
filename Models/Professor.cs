using System;
using System.Collections.Generic;

namespace university_project.Models
{
    public partial class Professor
    {
        public Professor()
        {
            Courses = new HashSet<Course>();
        }

        public long Afm { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string UsersUsername { get; set; } = null!;

        public virtual User UsersUsernameNavigation { get; set; } = null!;
        public virtual ICollection<Course> Courses { get; set; }
    }
}
