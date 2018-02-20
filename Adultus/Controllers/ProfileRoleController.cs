using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Adultus.Helpers;
using Adultus.Models;
using Adultus.ViewModels;

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

            return View(layoutViewModel.LayoutViewModelProfileRolesBuilder(Session["ProfileId"].ToString(), Session["UserId"].ToString()));
        }

        public ActionResult Create()
        {
            var userId = Session["UserId"].ToString();
            var profileId = Session["ProfileId"].ToString();

            return View(layoutViewModel.LayoutViewModelBuilder(profileId, userId));
        }

        // POST: userinfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] ProfileRoles profileRole)
        {
            profileRole.Id = Guid.NewGuid().ToString();
            if (ModelState.IsValid)
            {
                SqlHelper.CreateProfileRole(profileRole.Id, profileRole.ProfileId, profileRole.RoleId);
                return RedirectToAction("Index");
            }

            return View(profileRole);
        }

        public ActionResult Edit()
        {
            var userId = Session["UserId"].ToString();
            var profileId = Session["ProfileId"].ToString();

            return View(layoutViewModel.LayoutViewModelBuilder(profileId, userId));
        }

        // POST: userinfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection collection)
        {
            ProfileRoles profileRole = new ProfileRoles();
            profileRole.Id = collection["Id"];
            profileRole.ProfileId = collection["Profiles"];
            profileRole.RoleId = collection["Roles"];
            profileRole.Id = Guid.NewGuid().ToString();
            if (ModelState.IsValid)
            {
                SqlHelper.CreateProfileRole(profileRole.Id, profileRole.ProfileId, profileRole.RoleId);
                return RedirectToAction("Index");
            }

            return View(profileRole);
        }
    }
}