namespace WorkflowSample.Engine
{
    public class TravelRequestHODApprovalStateConfigurator : ITravelRequestStateMachineConfigurator
    {
        public void Configure(TravelRequestStateMachineContext context)
        {
            context.StateMachine.Configure(TravelRequestState.HODApproval)
                .Permit(TravelRequestAction.Approve, TravelRequestState.BookTickets)
                .OnEntryFrom(TravelRequestAction.Approve, transition =>
                {
                    if (context.CurrentTravelRequest.IsEmployee) context.StateMachine.Fire(TravelRequestAction.Approve);
                });
        }
    }
}