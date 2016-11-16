namespace WorkflowSample.Engine
{
    public interface ITravelRequestStateMachineConfigurator
    {
        void Configure(TravelRequestStateMachineContext context);
    }
}