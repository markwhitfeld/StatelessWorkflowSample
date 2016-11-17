using System;

namespace WorkflowSample.Engine
{
    public class TravelRequestProcurementApprovalStateConfigurator : IReusableTravelRequestStateMachineConfigurator
    {
        private readonly IUserSecurityContext _userSecurityContext;

        public TravelRequestProcurementApprovalStateConfigurator(IUserSecurityContext userSecurityContext)
        {
            if (userSecurityContext == null) throw new ArgumentNullException(nameof(userSecurityContext));
            _userSecurityContext = userSecurityContext;
        }

        public void Configure(TravelRequestStateMachineContext context)
        {
            context.StateMachine.Configure(TravelRequestState.ProcurementApproval)
                .PermitIf(TravelRequestAction.Approve, TravelRequestState.HODApproval, () => context.CurrentTravelRequest.Approver == _userSecurityContext.CurrentUser);
        }
    }
}