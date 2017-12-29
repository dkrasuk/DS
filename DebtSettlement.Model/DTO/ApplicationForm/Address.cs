using WorkflowEngine.Client.Attributes;

namespace DebtSettlement.Model.DTO.ApplicationForm
{
    public class Address
    {
        public bool? MatchAddress { get; set; }

        public string RegistrationAddress { get; set; }

        public string MembershipInterestOnResidentAddress { get; set; }

        public string MembershipInterestOnRegistrAddress { get; set; }

        public string ResidentialAddress { get; set; }
    }
}
