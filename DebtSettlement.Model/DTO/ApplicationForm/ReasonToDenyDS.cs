using WorkflowEngine.Client.Attributes;

namespace DebtSettlement.Model.DTO.ApplicationForm
{
    public class ReasonToDenyDS
    {
        public string AbsenceOfDocuments { get; set; }

        public bool? PresenceOfArrest { get; set; }

        public bool? BankruptsyReorganisation { get; set; }

        public bool? DivorceProceedings { get; set; }

        public bool? Heirs { get; set; }

        public bool? AbsenceOfMortgagor { get; set; }

        public bool? JudgementInTheCase { get; set; }

        public bool? DeadlyDisease { get; set; }
    }
}
