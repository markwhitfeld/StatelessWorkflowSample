namespace WorkflowSample.Engine
{
    public class TravelRequestHRApprovalStateConfigurator : IReusableTravelRequestStateMachineConfigurator
    {
        public void Configure(TravelRequestStateMachineContext context)
        {
            context.StateMachine.Configure(TravelRequestState.HRApproval)
                .Permit(TravelRequestAction.Approve, TravelRequestState.ProcurementApproval);
        }
    }
}