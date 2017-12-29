using System.Threading.Tasks;
using DebtSettlement.Model.DTO;
using DebtSettlement.Model.Enums;

namespace DebtSettlement.BusinessLayer.Services.Interfaces
{
    public interface INotificationsService
    {
        Task SendAsync(string receiverLogin, string notificationBody);
        Task SendAsync(string[] receiverLoginsList, string notificationBody);
        
        Task SendAsync(NotificationTemplate type, string receiverLogin, TaskDTO task);
        Task SendAsync(NotificationTemplate type, string[] receiverLoginList, TaskDTO task);
    }
}