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
                .Permit(TravelRequestTransition.Submit, TravelRequestState.TravelerReview);
            return stateMachine;
        }

        public void Submit(TravelRequest travelRequest)
        {
            WithStateMachineFor(travelRequest).Fire(TravelRequestTransition.Submit);
        }
    }
}