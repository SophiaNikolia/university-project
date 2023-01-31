using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace university_project.Models
{
    public partial class NewUser
    {
        public User? User { get; set; }
        public Secretary? Secretary { get; set; }
        public Professor? Professor{ get; set; }
        public Student? Student { get; set; }

        public List<SelectListItem> SelectRole { get; private set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Professor", Text = "Professor" },
            new SelectListItem { Value = "Secretary", Text = "Secretary" },
            new SelectListItem { Value = "Student", Text = "Student" }
        };

        public List<SelectListItem> SelectDepartment { get; private set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "BFM", Text = "Banking and Financial Management" },
            new SelectListItem { Value = "BA", Text = "Business Administration" },
            new SelectListItem { Value = "CS", Text = "Computer Science" },
            new SelectListItem { Value = "DS", Text = "Digital Systems" },
            new SelectListItem { Value = "IMT", Text = "Industrial Management and Technology" },
            new SelectListItem { Value = "IES", Text = "International and European Studies" },
            new SelectListItem { Value = "SIS", Text = "Statistics and Insurance Science" },
            new SelectListItem { Value = "TS", Text = "Tourism Studies " }
        };
    }
}
