using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DocumentViewer
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{returnUrl}",
                defaults: new { controller = "Account", action = "Login", returnUrl = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "Delete",
              url: "{controller}/{action}/{id}"
          );


           // routes.MapMvcAttributeRoutes();
        }
    }
}
