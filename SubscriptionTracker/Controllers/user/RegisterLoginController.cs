using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.Web.Security;
using SubscriptionTracker.Models;
using System.Web.Helpers;

namespace SubscriptionTracker.Controllers
{
    public class RegisterLoginController : Controller
    {

        // GET: RegisterLogin
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(User userRegister)
        {
            bool Status = false;
            string message = "";
            // Model Validation
            if (ModelState.IsValid)
            {
                // //Email is already Exist
                var isExist = IsEmailExist(userRegister.EmailId);
                if (isExist)
                {
                    ModelState.AddModelError("Email Exist", "Email Address already exist");
                    return View(userRegister);
                }
                #region Password Hashing
                userRegister.Password = Crypto.Hash(userRegister.Password);
                userRegister.ConfirmPassword = Crypto.Hash(userRegister.ConfirmPassword);
                #endregion
                #region Save to Database
                using (SubTrackerContext dc = new SubTrackerContext())
                {
                    dc.UsersTable.Add(userRegister);
                    dc.SaveChanges();
                    // SendVerificationLinkEmail(user.EmailID, user.ActivationCode.ToString());
                    message = "Registration Successfully done";
                    Status = true;
                }
                #endregion
                SendEmail(userRegister.EmailId);

            }
            else
            {
                message = "InValid Request";
            }
            ViewBag.Message = message;
            ViewBag.Status = Status;

            //Generate Activation Code

            //Password Hashing
            //Save data to database
            //Send Email to user



            return RedirectToAction("LogIn", "RegisterLogin");

        }
        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (SubTrackerContext dc = new SubTrackerContext())
            {
                var v = dc.UsersTable.Where(a => a.EmailId == emailID).FirstOrDefault();
                return v != null;
            }
        }

        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(User login, string ReturnUrl)
        {
            string message = "";
            ViewBag.Message = message;
            using (SubTrackerContext dc = new SubTrackerContext())
            {
                var v = dc.UsersTable.Where(a => a.EmailId == login.EmailId).FirstOrDefault();
                if (v != null)
                {

                    if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
                    {
                        int timeout = login.RememerMe ? 525600 : 1;
                        var ticket = new FormsAuthenticationTicket(login.EmailId, login.RememerMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        FormsAuthentication.SetAuthCookie(login.EmailId, false);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);
                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            Session["Emailid"] = login.EmailId;
                            return RedirectToAction("index", "Services");
                        }
                    }
                    else
                    {
                        message = "Invalid Credentials provided";
                    }
                }
                else
                {
                    message = "Invalid Credentials provided";
                }
            }
            ViewBag.Message = message;
            return View();
        }
        public ActionResult Logout()
        {
            string Id = (string)Session["Id"];
            Session.Abandon();
            return RedirectToAction("LogIn", "RegisterLogin");
        }
        [NonAction]
        public void SendEmail(string EmailId)
        {
            var from = new MailAddress("trackersubscription@gmail.com", "SubTracker");
            var to = new MailAddress(EmailId);
            var frompw = "subscriptionTracker123";
            string sub = "your account is successfully created";
            string body = "<br/><br/>We are excited to tell you that your account is successfully created on <strong>SubTracker</strong>" +
                       "<br/><br/> Congratulations!!";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from.Address, frompw)


            };
            using (var message = new MailMessage(from, to))
            {
                message.Subject = sub;
                message.Body = body;
                message.IsBodyHtml = true;

                smtp.Send(message);

            }
        }

    }
}