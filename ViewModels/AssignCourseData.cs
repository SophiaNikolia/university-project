using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using university_project.Models;

namespace university_project.ViewModels
{
    public partial class AssignCourseData
    {
        public long IdCourse { get; set; }

        public long RegistrationNumber { get; set; }

        public List<SelectListItem> SelectCourse { get; set; }

    }
}
    