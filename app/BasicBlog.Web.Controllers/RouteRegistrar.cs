using System.Web.Mvc;
using System.Web.Routing;

namespace BasicBlog.Web.Controllers
{
    public class RouteRegistrar
    {
        public static void RegisterRoutesTo(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            routes.MapRoute(
                "Entries",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Entries", action = "Index", id = UrlParameter.Optional }  // Parameter defaults
            );
        }
    }
}
