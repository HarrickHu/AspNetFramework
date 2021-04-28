using Asp.NetWebAPI.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Asp.NetWebAPI
{
    public static class WebApiConfig
    {
        /// <summary>
        /// Web API 配置和服务
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            // 自定义路由约束
            var constraintResolver = new DefaultInlineConstraintResolver();
            constraintResolver.ConstraintMap.Add("nonzero", typeof(NonZeroConstraint));

            config.MapHttpAttributeRoutes(constraintResolver);

            // 特性路由
            //config.MapHttpAttributeRoutes();

            // 基于约定的路由
            // https://docs.microsoft.com/zh-cn/aspnet/web-api/overview/web-api-routing-and-actions/
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}", // API路由路径模板
                defaults: new { id = RouteParameter.Optional } // {id}参数默认为可选
            );
        }
    }
}
