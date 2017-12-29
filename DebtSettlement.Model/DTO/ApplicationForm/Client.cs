using WorkflowEngine.Client.Attributes;

namespace DebtSettlement.Model.DTO.ApplicationForm
{
    public class Client
    {
        public int? PersonId { get; set; }

        public string INN { get; set; }

        public string FIO { get; set; }

        public string Region { get; set; }
        
        public string City { get; set; }
    }
}
