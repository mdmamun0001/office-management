using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection;
using System.Xml.Linq;
using X.PagedList;
using Zay.Data;
using Zdm_management.CustomHelper;
using Zdm_management.Data;
using Zdm_management.Models;
using Zdm_management.Services;
using Zdm_management.ViewModel;
using IMailService = Zdm_management.Services.IMailService;

namespace Zdm_management.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment WebHostEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signinManager;
        private readonly IMailService mailService;
        public AccountController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signinManager, IMailService mailService)
        {
            this._db = db;
            this.WebHostEnvironment = webHostEnvironment;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signinManager = signinManager;
            this.mailService = mailService;
            this.mailService = mailService;
        }
        [Authorize(Roles = "Super Admin,Admin")]
        public IActionResult Index(int? page)
        {
            ViewBag.AllUsers = userManager.Users;
            int pageNumber = page ?? 1;
            int pageSize = 10; // Get 10 entities for each requested page.
            ViewBag.AllUsers = userManager.Users.OrderBy(cm => cm.IdCardNumber).ToPagedList(pageNumber, pageSize);
            return View();
        }
        [HttpGet]

        [Authorize(Roles = "Super Admin,Admin")]
        [Route("Account/Register")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]

        [Authorize(Roles = "Super Admin,Admin")]
        [Route("Account/Register")]
        public async Task<IActionResult> Register(UserCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                string? ProfileImage = null;
                string? CoverImage = null;
           
                 var user = new ApplicationUser { 
                     Name = vm.Name, 
                     UserName = vm.Email,
                     Email = vm.Email, 
                     PhoneNumber = vm.PhoneNumber,
                     IdCardNumber = vm.IdCardNumber,
                     JobTitle = vm.JobTitle,
                     BirthDay = vm.BirthDay,
                     Gender = vm.Gender,
                     Address = vm.Address,
                     ProfileImage = ProfileImage,
                     CoverImage = CoverImage,
                     BloodGroup = vm.BloodGroup
                 };
                 var result = await userManager.CreateAsync(user, vm.Password);
                 if (result.Succeeded)
                 {
                    if (vm.ProfileImage != null)
                    {

                        ProfileImage = Helper.UploadFile(vm.ProfileImage, "storage/img/user", this.WebHostEnvironment);
                    }
                    if (vm.CoverImage != null)
                    {

                        CoverImage = Helper.UploadFile(vm.CoverImage, "storage/img/user", this.WebHostEnvironment);
                    }
                    var User = await userManager.FindByIdAsync(user.Id);
                    User.ProfileImage = ProfileImage;
                    User.CoverImage = CoverImage;
                    await userManager.UpdateAsync(User);
                    TempData["FlashMessage"] = "An account has been created successfully!";
                    TempData["FlashMessageClass"] = "alert-success";
                     return RedirectToAction("Index");
                 }
                 foreach (var error in result.Errors)
                 {
                     ModelState.AddModelError(string.Empty, error.Description);
                 }
            }
            return View(vm);
        }
        [HttpGet]
        [Route("Account/Login")]
        public  async Task <IActionResult> Login()
        {
            if (userManager.Users.ToList().Count < 1)
            {
                var Result = await DumyData.Initialize(userManager, roleManager);
                if (Result == false)
                {
                    ModelState.AddModelError(string.Empty, "Create User First");
                    return View();
                }
            }
            return View();
        }
        [HttpPost]
        [Route("Account/Login")]
        public async Task<IActionResult> Login(UserLoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                 var result = await signinManager.PasswordSignInAsync(vm.UserEmail, vm.Password, vm.RememberMe, false);

                 if (result.Succeeded)
                 {
                     return Redirect("/");
                 }

                 ModelState.AddModelError(string.Empty, "Invalid Attempt");
                return View(vm);
            }
            return View(vm);
        }
        [HttpGet]
        [Route("Account/Edit/{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            if (id != null)
            {

                var User = await userManager.FindByIdAsync(id);
                if (User != null)
                {
                    UserEditViewModel EditModel = new UserEditViewModel
                    {
                        Name = User.Name,
                        Email = User.Email,
                        PhoneNumber = User.PhoneNumber,
                        IdCardNumber = User.IdCardNumber,
                        JobTitle = User.JobTitle,
                        BirthDay = User.BirthDay,
                        Gender = User.Gender,
                        Address = User.Address,
                        ExistingProfileImage = User.ProfileImage,
                        ExistingCoverImage = User.CoverImage,
                        BloodGroup = User.BloodGroup
                    };
                    return View(EditModel);
                }
                return NotFound();
            }
            return NotFound();
        }
        [HttpPost]
        [Route("Account/Edit/{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(UserEditViewModel EditModel)
        {
            if (ModelState.IsValid)
            {

                var Edituser = await userManager.FindByIdAsync(EditModel.Id);
             
                if (Edituser != null)
                {
                    if (EditModel.ProfileImage != null)
                    {
                       
                        string?  NewProfileImage = Helper.UploadFile(EditModel.ProfileImage, "storage/img/user", this.WebHostEnvironment);
                        
                        if (EditModel.ExistingProfileImage != null && NewProfileImage != null)
                        {

                            Helper.DeleteFile(this.WebHostEnvironment, EditModel.ExistingProfileImage, "storage/img/user");
                          
                        }
                        if (NewProfileImage != null)
                        {
                            EditModel.ExistingProfileImage = NewProfileImage;
                        }
                    }
                    if (EditModel.CoverImage != null)
                    {
                        string? NewCoverImage = Helper.UploadFile(EditModel.CoverImage, "storage/img/user", this.WebHostEnvironment);

                        if (EditModel.ExistingCoverImage != null && NewCoverImage != null)
                        {

                            Helper.DeleteFile(this.WebHostEnvironment, EditModel.ExistingCoverImage, "storage/img/user");

                        }
                        if (NewCoverImage != null)
                        {
                            EditModel.ExistingCoverImage = NewCoverImage;
                        }
                    }
                    Edituser.Name = EditModel.Name;
                    Edituser.Email = EditModel.Email;
                    Edituser.UserName = EditModel.Email;
                    Edituser.PhoneNumber = EditModel.PhoneNumber;
                    Edituser.IdCardNumber = EditModel.IdCardNumber;
                    Edituser.JobTitle = EditModel.JobTitle;
                    Edituser.BirthDay = EditModel.BirthDay;
                    Edituser.Gender = EditModel.Gender;
                    Edituser.Address = EditModel.Address;
                    Edituser.ProfileImage = EditModel.ExistingProfileImage;
                    Edituser.CoverImage = EditModel.ExistingCoverImage;
                    Edituser.BloodGroup = EditModel.BloodGroup;
                    var resultInfo = await userManager.UpdateAsync(Edituser);
                    if (resultInfo.Succeeded)
                    {  
                        TempData["FlashMessage"] = "Information has been updated successfully.";
                        TempData["FlashMessageClass"] = "alert-success";
                        ApplicationUser AuthUser = await  userManager.FindByNameAsync(User.Identity.Name);
                        IList<string> AuthRoles = await userManager.GetRolesAsync(AuthUser);
                        if (AuthRoles.Contains("Admin") || AuthRoles.Contains("Super Admin"))
                        {

                            return RedirectToAction("Index");
                        }
                        return RedirectToAction("MyProfile");
                    }
                    foreach (var error in resultInfo.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }
                TempData["FlashMessage"] = "User account not found.";
                TempData["FlashMessageClass"] = "alert-danger";
                return View();
            }
            return View();
        }
        [HttpGet]
        [Authorize]
        [Route("Account/Logout")]
        public async Task<IActionResult> Logout()
        {
            await signinManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        [HttpGet]
        [Authorize]
        [Route("Account/ChangePassword/{id}")]
        public async  Task<IActionResult> ChangePassword(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id); 
            if (user != null)
            {
                PasswordResetViewModel vm = new PasswordResetViewModel { Email = user.Email };
                return View(vm);
            }
            return NotFound();
        }
        [HttpPost]
        [Authorize]
        [Route("Account/ChangePassword/{id}")]
        public async Task<IActionResult> ChangePassword(PasswordResetViewModel EditModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByEmailAsync(EditModel.Email);
               // var User = await userManager.FindByIdAsync(EditModel.Id);
                if (user != null && EditModel.NewPassword != null)
                {
                    IList<string> Roles = await userManager.GetRolesAsync(user);
                    if (Roles.Contains("Super Admin"))
                    {

                        TempData["FlashMessage"] = "Password reset failed. It's a Super Admin";
                        TempData["FlashMessageClass"] = "alert-danger";
                        return View();
                    }
                    user.PasswordHash = userManager.PasswordHasher.HashPassword(user, EditModel.NewPassword);
                    var resultInfo = await userManager.UpdateAsync(user);
                    if (resultInfo.Succeeded)
                    {

                        TempData["FlashMessage"] = "Password has been updated successfully!";
                        TempData["FlashMessageClass"] = "alert-success";
                        ApplicationUser AuthUser = await userManager.FindByNameAsync(User.Identity.Name);
                        IList<string> AuthRoles = await userManager.GetRolesAsync(AuthUser);
                        if (AuthRoles.Contains("Admin") || AuthRoles.Contains("Super Admin"))
                        {

                            return RedirectToAction("Index");
                        }
                        return Redirect("/Account/Profile/" + user.Id);
                    }
                    foreach (var error in resultInfo.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }
                TempData["FlashMessage"] = "User account not found.";
                TempData["FlashMessageClass"] = "alert-danger";
                return View();
            }
            return View();
        }
        [HttpGet]
        [Authorize]
        [Route("Account/MyProfile")]
        public  async Task<IActionResult> MyProfile()
        {
            var user = User.Identity;
            if (user != null)
            {
                ApplicationUser userInfo = await userManager.FindByEmailAsync(user.Name);
                ViewBag.Roles = await userManager.GetRolesAsync(userInfo);
                return View("Profile", userInfo);
            }
            return NotFound();
        }
        [HttpGet]
        [Authorize]
        [Route("Account/Profile/{id}")]
        public async  Task<IActionResult> Profile( string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);

            if (user !=null) {
                ViewBag.Roles = await userManager.GetRolesAsync(user);
                return View(user); 
            }
            return NotFound();
        }
        [HttpGet]
       
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(vm.Email);
                if (user != null)
                {
                    string? Token = null;
                    try
                    {
                         Token = await userManager.GeneratePasswordResetTokenAsync(user);
                    }
                    catch(Exception ex)
                    {
                        TempData["FlashMessage"] = ex.Message;
                        TempData["FlashMessageClass"] = "alert-danger";
                        return View();

                    }
                   

                    var PasswordResetLink = Url.Action("ResetPassword", "Account", new { email = vm.Email, token = Token }, Request.Scheme);
                    List<string> mailingList = new List<string>();
                    mailingList.Add(vm.Email);
                    MailRequest mailrequest = new MailRequest
                    {
                        Subject = "Password Reset Link - ZDM",
                        Body = "<html> </body>" + "<p>" + PasswordResetLink + "</p>" +  "<p> Click on above link to reset your password </p>" +"</body></html>"
                    };
                    try
                    {

                        await this.mailService.SendEmailAsync(mailrequest, mailingList);
                        TempData["FlashMessage"] = "Password resetlink has been sent to your email, Please check your email.";
                        TempData["FlashMessageClass"] = "alert-success";
                        return View();
                    }
                    catch (Exception ex)
                    {
                        TempData["FlashMessage"] = ex.Message;
                        TempData["FlashMessageClass"] = "alert-danger";
                        return RedirectToAction("ForgotPassword");
                    }
                }
                TempData["FlashMessage"] = "User not found!";
                TempData["FlashMessageClass"] = "alert-danger";
                return View();
            }
            return View();
           
        }
        [HttpGet]
        [Route("Account/ResetPassword")]
        public IActionResult ResetPassword(string email, string token)
        {
            if (email == null || token == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid token or email");
                return View();
            }
            PasswordResetWithTokenViewModel model = new PasswordResetWithTokenViewModel
            {
                Email = email,
                Token = token

            };
            return View();
        }
        [HttpPost]
        [Route("Account/ResetPassword")]
        public async Task<IActionResult> ResetPassword(PasswordResetWithTokenViewModel prvm)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(prvm.Email);
                if (user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, prvm.Token, prvm.NewPassword);
                    if (result.Succeeded)
                    {
                        TempData["FlashMessage"] = "Successfully password reset done!";
                        TempData["FlashMessageClass"] = "alert-success";
                        return RedirectToAction("Login");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }
                TempData["FlashMessage"] = "User not found!";
                TempData["FlashMessageClass"] = "alert-danger";
                return View();
            }
            return View();
        }
        [HttpGet]

        [Authorize(Roles = "Super Admin,Admin")]
        [Route("Account/Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id != null)
            {

                var user = await userManager.FindByIdAsync(id);
                if (user != null)
                {
                    IList<string> Roles = await userManager.GetRolesAsync(user);
                    if (Roles.Contains("Super Admin"))
                    {

                        TempData["FlashMessage"] = " Account deletion failed. It's a Super Admin";
                        TempData["FlashMessageClass"] = "alert-danger";
                        return RedirectToAction("Index");
                    }
                    var result = await userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        if (user.ProfileImage != null)
                        {
                            Helper.DeleteFile(this.WebHostEnvironment, user.ProfileImage, "storage/img/user");
                        }
                        if (user.CoverImage != null)
                        {
                            Helper.DeleteFile(this.WebHostEnvironment, user.CoverImage, "storage/img/user");
                        }
                        string authId = userManager.GetUserId(User);
                        if (authId == id)
                        {
                            return RedirectToAction("Logout");
                        }
                        TempData["FlashMessage"] = "A user account has been deleted successfully.";
                        TempData["FlashMessageClass"] = "alert-success";
                        return RedirectToAction("Index");
                    }
                    TempData["FlashMessage"] = "A user account deletion failed.";
                    TempData["FlashMessageClass"] = "alert-danger";
                    return RedirectToAction("Index");
                }
                TempData["FlashMessage"] = "User account not found.";
                TempData["FlashMessageClass"] = "alert-danger";
                return RedirectToAction("Index");
            }
            TempData["FlashMessage"] = "A user account not found.";
            TempData["FlashMessageClass"] = "alert-danger";
            return RedirectToAction("Index");
        }
    }
}
