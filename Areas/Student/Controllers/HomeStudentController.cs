using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web2.Areas.Student.Controllers
{
    public class HomeStudentController : Controller
    {
        // GET: Student/HomeStudent
        public ActionResult Index()
        {
            return View();
        }
    }
}