using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kztek.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //News
            routes.MapRoute(
               name: "NewsDetail",
               url: "chi-tiet-tin-tuc/{newcategory}/{title}",
               defaults: new { controller = "News", action = "Detail", newcategory = UrlParameter.Optional, title = UrlParameter.Optional },
               namespaces: new[] { "Kztek.Web.Controllers" }
           );

            routes.MapRoute(
                name: "News",
                url: "dm-tin-tuc/{newcategory}",
                defaults: new { controller = "News", action = "Index", newcategory = UrlParameter.Optional },
                namespaces: new[] { "Kztek.Web.Controllers" }
            );

            //Service
            routes.MapRoute(
               name: "ServiceDetail",
               url: "chi-tiet-giai-phap/{newcategory}/{title}",
               defaults: new { controller = "Service", action = "Detail", newcategory = UrlParameter.Optional, title = UrlParameter.Optional },
               namespaces: new[] { "Kztek.Web.Controllers" }
           );

            routes.MapRoute(
                name: "Service",
                url: "dm-giai-phap/{newcategory}",
                defaults: new { controller = "Service", action = "Index", newcategory = UrlParameter.Optional },
                namespaces: new[] { "Kztek.Web.Controllers" }
            );
            //Contact
            routes.MapRoute(
                name: "Contact",
                url: "lien-he",
                defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Kztek.Web.Controllers" }
              
            );
            //Media
            routes.MapRoute(
                name: "Media",
                url: "bo-suu-tap",
                defaults: new { controller = "Media", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Kztek.Web.Controllers" }

            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Kztek.Web.Controllers" }
            );

            //Device
            routes.MapRoute(
                name: "Product",
                url: "thiet-bi",
                defaults: new { controller = "Product", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Kztek.Web.Controllers" }

            );
        }
    }
}
