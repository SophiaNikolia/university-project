using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using university_project.Models;

namespace university_project.ViewModels
{
    public partial class RegisterCourseData
    {
        public long IdCourse { get; set; }

        [StringLength(11, ErrorMessage = "AFM must be 11 characters long")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Only numbers are allowed.")]
        public string ProfessorAfm { get; set; }

        public List<SelectListItem> SelectCourse { get; set; }

    }
}
    