using AlfaBank.Logger;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Threading.Tasks;
using DebtSettlement.BusinessLayer.Models;
using DebtSettlement.BusinessLayer.Services.Interfaces;

namespace DebtSettlement.BusinessLayer.Services.Notifications
{
    public class NotificationsServiceBusService : NotificationsBaseService
    {       
        private readonly ILogger _logger;
        
        public NotificationsServiceBusService(INotificationTemplateService templateService, ILogger logger) 
            : base(templateService)
        {
            _logger = logger;
        }

        protected override async Task SendMessageToService(Notification notification)
        {
            var connectionString = ConfigurationManager.AppSettings.Get("ServiceBusConnectionString");
            var topicName = ConfigurationManager.AppSettings.Get("ServiceBusNotificationsTopic");

            if (string.IsNullOrEmpty(connectionString))
            {
                string errorMessage = "Service Bus error: ServiceBusConnectionString setting is empty";
                _logger.Error(errorMessage);
                throw new Exception(errorMessage);
            }
            
            if (string.IsNullOrEmpty(topicName))
            {
                string errorMessage = "Service Bus error: ServiceBusNotificationsTopic setting is empty";
                _logger.Error(errorMessage);
                throw new Exception(errorMessage);
            }            
            
            var client = TopicClient.CreateFromConnectionString(connectionString, topicName);
            var message = new BrokeredMessage(JsonConvert.SerializeObject(notification));
            try
            {
                await client.SendAsync(message);
            }
            catch (Exception e)
            {
                _logger.Error($"Send message to Service Bus error (topicName: {topicName})", e);
                throw;
            }
        }
    }
}