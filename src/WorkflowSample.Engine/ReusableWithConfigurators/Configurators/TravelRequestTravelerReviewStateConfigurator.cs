namespace WorkflowSample.Engine
{
    public class TravelRequestTravelerReviewStateConfigurator : IReusableTravelRequestStateMachineConfigurator
    {
        public void Configure(TravelRequestStateMachineContext context)
        {
            context.StateMachine.Configure(TravelRequestState.TravelerReview)
                .Permit(TravelRequestAction.Accept, TravelRequestState.ManagerApproval);
        }
    }
}