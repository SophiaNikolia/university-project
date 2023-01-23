using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using university_project.Models;

namespace university_project.ViewModels
{
    public partial class SecretaryDashboardData
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public int ListedCourses { get; set; }

        public int ListedProfessors { get; set; }

        public int ListedStudents { get; set; }

    }
}
