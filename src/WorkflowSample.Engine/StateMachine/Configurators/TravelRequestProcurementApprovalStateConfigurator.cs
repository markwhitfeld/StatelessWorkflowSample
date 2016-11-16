namespace WorkflowSample.Engine
{
    public class TravelRequestProcurementApprovalStateConfigurator : ITravelRequestStateMachineConfigurator
    {
        public void Configure(TravelRequestStateMachineContext context)
        {
            context.StateMachine.Configure(TravelRequestState.ProcurementApproval)
                .Permit(TravelRequestAction.Approve, TravelRequestState.HODApproval);
        }
    }
}