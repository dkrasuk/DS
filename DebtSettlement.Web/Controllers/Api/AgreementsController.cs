using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using DebtSettlement.AgreementLoader.Interface;
using DebtSettlement.AgreementLoader.Models;

namespace DebtSettlement.Web.Controllers.Api
{
    /// <summary>
    /// Class UsersController.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [Authorize]
    [RoutePrefix("api/agreements")]
    public class AgreementsController : ApiController
    {
        private readonly IAgreementService _agreementService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="agreementService"></param>
        public AgreementsController(IAgreementService agreementService)
        {
            _agreementService = agreementService;
        }

        /// <summary>
        /// Gets the agreement.
        /// </summary>
        /// <param name="agreementId">The agreement identifier.</param>
        /// <returns>IEnumerable&lt;System.Object&gt;.</returns>
        [Route("")]
        [HttpGet]
        public async Task<Agreement> GetAgreement(int agreementId)
        {
            var agreement = await Task.Run(() => _agreementService.GetAgreement(agreementId));

            return agreement;
        }
        /// <summary>
        /// Searches the agreements.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>System.Threading.Tasks.Task&lt;System.Collections.Generic.IEnumerable&lt;DebtSettlement.AgreementLoader.Models.SearchResult&gt;&gt;.</returns>
        [HttpGet]
        [Route("search")]
        public async Task<IEnumerable<SearchResult>> SearchAgreements(string searchTerm)
        {
            var searcherLogin = User.Identity.Name;

            return await _agreementService.SearchAgreements(searcherLogin, searchTerm);
        }

        /// <summary>
        /// Get ection by id
        /// </summary>
        /// <param name="type">legal|general</param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("actions/{type}/{id}")]
        [HttpGet]
        public async Task<ActionType> GetActions(string type, int id)
        {
            if (type.ToLower() == "general")
            {
                return await Task.Run(
                    () => _agreementService.GetGeneralActions().FirstOrDefault(i => i.Id == id));
            }
            if (type.ToLower() == "legal")
            {
                return await Task.Run(
                    () => _agreementService.GetLegalActions().FirstOrDefault(i => i.Id == id));
            }
            return null;
        }

    }
}
