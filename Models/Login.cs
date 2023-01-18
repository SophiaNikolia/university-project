using System.ComponentModel.DataAnnotations;

namespace university_project.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Please enter your email address!")]
        //[EmailAddress]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Please enter your password!")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
