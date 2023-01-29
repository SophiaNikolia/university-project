using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using university_project.Models;

namespace university_project.ViewModels
{
    public partial class ProfessorDashboardData
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public int? Hours { get; set; }

        public int Courses { get; set; }

        public int Students { get; set; }

        public List<DashboardCardData>? DashboardCardDataList { get; set; } 

    }
}
