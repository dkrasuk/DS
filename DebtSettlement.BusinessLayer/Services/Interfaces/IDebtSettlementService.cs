using System;
using System.Threading.Tasks;
using DebtSettlement.Model.DTO.ApplicationForm;
using System.Linq;

namespace DebtSettlement.BusinessLayer.Services.Interfaces
{
    public interface IDebtSettlementService
    {
        /// <summary>
        /// Creates the specified Debt Settlement process.
        /// </summary>
        /// <param name="applicationForm">The application form.</param>
        Task<Guid> CreateDebtSettlement(ApplicationForm applicationForm);

        /// <summary>
        /// Gets DebtSettlement processes
        /// </summary>
        /// <returns></returns>
        Task<IQueryable<ApplicationForm>> GetDebtSettlementProcessesAsQueryable();

        /// <summary>
        /// GetAgreementInfo by AgreementId
        /// </summary>
        /// <param name="agreementId"></param>
        /// <returns></returns>
        Task<ApplicationForm> GetAgreementInfo(int agreementId);
    }
}
