using System;
using System.Collections.Generic;

namespace university_project.Models
{
    public partial class User
    {
        public User()
        {
            Professors = new HashSet<Professor>();
            Secretaties = new HashSet<Secretary>();
            Students = new HashSet<Student>();
        }

        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;

        public virtual ICollection<Professor> Professors { get; set; }
        public virtual ICollection<Secretary> Secretaties { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
