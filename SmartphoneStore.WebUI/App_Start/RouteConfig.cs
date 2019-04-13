using System.Web.Mvc;
using System.Web.Routing;

namespace SmartphoneStore.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(null,
                "",
                new
                {
                    controller = "Smartphone",
                    action = "List",
                    manufacturer = (string)null,
                    page = 1
                }
            );

            routes.MapRoute(
                name: null,
                url: "Page{page}",
                defaults: new { controller = "Smartphone", action = "List", manufacturer = (string)null },
                constraints: new { page = @"\d+" }
            );

            routes.MapRoute(null,
                "{manufacturer}",
                new { controller = "Smartphone", action = "List", page = 1 }
            );

            routes.MapRoute(null,
                "{manufacturer}/Page{page}",
                new { controller = "Smartphone", action = "List" },
                new { page = @"\d+" }
            );

            routes.MapRoute(null, "{controller}/{action}");
        }
    }
}