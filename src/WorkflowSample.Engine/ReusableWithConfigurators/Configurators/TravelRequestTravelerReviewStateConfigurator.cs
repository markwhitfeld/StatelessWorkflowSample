using System;

namespace WorkflowSample.Engine
{
    public class TravelRequestTravelerReviewStateConfigurator : IReusableTravelRequestStateMachineConfigurator
    {
        private readonly IUserSecurityContext _userSecurityContext;
        private readonly INotifier _notifier;

        public TravelRequestTravelerReviewStateConfigurator(IUserSecurityContext userSecurityContext, INotifier notifier)
        {
            if (userSecurityContext == null) throw new ArgumentNullException(nameof(userSecurityContext));
            if (notifier == null) throw new ArgumentNullException(nameof(notifier));
            _userSecurityContext = userSecurityContext;
            _notifier = notifier;
        }

        public void Configure(TravelRequestStateMachineContext context)
        {
            context.StateMachine.Configure(TravelRequestState.TravelerReview)
                .PermitIf(TravelRequestAction.Accept, TravelRequestState.ManagerApproval, () => context.CurrentTravelRequest.Traveller == _userSecurityContext.CurrentUser)
                .OnEntry(() => _notifier.Notify("NotifyTravellerOfReview", context.CurrentTravelRequest), "Notify Traveller")
                .OnEntry(() => _notifier.Notify("NotifyTravelAdminOfReview", context.CurrentTravelRequest), "Notify Travel Admin");
        }
    }
}