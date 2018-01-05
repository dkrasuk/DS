using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtSettlement.Model.DTO.ApplicationForm
{
    public class CollateralOtherParameters
    {
        
        public string Selected { get; set; }
        
        public double? CoverOutstanding { get; set; }
        
        public double? RepaymentAmount { get; set; }
      
        public bool? OtherAssetsForCollection { get; set; }
       
        public bool? CheckEvaluation { get; set; }
        
        public bool? Approve { get; set; }
    }
}
