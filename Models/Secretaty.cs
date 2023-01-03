using System;
using System.Collections.Generic;

namespace university_project.Models
{
    public partial class Secretaty
    {
        public long Phonenumber { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string UsersUsername { get; set; } = null!;

        public virtual User UsersUsernameNavigation { get; set; } = null!;
    }
}
