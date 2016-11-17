namespace WorkflowSample.Engine
{
    public class TravelRequestCapturedStateConfigurator : ITravelRequestStateMachineConfigurator
    {
        public void Configure(TravelRequestStateMachineContext context)
        {
            context.StateMachine.Configure(TravelRequestState.Captured)
                .PermitIf(TravelRequestAction.Submit, TravelRequestState.TravelerReview, () => context.CurrentTravelRequest.IsEmployee)
                .PermitIf(TravelRequestAction.Submit, TravelRequestState.HRApproval, () => !context.CurrentTravelRequest.IsEmployee);
        }
    }
}