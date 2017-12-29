using System.IO;
using System.Web.Mvc;

namespace DebtSettlement.Web.Filters
{
    /// <summary>
    /// Class DeleteFileAttribute.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.ActionFilterAttribute" />
    public class DeleteFileAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Called by the ASP.NET MVC framework after the action result executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Flush();

            var filePathResult = filterContext.Result as FilePathResult;

            if (filePathResult != null)
            {
                File.Delete(filePathResult.FileName);
            }
        }
    }
}