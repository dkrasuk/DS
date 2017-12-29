using AlfaBank.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DebtSettlement.AgreementLoader.Interface;
using DebtSettlement.BusinessLayer.Services.Interfaces;
using WorkflowEngine.Client.Converters.Interfaces;
using WorkflowEngine.Client.Interfaces;
using AlfaBank.Common.Utils;
using AlfaBank.Common.Interfaces;
using DebtSettlement.Model.Dictionary;
using DebtSettlement.Model.DTO.ApplicationForm;
using DebtSettlement.BusinessLayer.Models;
using Newtonsoft.Json.Linq;

namespace DebtSettlement.BusinessLayer.Services
{
    public class DebtSettlementService : IDebtSettlementService
    {
        private readonly IWorkflowEngineService _workflowEngineService;
        private readonly IParametersConverter _parametersConverter;
        private readonly ILogger _logger;
        private readonly IHRService _hrService;
        private readonly IAgreementService _agreementService;
        private readonly INotificationsService _notificationsService;
        private readonly ISmtpService _smtpService;
        private readonly INotificationTemplateService _notifyTemplateService;
        private readonly IApiExecuter _apiExecuter;

        public const int MaxTitleLength = 255;
        public const int MaxDescriptionLength = 4000;
        private const string AutocloseParameterName = "Autocompleted";

        public DebtSettlementService(
            ILogger logger,
            IWorkflowEngineService workflowEngineService,
            IParametersConverter parametersConverter,
            IAgreementService agreementService,
            IHRService hrService,
            INotificationsService notificationsService,
            ISmtpService smtpService,
            INotificationTemplateService notifyTemplateService,
            IApiExecuter apiExecuter)
        {
            _logger = logger;

            _workflowEngineService = workflowEngineService;
            _workflowEngineService.ApiUri = AppSettings.WorkflowWebAPIAddress;

            _parametersConverter = parametersConverter;
            _agreementService = agreementService;
            _hrService = hrService;
            _notificationsService = notificationsService;
            _smtpService = smtpService;
            _notifyTemplateService = notifyTemplateService;
            _apiExecuter = apiExecuter;
        }
        /// <summary>
        /// GetDictionaryByName get Value by ValueId
        /// </summary>
        /// <param name="dictionaryName"></param>
        /// <param name="idValue"></param>
        /// <returns></returns>
        private async Task<string> GetDictionaryByName(string dictionaryName, int? idValue)
        {
            if (idValue == null)
            {
                return null;
            }
            Dictionary dictionary = await _apiExecuter.ExecuteAsync<Dictionary>(
                AppSettings.DictionaryAPIAddress + $"{dictionaryName}/1");
            string result = dictionary?.Items?.FirstOrDefault(i => i.ValueId == idValue.ToString())?.Value?.Name;
            return result;
        }

        /// <summary>
        /// Create application form.
        /// </summary>
        /// <param name="applicationForm">Application form.</param>
        public async Task<Guid> CreateDebtSettlement(ApplicationForm applicationForm)
        {
            _logger.Info($"DebtSettlement.CreateApplicationForm [agreementId: {applicationForm?.AgreementId}]");

            if (applicationForm == null)
            {
                throw new DebtSettlementException("Input application form is null", _logger);
            }

            var processId = Guid.NewGuid();

            List<string> Responsibles = new List<string>();
            Responsibles.Add(Thread.CurrentPrincipal.Identity.Name.FormatUserName());

            var applicationFormProcessInstanceParameters = MapAplicationForm(applicationForm);

            var result = GeneralApplicationFormValidation(applicationFormProcessInstanceParameters);

            if (!string.IsNullOrWhiteSpace(result))
            {
                throw new DebtSettlementException(result, _logger);
            }

            await _workflowEngineService.CreateProcessAsync(
                AppSettings.DefaultWorkflowSchemeCode,
                processId,
                Thread.CurrentPrincipal.Identity.Name.FormatUserName(),
                _parametersConverter.CreateProcessInstanceParameters(applicationFormProcessInstanceParameters));

            return processId;
        }

        /// <summary>
        /// Gets DS processes.
        /// </summary>
        /// <returns></returns>
        public async Task<IQueryable<ApplicationForm>> GetDebtSettlementProcessesAsQueryable()
        {
            _logger.Info("DebtSettlementService.GetDebtSettlementProcesses");

            var result = MapAplicationFormList(_workflowEngineService.GetDebtSettlementProcessesAsQueryable().ToList());

            return result;
        }

        private static string GeneralApplicationFormValidation(DebtSettlementProcess applicationForm)
        {
            if (applicationForm == null)
            {
                return "Объект 'Анкета инициатора' пуст";
            }

            return null;
        }

        public async Task<ApplicationForm> GetAgreementInfo(int agreementId)
        {
            var agreementDto = await _agreementService.GetApplicationFormFilling(agreementId);
            return agreementDto;
        }


        /*
 В view, надо добавить такие поля:
- Дата входа в этап;
- Сума погашение;
- Инициатор;
- Ответственный;
- Ответственный сотрудник ОРК;
- Ответственное подразделение (это массив с названиями ролей (Actor), которые могут выполнять комманды из данного Activity
*/


        #region Mappings

        /// <summary>
        /// Create application form.
        /// </summary>
        /// <param name="applicationForm">Application form.</param>
        private IQueryable<ApplicationForm> MapAplicationFormList(List<WorkflowEngine.Client.Models.DebtSettlement.DebtSettlementProcess> debtSettlementProcesses)
        {
            List<ApplicationForm> result = new List<ApplicationForm>();

            foreach (var debtSettlementProcess in debtSettlementProcesses)
            {
                if (debtSettlementProcess == null)
                    continue;

                ApplicationForm process = new ApplicationForm();

                process.Id = debtSettlementProcess.Id;

                process.AgreementId = debtSettlementProcess.AgreemNumber;

                process.Client = new Client()
                {
                    INN = debtSettlementProcess.INN,
                    FIO = debtSettlementProcess.FIO,
                    Region = debtSettlementProcess.Region,
                    City = debtSettlementProcess.City
                };

                process.Credit = new Credit()
                {
                    Portfolio = debtSettlementProcess.Portfolio,
                    Outstanding = debtSettlementProcess.Outstanding,
                    Fees = debtSettlementProcess.Fees,
                    DSType = debtSettlementProcess.DSType
                };

                process.Job = new Job()
                {
                    WorkPlace = debtSettlementProcess.WorkPlace,
                    Position = debtSettlementProcess.Position,
                    Source = debtSettlementProcess.Source
                };

                process.Liability = new Liability()
                {
                    OtherLiability = debtSettlementProcess.OtherLiability,
                    SourceOfOtherLiability = debtSettlementProcess.SourceOfOtherLiabilities
                };

                process.Income = new Income()
                {
                    IncomeValue = debtSettlementProcess.Incomes,
                    SourceOfIncome = debtSettlementProcess.SourceOfIncome,
                    SourceInformationOfIncome = debtSettlementProcess.SourceInformationOfIncome,
                    SourceOfFundsDS = debtSettlementProcess.SourceOfFundsDS
                };

                process.Address = new Address()
                {
                    MatchAddress = debtSettlementProcess.MatchAddress,
                    RegistrationAddress = debtSettlementProcess.RegistrationAddress,
                    MembershipInterestOnResidentAddress = debtSettlementProcess.MembershipInterestOnResidentAddress,
                    MembershipInterestOnRegistrAddress = debtSettlementProcess.MembershipInterestOnRegistrAddress,
                    ResidentialAddress = debtSettlementProcess.ResidentialAddress
                };

                process.OtherActives = debtSettlementProcess.OtherActives;

                process.SourceOfOtherActives = debtSettlementProcess.SourceOfOtherActives;

                //result.Collaterals = debtSettlementProcess.Collaterals;

                process.ReasonToDenyDS = new ReasonToDenyDS()
                {
                    AbsenceOfDocuments = debtSettlementProcess.AbsenceOfDocuments,
                    PresenceOfArrest = debtSettlementProcess.PresenceOfArrest,
                    BankruptsyReorganisation = debtSettlementProcess.BankruptsyReorganisation,
                    DivorceProceedings = debtSettlementProcess.DivorceProceedings,
                    Heirs = debtSettlementProcess.Heirs,
                    AbsenceOfMortgagor = debtSettlementProcess.AbsenceOfMortgagor,
                    JudgementInTheCase = debtSettlementProcess.JudgementInTheCase,
                    DeadlyDisease = debtSettlementProcess.DeadlyDisease
                };

                process.HistoryOfBusinessNegotiations = debtSettlementProcess.HistoryOfBusinessNegotiations;

                process.Guarantors = debtSettlementProcess.Guarantors;

                //result.Actives = debtSettlementProcess.Actives;

                result.Add(process);
            }

            return result.AsQueryable();
        }

        /// <summary>
        /// Create application form.
        /// </summary>
        /// <param name="applicationForm">Application form.</param>
        private DebtSettlementProcess MapAplicationForm(ApplicationForm applicationForm)
        {
            if (applicationForm == null)
                return null;

            DebtSettlementProcess result = new DebtSettlementProcess();

            result.INN = applicationForm.Client?.INN;
            result.FIO = applicationForm.Client?.FIO;
            result.Region = applicationForm.Client?.Region;
            result.City = applicationForm.Client?.City;

            result.AgreemNumber = applicationForm.AgreementId;
            result.Portfolio = applicationForm.Credit?.Portfolio;
            result.Outstanding = applicationForm.Credit?.Outstanding?.ToString();
            result.Fees = applicationForm.Credit?.Fees?.ToString();
            result.DSType = applicationForm.Credit?.DSType;

            result.Collaterals = applicationForm.Collaterals.Select(i => i.CollateralId).ToList();

            result.WorkPlace = applicationForm.Job?.WorkPlace;
            result.Position = applicationForm.Job?.Position;
            result.Source = applicationForm.Job?.Source;

            result.OtherLiability = applicationForm.Liability?.OtherLiability;
            result.SourceOfOtherLiabilities = applicationForm.Liability?.SourceOfOtherLiability;

            result.Incomes = applicationForm.Income?.IncomeValue?.ToString();
            result.SourceOfIncome = applicationForm.Income?.SourceOfIncome;
            result.SourceInformationOfIncome = applicationForm.Income?.SourceInformationOfIncome;
            result.SourceOfFundsDS = applicationForm.Income?.SourceOfFundsDS;

            result.MatchAddress = applicationForm.Address?.MatchAddress;
            result.RegistrationAddress = applicationForm.Address?.RegistrationAddress;
            result.MembershipInterestOnResidentAddress = applicationForm.Address?.MembershipInterestOnResidentAddress.ToString();
            result.MembershipInterestOnRegistrAddress = applicationForm.Address?.MembershipInterestOnRegistrAddress.ToString();
            result.ResidentialAddress = applicationForm.Address?.ResidentialAddress;

            result.OtherActives = applicationForm.OtherActives;
            result.SourceOfOtherActives = applicationForm.SourceOfOtherActives;
            result.Actives = applicationForm.Actives.Select(i => i.CollateralId).ToList();

            result.AbsenceOfDocuments = applicationForm.ReasonToDenyDS?.AbsenceOfDocuments;
            result.PresenceOfArrest = applicationForm.ReasonToDenyDS?.PresenceOfArrest;
            result.BankruptsyReorganisation = applicationForm.ReasonToDenyDS?.BankruptsyReorganisation;
            result.DivorceProceedings = applicationForm.ReasonToDenyDS?.DivorceProceedings;
            result.Heirs = applicationForm.ReasonToDenyDS?.Heirs;
            result.AbsenceOfMortgagor = applicationForm.ReasonToDenyDS?.AbsenceOfMortgagor;
            result.JudgementInTheCase = applicationForm.ReasonToDenyDS?.JudgementInTheCase;
            result.DeadlyDisease = applicationForm.ReasonToDenyDS?.DeadlyDisease;

            result.HistoryOfBusinessNegotiations = applicationForm.HistoryOfBusinessNegotiations;
            result.Guarantors = applicationForm.Guarantors;


            return result;
        }


        #endregion

    }
}
