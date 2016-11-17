using System;
using Stateless;

namespace WorkflowSample.Engine
{
    public class StateMachineFactory : IStateMachineFactory
    {
        private readonly ITravelRequestWorkflowConfigurator _travelRequestWorkflowConfigurator;

        public StateMachineFactory(ITravelRequestWorkflowConfigurator travelRequestWorkflowConfigurator)
        {
            if (travelRequestWorkflowConfigurator == null)
                throw new ArgumentNullException(nameof(travelRequestWorkflowConfigurator));
            _travelRequestWorkflowConfigurator = travelRequestWorkflowConfigurator;
        }

        public StateMachine<TravelRequestState, TravelRequestAction> CreateStateMachineFor(TravelRequest travelRequest)
        {
            var stateMachine = new StateMachine<TravelRequestState, TravelRequestAction>(() => travelRequest.Status,
                ((ISupportWorkflowState<TravelRequestState>) travelRequest).SetStatus);

            _travelRequestWorkflowConfigurator.Configure(stateMachine, travelRequest);
            return stateMachine;
        }
    }
}