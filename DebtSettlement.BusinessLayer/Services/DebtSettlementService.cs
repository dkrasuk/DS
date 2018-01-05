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
                    City = debtSettlementProcess.City,
                    MacroSegment = debtSettlementProcess.MacroSegment,
                    Portfolio = debtSettlementProcess.Portfolio,
                    SubSegment = debtSettlementProcess.SubSegment
                };

                process.Credit = new Credit()
                {
                    Outstanding = debtSettlementProcess.Outstanding,
                    Fees = debtSettlementProcess.Fees,
                    DSType = debtSettlementProcess.DSType,
                    Principal = debtSettlementProcess.Principal,
                    Interest = debtSettlementProcess.Interest,
                    PurchasePrice = debtSettlementProcess.PurchasePrice,
                    AllPayments = debtSettlementProcess.AllPayments,
                    DPD = debtSettlementProcess.DPD
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

                process.Decision = new Decision()
                {
                    DraftDecision = debtSettlementProcess.PreparationOfaDraftDecision,
                    FinEffect = debtSettlementProcess.FinancialEffect,
                    CashOutstanding = debtSettlementProcess.CashPercentToOutstanding,
                    CashOutstandingLiquidation = debtSettlementProcess.CashPercentToOutstandingLiquidation,
                    CostGIS = debtSettlementProcess.CostInStateInfoSystem,
                    LiquidationValue = debtSettlementProcess.LiquidationValue
                };

                process.Finance = new Finance()
                {
                    CashTotal = debtSettlementProcess.CashTotal,
                    NPV  = debtSettlementProcess.NPV,
                    CashCollateralValue = debtSettlementProcess.CashToCollateralValue,
                    LTV = debtSettlementProcess.LTV,
                    CashToOut = debtSettlementProcess.CashToOut,
                    TermInstallments = debtSettlementProcess.InstallmentPayments,
                    FirstPayment = debtSettlementProcess.FirstPayment,
                    FPValue = debtSettlementProcess.FpToValue,
                    OtherCredits  = debtSettlementProcess.OtherCredits,
                    TypeDS  = debtSettlementProcess.DSType,
                    Reject = debtSettlementProcess.IsRejected,
                    EssenceDeviation  = debtSettlementProcess.CauseOfReject
                };

                process.Status = new Status()
                {
                    HaveAnotherBankProduct = debtSettlementProcess.HaveAnotherBankProduct,
                    LegalStage = debtSettlementProcess.LegalStage,
                    Date = debtSettlementProcess.DateOfLegalStage,
                    LegalStatus = debtSettlementProcess.LegalStatus,
                    StatusOfBankruptcy  = debtSettlementProcess.StatusOfBankruptcy,
                    OpenDateOfBankruptcy = debtSettlementProcess.OpenDateOfBankruptcy,
                    RiskOfLoss  = debtSettlementProcess.RiskOfLoss,
                    DateRiskOfLoss = debtSettlementProcess.DateRiskOfLoss,
                    ActionsPlanned = debtSettlementProcess.ActionResultsOfRisk,
                    CurrentTool = debtSettlementProcess.CurrentTools,
                    Argumentation = debtSettlementProcess.ArgumentationOfDesicion,
                    ConclusionSecurity = debtSettlementProcess.SecurityConclusion,
                    HistoryTalks = debtSettlementProcess.HistoryOfBusinessNegotiations
                };

                process.CollateralOtherParameters = new CollateralOtherParameters()
                {
                    Selected = debtSettlementProcess.Collaterals,
                    CoverOutstanding = debtSettlementProcess.PercentOutstanding,
                    RepaymentAmount = debtSettlementProcess.PaymentSum,
                    OtherAssetsForCollection = debtSettlementProcess.CollectionOtherAssets,
                    CheckEvaluation = debtSettlementProcess.CheckEvaluation,
                    Approve = debtSettlementProcess.Approve
                };

                process.ActiveOtherParameters = new ActiveOtherParameters()
                {
                    ActivesSelected = debtSettlementProcess.Actives,
                    OwnershipShare = debtSettlementProcess.MembershipInterestOfProperty,
                    SourceInformationOfActive = debtSettlementProcess.SourceOfOtherActives,
                    Comment = debtSettlementProcess.CommentOnProperty
                };

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
            result.MacroSegment = applicationForm.Client?.MacroSegment;
            result.Portfolio = applicationForm.Client?.Portfolio;
            result.SubSegment = applicationForm.Client?.SubSegment;

            result.AgreemNumber = applicationForm.AgreementId;
            result.Outstanding = applicationForm.Credit?.Outstanding?.ToString();
            result.Fees = applicationForm.Credit?.Fees?.ToString();
            result.DSType = applicationForm.Credit?.DSType;
            result.Principal = applicationForm.Credit?.Principal;
            result.Interest = applicationForm.Credit?.Interest;
            result.PurchasePrice = applicationForm.Credit?.PurchasePrice;
            result.AllPayments = applicationForm.Credit?.AllPayments;
            result.DPD = applicationForm.Credit?.DPD;

            result.Collaterals = applicationForm.CollateralOtherParameters?.Selected;

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
            result.MembershipInterestOnResidentAddress = applicationForm.Address?.MembershipInterestOnResidentAddress;
            result.MembershipInterestOnRegistrAddress = applicationForm.Address?.MembershipInterestOnRegistrAddress;
            result.ResidentialAddress = applicationForm.Address?.ResidentialAddress;

            result.OtherActives = applicationForm.OtherActives;

            result.Actives = applicationForm.ActiveOtherParameters?.ActivesSelected;

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

            result.PreparationOfaDraftDecision = applicationForm.Decision?.DraftDecision;
            result.FinancialEffect = applicationForm.Decision?.FinEffect;
            result.CashPercentToOutstanding = applicationForm.Decision?.CashOutstanding;
            result.CashPercentToOutstandingLiquidation = applicationForm.Decision?.CashOutstandingLiquidation;
            result.CostInStateInfoSystem = applicationForm.Decision?.CostGIS;
            result.LiquidationValue = applicationForm.Decision?.LiquidationValue;

            result.CashTotal = applicationForm.Finance?.CashTotal;
            result.NPV = applicationForm.Finance?.NPV;
            result.CashToCollateralValue = applicationForm.Finance?.CashCollateralValue;
            result.LTV = applicationForm.Finance?.LTV;
            result.CashToOut = applicationForm.Finance?.CashToOut;
            result.InstallmentPayments = applicationForm.Finance?.TermInstallments;
            result.FirstPayment = applicationForm.Finance?.FirstPayment;
            result.FpToValue = applicationForm.Finance?.FPValue;
            result.OtherCredits = applicationForm.Finance?.OtherCredits;
            result.DSType = applicationForm.Finance?.TypeDS;
            result.IsRejected = applicationForm.Finance?.Reject;
            result.CauseOfReject = applicationForm.Finance?.EssenceDeviation;

            result.HaveAnotherBankProduct = applicationForm.Status?.HaveAnotherBankProduct;
            result.LegalStage = applicationForm.Status?.LegalStage;
            result.DateOfLegalStage = applicationForm.Status?.Date;
            result.LegalStatus = applicationForm.Status?.LegalStatus;
            result.StatusOfBankruptcy = applicationForm.Status?.StatusOfBankruptcy;
            result.OpenDateOfBankruptcy = applicationForm.Status?.OpenDateOfBankruptcy;
            result.RiskOfLoss = applicationForm.Status?.RiskOfLoss;
            result.DateRiskOfLoss = applicationForm.Status?.DateRiskOfLoss;
            result.ActionResultsOfRisk = applicationForm.Status?.ActionsPlanned;
            result.CurrentTools = applicationForm.Status?.CurrentTool;
            result.ArgumentationOfDesicion = applicationForm.Status?.Argumentation;
            result.SecurityConclusion = applicationForm.Status?.ConclusionSecurity;
            result.HistoryOfBusinessNegotiations = applicationForm.Status?.HistoryTalks;

            result.Collaterals = applicationForm.CollateralOtherParameters?.Selected;
            result.PercentOutstanding = applicationForm.CollateralOtherParameters?.CoverOutstanding;
            result.PaymentSum = applicationForm.CollateralOtherParameters?.RepaymentAmount;
            result.CollectionOtherAssets = applicationForm.CollateralOtherParameters?.OtherAssetsForCollection;
            result.CheckEvaluation = applicationForm.CollateralOtherParameters?.CheckEvaluation;
            result.Approve = applicationForm.CollateralOtherParameters?.Approve;

            result.Actives = applicationForm.ActiveOtherParameters?.ActivesSelected;
            result.MembershipInterestOfProperty = applicationForm.ActiveOtherParameters?.OwnershipShare;
            result.SourceOfOtherActives = applicationForm.ActiveOtherParameters?.SourceInformationOfActive;
            result.CommentOnProperty = applicationForm.ActiveOtherParameters?.Comment;

            return result;
        }


        #endregion

    }
}
