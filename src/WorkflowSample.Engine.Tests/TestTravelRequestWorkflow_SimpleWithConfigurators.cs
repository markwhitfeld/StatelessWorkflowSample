using NUnit.Framework;

namespace WorkflowSample.Engine.Tests
{
    [TestFixture]
    public class TestTravelRequestWorkflow_SimpleWithConfigurators : TestTravelRequestWorkflow_Base
    {
        protected override ITravelRequestWorkflow CreateTravelRequestWorkflow()
        {
            var workflowConfigurator = new TravelRequestWorkflowGeneralConfigurator();
            var stateMachineFactory = new StateMachineFactory(workflowConfigurator);
            return new TravelRequestWorkflow_SimpleWithConfigurators(stateMachineFactory);
        }
    }
}