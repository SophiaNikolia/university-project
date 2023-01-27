namespace university_project.ViewModels
{
    public class DashboardStudentData
    {
        public string StudentName { get; set; }
        public string StudentSurname { get; set; }
        public string ProfessorName { get; set; }
        public string ProfessorSurname { get; set; }
        public string CourseTitle { get; set; }
        public decimal? Gpa { get; set; }
        public int? CompletedCourses { get; set; }
        public int? Ects { get; set; }
    }
}
