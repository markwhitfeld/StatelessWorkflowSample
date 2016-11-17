using System;

namespace WorkflowSample.Engine
{
    public class TravelRequestManagerApprovalStateConfigurator : IReusableTravelRequestStateMachineConfigurator
    {
        private readonly IUserSecurityContext _userSecurityContext;

        public TravelRequestManagerApprovalStateConfigurator(IUserSecurityContext userSecurityContext)
        {
            if (userSecurityContext == null) throw new ArgumentNullException(nameof(userSecurityContext));
            _userSecurityContext = userSecurityContext;
        }

        public void Configure(TravelRequestStateMachineContext context)
        {
            context.StateMachine.Configure(TravelRequestState.ManagerApproval)
                .PermitIf(TravelRequestAction.Approve, TravelRequestState.ProcurementApproval, () => context.CurrentTravelRequest.Approver == _userSecurityContext.CurrentUser);
        }
    }
}