using System;
using System.Web.Http;
using System.Web.Routing;
using DPE.ExceptionFilter.Service.Utilities;
using WebApiContrib.IoC.Ninject;

namespace DPE.ExceptionFilter.Service
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.MapHttpRoute("Service", "api/{controller}/{id}", new { id = RouteParameter.Optional });

            GlobalConfiguration.Configuration.DependencyResolver = new NinjectResolver(IocContainer.Initialize());
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
        }
    }
}