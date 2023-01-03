using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Zdm_management.Data;
using Zdm_management.Models;
using Z.EntityFramework.Plus;
using Microsoft.AspNetCore.Mvc.RazorPages;
using X.PagedList;

namespace Zdm_management.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment WebHostEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signinManager;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signinManager)
        {
            _logger = logger; this._db = db;
            this.WebHostEnvironment = webHostEnvironment;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signinManager = signinManager;
        }
        [Authorize]
        public IActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 20; // Get 20 entities for each requested page.
            ViewBag.AllUsers = this.userManager.Users.OrderBy(u => u.IdCardNumber).IncludeFilter( u => u.Attendances.Where( a => a.CreatedDateTime.Date == DateTime.Now.Date).FirstOrDefault()).ToPagedList(pageNumber, pageSize); ;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}