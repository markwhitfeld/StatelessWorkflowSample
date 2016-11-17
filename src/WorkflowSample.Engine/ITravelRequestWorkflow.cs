using System.Collections.Generic;

namespace WorkflowSample.Engine
{
    public interface ITravelRequestWorkflow
    {
        void Init(TravelRequest travelRequest);
        void Submit(TravelRequest travelRequest);
        void Accept(TravelRequest travelRequest);
        void Approve(TravelRequest travelRequest);
        IEnumerable<TravelRequestAction> GetAllowedActions(TravelRequest travelRequest);
        void Finish(TravelRequest travelRequest);
    }
}