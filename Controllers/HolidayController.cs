using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Z.EntityFramework.Plus;
using Zdm_management.Data;
using Zdm_management.Data;
using Zdm_management.Models;
using Zdm_management.ViewModel;

namespace Zdm_management.Controllers
{
    public class HolidayController : Controller
    {
        private readonly ApplicationDbContext _db; 

        public HolidayController(ApplicationDbContext db)
        {
            this._db = db;
        }
        [Authorize]
        public IActionResult Index()
        {
            ViewBag.Holidays = this._db.Holidays.Where(h => h.HolidayDate.Date.Year == DateTime.Now.Date.Year).OrderBy( h => h.HolidayDate).ToList();
            return View();
        }
        [HttpGet]

        [Authorize(Roles = "Super Admin,Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]

        [Authorize(Roles = "Super Admin,Admin")]
        public IActionResult Create(HolidayCreateViewModel Cvm)
        {
            if (ModelState.IsValid) 
            {
                var isExist = this._db.Holidays.Where(h => h.HolidayDate.Date == Cvm.HoldayDate.Date).FirstOrDefault();
                if (isExist != null) 
                {
                    TempData["FlashMessage"] = "This day is reserved for another holiday! You Can Modify only.";
                    TempData["FlashMessageClass"] = "alert-danger";
                    return View();
                }
                Holiday NewHoliday = new Holiday
                {
                    HolidayDate = Cvm.HoldayDate,
                    ShortDescription = Cvm.ShortDescription,
                };
                this._db.Holidays.Add(NewHoliday);
                this._db.SaveChanges();

                TempData["FlashMessage"] = "Successfully new holiday added!";
                TempData["FlashMessageClass"] = "alert-success";
                return View();
            }
            return View();
        }
        [HttpGet]
        [Route("Holiday/Edit/{id}")]
        [Authorize(Roles = "Super Admin,Admin")]
        public IActionResult Edit(int? id)
        {
            if (id != null)
            {
               var holiday = this._db.Holidays.Find(id);
                if (holiday != null)
                {
                    HolidayEditViewModel editModel = new HolidayEditViewModel
                    {
                        HoldayDate = holiday.HolidayDate,
                        ShortDescription = holiday.ShortDescription,
                        Id = holiday.Id
                    };
                    return View(editModel);
                }
                return NotFound();
            }
            return NotFound();
        }
        [HttpPost]

        [Route("Holiday/Edit/{id}")]
        [Authorize(Roles = "Super Admin,Admin")]
        public IActionResult Create(HolidayEditViewModel Evm)
        {
            if (ModelState.IsValid)
            {
            
                var editHoliday = this._db.Holidays.Find(Evm.Id);
                if (editHoliday != null)
                { 
                    editHoliday.ShortDescription = Evm.ShortDescription;
                    editHoliday.HolidayDate = Evm.HoldayDate;
                    this._db.Holidays.Update(editHoliday);
                    this._db.SaveChanges();

                    TempData["FlashMessage"] = "Update operation successfully done!";
                    TempData["FlashMessageClass"] = "alert-success";
                    return View();
                }
                TempData["FlashMessage"] = "Not found";
                TempData["FlashMessageClass"] = "alert-danger";
                return View();
            }
            return View();
        }
        //[HttpGet]
        //[Route("Holiday/Year")]
        //[Authorize(Roles = "Super Admin,Admin")]
        //public IActionResult HolidayByYear()
        //{
            
        //        ViewBag.Holidays = this._db.Holidays.Where(h => h.HolidayDate.Year == DateTime.Now.Year);
        //        ViewBag.Year = DateTime.Now.Year;
        //        return View();
        //}
        [HttpGet]
        [Route("Holiday/Year")]
        [Authorize(Roles = "Super Admin,Admin")]
        public IActionResult HolidayByYear(int? Year)
        {
           
            if (Year != null )
            {
                ViewBag.Holidays = this._db.Holidays.Where(h => h.HolidayDate.Year == Year).ToList();
                ViewBag.Year = Year;
                return View();
            }
            ViewBag.Holidays = this._db.Holidays.Where(h => h.HolidayDate.Year == DateTime.Now.Year).ToList();
            ViewBag.Year = DateTime.Now.Year;
            return View();
        }
        [HttpGet]
        [Route("Holiday/Delete/{id}")]
        [Authorize(Roles = "Super Admin,Admin")]
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                try{
                    this._db.Holidays.Where(h => h.Id == id).Delete();

                }
                catch(Exception exc) 
                {

                    TempData["FlashMessage"] = "Deletion failed";
                    TempData["FlashMessageClass"] = "alert-danger";
                    return RedirectToAction("HolidayByYear");
                }
                this._db.SaveChanges();
                TempData["FlashMessage"] = "Successfully deletion done!";
                TempData["FlashMessageClass"] = "alert-success";
                return RedirectToAction("HolidayByYear");
            }
            return RedirectToAction("HolidayByYear");
        }
    }
}
