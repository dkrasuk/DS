namespace DebtSettlement.BusinessLayer.Models.Enums
{
    public enum CommandNameEnum
    {
        Reject,
        Approve,
        ApproveNewPlannedDate,
        ProposeNewObserverUser,
        ProposeNewResponsibleUser,
        AppointNewObserverUser,
        AppointNewResponsibleUser,
        RescheduleNewPlannedDate,
        CloseTheTask
    }
}