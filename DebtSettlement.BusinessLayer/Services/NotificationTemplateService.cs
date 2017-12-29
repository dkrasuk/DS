using AlfaBank.Logger;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using DebtSettlement.BusinessLayer.Services.Interfaces;
using DebtSettlement.Model.DTO;
using DebtSettlement.Model.Enums;
using AlfaBank.Common.Utils;

namespace DebtSettlement.BusinessLayer.Services
{

    /// <summary>
    /// Class HRService.
    /// </summary>
    /// <seealso cref="DebtSettlement.BusinessLayer.Interface.IHRService" />
    public class NotificationTemplateService : INotificationTemplateService
    {
        #region Private members

        private static ILogger _logger;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationTemplateService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="userService">The user service.</param>
        public NotificationTemplateService(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get the specified notification template.
        /// </summary>
        /// <param name="templateType">Template type.</param>
        /// <param name="task">The task.</param>
        public async Task<string> GetTemplate(NotificationTemplate templateType, TaskDTO task)
        {
            String template = "";

            if (HttpContext.Current.Cache[templateType.ToString()] == null)
            {
                try
                {
                    string templatePath = HttpContext.Current.Server.MapPath("~/App_Data/NotificationTemplates/") + templateType.ToString() + ".html";

                    using (StreamReader reader = File.OpenText(templatePath))
                    {
                        template = await reader.ReadToEndAsync();
                    }

                    CacheDependency dependency =
                        new CacheDependency(templatePath);

                    HttpContext.Current.Cache.Insert(templateType.ToString(), template, dependency);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }
            }
            else
            {
                template = (string)HttpContext.Current.Cache[templateType.ToString()];
            }

            string link = HttpContext.Current.Request.Url.Scheme + Uri.SchemeDelimiter + HttpContext.Current.Request.Url.Host + (HttpContext.Current.Request.Url.IsDefaultPort ? "" : ":" + HttpContext.Current.Request.Url.Port) + "/task/item?id=";

            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(task))
            {
                template = template.Replace("{{" + descriptor.Name + "}}", descriptor.GetValue(task)?.ToString());
            }

            template = template.Replace("{{" + "Link" + "}}", link + task.ID.ToString());

            template = template.Replace("{{" + "ApprovedBy" + "}}", Thread.CurrentPrincipal.Identity.Name.FormatUserName());


            return template;
        }
    }
}
