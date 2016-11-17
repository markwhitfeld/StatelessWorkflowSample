using System;
using System.Collections;
using System.Collections.Generic;
using Stateless;

namespace WorkflowSample.Engine
{
    public class TravelRequestWorkflow_Simple : ITravelRequestWorkflow
    {
        private readonly IUserSecurityContext _userSecurityContext;
        private readonly INotifier _notifier;

        public TravelRequestWorkflow_Simple(IUserSecurityContext userSecurityContext, INotifier notifier)
        {
            if (userSecurityContext == null) throw new ArgumentNullException(nameof(userSecurityContext));
            if (notifier == null) throw new ArgumentNullException(nameof(notifier));
            _userSecurityContext = userSecurityContext;
            _notifier = notifier;
        }

        private StateMachine<TravelRequestState, TravelRequestAction> WithStateMachineFor(TravelRequest travelRequest)
        {
            var stateMachine = new StateMachine<TravelRequestState, TravelRequestAction>(() => travelRequest.Status,
                ((ISupportWorkflowState<TravelRequestState>) travelRequest).SetStatus);

            stateMachine.Configure(TravelRequestState.New)
                .Permit(TravelRequestAction.Init, TravelRequestState.Captured);

            stateMachine.Configure(TravelRequestState.Captured)
                .PermitIf(TravelRequestAction.Submit, TravelRequestState.TravelerReview, () => travelRequest.IsEmployee)
                .PermitIf(TravelRequestAction.Submit, TravelRequestState.HRApproval, () => !travelRequest.IsEmployee);

            stateMachine.Configure(TravelRequestState.TravelerReview)
                .PermitIf(TravelRequestAction.Accept, TravelRequestState.ManagerApproval, () => travelRequest.Traveller == _userSecurityContext.CurrentUser, "Traveller")
                .OnEntry(() => _notifier.Notify("NotifyTravellerOfReview", travelRequest), "Notify Traveller")
                .OnEntry(() => _notifier.Notify("NotifyTravelAdminOfReview", travelRequest), "Notify Travel Admin");

            stateMachine.Configure(TravelRequestState.HRApproval)
                .PermitIf(TravelRequestAction.Approve, TravelRequestState.ProcurementApproval, () => travelRequest.Approver == _userSecurityContext.CurrentUser, "Approver");

            stateMachine.Configure(TravelRequestState.ManagerApproval)
                .PermitIf(TravelRequestAction.Approve, TravelRequestState.ProcurementApproval, () => travelRequest.Approver == _userSecurityContext.CurrentUser, "Approver");

            stateMachine.Configure(TravelRequestState.ProcurementApproval)
                .PermitIf(TravelRequestAction.Approve, TravelRequestState.HODApproval, () => travelRequest.Approver == _userSecurityContext.CurrentUser, "Approver");

            stateMachine.Configure(TravelRequestState.HODApproval)
                .PermitIf(TravelRequestAction.Approve, TravelRequestState.BookTickets, () => travelRequest.Approver == _userSecurityContext.CurrentUser, "Approver")
                .OnEntryFrom(TravelRequestAction.Approve, transition =>
                {
                    if (travelRequest.IsEmployee) stateMachine.Fire(TravelRequestAction.Approve);
                });

            stateMachine.Configure(TravelRequestState.BookTickets)
                .Permit(TravelRequestAction.Finish, TravelRequestState.BookingComplete);
            
            return stateMachine;
        }

        public void Init(TravelRequest travelRequest)
        {
            WithStateMachineFor(travelRequest).Fire(TravelRequestAction.Init);
        }

        public void Submit(TravelRequest travelRequest)
        {
            WithStateMachineFor(travelRequest).Fire(TravelRequestAction.Submit);
        }

        public void Accept(TravelRequest travelRequest)
        {
            WithStateMachineFor(travelRequest).Fire(TravelRequestAction.Accept);
        }

        public void Approve(TravelRequest travelRequest)
        {
            WithStateMachineFor(travelRequest).Fire(TravelRequestAction.Approve);
        }

        public IEnumerable<TravelRequestAction> GetAllowedActions(TravelRequest travelRequest)
        {
            return WithStateMachineFor(travelRequest).PermittedTriggers;
        }

        public void Finish(TravelRequest travelRequest)
        {
            WithStateMachineFor(travelRequest).Fire(TravelRequestAction.Finish);
        }
    }
}