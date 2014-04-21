using System;
using System.Net;
using System.Web.Http;
using System.Web.Routing;
using DPE.ExceptionFilter.Service.Utilities;
using Ninject;
using WebApiContrib.IoC.Ninject;

namespace DPE.ExceptionFilter.Service
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.MapHttpRoute("Service", "api/{controller}/{id}", new { id = RouteParameter.Optional });

            var container = IocContainer.Initialize();
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectResolver(container);
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            GlobalConfiguration.Configuration.Filters.Add(container.Get<ExceptionFilter>());
        }
    }
}