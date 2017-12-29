using System.Web.Http;
using Microsoft.Practices.Unity;
using System.Web.OData.Extensions;
using System.Net.Http.Headers;
using System.Web.Http.ExceptionHandling;
using DebtSettlement.Web.Attributes;
using DebtSettlement.Web.Extensions;
using Unity;
using Unity.ServiceLocation;
using CommonServiceLocator;
using AlfaBank.Common.Unity;

namespace DebtSettlement.Web
{
    /// <summary>
    /// Class WebApiConfig.
    /// </summary>   
    public static class WebApiConfig
    {
        /// <summary>
        /// Registers the specified configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public static void Register(HttpConfiguration config)
        {

            RegisterDependencyResolver(config);

            config.MapHttpAttributeRoutes();
            
            var json = config.Formatters.JsonFormatter;
            
            json.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
            json.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

            config.EnableDependencyInjection();
            config.AddODataQueryFilter(new ODataQueryableAttribute());

            json.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            config.Services.Add(typeof(IExceptionLogger), new GlobalExceptionLogger());
        }

        /// <summary>
        /// Registers the dependency resolver.
        /// </summary>
        /// <param name="config">The configuration.</param>
        private static void RegisterDependencyResolver(HttpConfiguration config)
        {
            var container = new UnityContainer();

            config.DependencyResolver = new UnityResolver(container);

            var locator = new UnityServiceLocator(container);

            ServiceLocator.SetLocatorProvider(() => locator);
        }
    }
}
