using AutoMapper;
using AlfaBank.Logger;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using DebtSettlement.AgreementLoader.Interface;
using DebtSettlement.BusinessLayer.Services.Interfaces;
using DebtSettlement.Model.DTO.ApplicationForm;
using DebtSettlement.Web.Models.ApplicationForm;
using DebtSettlement.BusinessLayer;
using System.Linq;

namespace DebtSettlement.Web.Controllers.Api
{
    /// <summary>
    /// Class DebtSettlementController.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    //[Authorize]
    //[ApiAuthorizeFilter(Role = Common.Constants.Roles.DebtSettlementUser)]
    [RoutePrefix("api/debtsettlements")]
    public class DebtSettlementController : ApiController
    {
        private readonly IDebtSettlementService _debtSettlementService;
        private readonly IHRService _hrService;
        private readonly IAgreementService _agreementService;
        private readonly ILogger _logger;
        
        private const string SuccessCreationResult = "Success";
        private const string ErrorCreationResult = "Error";
        private const string AutomaticValue = "auto";

        /// <summary>
        /// Initializes a new instance of the <see cref="DebtSettlementController" /> class.
        /// </summary>
        /// <param name="debtSettlementService">The task service.</param>
        /// <param name="hrService">The hr service.</param>
        /// <param name="agreementService">The agreement service.</param>
        /// <param name="logger">The logger.</param>
        public DebtSettlementController(
            IDebtSettlementService debtSettlementService,
            IHRService hrService,
            IAgreementService agreementService,
            ILogger logger)
        {
            _debtSettlementService = debtSettlementService;
            _hrService = hrService;
            _agreementService = agreementService;
            _logger = logger;
        }

        /// <summary>
        /// Creates the DebtSettlement.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task;.</returns>
        [Route("")]
        [HttpPost]
        public async Task<Guid> CreateDebtSettlement(ApplicationFormViewModel model)
        {
            Guid result;

            var modelDto = Mapper.Map<ApplicationFormViewModel, ApplicationForm>(model);

            try
            {
                result = await _debtSettlementService.CreateDebtSettlement(modelDto);
            }
            catch (Exception ex)
            {
                throw new DebtSettlementException($"DebtSettlement.CreateDebtSettlement [agreementId: {model?.AgreementId} exception: {ex.Message }]", _logger);
            }

            return result;
        }

        /// <summary>
        /// Gets all DebtSettlement processes.
        /// </summary>
        /// <returns>Task;.</returns>
        [Route("")]
        [HttpGet]
        public async Task<IQueryable> GetDebtSettlementProcessesAsQueryable()
        {
            return await _debtSettlementService.GetDebtSettlementProcessesAsQueryable();
        }
    }
}