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
    public class RoleController : Controller
    {
        public LayoutViewModel layoutViewModel = new LayoutViewModel();
        // GET: Role
        public ActionResult Index()
        {
            SqlHelper.DbContext();

            return View(layoutViewModel.LayoutViewModelRolesBuilder(Session["UserId"].ToString()));
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
            Roles role = new Roles();
            role.Id = collection["Id"];
            role.name = collection["Name"];
            if (ModelState.IsValid)
            {
                SqlHelper.EditProfile(role.Id, role.name);
                return RedirectToAction("Index");
            }
            return View(role);
        }
    }
}