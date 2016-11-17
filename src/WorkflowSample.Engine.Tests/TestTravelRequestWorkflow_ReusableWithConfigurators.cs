using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace WorkflowSample.Engine.Tests
{
    [TestFixture]
    public class TestTravelRequestWorkflow_ReusableWithConfigurators : TestTravelRequestWorkflow_Base
    {
        protected override ITravelRequestWorkflow CreateTravelRequestWorkflow(IUserSecurityContext userSecurityContext = null, INotifier notifier = null)
        {
            userSecurityContext = userSecurityContext ?? new UserSecurityContext();
            notifier = notifier ?? Substitute.For<INotifier>();
            var reusableTravelRequestStateMachineConfigurators = new List<IReusableTravelRequestStateMachineConfigurator>
            {
                new TravelRequestNewStateConfigurator(),
                new TravelRequestCapturedStateConfigurator(),
                new TravelRequestTravelerReviewStateConfigurator(userSecurityContext, notifier),
                new TravelRequestHRApprovalStateConfigurator(userSecurityContext),
                new TravelRequestManagerApprovalStateConfigurator(userSecurityContext),
                new TravelRequestProcurementApprovalStateConfigurator(userSecurityContext),
                new TravelRequestHODApprovalStateConfigurator(userSecurityContext),
                new TravelRequestBookTicketsStateConfigurator(),
            };
            var reusableTravelRequestStateMachine = new ReusableTravelRequestStateMachine(reusableTravelRequestStateMachineConfigurators);
            return new TravelRequestWorkflow_ReusableWithConfigurators(reusableTravelRequestStateMachine);
        }
    }
}