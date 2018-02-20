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
    public class ProfileController : Controller
    {
        public LayoutViewModel layoutViewModel = new LayoutViewModel();

        // GET: Profile
        public ActionResult Index()
        {
            SqlHelper.DbContext();

            return View(layoutViewModel.LayoutViewModelAllProfilesBuilder(Session["UserId"].ToString()));
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
            Profiles profile = new Profiles();
            profile.Id = collection["Id"];
            profile.name = collection["Name"];
            if (ModelState.IsValid)
            {
                SqlHelper.EditProfile(profile.Id, profile.name);
                return RedirectToAction("Index");
            }
            return View(profile);
        }
    }
}