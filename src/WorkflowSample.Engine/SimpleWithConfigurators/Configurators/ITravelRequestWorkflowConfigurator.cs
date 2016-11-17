using Stateless;

namespace WorkflowSample.Engine
{
    public interface ITravelRequestWorkflowConfigurator
    {
        void Configure(StateMachine<TravelRequestState, TravelRequestAction> stateMachine, TravelRequest travelRequest);
    }
}