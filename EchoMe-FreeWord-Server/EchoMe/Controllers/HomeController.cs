using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EchoMe.Filters;

namespace EchoMe.Controllers
{
    public class HomeController : Controller
    {
        [InitializeSimpleMembership]
        public ActionResult Index()
        {
            ViewBag.Message = "Message";

            return View();
        }

        [InitializeSimpleMembership]
        public ActionResult About()
        {
            ViewBag.Message = "";

            return View();
        }

        [InitializeSimpleMembership]
        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Info.";

            return View();
        }
    }
}
