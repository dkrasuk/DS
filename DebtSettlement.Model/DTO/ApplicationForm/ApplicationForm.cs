using System;
using System.Collections.Generic;
using WorkflowEngine.Client.Attributes;

namespace DebtSettlement.Model.DTO.ApplicationForm
{
    public class ApplicationForm
    {
        public ApplicationForm()
        {
            Collaterals = new List<Collateral>();
            Actives = new List<Collateral>();
            Credit = new Credit();
        }

        public Guid? Id { get; set; }

        public int? AgreementId { get; set; }


        public bool IsClientVerified { get; set; }

        public Client Client { get; set; }


        public bool IsCreditVerified { get; set; }

        public Credit Credit { get; set; }


        public bool IsCollateralsVerified { get; set; }

        public List<Collateral> Collaterals { get; set; }


        public bool IsJobsVerified { get; set; }

        public Job Job { get; set; }


        public bool IsLiabilitiesVerified { get; set; }

        public Liability Liability { get; set; }


        public bool IsIncomesVerified { get; set; }

        public Income Income { get; set; }


        public bool IsAddressesVerified { get; set; }

        public Address Address { get; set; }


        public bool IsActivesVerified { get; set; }

        public bool? OtherActives { get; set; }

        public string SourceOfOtherActives { get; set; }

        public List<Collateral> Actives { get; set; }


        public bool IsReasonToDenyDSVerified { get; set; }

        public ReasonToDenyDS ReasonToDenyDS { get; set; }


        public bool IsHistoryOfBusinessNegotiationsVerified { get; set; }

        public string HistoryOfBusinessNegotiations { get; set; }


        public bool IsGuarantorsVerified { get; set; }

        public string Guarantors { get; set; }
    }
}