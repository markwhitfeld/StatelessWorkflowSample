namespace WorkflowSample.Engine
{
    public interface IReusableTravelRequestStateMachineConfigurator
    {
        void Configure(TravelRequestStateMachineContext context);
    }
}