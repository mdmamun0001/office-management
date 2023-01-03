using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Zdm_management.Data;
using Zdm_management.Models;
using Zdm_management.ViewModel;

namespace Zdm_management.Controllers
{
    public class IpWishListController : Controller
    {
        private readonly ApplicationDbContext _db;
        public IpWishListController(ApplicationDbContext db)
        {
            this._db = db;
        }

        [Authorize(Roles = "Super Admin,Admin")]
        public IActionResult Index()
        {
            ViewBag.IpWiahlists = this._db.IpWishLists.ToList();
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
        public IActionResult Create(IpWishListCreateViewModel Cvm)
        {
            if (ModelState.IsValid)
            {
                IpWishList NewIp = new IpWishList
                {
                    IpAddress = Cvm.IpAddress
               };
                this._db.IpWishLists.Add(NewIp);
                this._db.SaveChanges();
                TempData["FlashMessage"] = "Successfully added the ip address!";
                TempData["FlashMessageClass"] = "alert-success";
                return RedirectToAction("Index");

            }
            return View();
        }
        [Authorize(Roles = "Super Admin,Admin")]
        [Route("IpWishList/Delete/{id}")]
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                IpWishList? IpAddress = this._db.IpWishLists.Find(id);
                if (IpAddress != null) 
                {
                    this._db.IpWishLists.Remove(IpAddress);
                    this._db.SaveChanges();
                    TempData["FlashMessage"] = "Successfully deleted!";
                    TempData["FlashMessageClass"] = "alert-success";
                    return RedirectToAction("Index");
                }
            }
            TempData["FlashMessage"] = "Not fount!";
            TempData["FlashMessageClass"] = "alert-danger";
            return RedirectToAction("Index");
        }
    }
}
