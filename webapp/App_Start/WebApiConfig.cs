using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace WebApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            configuration.Routes.MapHttpRoute("API Default", "api/{APIController}/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional });
        }

        //public static void Register(HttpConfiguration config)
        //{

        //    config.MapHttpAttributeRoutes();

        //    config.Routes.MapHttpRoute(
        //        name: "DefaultWithActionApi",
        //        routeTemplate: "api/{controller}/{action}",
        //        defaults: new { id = RouteParameter.Optional }
        //    );


        //    config.Routes.MapHttpRoute(
        //        name: "DefaultApi",
        //        routeTemplate: "api/{controller}/{id}",
        //        defaults: new { id = RouteParameter.Optional }
        //    );

        //    var formatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
        //    formatter.SerializerSettings.ContractResolver =
        //        new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
        //}
    }
}
