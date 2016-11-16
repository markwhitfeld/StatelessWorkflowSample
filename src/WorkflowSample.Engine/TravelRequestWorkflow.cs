using Stateless;

namespace WorkflowSample.Engine
{
    public class TravelRequestWorkflow
    {
        public void Init(TravelRequest travelRequest)
        {
            WithStateMachineFor(travelRequest).Fire(TravelRequestTransition.Init);
        }

        private static StateMachine<TravelRequestState, TravelRequestTransition> WithStateMachineFor(TravelRequest travelRequest)
        {
            var stateMachine = new StateMachine<TravelRequestState, TravelRequestTransition>(() => travelRequest.Status,
                ((ISupportWorkflowState<TravelRequestState>) travelRequest).SetStatus);

            stateMachine.Configure(TravelRequestState.New)
                .Permit(TravelRequestTransition.Init, TravelRequestState.Captured);

            stateMachine.Configure(TravelRequestState.Captured)
                .PermitIf(TravelRequestTransition.Submit, TravelRequestState.TravelerReview, () => travelRequest.IsEmployee)
                .PermitIf(TravelRequestTransition.Submit, TravelRequestState.HRApproval, () => !travelRequest.IsEmployee);
            return stateMachine;
        }

        public void Submit(TravelRequest travelRequest)
        {
            WithStateMachineFor(travelRequest).Fire(TravelRequestTransition.Submit);
        }
    }
}