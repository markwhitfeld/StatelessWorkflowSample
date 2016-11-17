using System;

namespace WorkflowSample.Engine
{
    public class TravelRequestHODApprovalStateConfigurator : IReusableTravelRequestStateMachineConfigurator
    {
        private readonly IUserSecurityContext _userSecurityContext;

        public TravelRequestHODApprovalStateConfigurator(IUserSecurityContext userSecurityContext)
        {
            if (userSecurityContext == null) throw new ArgumentNullException(nameof(userSecurityContext));
            _userSecurityContext = userSecurityContext;
        }

        public void Configure(TravelRequestStateMachineContext context)
        {
            context.StateMachine.Configure(TravelRequestState.HODApproval)
                .PermitIf(TravelRequestAction.Approve, TravelRequestState.BookTickets, () => context.CurrentTravelRequest.Approver == _userSecurityContext.CurrentUser)
                .OnEntryFrom(TravelRequestAction.Approve, transition =>
                {
                    if (context.CurrentTravelRequest.IsEmployee) context.StateMachine.Fire(TravelRequestAction.Approve);
                });
        }
    }
}