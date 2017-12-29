using System.Collections.Generic;
using System.Threading.Tasks;
using DebtSettlement.AgreementLoader.Models;
using DebtSettlement.Model.DTO.ApplicationForm;

namespace DebtSettlement.AgreementLoader.Interface
{
    public interface IAgreementService
    {
        /// <summary>
        /// Gets the agreement.
        /// </summary>
        /// <param name="agreementId">The agreement identifier.</param>
        /// <returns>Agreement.</returns>
        Agreement GetAgreement(int agreementId);

        /// <summary>
        /// Gets the general actions.
        /// </summary>
        /// <returns>IEnumerable&lt;ActionType&gt;.</returns>
        IEnumerable<ActionType> GetGeneralActions();

        /// <summary>
        /// Gets the legal actions.
        /// </summary>
        /// <returns>IEnumerable&lt;ActionType&gt;.</returns>
        IEnumerable<ActionType> GetLegalActions();

        /// <summary>
        /// Gets the responsible persons by agreement.
        /// </summary>
        /// <param name="agreementId">The agreement identifier.</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        IEnumerable<string> GetResponsiblePersonsByAgreement(int agreementId);

        /// <summary>
        /// Gets the responsible persons for agreement processes.
        /// </summary>
        /// <param name="agreementId">The agreement identifier.</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        IEnumerable<string> GetResponsiblePersonsByAgreementProcesses(int agreementId);

        /// <summary>
        /// Gets the responsible person by process.
        /// </summary>
        /// <param name="processId">The process identifier.</param>
        /// <returns>System.String.</returns>
        string GetResponsiblePersonByProcess(int processId);

        /// <summary>
        /// Searches the agreements.
        /// </summary>
        /// <param name="searcherLogin">The searcher login.</param>
        /// <param name="searhTerm">The searh term.</param>
        /// <returns>IEnumerable&lt;SearchResult&gt;.</returns>
        Task<IEnumerable<SearchResult>> SearchAgreements(string searcherLogin, string searhTerm);

        /// <summary>
        /// Application Form Filling 
        /// </summary>
        /// <param name="agreementId"></param>
        /// <returns></returns>
        Task<ApplicationForm> GetApplicationFormFilling(int agreementId);
    }
}
