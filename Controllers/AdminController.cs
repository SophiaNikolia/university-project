using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using university_project.Areas.Identity.Data;
using university_project.Models;

namespace university_project.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly SignInManager<EntityUser> _signInManager;
        private readonly UserManager<EntityUser> _userManager;
        private readonly IUserStore<EntityUser> _userStore;
        private readonly ILogger<Admin> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserEmailStore<EntityUser> _emailStore;

        private readonly universityContext _context;

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public AdminController(SignInManager<EntityUser> signInManager, ILogger<Admin> logger, UserManager<EntityUser> userManager, IUserStore<EntityUser> userStore, RoleManager<IdentityRole> roleManager, universityContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(Admin admin)
        {
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // find user by email
                var usr = await _signInManager.UserManager.FindByEmailAsync(admin.Email);

                if (usr == null)
                {
                    ModelState.AddModelError(string.Empty, "Email not found");
                    return View();
                }

                var result = await _signInManager.PasswordSignInAsync(admin.Email, admin.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return RedirectToAction("Index", "Admin");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View();
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new NewUser());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewUser model)
        {
            ModelState.Remove("Professor");
            ModelState.Remove("Secretary");

            string validationErrors = string.Join(",",
                                ModelState.Values.Where(E => E.Errors.Count > 0)
                                .SelectMany(E => E.Errors)
                                .Select(E => E.ErrorMessage)
                                .ToArray());

            if (ModelState.IsValid)
            {

                EntityUser identityUser = this.CreateUser();

                string email = string.Empty;

                email = CustomEmail(model.User.Username, model.User.Role);

                await _userStore.SetUserNameAsync(identityUser, model.User.Username, CancellationToken.None);
                await _emailStore.SetEmailAsync(identityUser, email, CancellationToken.None);

                var result = await _userManager.CreateAsync(identityUser, model.User.Password);

                if (!result.Succeeded)
                {
                    return View();
                }

                // Add the user to the equivalent identity role
                await _userManager.AddToRoleAsync(identityUser, model.User.Role);

                // save the hashed password instead of the real one
                // in the university.db
                model.User.Password = identityUser.PasswordHash;

                // add the user to the Users table
                await _context.Users.AddAsync(model.User);

                await _context.SaveChangesAsync();

                // save the username of the newly created user
                TempData["username"] = model.User.Username;

                // redirect to the action based on the user's role
                return RedirectToAction(model.User.Role);
            }

            return View(new NewUser());
        }

        [HttpGet]
        [Route("/admin/create/secretary")]
        public IActionResult Secretary()
        {
            return View(new NewUser());
        }

        [HttpPost]
        [Route("/admin/create/secretary")]
        public async Task<IActionResult> Secretary(NewUser model)
        {
            ModelState.Remove("Professor");
            ModelState.Remove("User");
            ModelState.Remove("Secretary.UsersUsernameNavigation");
            ModelState.Remove("Secretary.UsersUsername");

            string validationErrors = string.Join(",",
                                ModelState.Values.Where(E => E.Errors.Count > 0)
                                .SelectMany(E => E.Errors)
                                .Select(E => E.ErrorMessage)
                                .ToArray());

            string username = TempData["username"].ToString();

            if (ModelState.IsValid)
            {
                var user = await _context.Users.FindAsync(username);

                model.Secretary.UsersUsername = user.Username;
                
                await _context.Secretaties.AddAsync(model.Secretary);

                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Admin");
            }

            return View(new NewUser());
        }

        [HttpGet]
        [Route("/admin/create/professor")]
        public IActionResult Professor()
        {
            return View(new NewUser());
        }

        [HttpPost]
        [Route("/admin/create/professor")]
        public async Task<IActionResult> Professor(NewUser model)
        {
            ModelState.Remove("UsersUsernameNavigation");
            ModelState.Remove("UsersUsername");

            string username = TempData["username"].ToString();

            if (ModelState.IsValid)
            {
                var user = await _context.Users.FindAsync(username);

                model.Professor.UsersUsername = user.Username;
                
                await _context.Professors.AddAsync(model.Professor);

                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Admin");
            }

            return View(new NewUser());
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? username)
        {
            if (username == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Username.Equals(username));
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? username)
        {
            if (username == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Username.Equals(username));

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string? username)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'TestContext.Users'  is null.");
            }

            User? user = await _context.Users.FindAsync(username);

            if (user != null)
            {
                EntityUser identityUser = await _userManager.FindByNameAsync(user.Username);

                await _userManager.DeleteAsync(identityUser);
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string username)
        {
            return _context.Users.Any(e => e.Username.Equals(username));
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
                    $"Ensure that '{nameof(User)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
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

        private IUserEmailStore<EntityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<EntityUser>)_userStore;
        }
    }
}
