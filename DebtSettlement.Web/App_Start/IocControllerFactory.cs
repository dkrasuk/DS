using System;
using System.Web.Mvc;
using System.Web.Routing;
using Unity;

namespace DebtSettlement.Web
{
    /// <summary>
    /// Class IocControllerFactory.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.DefaultControllerFactory" />
    public class IocControllerFactory : DefaultControllerFactory
    {
        private readonly IUnityContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="IocControllerFactory"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public IocControllerFactory(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Retrieves the controller instance for the specified request context and controller type.
        /// </summary>
        /// <param name="requestContext">The context of the HTTP request, which includes the HTTP context and route data.</param>
        /// <param name="controllerType">The type of the controller.</param>
        /// <returns>The controller instance.</returns>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType != null)
            {
                return _container.Resolve(controllerType) as IController;
            }

            return base.GetControllerInstance(requestContext, controllerType);
        }
    }
}