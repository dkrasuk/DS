using System.Web.Mvc;
using System.Web.Security;

namespace DebtSettlement.Web.Controllers
{
    /// <summary>
    /// Class AccountController.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [Authorize]
    public class AccountController : Controller
    {
        /// <summary>
        /// Logouts this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return Redirect("~/");
        }
    }
}