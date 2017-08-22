using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcPatterns.RouteSpace;

namespace MvcPatterns
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Vue", action = "Index", id = UrlParameter.Optional }
            );

            //http://localhost:7449/archive/1988/9/10
            routes.MapRoute(
            "Archive",
            "archivetest/{year}/{month}/{day}",
            new { controller = "Archive", action = "Set", year = "", month = "", day = "" }, 
            new { year = new YearRouteConstraint(), month = new MonthRouteConstraint(), day = new DayRouteConstraint() }
        );
        }
    }
}
