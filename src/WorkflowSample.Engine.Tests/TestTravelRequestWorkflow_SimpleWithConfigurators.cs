using NSubstitute;
using NUnit.Framework;

namespace WorkflowSample.Engine.Tests
{
    [TestFixture]
    public class TestTravelRequestWorkflow_SimpleWithConfigurators : TestTravelRequestWorkflow_Base
    {
        protected override ITravelRequestWorkflow CreateTravelRequestWorkflow(IUserSecurityContext userSecurityContext = null, INotifier notifier = null)
        {
            userSecurityContext = userSecurityContext ?? new UserSecurityContext();
            notifier = notifier ?? Substitute.For<INotifier>();
            var workflowConfigurator = new TravelRequestWorkflowGeneralConfigurator(userSecurityContext, notifier);
            var stateMachineFactory = new StateMachineFactory(workflowConfigurator);
            return new TravelRequestWorkflow_SimpleWithConfigurators(stateMachineFactory);
        }
    }
}