using System;
using System.Collections;
using System.Collections.Generic;
using Stateless;

namespace WorkflowSample.Engine
{
    public class TravelRequestWorkflow_SimpleWithConfigurators : ITravelRequestWorkflow
    {
        private readonly IStateMachineFactory _stateMachineFactory;

        public TravelRequestWorkflow_SimpleWithConfigurators(IStateMachineFactory stateMachineFactory)
        {
            if (stateMachineFactory == null) throw new ArgumentNullException(nameof(stateMachineFactory));
            _stateMachineFactory = stateMachineFactory;
        }

        private StateMachine<TravelRequestState, TravelRequestAction> WithStateMachineFor(TravelRequest travelRequest)
        {
            return _stateMachineFactory.CreateStateMachineFor(travelRequest);
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