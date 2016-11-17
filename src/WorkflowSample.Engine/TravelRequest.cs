namespace WorkflowSample.Engine
{
    public class TravelRequest: ISupportWorkflowState<TravelRequestState>
    {
        public TravelRequestState Status { get; private set; }
        public bool IsEmployee { get; set; }
        public User Traveller { get; set; }
        public User Approver { get; set; }

        void ISupportWorkflowState<TravelRequestState>.SetStatus(TravelRequestState status)
        {
            Status = status;
        }
    }
}