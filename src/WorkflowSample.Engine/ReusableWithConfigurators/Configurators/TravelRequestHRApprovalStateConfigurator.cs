namespace WorkflowSample.Engine
{
    public class TravelRequestHRApprovalStateConfigurator : ITravelRequestStateMachineConfigurator
    {
        public void Configure(TravelRequestStateMachineContext context)
        {
            context.StateMachine.Configure(TravelRequestState.HRApproval)
                .Permit(TravelRequestAction.Approve, TravelRequestState.ProcurementApproval);
        }
    }
}