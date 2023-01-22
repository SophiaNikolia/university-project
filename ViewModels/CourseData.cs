using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using university_project.Models;

namespace university_project.ViewModels
{
    public partial class CourseData
    {
        public Course Course { get; set; }

        public Professor Professor { get; set; }

        public List<SelectListItem> SelectSemester { get; private set; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "1st" },
            new SelectListItem { Value = "2", Text = "2nd" },
            new SelectListItem { Value = "3", Text = "3rd" },
            new SelectListItem { Value = "4", Text = "4th" },
            new SelectListItem { Value = "5", Text = "5th" },
            new SelectListItem { Value = "6", Text = "6th" },
            new SelectListItem { Value = "7", Text = "7th" },
            new SelectListItem { Value = "8", Text = "8th" }
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
