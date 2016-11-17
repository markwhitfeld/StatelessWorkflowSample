using System.Collections.Generic;
using NUnit.Framework;

namespace WorkflowSample.Engine.Tests
{
    [TestFixture]
    public class TestTravelRequestWorkflow_ReusableWithConfigurators : TestTravelRequestWorkflow_Base
    {
        protected override ITravelRequestWorkflow CreateTravelRequestWorkflow(IUserSecurityContext userSecurityContext = null)
        {
            userSecurityContext = userSecurityContext ?? new UserSecurityContext();
            var reusableTravelRequestStateMachineConfigurators = new List<IReusableTravelRequestStateMachineConfigurator>
            {
                new TravelRequestNewStateConfigurator(),
                new TravelRequestCapturedStateConfigurator(),
                new TravelRequestTravelerReviewStateConfigurator(userSecurityContext),
                new TravelRequestHRApprovalStateConfigurator(),
                new TravelRequestManagerApprovalStateConfigurator(),
                new TravelRequestProcurementApprovalStateConfigurator(),
                new TravelRequestHODApprovalStateConfigurator(),
                new TravelRequestBookTicketsStateConfigurator(),
            };
            var reusableTravelRequestStateMachine = new ReusableTravelRequestStateMachine(reusableTravelRequestStateMachineConfigurators);
            return new TravelRequestWorkflow_ReusableWithConfigurators(reusableTravelRequestStateMachine);
        }
    }
}