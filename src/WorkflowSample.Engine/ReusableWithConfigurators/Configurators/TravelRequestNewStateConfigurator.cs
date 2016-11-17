namespace WorkflowSample.Engine
{
    public class TravelRequestNewStateConfigurator : ITravelRequestStateMachineConfigurator
    {
        public void Configure(TravelRequestStateMachineContext context)
        {
            context.StateMachine.Configure(TravelRequestState.New)
                .Permit(TravelRequestAction.Init, TravelRequestState.Captured);
        }
    }
}