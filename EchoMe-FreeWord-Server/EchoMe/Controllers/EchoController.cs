using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using EchoMe.Filters;

namespace EchoMe.Controllers
{
    public class EchoController : Controller
    {
        //
        // GET: /Echo/
        [InitializeSimpleMembership]
        public ActionResult Index()
        {
            ViewBag.Message = "Echo.Me V2 is under development";
            return View();
        }
    }
}
