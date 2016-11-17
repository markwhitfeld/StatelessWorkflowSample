using NUnit.Framework;

namespace WorkflowSample.Engine.Tests
{
    [TestFixture]
    public class TestTravelRequestWorkflow_SimpleWithConfigurators : TestTravelRequestWorkflow_Base
    {
        protected override ITravelRequestWorkflow CreateTravelRequestWorkflow(IUserSecurityContext userSecurityContext = null)
        {
            userSecurityContext = userSecurityContext ?? new UserSecurityContext();
            var workflowConfigurator = new TravelRequestWorkflowGeneralConfigurator(userSecurityContext);
            var stateMachineFactory = new StateMachineFactory(workflowConfigurator);
            return new TravelRequestWorkflow_SimpleWithConfigurators(stateMachineFactory);
        }
    }
}