using System;
using System.Collections;
using System.Collections.Generic;
using Stateless;

namespace WorkflowSample.Engine
{
    public class TravelRequestWorkflow_ReusableWithConfigurators : ITravelRequestWorkflow
    {
        private ReusableTravelRequestStateMachine WithStateMachineFor(TravelRequest travelRequest)
        {
            return new ReusableTravelRequestStateMachine { CurrentTravelRequest = travelRequest };
        }

        public void Init(TravelRequest travelRequest)
        {
            WithStateMachineFor(travelRequest).Fire(TravelRequestAction.Init);
        }

        public void Submit(TravelRequest travelRequest)
        {
            WithStateMachineFor(travelRequest).Fire(TravelRequestAction.Submit);
        }

        public void Accept(TravelRequest travelRequest)
        {
            WithStateMachineFor(travelRequest).Fire(TravelRequestAction.Accept);
        }

        public void Approve(TravelRequest travelRequest)
        {
            WithStateMachineFor(travelRequest).Fire(TravelRequestAction.Approve);
        }

        public IEnumerable<TravelRequestAction> GetAllowedActions(TravelRequest travelRequest)
        {
            return WithStateMachineFor(travelRequest).PermittedTriggers;
        }

        public void Finish(TravelRequest travelRequest)
        {
            WithStateMachineFor(travelRequest).Fire(TravelRequestAction.Finish);
        }
    }
}