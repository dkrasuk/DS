using System.Threading.Tasks;
using DebtSettlement.Model.DTO;
using DebtSettlement.Model.Enums;

namespace DebtSettlement.BusinessLayer.Services.Interfaces
{
    public interface INotificationTemplateService
    {
        /// <summary>
        /// Get the specified notification template.
        /// </summary>
        /// <param name="templateType">Template type.</param>
        /// <param name="task">The task.</param>
        Task<string> GetTemplate(NotificationTemplate templateType, TaskDTO task);
    }
}
