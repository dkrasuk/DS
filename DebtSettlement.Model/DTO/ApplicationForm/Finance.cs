using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtSettlement.Model.DTO.ApplicationForm
{
    public class Finance
    {
        public double? CashTotal { get; set; }
      
        public double? NPV { get; set; }

        public double? CashCollateralValue { get; set; }
       
        public double? LTV { get; set; }
     
        public double? CashToOut { get; set; }
      
        public double? TermInstallments { get; set; }
      
        public double? FirstPayment { get; set; }
      
        public double? FPValue { get; set; }
       
        public bool? OtherCredits { get; set; }
       
        public string TypeDS { get; set; }
      
        public bool? Reject { get; set; }

        public string EssenceDeviation { get; set; }
     
    }
}
