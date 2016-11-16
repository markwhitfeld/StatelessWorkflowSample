namespace WorkflowSample.Engine
{
    public class TravelRequestManagerApprovalStateConfigurator : ITravelRequestStateMachineConfigurator
    {
        public void Configure(TravelRequestStateMachineContext context)
        {
            context.StateMachine.Configure(TravelRequestState.ManagerApproval)
                .Permit(TravelRequestAction.Approve, TravelRequestState.ProcurementApproval);
        }
    }
}