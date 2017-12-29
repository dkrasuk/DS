using System.Threading.Tasks;

namespace DebtSettlement.BusinessLayer.Services.Interfaces
{
    public interface ISmtpService
    {
        Task SendAsync(string[] emailsTo, string body);
        Task SendAsync(string emailTo, string body);
    }
}