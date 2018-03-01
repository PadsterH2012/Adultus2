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
    public class RoleController : Controller
    {
        public LayoutViewModel layoutViewModel = new LayoutViewModel();
        // GET: Role
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

            return View(layoutViewModel.LayoutViewModelRolesBuilder(Session["UserId"].ToString()));
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
        public ActionResult Create([Bind(Include = "Name")] Roles role)
        {
            role.Id = Guid.NewGuid().ToString();
            if (ModelState.IsValid)
            {
                SqlHelper.CreateRole(role.Id, role.name);
                return RedirectToAction("Index");
            }

            return View(role);
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

            SqlHelper.DbContext();
            List<Roles> roles = SqlHelper.GetAllRoles();

            List<SelectListItem> rolesSelectList = new List<SelectListItem>();

            foreach (Roles p in roles)
            {
                rolesSelectList.Add(new SelectListItem()
                {
                    Value = p.Id,
                    Text = p.name
                });
            }

            ViewBag.Roles = rolesSelectList;

            return View(layoutViewModel.LayoutViewModelRolesBuilder(userId));
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
                List<Roles> roles = SqlHelper.GetAllRoles();
                for (var i = 0; i < roles.Count; i++)
                {
                    string updateRole = collection["Roles[" + i + "].name"];
                    if (roles[i].name != updateRole)
                    {
                        SqlHelper.EditRole(roles[i].Id, updateRole);
                    }
                }
                return RedirectToAction("Index");
            }
            return View(layoutViewModel.LayoutViewModelRolesBuilder(userId));
        }
    }
}