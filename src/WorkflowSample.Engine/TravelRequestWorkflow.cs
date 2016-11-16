using Stateless;

namespace WorkflowSample.Engine
{
    public class TravelRequestWorkflow
    {
        private static StateMachine<TravelRequestState, TravelRequestTransition> WithStateMachineFor(TravelRequest travelRequest)
        {
            var stateMachine = new StateMachine<TravelRequestState, TravelRequestTransition>(() => travelRequest.Status,
                ((ISupportWorkflowState<TravelRequestState>) travelRequest).SetStatus);

            stateMachine.Configure(TravelRequestState.New)
                .Permit(TravelRequestTransition.Init, TravelRequestState.Captured);

            stateMachine.Configure(TravelRequestState.Captured)
                .PermitIf(TravelRequestTransition.Submit, TravelRequestState.TravelerReview, () => travelRequest.IsEmployee)
                .PermitIf(TravelRequestTransition.Submit, TravelRequestState.HRApproval, () => !travelRequest.IsEmployee);

            stateMachine.Configure(TravelRequestState.TravelerReview)
                .Permit(TravelRequestTransition.Accept, TravelRequestState.ManagerApproval);

            stateMachine.Configure(TravelRequestState.HRApproval)
                .Permit(TravelRequestTransition.Approve, TravelRequestState.ProcurementApproval);

            stateMachine.Configure(TravelRequestState.ManagerApproval)
                .Permit(TravelRequestTransition.Approve, TravelRequestState.ProcurementApproval);

            return stateMachine;
        }

        public void Init(TravelRequest travelRequest)
        {
            WithStateMachineFor(travelRequest).Fire(TravelRequestTransition.Init);
        }

        public void Submit(TravelRequest travelRequest)
        {
            WithStateMachineFor(travelRequest).Fire(TravelRequestTransition.Submit);
        }

        public void Accept(TravelRequest travelRequest)
        {
            WithStateMachineFor(travelRequest).Fire(TravelRequestTransition.Accept);
        }

        public void Approve(TravelRequest travelRequest)
        {
            WithStateMachineFor(travelRequest).Fire(TravelRequestTransition.Approve);
        }
    }
}