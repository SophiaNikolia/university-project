using System;
using System.Collections.Generic;

namespace university_project.Models
{
    public partial class Student
    {
        public Student()
        {
            CourseHasStudents = new HashSet<CourseHasStudent>();
        }

        public long RegistrationNumber { get; set; }
        public long Name { get; set; }
        public long Surname { get; set; }
        public long Department { get; set; }
        public string UsersUsername { get; set; } = null!;

        public virtual User UsersUsernameNavigation { get; set; } = null!;
        public virtual ICollection<CourseHasStudent> CourseHasStudents { get; set; }
    }
}
