using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Project_Admin_Prac.Models;

namespace Project_Admin_Prac.Controllers
{
    public class UserController : Controller
    {
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
        public ActionResult Registration([Bind(Exclude = "IsEmailVerified,ActivationCode")] User user)
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
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                user.IsEmailVerified = false;
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
                    if (string.Compare(Crypto.Hash(login.Password), entity.Password) == 0)
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
                            return RedirectToAction("UserIndex", "Home");
                        }
                    }
                    else
                    {
                        Message = "Invalid Crediantials Provided";
                    }

                }
                else
                {
                    Message = "Invalid Crediantials Provided";
                }
            }

            ViewBag.Message = Message;
            return View();
        }

        //Logout
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
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