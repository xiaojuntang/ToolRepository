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
            //int a = 1;
            //int b = 0;
            //var c = a / b;
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

        [HttpPost]
        public JsonResult Login()
        {
            int a = 1;
            int b = 0;
            var c = a / b;

            string username = Request["username"];
            string pwd = Request["pwd"];

            message msg = null;

            if (username == "rain" && pwd == "m123")
            {
                msg = new message(true, "Success");
            }
            else
            {
                msg = new message(false, "Fail");
            }

            return Json(msg);
        }
    }

    public class message
    {
        public message(bool a,string b)
        {
            A = a;
            B = b;
        }

        public bool A { get; set; }

        public string B { get; set; }
    }
}