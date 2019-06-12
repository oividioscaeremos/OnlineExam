using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using OnlineSinav.Controllers;

namespace OnlineSinav
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var namespaces = new[] { typeof(AuthController).Namespace };
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.MapRoute("Home", "", new { controller = "Auth", action = "Index" }, namespaces);
            routes.MapRoute("Home", "", new { controller = "Auth", action = "Login" });
            routes.MapRoute("Login", "login", new { controller = "Auth", action = "Login" });
            routes.MapRoute("Logout", "Logout", new { controller = "Auth", action = "Logout" });
           

        }
    }
}
