namespace WorkflowSample.Engine
{
    public class TravelRequestBookTicketsStateConfigurator : ITravelRequestStateMachineConfigurator
    {
        public void Configure(TravelRequestStateMachineContext context)
        {
            context.StateMachine.Configure(TravelRequestState.BookTickets)
                .Permit(TravelRequestAction.Finish, TravelRequestState.BookingComplete);
        }
    }
}