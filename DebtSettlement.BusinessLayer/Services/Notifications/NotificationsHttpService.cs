using AlfaBank.Logger;
using RestSharp;
using System;
using System.Threading.Tasks;
using AlfaBank.Common.Interfaces;
using DebtSettlement.BusinessLayer.Models;
using DebtSettlement.BusinessLayer.Services.Interfaces;

namespace DebtSettlement.BusinessLayer.Services.Notifications
{
    public class NotificationsHttpService : NotificationsBaseService
    {
        private readonly ILogger _logger;
        private readonly IApiExecuter _apiExecuter;
        
        public NotificationsHttpService(
            INotificationTemplateService templateService, 
            IApiExecuter apiExecuter,
            ILogger logger) 
            : base(templateService)
        {
            _apiExecuter = apiExecuter;
            _logger = logger;
        }
        
        protected override async Task SendMessageToService(Notification notification)
        {
            var notificationsApiUrl = AppSettings.NotificationsServiceUrl;
            if (string.IsNullOrEmpty(notificationsApiUrl))
            {
                string errorMessage = "Notifications Http error: NotificationsServiceUrl setting is empty";
                _logger.Error(errorMessage);
                throw new Exception(errorMessage);
            }
            
            var url = notificationsApiUrl + (notificationsApiUrl.EndsWith("/") ? "": "/") + "notifications";
            try
            {
                await _apiExecuter.ExecuteAsync<Notification>(url, Method.POST, notification);
            }
            catch (Exception e)
            {
                _logger.Error("Send notification error", e);
            }
        }
    }
}