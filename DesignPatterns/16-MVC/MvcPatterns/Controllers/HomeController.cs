using MvcPatterns.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcPatterns.Controllers
{
    [TimingFilter]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            int a = 1;
            int b = 0;

            var c = a / b;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}