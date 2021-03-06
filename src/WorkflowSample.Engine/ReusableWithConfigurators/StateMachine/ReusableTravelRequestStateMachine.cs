﻿using System;
using System.Collections.Generic;
using Stateless;

namespace WorkflowSample.Engine
{
    public class ReusableTravelRequestStateMachine
    {
        private readonly TravelRequestStateMachineContext _stateMachineContext;

        public ReusableTravelRequestStateMachine(List<IReusableTravelRequestStateMachineConfigurator> configurators)
        {
            if (configurators == null) throw new ArgumentNullException(nameof(configurators));
            var stateMachine = new StateMachine<TravelRequestState, TravelRequestAction>(GetState, SetState);
            _stateMachineContext = new TravelRequestStateMachineContext(stateMachine);

            configurators.ForEach(configurator => configurator.Configure(_stateMachineContext));
        }

        private void SetState(TravelRequestState travelRequestState)
        {
            var statefulObject = (ISupportWorkflowState<TravelRequestState>)CurrentTravelRequest;
            statefulObject.SetStatus(travelRequestState);
        }

        private TravelRequestState GetState()
        {
            return CurrentTravelRequest.Status;
        }

        public TravelRequest CurrentTravelRequest
        {
            private get { return _stateMachineContext.CurrentTravelRequest; }
            set { _stateMachineContext.CurrentTravelRequest = value; }
        }

        public IEnumerable<TravelRequestAction> PermittedTriggers => _stateMachineContext.StateMachine.PermittedTriggers;

        public void Fire(TravelRequestAction travelRequestAction)
        {
            _stateMachineContext.StateMachine.Fire(travelRequestAction);
        }
    }
}