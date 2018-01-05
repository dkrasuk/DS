using System;
using System.Collections.Generic;
using WorkflowEngine.Client.Attributes;

namespace DebtSettlement.BusinessLayer.Models
{
    public class DebtSettlementProcess
    {
        [PersistanceProcessParameter]
        public string INN { get; set; }

        [PersistanceProcessParameter]
        public string FIO { get; set; }

        [PersistanceProcessParameter]
        public string Region { get; set; }

        [PersistanceProcessParameter]
        public string City { get; set; }


        [PersistanceProcessParameter]
        public int? AgreemNumber { get; set; }

        [PersistanceProcessParameter]
        public string Portfolio { get; set; }

        [PersistanceProcessParameter]
        public string Outstanding { get; set; }

        [PersistanceProcessParameter]
        public string Fees { get; set; }

        [PersistanceProcessParameter]
        public string DSType { get; set; }


        [PersistanceProcessParameter]
        public string Collaterals { get; set; }


        [PersistanceProcessParameter]
        public string WorkPlace { get; set; }

        [PersistanceProcessParameter]
        public string Position { get; set; }

        [PersistanceProcessParameter]
        public string Source { get; set; }


        [PersistanceProcessParameter]
        public string OtherLiability { get; set; }

        [PersistanceProcessParameter]
        public string SourceOfOtherLiabilities { get; set; }


        [PersistanceProcessParameter]
        public string Incomes { get; set; }

        [PersistanceProcessParameter]
        public string SourceOfIncome { get; set; }

        [PersistanceProcessParameter]
        public string SourceInformationOfIncome { get; set; }

        [PersistanceProcessParameter]
        public string SourceOfFundsDS { get; set; }


        [PersistanceProcessParameter]
        public bool? MatchAddress { get; set; }

        [PersistanceProcessParameter]
        public string RegistrationAddress { get; set; }

        [PersistanceProcessParameter]
        public string MembershipInterestOnResidentAddress { get; set; }

        [PersistanceProcessParameter]
        public string MembershipInterestOnRegistrAddress { get; set; }

        [PersistanceProcessParameter]
        public string ResidentialAddress { get; set; }


        [PersistanceProcessParameter]
        public bool? OtherActives { get; set; }

        [PersistanceProcessParameter]
        public string SourceOfOtherActives { get; set; }

        [PersistanceProcessParameter]
        public string Actives { get; set; }


        [PersistanceProcessParameter]
        public string AbsenceOfDocuments { get; set; }

        [PersistanceProcessParameter]
        public bool? PresenceOfArrest { get; set; }

        [PersistanceProcessParameter]
        public bool? BankruptsyReorganisation { get; set; }

        [PersistanceProcessParameter]
        public bool? DivorceProceedings { get; set; }

        [PersistanceProcessParameter]
        public bool? Heirs { get; set; }

        [PersistanceProcessParameter]
        public bool? AbsenceOfMortgagor { get; set; }

        [PersistanceProcessParameter]
        public bool? JudgementInTheCase { get; set; }

        [PersistanceProcessParameter]
        public bool? DeadlyDisease { get; set; }


        [PersistanceProcessParameter]
        public string HistoryOfBusinessNegotiations { get; set; }

        [PersistanceProcessParameter]
        public string Guarantors { get; set; }


        [PersistanceProcessParameter]
        public double? PaymentSum { get; set; }

        [PersistanceProcessParameter]
        public string Resposnible { get; set; }

        [PersistanceProcessParameter]
        public string Initiator { get; set; }

        [PersistanceProcessParameter]
        public string ResponsibleActors { get; set; }


        [PersistanceProcessParameter]
        public double? PercentOutstanding { get; set; }

        [PersistanceProcessParameter]
        public int? MembershipInterestOfProperty { get; set; }

        [PersistanceProcessParameter]
        public string CommentOnProperty { get; set; }


        [PersistanceProcessParameter]
        public string Authority { get; set; }

        [PersistanceProcessParameter]
        public int? DPD { get; set; }

        [PersistanceProcessParameter]
        public double? Principal { get; set; }

        [PersistanceProcessParameter]
        public double? PurchasePrice { get; set; }


        [PersistanceProcessParameter]
        public bool? CheckEvaluation { get; set; }

        [PersistanceProcessParameter]
        public bool? Approve { get; set; }


        [PersistanceProcessParameter]
        public bool? HaveAnotherBankProduct { get; set; }

        [PersistanceProcessParameter]
        public string StatusAndTypesOfAnotherBankProduct { get; set; }

        [PersistanceProcessParameter]
        public string LegalStage { get; set; }

        [PersistanceProcessParameter]
        public DateTime? DateOfLegalStage { get; set; }

        [PersistanceProcessParameter]
        public string LegalStatus { get; set; }

        [PersistanceProcessParameter]
        public bool? StatusOfBankruptcy { get; set; }

        [PersistanceProcessParameter]
        public DateTime? OpenDateOfBankruptcy { get; set; }

        [PersistanceProcessParameter]
        public bool? RiskOfLoss { get; set; }

        [PersistanceProcessParameter]
        public DateTime? DateRiskOfLoss { get; set; }

        [PersistanceProcessParameter]
        public string ActionResultsOfRisk { get; set; }

        [PersistanceProcessParameter]
        public string CurrentTools { get; set; }

        [PersistanceProcessParameter]
        public bool? IsRejected { get; set; }

        [PersistanceProcessParameter]
        public string CauseOfReject { get; set; }


        [PersistanceProcessParameter]
        public string SecurityConclusion { get; set; }

        [PersistanceProcessParameter]
        public string PreparationOfaDraftDecision { get; set; }

        [PersistanceProcessParameter]
        public string ArgumentationOfDesicion { get; set; }

        [PersistanceProcessParameter]
        public double? CashTotal { get; set; }

        [PersistanceProcessParameter]
        public double? FinancialEffect { get; set; }

        [PersistanceProcessParameter]
        public double? CashPercentToOutstanding { get; set; }

        [PersistanceProcessParameter]
        public double? CashPercentToOutstandingLiquidation { get; set; }

        [PersistanceProcessParameter]
        public double? CostInStateInfoSystem { get; set; }

        [PersistanceProcessParameter]
        public double? LiquidationValue { get; set; }


        [PersistanceProcessParameter]
        public string MacroSegment { get; set; }

        [PersistanceProcessParameter]
        public string SubSegment { get; set; }

        [PersistanceProcessParameter]
        public double? Interest { get; set; }

        [PersistanceProcessParameter]
        public double? AllPayments { get; set; }

        [PersistanceProcessParameter]
        public double? NPV { get; set; }

        [PersistanceProcessParameter]
        public double? CashToCollateralValue { get; set; }

        [PersistanceProcessParameter]
        public double? LTV { get; set; }

        [PersistanceProcessParameter]
        public double? CashToOut { get; set; }

        [PersistanceProcessParameter]
        public double? FirstPayment { get; set; }

        [PersistanceProcessParameter]
        public double? FpToValue { get; set; }

        [PersistanceProcessParameter]
        public bool? OtherCredits { get; set; }

        [PersistanceProcessParameter]
        public bool? CollectionOtherAssets { get; set; }

        [PersistanceProcessParameter]
        public double? InstallmentPayments { get; set; }

    }
}