using System;
using System.Collections.Generic;

namespace DebtSettlement.BusinessLayer.Models
{
    public class AgreementAction
    {
        public int? Id { get; set; }
        public int? ActionTypeId { get; set; }        
        public int? ProcessId { get; set; }
        public int? AgreementId { get; set; }
        public string Comment { get; set; }
        public DateTime? CreateDate { get; set; }
        public string Initiator { get; set; }
        public bool IsLegal { get; set; }
        public DateTime? ActionDate { get; set; }

        public List<AgreementActionParameter> Parameters { get; set; }
    }
}