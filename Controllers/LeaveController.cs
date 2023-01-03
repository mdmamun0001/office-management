using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Z.EntityFramework.Plus;
using Zdm_management.Data;
using Zdm_management.Models;
using Zdm_management.ViewModel;

namespace Zdm_management.Controllers
{
    public class LeaveController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> userManager;
        public LeaveController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            this._db = db;
            this.userManager = userManager;
        }
        [HttpGet]
        [Authorize(Roles = "Super Admin,Admin")]
        public IActionResult Index(String? UserId, int? Year)
        {
            ViewBag.AllUsers = userManager.Users.ToList<ApplicationUser>();
            if (UserId == null)
            {
                return View();
            }
            if (Year == null)
            {
                Year = DateTime.Now.Year;
            }
            ViewBag.Year = Year;
            ViewBag.User = userManager.Users.Where(au => au.Id == UserId).IncludeFilter(au => au.Leaves.Where(aul => aul.Leavedate.Year == Year)).FirstOrDefault();
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Super Admin,Admin")]
        public IActionResult Create()
        {
            ViewBag.AllUsers = userManager.Users.ToList<ApplicationUser>();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> Create(LeaveCreateViewModel Cvm)
        {
            ViewBag.AllUsers = userManager.Users.ToList<ApplicationUser>();
            if (ModelState.IsValid)
            {
                ApplicationUser? user = await userManager.FindByIdAsync(Cvm.UserId);
                if (user == null)
                {

                    TempData["FlashMessage"] = "User Not found!";
                    TempData["FlashMessageClass"] = "alert-danger";
                    return View();
                }
                Leave? isExistLeave = this._db.leaves.Where(l => l.Leavedate.Year == Cvm.LeaveDate.Year).Where(l => l.ApplicationUserID == Cvm.UserId).FirstOrDefault();
                if (isExistLeave != null)
                {
                    TempData["FlashMessage"] = "Leave is alredy taken in that date!";
                    TempData["FlashMessageClass"] = "alert-danger";
                    return View();
                }
                Leave newLeave = new Leave
                {
                    Leavedate = Cvm.LeaveDate,
                    Type = Cvm.Type,
                    Reason = Cvm.Reason,
                    ApplicationUserID = Cvm.UserId

                };

                this._db.leaves.Add(newLeave);
                this._db.SaveChanges();

                TempData["FlashMessage"] = "Successfully saved!";
                TempData["FlashMessageClass"] = "alert-success";
            }
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Super Admin,Admin")]
        [Route("Leave/Edit/{id}")]
        public IActionResult Edit(int id)
        {
           Leave? leave = this._db.leaves.Find(id);
            if (leave != null)
            {
                LeaveEditViewModel Editmdel = new LeaveEditViewModel
                {
                    Id = leave.id,
                    LeaveDate = leave.Leavedate,
                    Reason = leave.Reason,
                    Type = leave.Type,
                    UserId = leave.ApplicationUserID
                };
                var leaveUser = userManager.Users.Where( u => u.Id == Editmdel.UserId).FirstOrDefault();
                if (leaveUser != null)
                {
                    Editmdel.UserEmail = leaveUser.Email;

                }
                return View(Editmdel);
            }
            return NotFound();
        }
        [Authorize(Roles ="Super Admin,Admin")]
        [Route("Leave/Edit/{id}")]
        public IActionResult Edit(LeaveEditViewModel Evm)
        {
            if (ModelState.IsValid)
            {
                Leave? leave = this._db.leaves.Where( l => l.id == Evm.Id ).FirstOrDefault();
                if (leave == null)
                {
                    return NotFound();
                }

                leave.Type = Evm.Type;
                leave.Reason = Evm.Reason;
                this._db.leaves.Update(leave);
                this._db.SaveChanges();

                TempData["FlashMessage"] = "Update successfully done!";
                TempData["FlashMessageClass"] = "alert-success";
                return View(Evm);
            }
            return View(Evm);

        }

        [Route("Leave/Delete/{id}")]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id != null)
            {
                Leave? leave = this._db.leaves.Find(id);
                if (leave != null)
                {
                    this._db.leaves.Remove(leave);
                    this._db.SaveChanges();
                    TempData["FlashMessage"] = "Successfully deleted!";
                    TempData["FlashMessageClass"] = "alert-success";
                    ViewBag.User = userManager.Users.Where(au => au.Id == leave.ApplicationUserID).IncludeFilter(au => au.Leaves.Where(aul => aul.Leavedate.Year == leave.Leavedate.Year)).FirstOrDefault();

                    ViewBag.Year = leave.Leavedate.Year;
                    ViewBag.AllUsers = userManager.Users.ToList<ApplicationUser>();
                    return View("Index");
                }

            }
            TempData["FlashMessage"] = "Not Found!";
            TempData["FlashMessageClass"] = "alert-danger";
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize]
        
        public IActionResult MyLeaveHistory(int? Year )
        {
            if (Year == null)
            {
                Year = DateTime.Now.Year;
            }
            ViewBag.Year = Year;
            ViewBag.Authuser =  userManager.Users.Where(au => au.UserName == User.Identity.Name).IncludeFilter( au => au.Leaves.Where( aul => aul.Leavedate.Year == Year)).FirstOrDefault();
            return View();
        }
    }
}
