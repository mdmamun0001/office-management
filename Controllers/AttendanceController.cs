using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Z.EntityFramework.Plus;
using Zdm_management.Data;
using Zdm_management.Models;
using Zdm_management.ViewModel;
using System.Net;
using Telegram.Bot;
using Org.BouncyCastle.Crypto.Engines;

namespace Zdm_management.Controllers
{
    
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment WebHostEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signinManager;
        public AttendanceController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signinManager)
        {
            this._db = db;
            this.WebHostEnvironment = webHostEnvironment;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signinManager = signinManager;
        }
        [Authorize]
        public IActionResult Index()
        {
            return Redirect("/");
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CheckIn()
        {


            string ClientIp = Response.HttpContext.Connection.RemoteIpAddress.ToString();
            if (ClientIp == "::1")
            {
                ClientIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();
            }
            List<string> IpWishlists = this._db.IpWishLists.Select(i => i.IpAddress).ToList();
            if (!IpWishlists.Contains(ClientIp))
            {
                TempData["FlashMessage"] = "Ip restricted! Only office network is allowed to checkin and checkout.";
                TempData["FlashMessageClass"] = "alert-danger";
                return Redirect("/");
            }
            if (User.Identity != null)
            {

                ApplicationUser AuthUser = await userManager.FindByNameAsync(User.Identity.Name);

                var TodayAttendance = this._db.Attendances.Where(a => a.ApplicationUserID == AuthUser.Id).Where(a => a.CreatedDateTime.Date == DateTime.Now.Date).FirstOrDefault();

                if (TodayAttendance != null)
                {
                    if (TodayAttendance.CheckInTime == null)
                    {
                        TodayAttendance.CheckInTime = DateTime.Now;
                        this._db.Attendances.Update(TodayAttendance);
                        this._db.SaveChanges();
                        try{
                            string msg = AuthUser.Email+"(" + AuthUser.Name +")" + " Checkin at " + DateTime.Now.ToString("hh:mm tt");
                            TelegramBotClient Client = new TelegramBotClient("5958416598:AAGLwdQjNHiDefosfq7i-U2swL7_Po1mvq0");
                            Client.SendTextMessageAsync("@myoffice001chaneel", msg);
                        }
                        catch(Exception e)
                        {

                        }
                        TempData["FlashMessage"] = "Successfully checkIn done";
                        TempData["FlashMessageClass"] = "alert-success";
                        return Redirect("/");
                    }
                    TempData["FlashMessage"] = "Already checkedIn.";
                    TempData["FlashMessageClass"] = "alert-success";
                    return Redirect("/");
                }
                else
                {
                    Attendance TodayAttendanceNew = new Attendance
                    {
                        CheckInTime = DateTime.Now,
                        CheckOutTime = null,
                        ApplicationUserID = AuthUser.Id
                    };
                    this._db.Attendances.Add(TodayAttendanceNew);
                    this._db.SaveChanges();
                    try
                    {
                        string msg = AuthUser.Email + "(" + AuthUser.Name + ")" +" Checkin at " + DateTime.Now.ToString("hh:mm tt");
                        TelegramBotClient Client = new TelegramBotClient("5958416598:AAGLwdQjNHiDefosfq7i-U2swL7_Po1mvq0");
                        Client.SendTextMessageAsync("@myoffice001chaneel", msg);
                    }
                    catch (Exception e)
                    {

                    }
                    TempData["FlashMessage"] = "Successfully checkIn done!";
                    TempData["FlashMessageClass"] = "alert-success";
                }
            }
            return Redirect("/");
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CheckOut()
        {

            string ClientIp = Response.HttpContext.Connection.RemoteIpAddress.ToString();
            if (ClientIp == "::1")
            {
                ClientIp = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();
            }
            List<string> IpWishlists = this._db.IpWishLists.Select(i => i.IpAddress).ToList();
            if (!IpWishlists.Contains(ClientIp))
            {
                TempData["FlashMessage"] = "Ip restricted! Only office network is allowed to checkin and checkout.";
                TempData["FlashMessageClass"] = "alert-danger";
                return Redirect("/");
            }
            if (User.Identity != null)
            {

                ApplicationUser AuthUser = await userManager.FindByNameAsync(User.Identity.Name);

                var TodayAttendance = this._db.Attendances.Where(a => a.ApplicationUserID == AuthUser.Id).Where(a => a.CreatedDateTime.Date == DateTime.Now.Date).FirstOrDefault();

                if (TodayAttendance != null)
                {
                    if (TodayAttendance.CheckInTime !=null & TodayAttendance.CheckOutTime == null)
                    {
                        TodayAttendance.CheckOutTime = DateTime.Now;
                        this._db.Attendances.Update(TodayAttendance);
                        this._db.SaveChanges();
                        TimeSpan totalDuration = TodayAttendance.CheckOutTime.GetValueOrDefault().Subtract(TodayAttendance.CheckInTime.GetValueOrDefault());
                        try
                        {
                           
                           
                            string msg = AuthUser.Email + "(" + AuthUser.Name + ")" + " Checkout at " + DateTime.Now.ToString("hh:mm tt") +" and today working hour is " + totalDuration.Hours +" : " + totalDuration.Minutes;
                            TelegramBotClient Client = new TelegramBotClient("5958416598:AAGLwdQjNHiDefosfq7i-U2swL7_Po1mvq0");
                            Client.SendTextMessageAsync("@myoffice001chaneel", msg);
                        }
                        catch (Exception e)
                        {

                        }
                        TempData["FlashMessage"] = "Checkout Successfully done!";
                        TempData["FlashMessageClass"] = "alert-success";
                        return Redirect("/");
                    }
                    TempData["FlashMessage"] = "Already checkout done!";
                    TempData["FlashMessageClass"] = "alert-success";
                    return Redirect("/");
                }
                else
                {
                    Attendance TodayAttendanceNew = new Attendance
                    {
                        CheckInTime = null,
                        CheckOutTime = DateTime.Now,
                        ApplicationUserID = AuthUser.Id
                    };
                    this._db.Attendances.Add(TodayAttendanceNew);
                    this._db.SaveChanges();
                    try
                    {
                        string msg = AuthUser.Email + "(" + AuthUser.Name + ")" +" Checkout at " + DateTime.Now.ToString("hh:mm tt");
                        TelegramBotClient Client = new TelegramBotClient("5958416598:AAGLwdQjNHiDefosfq7i-U2swL7_Po1mvq0");
                        Client.SendTextMessageAsync("@myoffice001chaneel", msg);
                    }
                    catch (Exception e)
                    {

                    }
                    TempData["FlashMessage"] = "Checkout Successfully done !";
                    TempData["FlashMessageClass"] = "alert-success";
                }
            }
            return Redirect("/");
        }
        [HttpGet]
        [Authorize]
        public async Task<JsonResult> GetTodayAttendace()
        {
            ApplicationUser AuthUser = await userManager.FindByNameAsync(User.Identity.Name);

            var TodayAttendance = this._db.Attendances.Where(a => a.ApplicationUserID == AuthUser.Id).Where(a => a.CreatedDateTime.Date == DateTime.Now.Date).FirstOrDefault();

            if (TodayAttendance != null)
            {
                var TodayCheckInCheckOut = new
                {
                    CheckIn = TodayAttendance.CheckInTime,
                    CheckOut = TodayAttendance.CheckOutTime,
                };

                return Json(TodayCheckInCheckOut);
            }
            Attendance TodayAttendanceNew = new Attendance
            {
                CheckInTime = null,
                CheckOutTime = null,
                ApplicationUserID = AuthUser.Id
            };
            this._db.Attendances.Add(TodayAttendanceNew);
            this._db.SaveChanges(); 
            var NewTodayCheckInCheckOut = new
            {
                CheckIn = TodayAttendanceNew.CheckInTime,
                CheckOut = TodayAttendanceNew.CheckOutTime,
            };
            return Json(NewTodayCheckInCheckOut);
        }
        [HttpGet]
        [Authorize(Roles = "Super Admin,Admin")]
        public IActionResult CheckInByDateTime()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> CheckInByDateTime(CheckInByDateTimeCreateViewModel Cvm)
        {
            

            ApplicationUser CheckInUser = await userManager.FindByEmailAsync(Cvm.UserEmail);
            if (CheckInUser != null)
            {
                var AttendanceForThatDay = this._db.Attendances.Where(a => a.ApplicationUserID == CheckInUser.Id).Where(a => a.CreatedDateTime.Date == Cvm.CheckInTime.Date).FirstOrDefault();

                if (AttendanceForThatDay != null)
                {
                    AttendanceForThatDay.CheckInTime = Cvm.CheckInTime;
                    this._db.Attendances.Update(AttendanceForThatDay);
                    this._db.SaveChanges();
                    TempData["FlashMessage"] = "Succesfully done checkIn for the day! don't forget to Checkout.";
                    TempData["FlashMessageClass"] = "alert-success";
                    return View();

                }
                else
                {
                    Attendance AttendanceForThatDayNew = new Attendance
                    {
                        CheckInTime = Cvm.CheckInTime,
                        CheckOutTime = null,
                        CreatedDateTime = Cvm.CheckInTime,
                        ApplicationUserID = CheckInUser.Id
                    };
                    this._db.Attendances.Add(AttendanceForThatDayNew);
                    this._db.SaveChanges();
                    TempData["FlashMessage"] = "Successfully done checkIn for the day ! don't forget to Checkout";
                    TempData["FlashMessageClass"] = "alert-success";
                }
                return View();
            }

            TempData["FlashMessage"] = "User doesn't exist";
            TempData["FlashMessageClass"] = "alert-danger";
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Super Admin,Admin")]
        public IActionResult CheckOutBytDateTime()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> CheckOutBytDateTime(CheckOutByDateTimeCreateViewModel Cvm)
        {


            ApplicationUser CheckOutUser = await userManager.FindByEmailAsync(Cvm.UserEmail);
            if (CheckOutUser != null)
            {
                var AttendanceForThatDay = this._db.Attendances.Where(a => a.ApplicationUserID == CheckOutUser.Id).Where(a => a.CreatedDateTime.Date == Cvm.CheckOutTime.Date).FirstOrDefault();

                if (AttendanceForThatDay != null && AttendanceForThatDay.CheckInTime != null)
                {
                    
                    AttendanceForThatDay.CheckOutTime = Cvm.CheckOutTime;
                    this._db.Attendances.Update(AttendanceForThatDay);
                    this._db.SaveChanges();
                    TempData["FlashMessage"] = "Succesfully done checkout for the day!";
                    TempData["FlashMessageClass"] = "alert-success";
                    return View();

                }

                TempData["FlashMessage"] = "First checkIn for the user for the day !";
                TempData["FlashMessageClass"] = "alert-danger";
                return View();
            }

            TempData["FlashMessage"] = "User doesn't exist!";
            TempData["FlashMessageClass"] = "alert-danger";
            return View();
        }
        [Authorize]
        [Route("Attendance/History/{id}")]
        public  async Task<IActionResult> History(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);
            if (user !=null)
            {
                var userAttendance =  this._db.Attendances.Where(a => a.ApplicationUserID == id).OrderByDescending( a => a.CreatedDateTime).Take(365).ToList();
                ViewBag.userAttendance_Group_Data = userAttendance.GroupBy(a => a.CreatedDateTime.Month);
                ViewBag.user = user;
                return View();
            }
            TempData["FlashMessage"] = "User doesn't exist!";
            TempData["FlashMessageClass"] = "alert-danger";
            return View();
        }
    }
}
