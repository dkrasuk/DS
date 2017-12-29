using System.Threading;
using System.Web.Mvc;
using HR.Client.Interface;
using System.Web.Http;
using AlfaBank.Common.Utils;

namespace DebtSettlement.Web.Views
{
    /// <summary>
    /// Class BaseView.
    /// </summary>
    /// <seealso cref="DebtSettlement.Web.Views.BaseView{dynamic}" />
    public abstract class BaseView : BaseView<dynamic> { }

    // There are not objects, initiated by constructor, because the constructor has been calling many times
    // (for each page and partial page) but class instances need not often.
    /// <summary>
    /// Class BaseView.
    /// </summary>
    /// <typeparam name="TModel">The type of the t model.</typeparam>
    /// <seealso cref="System.Web.Mvc.WebViewPage{TModel}" />
    public abstract class BaseView<TModel> : WebViewPage<TModel>
    {
        private IPermissionService _permissionService => 
            (IPermissionService) GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IPermissionService));
        /// <summary>
        /// User has access
        /// </summary>
        /// <param name="access"></param>
        /// <returns></returns>
        public bool UserHasAccess(string access)
        {
            var login = Thread.CurrentPrincipal.Identity.Name.FormatUserName();
            
            return _permissionService.UserHasAccess(login, access);
        }
    }
}
