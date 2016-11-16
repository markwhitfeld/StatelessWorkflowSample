namespace WorkflowSample.Engine
{
    public enum TravelRequestAction
    {
        Init = 1,
        Submit,
        Accept,
        Approve
    }

    public enum TravelRequestState
    {
        New = 0,
        Captured = 1,
        TravelerReview,
        HRApproval,
        ManagerApproval,
        ProcurementApproval,
        HODApproval,
        BookTickets
    }
}