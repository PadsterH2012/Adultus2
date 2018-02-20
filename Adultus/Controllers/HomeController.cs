using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Adultus.Helpers;
using Adultus.ViewModels;

namespace Adultus.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            LayoutViewModel layoutViewModel = new LayoutViewModel();

            ViewBag.Title="Adultus";
            return View(layoutViewModel.LayoutViewModelBuilder(Session["ProfileId"].ToString(), Session["UserId"].ToString()));
        }
    }
}