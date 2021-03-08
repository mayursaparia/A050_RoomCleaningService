﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_Admin_Prac.Models;
using System.Web.Security;

namespace Project_Admin_Prac.Controllers
{
    public class AdminController : Controller
    {
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
                            return RedirectToAction("AdminIndex", "Home");
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
    }
}