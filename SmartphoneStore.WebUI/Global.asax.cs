using System.Web.Mvc;
using System.Web.Routing;
using GameStore.WebUI.Infrastructure.Binders;
using SmartphoneStore.Domain.Entities;

namespace SmartphoneStore.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ModelBinders.Binders.Add(typeof(Cart), new CartModelBinder());
        }
    }
}
