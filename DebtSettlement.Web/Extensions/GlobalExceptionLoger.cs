using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using AlfaBank.Logger;

namespace DebtSettlement.Web.Extensions
{
    /// <summary>
    /// Global Exeptoon logger
    /// </summary>
    public class GlobalExceptionLogger : ExceptionLogger
    {
        private ILogger Loger =>
            (ILogger)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ILogger));
        /// <summary>
        /// Log
        /// </summary>
        /// <param name="context"></param>
        public override void Log(ExceptionLoggerContext context)
        {
            Loger.Error("Global error handler", context.Exception);
        }
    }
}