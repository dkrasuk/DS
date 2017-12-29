using System.Threading.Tasks;
using DebtSettlement.BusinessLayer.Models;
using DebtSettlement.BusinessLayer.Services.Interfaces;
using DebtSettlement.Model.DTO;
using DebtSettlement.Model.Enums;
using System.Linq;

namespace DebtSettlement.BusinessLayer.Services.Notifications
{
    public abstract class NotificationsBaseService : INotificationsService
    {
        private string notificationChannelName = "DebtSettlement";
        
        private readonly INotificationTemplateService _templateService;

        protected NotificationsBaseService(INotificationTemplateService templateService)
        {
            _templateService = templateService;
        }

        public async Task SendAsync(string receiverLogin, string notificationBody)
        {
            var notification = new Notification
            {
                Body = notificationBody,
                Receiver = receiverLogin,
                Channel = notificationChannelName,
                Type = "info",
                Title = " "
            };
            await SendMessageToService(notification);            
        }

        public async Task SendAsync(string[] receiverLoginsList, string notificationBody)
        {
            if (receiverLoginsList != null)
            {
                var recivers = receiverLoginsList
                    .Where(i => !string.IsNullOrEmpty(i))
                    .Select(i => i.Trim())
                    .Distinct()
                    .ToArray();

                for (int i = 0; i < recivers.Length; i++)
                {
                    await SendAsync(recivers[i], notificationBody);
                }
            }
        }  
        
        
        public async Task SendAsync(NotificationTemplate type, string receiverLogin, TaskDTO task)
        {
            var notificationBody = await _templateService.GetTemplate(type, task);
            
            await SendAsync(receiverLogin, notificationBody);
        }

        public async Task SendAsync(NotificationTemplate type, string[] receiverLoginList, TaskDTO task)
        {           
            var notificationBody = await _templateService.GetTemplate(type, task);            
            await SendAsync(receiverLoginList, notificationBody);
        }

        protected abstract Task SendMessageToService(Notification notification);
    }
}