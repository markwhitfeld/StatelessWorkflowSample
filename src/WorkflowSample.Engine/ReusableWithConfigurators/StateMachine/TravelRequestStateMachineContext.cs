using Stateless;

namespace WorkflowSample.Engine
{
    public class TravelRequestStateMachineContext
    {
        public StateMachine<TravelRequestState, TravelRequestAction> StateMachine { get; }
        public TravelRequest CurrentTravelRequest { get; set; }

        public TravelRequestStateMachineContext(StateMachine<TravelRequestState, TravelRequestAction> stateMachine)
        {
            StateMachine = stateMachine;
        }
    }
}