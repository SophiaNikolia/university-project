using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using university_project.Areas.Identity.Data;
using university_project.Models;
using university_project.ViewModels;

namespace university_project.Controllers
{
    // TODO: Add authorization
    public class SettingsController : Controller
    {
        private readonly SignInManager<EntityUser> _signInManager;
        
        private readonly universityContext _context;

        public SettingsController(SignInManager<EntityUser> signInManager, universityContext context)
        {
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Password()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Password(ChangePasswordData changePasswordData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var identityUser = await _signInManager.UserManager.GetUserAsync(User);

            var user = await _context.Users.FindAsync(identityUser.UserName);

            if (identityUser == null)
            {
                return NotFound($"Unable to load user with ID '{_signInManager.UserManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _signInManager.UserManager
                .ChangePasswordAsync(identityUser, changePasswordData.OldPassword, changePasswordData.OldPassword);

            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return RedirectToAction("Password", "Settings");
            }
            user.Password = identityUser.PasswordHash;

            _context.Users.Update(user);

            await _context.SaveChangesAsync();

            await _signInManager.RefreshSignInAsync(identityUser);

            return View();
        }

        [Route("settings/personal-data")]
        public IActionResult Personal()
        {
            return View();
        }

        public IActionResult Theme()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }
        
    }
}
