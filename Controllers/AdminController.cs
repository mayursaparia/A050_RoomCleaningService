using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_Admin_Prac.Models;
using System.Web.Security;
using System.Data.Entity;
using System.Net;

namespace Project_Admin_Prac.Controllers
{
   
    public class AdminController : Controller
    {
        AdminDataContext db = new AdminDataContext();
        // GET: Admin
        //Login GET
        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }

        //Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AdminLogin adminLogin,string returnUrl = "")
        {
            string message = "";
            ViewBag.Message = message;
            using(var context = new AdminDataContext())
            {
                var entity = context.AdminTbls.FirstOrDefault(x => x.AdminID == adminLogin.AdminID);
                if(entity != null)
                {
                    if (string.Compare(entity.Password, adminLogin.Password) == 0)
                    {
                        int timeout = adminLogin.RememberMe ? 525600 : 120;
                        var ticket = new FormsAuthenticationTicket(adminLogin.AdminID, adminLogin.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                    }
                    else
                    {
                        message = "Password not matching";
                    }
                }
                else
                {
                    message = "Admin ID not Present";
                }
            }
            ViewBag.Message = message;
            return View();
        }

        //Logout

        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login","Admin");
        }

        //Approve Cleaner
        [Authorize]
        public ActionResult Approve()
        {
            var context = new AdminDataContext();
            return View(context.Cleaners.ToList());
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var context = new AdminDataContext();
            var std = context.Cleaners.Where(s => s.Id == id).FirstOrDefault();

            return View(std);
        }
        [HttpPost]
        public ActionResult Edit(Cleaner updcleaner)
        {
            //update student in DB using EntityFramework in real-life application
            var context = new AdminDataContext();
            Cleaner originalcleaner = context.Cleaners.Where(s => s.Id == updcleaner.Id).FirstOrDefault();
            originalcleaner.AdminApproved = updcleaner.AdminApproved;
            originalcleaner.ConfirmPassword = originalcleaner.Password;
            context.Entry(originalcleaner).State = EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("Approve");
        }


        //Displaying  Cleaner Details
        [Authorize]
        public ActionResult Details(int? id)
        {
            var context = new AdminDataContext();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cleaner cleaner = context.Cleaners.Find(id);
            if (cleaner == null)
            {
                return HttpNotFound();
            }
            return View(cleaner);
        }

        //Landing Page
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        //Tickets Display

        public ActionResult TicketDisplay()
        {

            return View(db.Tickets.Where(x => x.Status == false).ToList());
        }

        //Ticket Reply

        public ActionResult TicketReply(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TicketReply([Bind(Include = "Id,Issue,Description,Date,Resolution,Status,UserEmail")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {

                db.Entry(ticket).State = EntityState.Modified;
                
                db.SaveChanges();
                return RedirectToAction("TicketDisplay");
            }
            return View(ticket);
        }
    }
}