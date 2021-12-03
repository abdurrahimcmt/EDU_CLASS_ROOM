using EDU_Models;
using EDU_Models.ViewModels;
using EDU.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using EDU_Utility;
using EDU_DataAccess.Repository.IRepository;

namespace EDU.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly ITeacherInfoRepository _teacherRepo;
        private readonly IStudentRepository _studentRepo;
        private readonly IAdminInfoRepository _adminRepo;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IEmailSender emailSender, RoleManager<IdentityRole> roleManager, ITeacherInfoRepository teacherRepo, IStudentRepository studentRepo, IAdminInfoRepository adminRepo)
        {
            _teacherRepo = teacherRepo;
            _studentRepo = studentRepo;
            _adminRepo = adminRepo;
            _userManager = userManager;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Register(string returnurl = null)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem()
            {
                Value = WC.AdminRole,
                Text = WC.AdminRole
            });
            listItems.Add(new SelectListItem()
            {
                Value = WC.TeacherRole,
                Text = WC.TeacherRole
            });
            listItems.Add(new SelectListItem()
            {
                Value = WC.StudentRole,
                Text = WC.StudentRole
            });
            ViewData["ReturnUrl"] = returnurl;
            RegisterViewModel registerViewModel = new RegisterViewModel()
            {
                RoleList = listItems
            };
            return View(registerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnurl = null)
        {
            
            ViewData["ReturnUrl"] = returnurl;
            //If the User wants directly login into the application it will go to the home page
            //If ReturnUrl is null
            returnurl = returnurl ?? Url.Content("~/");
            bool isEmailvalid = false;
            if (ModelState.IsValid)
            {

                if (model.RoleSelected != null && model.RoleSelected.Length > 0 && model.RoleSelected == WC.AdminRole)
                {
                    var obj = _adminRepo.FirstOrDefault(u => u.Email == model.Email);
                    if (obj!=null)
                    {
                        isEmailvalid = true;
                    }
                }
                else if (model.RoleSelected != null && model.RoleSelected.Length > 0 && model.RoleSelected == WC.TeacherRole)
                {
                    var obj = _teacherRepo.FirstOrDefault(u => u.Email == model.Email);
                    if (obj != null)
                    {
                        isEmailvalid = true;
                    }
                }
                else if (model.RoleSelected != null && model.RoleSelected.Length > 0 && model.RoleSelected == WC.StudentRole)
                {
                    var obj = _studentRepo.FirstOrDefault(u => u.Email == model.Email);
                    if (obj != null)
                    {
                        isEmailvalid = true;
                    }
                }
                if (isEmailvalid)
                {
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FullName = model.Name, RoleName = model.RoleSelected };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        if (model.RoleSelected != null && model.RoleSelected.Length > 0 && model.RoleSelected == WC.AdminRole)
                        {
                            await _userManager.AddToRoleAsync(user, WC.AdminRole);
                        }
                        else if (model.RoleSelected != null && model.RoleSelected.Length > 0 && model.RoleSelected == WC.TeacherRole)
                        {
                            await _userManager.AddToRoleAsync(user, WC.TeacherRole);
                        }
                        else if (model.RoleSelected != null && model.RoleSelected.Length > 0 && model.RoleSelected == WC.StudentRole)
                        {
                            await _userManager.AddToRoleAsync(user, WC.StudentRole);
                        }

                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackurl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

                        await _emailSender.SendEmailAsync(model.Email, "Confirm your account - Identity Manager",
                            "Please confirm your account by clicking here: <a href=\"" + callbackurl + "\">link</a>");
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnurl);
                    }
                    AddErrors(result);
                }
                else
                {
                    return View("Emailnotfound");
                }
                /*var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FullName = model.Name,RoleName =model.RoleSelected };
                var result = await _userManager.CreateAsync(user, model.Password);*/
               /* if (result.Succeeded)
                {
                    if (model.RoleSelected != null && model.RoleSelected.Length > 0 && model.RoleSelected == WC.AdminRole)
                    {
                        await _userManager.AddToRoleAsync(user, WC.AdminRole);
                    }
                    else if (model.RoleSelected != null && model.RoleSelected.Length > 0 && model.RoleSelected == WC.TeacherRole)
                    {
                        await _userManager.AddToRoleAsync(user, WC.TeacherRole);
                    }
                    else if(model.RoleSelected != null && model.RoleSelected.Length > 0 && model.RoleSelected == WC.StudentRole)
                    {
                        await _userManager.AddToRoleAsync(user, WC.StudentRole);
                    }

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackurl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

                    await _emailSender.SendEmailAsync(model.Email, "Confirm your account - Identity Manager",
                        "Please confirm your account by clicking here: <a href=\"" + callbackurl + "\">link</a>");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnurl);
                }
                AddErrors(result);*/

            }
            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem()
            {
                Value = WC.AdminRole,
                Text = WC.AdminRole
            });
            listItems.Add(new SelectListItem()
            {
                Value = WC.TeacherRole,
                Text = WC.TeacherRole
            });
            listItems.Add(new SelectListItem()
            {
                Value = WC.StudentRole,
                Text = WC.StudentRole
            });

            return View(model);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        [HttpGet]
        //Receive a string name of returnUrl. This contains the address or URL, which informs us user wants to go to which page.
        public IActionResult Login(string returnurl = null)
        {
            // Store temporary data in ViewData for passing returnUrl into the login page
           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,  lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    //Using LocalRedirect for avoiding open redirect attact. 
                    // when some other domain using this URL They can't access this page.
                    return LocalRedirect(returnurl);
                }
                if (result.IsLockedOut)
                {
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }

            }


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            TempData[WC.Success] = "Logout successfully";
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return RedirectToAction("ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackurl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

                await _emailSender.SendEmailAsync(model.Email, "Reset Password - Identity Manager",
                    "Please reset your password by clicking here: <a href=\"" + callbackurl + "\">link</a>");

                return RedirectToAction("ForgotPasswordConfirmation");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }




        [HttpGet]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }

                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }
                AddErrors(result);
            }

            return View();
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");

        }

    }
}
