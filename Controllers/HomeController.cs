using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Study()
        {
            ViewBag.Message = "Your  page.";

            return View();
        }
        public ActionResult Course_Piano()
        {
            return View();
        }
        public ActionResult Course_Guitar()
        {
            return View();
        }
        public ActionResult Course_Organ()
        {
            return View();
        }
        public ActionResult Course_Cajun()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

    }
}