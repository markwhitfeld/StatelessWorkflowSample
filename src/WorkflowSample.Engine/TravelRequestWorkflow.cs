using System.Collections;
using System.Collections.Generic;
using Stateless;

namespace WorkflowSample.Engine
{
    public class TravelRequestWorkflow
    {
        private static StateMachine<TravelRequestState, TravelRequestAction> WithStateMachineFor(TravelRequest travelRequest)
        {
            var stateMachine = new StateMachine<TravelRequestState, TravelRequestAction>(() => travelRequest.Status,
                ((ISupportWorkflowState<TravelRequestState>) travelRequest).SetStatus);

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