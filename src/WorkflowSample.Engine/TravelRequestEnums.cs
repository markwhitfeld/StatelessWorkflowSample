namespace WorkflowSample.Engine
{
    public enum TravelRequestTransition
    {
        Init = 1,
        Submit
    }

    public enum TravelRequestState
    {
        New = 0,
        Captured,
        TravelerReview,
        HRApproval
    }
}