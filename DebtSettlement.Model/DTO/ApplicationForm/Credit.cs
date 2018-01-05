using System.Collections.Generic;
using WorkflowEngine.Client.Attributes;

namespace DebtSettlement.Model.DTO.ApplicationForm
{
    public class Credit
    {
        public Credit()
        {
            AgreemNumber = new List<int>();
        }

        public List<int> AgreemNumber { get; set; }

        public double? Outstanding { get; set; }
       
        public double? Principal { get; set; }
      
        public double? Interest { get; set; }
      
        public double? PurchasePrice { get; set; }
     
        public double? AllPayments { get; set; }

        public double? Fees { get; set; }

        public string DSType { get; set; }

        public int? DPD { get; set; }
    }

}
