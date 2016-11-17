namespace WorkflowSample.Engine
{
    public interface INotifier
    {
        void Notify(string notificationType, TravelRequest travelRequest);
    }
}