using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace university_project.Models
{
    public partial class Student
    {
        public Student()
        {
            CourseHasStudents = new HashSet<CourseHasStudent>();
        }

        public long RegistrationNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Department { get; set; }
        public string UsersUsername { get; set; } = null!;

        public virtual User UsersUsernameNavigation { get; set; } = null!;
        public virtual ICollection<CourseHasStudent> CourseHasStudents { get; set; }
    }
}
