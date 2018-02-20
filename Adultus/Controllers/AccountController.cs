using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Adultus.Models;
using Adultus.Helpers;
using Adultus.ViewModels;

namespace Adultus.Controllers
{
    public class AccountController : Controller
    {
        public LayoutViewModel layoutViewModel = new LayoutViewModel();

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View("Login");
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(Users model, string returnUrl)
        {
            SqlHelper.DbContext();
            if (model.ConfirmPassword == "0")
            {
                Users userSetUp = SqlHelper.LoginQuery(model.UserName, model.Password, false);
                //ADD BASE 64 FOR ENCRYPTING PASSWORD WITH A HASH THERE IS A COLUMN FOR THE HAS ALREADY IN THE USER TABLE
                if (userSetUp.Id != null)
                {
                    SqlHelper.AddSession(userSetUp.Id);
                    SqlHelper.SetOnlineStatus(userSetUp.Id);
                    Session["UserName"] = userSetUp.UserName;
                    Session["UserId"] = userSetUp.Id;
                    Session["ProfileId"] = userSetUp.ProfileId;
                    return RedirectToAction("SetPassword", layoutViewModel.LayoutViewModelUserBuilder(userSetUp.ProfileId, userSetUp.Id));
                }
            }
            if (model.UserName == null || model.Password == null)
            {
                model.UserName = "error";
                return View("Login", model);
            }
           
            
            string encode = Base64Encode(model.Password);
            Users user = SqlHelper.LoginQuery(model.UserName, encode, true);
            //ADD BASE 64 FOR ENCRYPTING PASSWORD WITH A HASH THERE IS A COLUMN FOR THE HAS ALREADY IN THE USER TABLE
            if (user.Id != null)
            {
                SqlHelper.AddSession(user.Id);
                SqlHelper.SetOnlineStatus(user.Id);
                Session["UserName"] = user.UserName;
                Session["UserId"] = user.Id;
                Session["ProfileId"] = user.ProfileId;
                return RedirectToAction("Index", "Home");
            }
            model.UserName = "error";
            return View("Login", model);
        }

        internal class MinimumAgeAttribute : ValidationAttribute
        {
            int _minimumAge;

            public MinimumAgeAttribute(int minimumAge)
            {
                _minimumAge = minimumAge;
            }

            public override bool IsValid(object value)
            {
                DateTime date;
                if (DateTime.TryParse(value.ToString(), out date))
                {
                    return date.AddYears(_minimumAge) < DateTime.Now;
                }

                return false;
            }
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View("Registration");
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(Register model)
        {
            SqlHelper.DbContext();
            try
            {
                var newGuid = Guid.NewGuid().ToString();
                SqlHelper.AddUser(newGuid, model.UserName, model.EmailAddress, model.DateOfBirth);
                var rand = RandomString(8);
                SqlHelper.SetPassword(rand, newGuid, false);
                EmailService emailService = new EmailService();
                emailService.SendConfirmationEmail(newGuid);
                model.UserName = "Complete";
            }
            catch(Exception e)
            {
                model.UserName = "Failed";
                return View("Registration", model);
            }

            return View("Registration", model);
        }

        [AllowAnonymous]
        public ActionResult Confirm()
        {
            return View("Confirm");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(string id, FormCollection collection)
        {
            SqlHelper.DbContext();

            var user = SqlHelper.GetUser(id);

            if (!user.EmailConfirmed)
            {
                SqlHelper.EmailConfirmation(id);
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);

                        var path = Path.Combine(Server.MapPath("~/img/"), fileName);
                        SqlHelper.AddProfilePic(fileName, user.Id);
                        file.SaveAs(path);
                    }
                }
                FormsAuthentication.SetAuthCookie(user.UserName, false);
                string profileId = collection["profileId"];
                string gender = collection["gender"];
                string genderPreference = collection["genderPreference"];
                SqlHelper.ProfileSetUp(id, profileId, gender, genderPreference);
                List<string> items = new List<string>();
                items.Add("Please login with the temporary password that was emailed to you.");
                items.Add("Once you have logged in you will need to change your password.");

                ViewBag.Items = items;
                user.ConfirmPassword = "0";
                return View("Login", user);
            }
            else
            {
                FormsAuthentication.SignOut();
                user.UserName = "Already Confirmed";
                //ViewBag.Msg = "Account Already Approved";
                return View("Login", user);
            }
        }

        [AllowAnonymous]
        public ActionResult SetPassword()
        {
            var userId = Session["UserId"].ToString();
            var profileId = Session["ProfileId"].ToString();

            return View(layoutViewModel.LayoutViewModelUserBuilder(profileId, userId));
            //return View("SetPassword");
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetPassword(FormCollection collection)
        {
            var userId = Session["UserId"].ToString();
            var profileId = Session["ProfileId"].ToString();
            if (!ModelState.IsValid)
            {
                return View("SetPassword");
            }
            else
            {
                if (!Regex.IsMatch(collection["User.Password"], @"^(?=.{6,})(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\d)(?=.*?\W).*$") || collection["User.Password"] != collection["User.ConfirmPassword"])
                {
                    
                    return View("SetPassword", layoutViewModel.LayoutViewModelUserBuilder(profileId, userId));
                }

                SqlHelper.DbContext();
                SqlHelper.SetPassword(collection["User.Password"], collection["User.Id"], true);

                List<string> items = new List<string>();
                items.Add("Password Updated");

                ViewBag.Items = items;
            }

            return View("SetPassword", layoutViewModel.LayoutViewModelUserBuilder(profileId, userId));
        }

        //[AllowAnonymous]
        //public ActionResult SetAccount()
        //{
        //    return View("SetAccount");
        //}

        ////
        //// POST: /Account/Register
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult SetAccount(Users user)
        //{
        //   // user.ConfirmPassword = "0";
        //    return View("SetAccount", user);
        //}

        [AllowAnonymous]
        public ActionResult Verify()
        {
            return View("Verification");
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Verify(Verification model)
        {
            DateTime date;
            int minimum = 18;
            if (DateTime.TryParse(model.DateOfBirth.ToString(), out date))
            {
                if (date.AddYears(minimum) < DateTime.Now)
                {
                    return View("Login");
                }
            }
            // If we got this far, something failed, redisplay form
            return View("Verification");
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}