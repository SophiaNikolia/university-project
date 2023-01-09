using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using university_project.Areas.Identity.Data;
using university_project.Models;

namespace university_project.Controllers
{
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

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username,Password,Role")] User user)
        {
            if (ModelState.IsValid)
            {

                EntityUser identityUser = CreateUser();

                string email = string.Empty;

                email = CustomEmail(user.Username, user.Role);

                await _userStore.SetUserNameAsync(identityUser, user.Username, CancellationToken.None);
                await _emailStore.SetEmailAsync(identityUser, email, CancellationToken.None);

                var result = await _userManager.CreateAsync(identityUser, user.Password);

                if (!result.Succeeded)
                {
                    return View();
                }

                // Add the user to the equivalent identity role
                await _userManager.AddToRoleAsync(identityUser, user.Role);

                // save the hashed password instead of the real one
                // in the university.db
                user.Password = identityUser.PasswordHash;

                await _context.AddAsync(user);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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
                case "Professor":
                    email = username + "@unipi.gr";
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("Not known Role");
                    return email;
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
