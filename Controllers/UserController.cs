using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Project_Admin_Prac.Models;

namespace Project_Admin_Prac.Controllers
{
    public class UserController : Controller
    {

        //Logins
        //neilanu1702@gmail.com : neilanu
        //mayur.saparia15@gmail.com :saparia
        //jasm@gmail.com : jasmine
        // GET: User
        // Registration Action
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        //Registration POST action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "ActivationCode")] User user)//del this is email part
        {
            bool Status = false;
            string Message = "";
            //model validation
            if (ModelState.IsValid)
            {
                //Email already exist or not
                var isExist = IsEmailExist(user.Email);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(user);
                }
                //Generate Activation Code
                user.ActivationCode = Guid.NewGuid();
                //Password Hashing
                //user.Password = Crypto.Hash(user.Password); //using crypto
                //user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);//using crypto
                //user.IsEmailVerified = false;//Delete this
                //Save data to Database
                using (var context = new AdminDataContext())
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                    Status = true;
                    Message = "New User Created Successfully";
                }
            }
            else
            {
                Message = "Invalid Request";
            }
            ViewBag.Message = Message;
            ViewBag.Status = Status;
            return View(user);
        }

        //Login
        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }

        //Login Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnUrl = "")
        {
            string Message = "";
            using (var context = new AdminDataContext())
            {
                var entity = context.Users.FirstOrDefault(x => x.Email == login.EmailID);
                if (entity != null)
                {
                    //if (string.Compare(Crypto.Hash(login.Password), entity.Password) == 0) //using crypto
                    if (string.Compare(login.Password, entity.Password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 120;//525600 = 1 year
                        var ticket = new FormsAuthenticationTicket(login.EmailID, login.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            Session["UID"] = login.EmailID;
                            return RedirectToAction("UserIndex", "Home");
                        }
                    }
                    else
                    {
                        Message = "Wrong Password Entered";
                    }

                }
                else
                {
                    Message = "User with this email dosen't exist";
                }
            }

            ViewBag.Message = Message;
            return View();
        }

        //Forgot UserID
        [HttpGet]
        public ActionResult ForgotUserId()
        {
            return View();
        }
        //Forgot UserID POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotUserId(ForgotUserid user)
        {
            string message = "";
            bool Status = false;
            //model validation
            using (var context = new AdminDataContext())
            {
                var entity = context.Users.FirstOrDefault(x => x.ContactNumber == user.ContactNumber);
                if (entity != null)
                {
                    if (string.Compare(user.Ques1, entity.Ques1) == 0 && string.Compare(user.Ques2, entity.Ques2) == 0 && string.Compare(user.Ques3, entity.Ques3) == 0)
                    {
                        Status = true;
                        message = $"User ID is {entity.Email} ";
                    }
                    else
                    {
                        message = "Wrong Answers to the Questions";
                    }

                }
                else
                {
                    message = "User with this Contact NUmber dosen't exist";
                }
            }
            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(user);
        }

        //Forgot Password
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //Forgot Password POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(Forgotpassword forgotpassword, string ReturnUrl = "")
        {
            string message = "";
            bool Status = false;
            using (var context = new AdminDataContext())
            {
                User entity = context.Users.FirstOrDefault(x => x.Email == forgotpassword.Email);
                if (entity != null)
                {
                    if (string.Compare(forgotpassword.Ques1, entity.Ques1) == 0 && string.Compare(forgotpassword.Ques2, entity.Ques2) == 0 && string.Compare(forgotpassword.Ques3, entity.Ques3) == 0)
                    {
                        entity.Password = forgotpassword.NewPassword;
                        entity.ConfirmPassword = entity.Password;
                        context.Entry(entity).State = EntityState.Modified;
                        context.SaveChanges();

                        Status = true;
                        message = $"Password successfully changed for {entity.Email}";
                    }
                    else
                    {
                        message = "Wrong Answers to the Questions";
                    }
                }
                else
                {
                    message = "User with this email dosen't exist";
                }
            }
            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(forgotpassword);
        }

        //ServiceBooking get
        [HttpGet]
        public ActionResult ServiceBooking()
        {
            return View();
        }

        //ServiceBooking Post
        [HttpPost]
        public ActionResult ServiceBooking([Bind(Exclude = "Cleaner_Id,Status_Admin,Status_Cleaner,Service_Status")] Service serviceobj)
        {
            using (var context = new AdminDataContext())
            {
                serviceobj.Service_Status = "";
                serviceobj.Status_Admin = false;
                serviceobj.Status_Cleaner = false;
                context.Services.Add(serviceobj);
                context.SaveChanges();
            }

            return View(serviceobj);
        }

        //Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }


        //Ticket Create
        [Authorize]
        public ActionResult TicketCreate()
        {
            return View();
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TicketCreate([Bind(Include = "Id,Issue,Description,Date,Resolution,Status,UserEmail")] Ticket ticket)
        {
            string Message = "";
            AdminDataContext db = new AdminDataContext();
            if (ModelState.IsValid)
            {
                ticket.UserEmail = Convert.ToString(Session["UID"]);
                ticket.Date = DateTime.Now;
                ticket.Resolution = "Not Resolved";
                ticket.Status = false;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                Message = "Ticket Submited Successfully";
                
            }

            ViewBag.Message = Message;

            return View(ticket);
        }

        //Tickets View

        public ActionResult TicketView()
        {
            AdminDataContext db = new AdminDataContext();
            string uid = Convert.ToString(Session["UID"]);
            return View(db.Tickets.Where(x => x.UserEmail == uid).ToList());
        }

        [NonAction]
        public bool IsEmailExist(string email)
        {
            using (var context = new AdminDataContext())
            {
                var v = context.Users.FirstOrDefault(x => x.Email == email);
                if (v != null)
                {
                    return true;
                }
                else
                    return false;
            }
        }
    }
}