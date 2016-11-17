using Stateless;

namespace WorkflowSample.Engine
{
    public class TravelRequestWorkflowGeneralConfigurator : ITravelRequestWorkflowConfigurator
    {
        public void Configure(StateMachine<TravelRequestState, TravelRequestAction> stateMachine, TravelRequest travelRequest)
        {
            stateMachine.Configure(TravelRequestState.New)
                .Permit(TravelRequestAction.Init, TravelRequestState.Captured);

            stateMachine.Configure(TravelRequestState.Captured)
                .PermitIf(TravelRequestAction.Submit, TravelRequestState.TravelerReview, () => travelRequest.IsEmployee)
                .PermitIf(TravelRequestAction.Submit, TravelRequestState.HRApproval, () => !travelRequest.IsEmployee);

            stateMachine.Configure(TravelRequestState.TravelerReview)
                .Permit(TravelRequestAction.Accept, TravelRequestState.ManagerApproval);

            stateMachine.Configure(TravelRequestState.HRApproval)
                .Permit(TravelRequestAction.Approve, TravelRequestState.ProcurementApproval);

            stateMachine.Configure(TravelRequestState.ManagerApproval)
                .Permit(TravelRequestAction.Approve, TravelRequestState.ProcurementApproval);

            stateMachine.Configure(TravelRequestState.ProcurementApproval)
                .Permit(TravelRequestAction.Approve, TravelRequestState.HODApproval);

            stateMachine.Configure(TravelRequestState.HODApproval)
                .Permit(TravelRequestAction.Approve, TravelRequestState.BookTickets)
                .OnEntryFrom(TravelRequestAction.Approve, transition =>
                {
                    if (travelRequest.IsEmployee) stateMachine.Fire(TravelRequestAction.Approve);
                });

            stateMachine.Configure(TravelRequestState.BookTickets)
                .Permit(TravelRequestAction.Finish, TravelRequestState.BookingComplete);
        }
    }
}