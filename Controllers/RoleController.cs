using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Zdm_management.Data;
using Zdm_management.Models;
using Zdm_management.ViewModel;

namespace Zdm_management.Controllers
{

    [Authorize(Roles = "Super Admin,Admin")]
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment WebHostEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signinManager;
        public RoleController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signinManager)
        {
            this._db = db;
            this.WebHostEnvironment = webHostEnvironment;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signinManager = signinManager;
        }
        public  IActionResult Index()
        {
            ViewBag.AllRoles =  roleManager.Roles;
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async  Task<IActionResult> Create(RoleCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
               IdentityRole identityRole = new IdentityRole { Name = vm.Name };
               IdentityResult result = await roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult AddRoleToUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRoleToUser(AddRoleToUserCeateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = await roleManager.FindByNameAsync(vm.RoleName);

                ApplicationUser user = await userManager.FindByEmailAsync(vm.UserEmail);
                if (role != null && user != null )
                {
                   IdentityResult result = await userManager.AddToRoleAsync(user, role.Name);
                    if (result.Succeeded) 
                    {

                        TempData["FlashMessage"] = "Successfully added the role to user";
                        TempData["FlashMessageClass"] = "alert-success";
                        return View();
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }
                else 
                {
                    ModelState.AddModelError(string.Empty, "User or Role not found");
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult RemoveUserFromRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RemoveUserFromRole(RemoveUserFromRoleViewModel vm)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = await roleManager.FindByNameAsync(vm.RoleName);

                ApplicationUser user = await userManager.FindByEmailAsync(vm.UserEmail);
                if (role != null && user != null)
                {
                    if (await userManager.IsInRoleAsync(user, role.Name))
                    {
                        IdentityResult result = await userManager.RemoveFromRoleAsync(user, role.Name);
                        if (result.Succeeded)
                        {

                            TempData["FlashMessage"] = "Successfully removed the user from that Role.";
                            TempData["FlashMessageClass"] = "alert-success";
                            return View();
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View();
                    }
                    ModelState.AddModelError(string.Empty, "User is not in this Role");
                    return View();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User or Role not found");
                }
            }
            return View();
        }
    }
}
