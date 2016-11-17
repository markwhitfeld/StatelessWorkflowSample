using System;
using Stateless;

namespace WorkflowSample.Engine
{
    public class TravelRequestWorkflowGeneralConfigurator : ITravelRequestWorkflowConfigurator
    {
        private readonly IUserSecurityContext _userSecurityContext;

        public TravelRequestWorkflowGeneralConfigurator(IUserSecurityContext userSecurityContext)
        {
            if (userSecurityContext == null) throw new ArgumentNullException(nameof(userSecurityContext));
            _userSecurityContext = userSecurityContext;
        }

        public void Configure(StateMachine<TravelRequestState, TravelRequestAction> stateMachine, TravelRequest travelRequest)
        {
            stateMachine.Configure(TravelRequestState.New)
                .Permit(TravelRequestAction.Init, TravelRequestState.Captured);

            stateMachine.Configure(TravelRequestState.Captured)
                .PermitIf(TravelRequestAction.Submit, TravelRequestState.TravelerReview, () => travelRequest.IsEmployee)
                .PermitIf(TravelRequestAction.Submit, TravelRequestState.HRApproval, () => !travelRequest.IsEmployee);

            stateMachine.Configure(TravelRequestState.TravelerReview)
                .PermitIf(TravelRequestAction.Accept, TravelRequestState.ManagerApproval, () => travelRequest.Traveller == _userSecurityContext.CurrentUser);

            stateMachine.Configure(TravelRequestState.HRApproval)
                .PermitIf(TravelRequestAction.Approve, TravelRequestState.ProcurementApproval, () => travelRequest.Approver == _userSecurityContext.CurrentUser);

            stateMachine.Configure(TravelRequestState.ManagerApproval)
                .PermitIf(TravelRequestAction.Approve, TravelRequestState.ProcurementApproval, () => travelRequest.Approver == _userSecurityContext.CurrentUser);

            stateMachine.Configure(TravelRequestState.ProcurementApproval)
                .PermitIf(TravelRequestAction.Approve, TravelRequestState.HODApproval, () => travelRequest.Approver == _userSecurityContext.CurrentUser);

            stateMachine.Configure(TravelRequestState.HODApproval)
                .PermitIf(TravelRequestAction.Approve, TravelRequestState.BookTickets, () => travelRequest.Approver == _userSecurityContext.CurrentUser)
                .OnEntryFrom(TravelRequestAction.Approve, transition =>
                {
                    if (travelRequest.IsEmployee) stateMachine.Fire(TravelRequestAction.Approve);
                });

            stateMachine.Configure(TravelRequestState.BookTickets)
                .Permit(TravelRequestAction.Finish, TravelRequestState.BookingComplete);
        }
    }
}