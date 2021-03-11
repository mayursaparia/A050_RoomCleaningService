﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Project_Admin_Prac.Models;


namespace Project_Admin_Prac.Controllers
{
    public class CleanerController : Controller
    {
        // Cleaner Registration
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        //Cleaner Registratin POST

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "AdminApproved")] Cleaner cleaner)
        {
            bool Status = false;
            string Message = "";
            if (ModelState.IsValid)
            {
                var isExist = IsCleanerIdExist(cleaner.CleanerId);
                if (isExist)
                {
                    ModelState.AddModelError("CleanerIdExist", "CleanerId already exist");
                    return View(cleaner);
                }
                //encryption of Passwords
                //cleaner.Password = Crypto.Hash(cleaner.Password);// using crypto
                //cleaner.ConfirmPassword = Crypto.Hash(cleaner.ConfirmPassword);// using crypto
                cleaner.AdminApproved = false;
                using (var context = new AdminDataContext())
                {
                    context.Cleaners.Add(cleaner);
                    context.SaveChanges();
                    Status = true;
                    Message = "New Cleaner Created Successfully, Registration sent for approval";
                }
            }
            else
            {
                Message = "Invalid Request";
            }
            ViewBag.Message = Message;
            ViewBag.Status = Status;
            return View(cleaner);
        }


        //Cleaner LOGIN
        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }

        //Cleaner Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(CleanerLogin cleanerLogin, string returnUrl = "")
        {
            string message = "";
            ViewBag.Message = message;
            using (var context = new AdminDataContext())
            {
                var entity = context.Cleaners.FirstOrDefault(x => x.CleanerId == cleanerLogin.CleanerID);
                if (entity != null)
                {
                    if(entity.AdminApproved != false)
                    {
                        //if (string.Compare(Crypto.Hash(cleanerLogin.Password), entity.Password) == 0) // using crypto
                        if (string.Compare(cleanerLogin.Password, entity.Password) == 0)
                        {
                            int timeout = cleanerLogin.RememberMe ? 525600 : 120;
                            var ticket = new FormsAuthenticationTicket(cleanerLogin.CleanerID, cleanerLogin.RememberMe, timeout);
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
                                return RedirectToAction("CleanerIndex", "Cleaner");
                            }
                        }
                        else
                        {
                            message = "Password not matching";
                        }
                    }
                    else
                    {
                        message = "Admin has not Approved Yet";
                    }
                    
                }
                else
                {
                    message = "Cleaner ID not Present";
                }
            }
            ViewBag.Message = message;
            return View();
        }
        //Log Out Cleaner
        //Logout

        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Cleaner");
        }


        //LandingPAGE for Cleaner
        [Authorize]
        public ActionResult CleanerIndex()
        {
            return View();
        }


        //CHECK FOR DUPlicates
        [NonAction]
        public bool IsCleanerIdExist(string cid)
        {
            using (var context = new AdminDataContext())
            {
                var v = context.Cleaners.FirstOrDefault(x => x.CleanerId == cid);
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