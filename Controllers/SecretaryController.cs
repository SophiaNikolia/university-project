using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using university_project.Areas.Identity.Data;
using university_project.Models;

namespace university_project.Controllers
{
    // TODO: Add authorization
    [Authorize(Roles = "Secretary")]
    public class SecretaryController : Controller
    {
        private readonly SignInManager<EntityUser> _signInManager;

        private readonly UserManager<EntityUser> _userManager;
        private readonly IUserStore<EntityUser> _userStore;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserEmailStore<EntityUser> _emailStore;
        private readonly universityContext _context;


        public SecretaryController(SignInManager<EntityUser> signInManager, UserManager<EntityUser> userManager, RoleManager<IdentityRole> roleManager, IUserStore<EntityUser> userStore, universityContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _emailStore = this.GetEmailStore();
            _roleManager = roleManager;
            _context = context;
        }

        [Route("/secretary/add/course")]
        public IActionResult AddCourse()
        {
            return View();
        }

        [Route("/secretary/add/professor")]
        public IActionResult AddProfessor()
        {
            return View(new NewUser());
        }

        [Route("/secretary/add/professor")]
        [HttpPost]
        public async Task<IActionResult> AddProfessor(NewUser model)
        {
            ModelState.Remove("Student");
            ModelState.Remove("Secretary");
            ModelState.Remove("User.Password");
            ModelState.Remove("User.Role");
            ModelState.Remove("Professor.UsersUsernameNavigation");
            ModelState.Remove("Professor.UsersUsername");
            
            if (ModelState.IsValid)
            {
                if(await AddUserAsync(model, "Professor"))
                    return RedirectToAction("Secretary", "Dashboard");
            }

            return View(new NewUser());
        }

        [Route("/secretary/add/student")]
        public IActionResult AddStudent()
        {
            return View(new NewUser());
        }

        [Route("/secretary/add/student")]
        [HttpPost]
        public async Task<IActionResult> AddStudent(NewUser model)
        {
            ModelState.Remove("Professor");
            ModelState.Remove("Secretary");
            ModelState.Remove("User.Password");
            ModelState.Remove("User.Role");
            ModelState.Remove("Student.UsersUsernameNavigation");
            ModelState.Remove("Student.UsersUsername");
            
            if (ModelState.IsValid)
            {
                if(await AddUserAsync(model, "Student"))
                    return RedirectToAction("Secretary", "Dashboard");
            }

            return View(new NewUser());
        }

        public async Task<Boolean> AddUserAsync(NewUser model, string role)
        {
            EntityUser identityUser = this.CreateUser();

            string email = string.Empty;
            int randomNumber = System.Security.Cryptography.RandomNumberGenerator.GetInt32(100000, 1000000);

            model.User.Role = role;
            model.User.Password = model.User.Username + randomNumber.ToString();

            email = CustomEmail(model.User.Username, model.User.Role);

            await _userStore.SetUserNameAsync(identityUser, model.User.Username, CancellationToken.None);
            await _emailStore.SetEmailAsync(identityUser, email, CancellationToken.None);

            var result = await _userManager.CreateAsync(identityUser, model.User.Password);

            if (!result.Succeeded)
            {
                return false;
            }

            // Add the user to the equivalent identity role
            await _userManager.AddToRoleAsync(identityUser, model.User.Role);

            // save the hashed password instead of the real one
            // in the university.db
            model.User.Password = identityUser.PasswordHash;

            // add the user to the Users table
            await _context.Users.AddAsync(model.User);

            if(model.User.Role.Equals("Professor"))
            {
                model.Professor.UsersUsername = model.User.Username;

                // add the user to the Professors table
                await _context.Professors.AddAsync(model.Professor);
            }

            if(model.User.Role.Equals("Secretary"))
            {
                model.Secretary.UsersUsername = model.User.Username;

                // add the user to the Secretaties table
                await _context.Secretaties.AddAsync(model.Secretary);
            }

            if(model.User.Role.Equals("Student"))
            {
                model.Student.UsersUsername = model.User.Username;

                // add the user to the Students table
                await _context.Students.AddAsync(model.Student);
            }

            await _context.SaveChangesAsync();

            return true;
        }

        private EntityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<EntityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. " +
                    $"Ensure that '{nameof(User)}' is not an abstract class and has a parameterless constructor.");
            }
        }

        private IUserEmailStore<EntityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<EntityUser>)_userStore;
        }

        private string CustomEmail(string username, string role)
        {
            string email = string.Empty;

            switch(role)
            {
                case "Secretary":
                    email = username + "@sec.unipi.gr";
                    break;
                default:
                    email = username + "@unipi.gr";
                    break;
            }

            return email;
        }
    }
}