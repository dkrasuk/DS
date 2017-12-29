using System.Web.Mvc;

namespace DebtSettlement.Web.Controllers
{
    /// <summary>
    /// Class ErrorController.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class ErrorController : Controller
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Nots the found.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult NotFound()
        {
            return View();
        }

        /// <summary>
        /// Unauthorizeds this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Unauthorized()
        {
            return View();
        }

        /// <summary>
        /// Ajaxes this instance.
        /// </summary>
        /// <returns>JsonResult.</returns>
        public JsonResult Ajax()
        {
            var model = (HandleErrorInfo)(ViewData.Model);

            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new
                {
                    error = true,
                    message = model.Exception.Message
                }
            };
        }
    }
}