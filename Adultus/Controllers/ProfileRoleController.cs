using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Adultus.Helpers;
using Adultus.Models;
using Adultus.ViewModels;
using Roles = Adultus.Models.Roles;

namespace Adultus.Controllers
{
    public class ProfileRoleController : Controller
    {
        public LayoutViewModel layoutViewModel = new LayoutViewModel();

        // GET: ProfileRole
        public ActionResult Index()
        {
            SqlHelper.DbContext();
            LayoutViewModel layoutViewModel = new LayoutViewModel();

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

            return View(layoutViewModel.LayoutViewModelProfileRolesBuilder(Session["UserId"].ToString()));
        }

        public ActionResult Create()
        {
            var userId = Session["UserId"].ToString();
            var profileId = Session["ProfileId"].ToString();

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
        public ActionResult Create(FormCollection collection)
        {
            var userId = Session["UserId"].ToString();
            SqlHelper.DbContext();
            string id = Guid.NewGuid().ToString();
            string profileId = collection["Profiles"];
            Profiles profileName = SqlHelper.GetProfile(profileId);
            string roleId = collection["Roles"];
            Roles roleName = SqlHelper.GetRole(roleId);
            if (ModelState.IsValid)
            {
                SqlHelper.CreateProfileRole(id,profileId, profileName.name,roleId, roleName.name);
                return RedirectToAction("Index");
            }

            return View(layoutViewModel.LayoutViewModelProfileRolesBuilder(Session["UserId"].ToString()));
        }

        public ActionResult Edit()
        {
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

            return View(layoutViewModel.LayoutViewModelProfileRolesBuilder(userId));
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
                List<ProfileRoles> profileRoles = SqlHelper.GetAllProfileRoles();
                for (var i = 0; i < profileRoles.Count; i++)
                {
                    string updateProfile = collection["ProfileRoles[" + i + "].ProfileName"];
                    string updateRole = collection["ProfileRoles[" + i + "].RoleName"];
                    if (profileRoles[i].ProfileName != updateProfile || profileRoles[i].RoleName != updateRole && profileRoles[i].RoleName != null)
                    {
                        Profiles profile = SqlHelper.GetProfileByName(updateProfile);
                        Roles role = SqlHelper.GetRoleByName(updateRole);
                        SqlHelper.EditProfileRoles(profileRoles[i].Id, profile.Id, updateProfile, role.Id, updateRole);
                    }
                }
                return RedirectToAction("Index");
            }
            return View(layoutViewModel.LayoutViewModelProfileRolesBuilder(userId));
        }
    }
}