using HR.AuthorizationAccessHelper;
using HR.AuthorizationAccessHelper.Interface;
using AlfaBank.Logger;
using Newtonsoft.Json;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DebtSettlement.BusinessLayer;
using Unity.ServiceLocation;
using Unity;
using CommonServiceLocator;
using Unity.Injection;
using AlfaBank.Common.Unity;

namespace DebtSettlement.Web
{
    /// <summary>
    /// Class MvcApplication.
    /// </summary>
    /// <seealso cref="System.Web.HttpApplication" />
    public class MvcApplication : HttpApplication
    {
        private ILogger Loger => 
            (ILogger)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ILogger));

        /// <summary>
        /// Applications the start.
        /// </summary>d
        protected void Application_Start()
        {
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes); 
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutoMapperConfig.RegisterMappings();
            GlobalConfiguration.Configure(Register);

            var formatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            formatter.SerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects
            };
        }

        /// <summary>
        /// Application exeptions handler
        /// </summary>
        protected void Application_Error()
        {
            var ex = Server.GetLastError();
            Loger.Error("Global error handler", ex);
        }
        /// <summary>
        /// Registers the specified configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        private static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();
            
            RegisterDependency(container);

            GlobalConfiguration.Configuration.DependencyResolver = new UnityResolver(container);

            ControllerBuilder.Current.SetControllerFactory(new IocControllerFactory(container));

            var locator = new UnityServiceLocator(container);

            ServiceLocator.SetLocatorProvider(() => locator);
        }

        /// <summary>
        /// Registers the dependency.
        /// </summary>
        /// <param name="container">The container.</param>
        private static void RegisterDependency(IUnityContainer container)
        {
            DebtSettlement.BusinessLayer.Bootstraper.Register(container);
          //  AgreementLoader.Bootstraper.Register(container);

            AlfaBank.Logger.Bootstraper.Register(container);
            HR.Client.Bootstraper.Register(container);

            container.RegisterType<IAuthorizeService, AuthorizeService>(
                new InjectionConstructor(AppSettings.HRWebAPIAddress));
        }
    }
}
