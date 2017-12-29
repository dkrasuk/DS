using WorkflowEngine.Client.Attributes;

namespace DebtSettlement.Model.DTO.ApplicationForm
{
    public class Income
    {
        public double? IncomeValue { get; set; }

        public string SourceOfIncome { get; set; }

        public string SourceInformationOfIncome { get; set; }

        public string SourceOfFundsDS { get; set; }
    }
}
