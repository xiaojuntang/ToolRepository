using MvcPatterns.Handler;
using System.Web;
using System.Web.Mvc;

namespace MvcPatterns
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ErrorAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
