using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Routing;
using MCLYGV3.Web.Models;

namespace MCLYGV3.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //配置全局错误日志
            config.Filters.Add(new ExceptionHandlingAttribute());

            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            //RouteTable.Routes.MapHttpRoute(
            //     name: "DefaultApi",
            //     routeTemplate: "api/{controller}/{id}",
            //     defaults: new { id = RouteParameter.Optional }).RouteHandler = new SessionControllerRouteHandler();

            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

        }
    }
}
