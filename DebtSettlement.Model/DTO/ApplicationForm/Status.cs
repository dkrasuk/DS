using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtSettlement.Model.DTO.ApplicationForm
{
    public class Status
    {
        public bool? HaveAnotherBankProduct { get; set; }
        
        public string LegalStage { get; set; }
       
        public DateTime? Date { get; set; }
     
        public string LegalStatus { get; set; }
       
        public bool? StatusOfBankruptcy { get; set; }
      
        public DateTime? OpenDateOfBankruptcy { get; set; }
      
        public bool? RiskOfLoss { get; set; }
      
        public DateTime? DateRiskOfLoss { get; set; }
        
        public string ActionsPlanned { get; set; }
      
        public string CurrentTool { get; set; }
       
        public string Argumentation { get; set; }
      
        public string ConclusionSecurity { get; set; }
     
        public string HistoryTalks { get; set; }
    }
}
