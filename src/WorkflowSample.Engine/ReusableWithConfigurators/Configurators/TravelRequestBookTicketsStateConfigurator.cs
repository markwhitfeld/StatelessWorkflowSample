namespace WorkflowSample.Engine
{
    public class TravelRequestBookTicketsStateConfigurator : IReusableTravelRequestStateMachineConfigurator
    {
        public void Configure(TravelRequestStateMachineContext context)
        {
            context.StateMachine.Configure(TravelRequestState.BookTickets)
                .Permit(TravelRequestAction.Finish, TravelRequestState.BookingComplete);
        }
    }
}