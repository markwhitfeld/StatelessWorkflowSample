using Stateless;

namespace WorkflowSample.Engine
{
    public interface IStateMachineFactory
    {
        StateMachine<TravelRequestState, TravelRequestAction> CreateStateMachineFor(TravelRequest travelRequest);
    }
}