using System;

namespace DebtSettlement.AgreementLoader.Models
{
    public class Process
    {
        public int Id { get; set; }

        public int AgreementId { get; set; }

        public int? ProcessType { get; set; }

        public int State { get; set; }

        public string Stage { get; set; }

        public string Status { get; set; }

        public DateTime? DateOpen { get; set; }

        public DateTime? DateClose { get; set; }

        public string Identifier{ get; set; }

        public string Urisdiction { get; set; }

        public string Initiator { get; set; }

        public string Responsible { get; set; }

        public string Name { get; set; }

        public DateTime? ControlDate { get; set; }

        public string NumberDeal { get; set; }

        public int? ControlDateTypeId { get; set; }
    }
}
