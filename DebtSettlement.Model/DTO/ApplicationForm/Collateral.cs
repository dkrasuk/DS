using WorkflowEngine.Client.Attributes;

namespace DebtSettlement.Model.DTO.ApplicationForm
{
    public class Collateral
    {
        public string CollateralId { get; set; }

        public string CollateralType { get; set; }

        public string CollateralDescription { get; set; }

        public string CollateralValue { get; set; }

        public string PersentOutstanding { get; set; }

        public string PaymentSum { get; set; }

        public bool? isAdditionalProperty { get; set; }
    }
}
