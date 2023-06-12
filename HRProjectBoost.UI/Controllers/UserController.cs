using HRProjectBoost.DTOs.DTOs.Authentication;
using HRProjectBoost.DTOs.DTOs.Personnel;
using HRProjectBoost.Entities.Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HRProjectBoost.UI.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto user)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = await _userManager.FindByEmailAsync(user.Email);

                if (appUser != null && user.Password == appUser.Password)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(appUser.UserName, appUser.Password, false, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        var personnelUser = await _userManager.IsInRoleAsync(appUser, "Personnel");
                        var managerUser = await _userManager.IsInRoleAsync(appUser, "Manager");
                        var adminUser = await _userManager.IsInRoleAsync(appUser, "Admin");


                        //ViewBag.NotificationType = "Succes";
                        //ViewBag.NotificationMessage = "Login succeed!";

                        if (personnelUser)
                            return RedirectToAction("ChangePassword", "Personnel", new { area = "Personnel" });
                        else if (managerUser)
                            return RedirectToAction("Index", "Manager", new { area = "Manager" });
                        else if (adminUser)
                            return RedirectToAction("Index", "Admin", new { area = "Admin" });
                    }

                    //Admin rolü gelirse burası değişebilir.
                }
                else
                {
                    ModelState.AddModelError("", "Yanlış Kullanıcı Adı/Şifre.");
                    TempData["toastr"] = "LoginFailed";
                    //ViewBag.NotificationMessage = "Login failed!";
                }

                return View(user);
            }
            return View(user);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(PersonnelChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return View(changePasswordDto);
            }
            var user = await _userManager.GetUserAsync(User);

            if (user is not null)
            {
                var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

                if (result.Succeeded)
                {
                    user.Password = changePasswordDto.NewPassword;
                    await _userManager.UpdateAsync(user);
                    return RedirectToAction(nameof(Logout));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            else
                ModelState.AddModelError("", "User cannot be found");

            return RedirectToAction("Logout");
        }
    }
}
