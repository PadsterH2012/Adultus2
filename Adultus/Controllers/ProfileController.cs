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
    public class ProfileController : Controller
    {
        public LayoutViewModel layoutViewModel = new LayoutViewModel();

        // GET: Profile
        public ActionResult Index()
        {
            SqlHelper.DbContext();

            var userId = Session["UserId"].ToString();
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

            return View(layoutViewModel.LayoutViewModelAllProfilesBuilder(userId));
        }

        public ActionResult Create()
        {
            var userId = Session["UserId"].ToString();
            var profileId = Session["ProfileId"].ToString();

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

            return View(layoutViewModel.LayoutViewModelBuilder(profileId, userId));
        }

        // POST: userinfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")]  Profiles profile)
        {
            profile.Id = Guid.NewGuid().ToString();
            if (ModelState.IsValid)
            {
                SqlHelper.CreateProfile(profile.Id, profile.name);
                return RedirectToAction("Index");
            }

            return View(profile);
        }

        public ActionResult Edit()
        {
            var userId = Session["UserId"].ToString();

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

            return View(layoutViewModel.LayoutViewModelAllProfilesBuilder(userId));
        }

        // POST: userinfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection collection)
        {
            var userId = Session["UserId"].ToString();
            if (ModelState.IsValid)
            {
                
            SqlHelper.DbContext();
            List<Profiles> profiles = SqlHelper.GetAllProfiles();
            for(var i = 0; i < profiles.Count; i++)
            {
                string updateProfile = collection["Profiles[" + i + "].name"];
                    if (profiles[i].name != updateProfile)
                {
                    SqlHelper.EditProfile(profiles[i].Id, updateProfile);
                }
            }
                return RedirectToAction("Index");
            }
            return View(layoutViewModel.LayoutViewModelAllProfilesBuilder(userId));
        }
    }
}