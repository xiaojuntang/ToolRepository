using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcPatterns.Controllers
{
    public class ArchiveController : Controller
    {
        // GET: Archive
        public ActionResult Index(string year,string month,string day)
        {
            return View();
        }


        public ActionResult Set(string year, string month, string day)
        {
            return View();
        }
    }
}