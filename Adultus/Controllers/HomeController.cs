using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Adultus.Helpers;
using Adultus.Models;
using Adultus.ViewModels;

namespace Adultus.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var ctx = System.Web.HttpContext.Current;
            string UserName = Session["UserName"].ToString();
            string logout = (string)ctx.Session[UserName];
            if (logout == "logout")
            {
                return View("~/Views/Account/Login.cshtml");
            }
            LayoutViewModel layoutViewModel = new LayoutViewModel();

            ViewBag.Title="Adultus";
            return View(layoutViewModel.LayoutViewModelBuilder(Session["ProfileId"].ToString(), Session["UserId"].ToString()));
        }

        public ActionResult Search(string searchName)
        {
            string userId = Session["UserId"].ToString();

            SqlHelper.DbContext();
            Users user = SqlHelper.GetUser(userId);

            HttpCookie cookie = FormsAuthentication.GetAuthCookie(user.UserName, true);
            var ticket = FormsAuthentication.Decrypt(cookie.Value);

            FormsAuthenticationTicket authTicket = new
                FormsAuthenticationTicket(1, //version
                    ticket.Name,
                    DateTime.Now,             //creation
                    DateTime.Now.AddMinutes(60), //Expiration
                    true, "");
            // Encrypt the ticket.
            string encTicket = FormsAuthentication.Encrypt(authTicket);

            // Create the cookie.
            Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

            LayoutViewModel layoutViewModel = new LayoutViewModel();

            return View(layoutViewModel.LayoutViewModelUserSearchBuilder(Session["ProfileId"].ToString(), userId, searchName));
        }
    }
}