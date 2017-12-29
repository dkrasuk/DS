using System.Collections.Generic;
using WorkflowEngine.Client.Attributes;

namespace DebtSettlement.BusinessLayer.Models
{
    public class DebtSettlementProcess
    {
        public DebtSettlementProcess()
        {
            Collaterals = new List<string>();
            Actives = new List<string>();
        }

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


        //[PersistanceProcessParameter]
        public List<string> Collaterals { get; set; }


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

        //[PersistanceProcessParameter]
        public List<string> Actives { get; set; }


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
    }
}