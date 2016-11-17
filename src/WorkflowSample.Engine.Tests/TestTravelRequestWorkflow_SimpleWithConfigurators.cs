using NUnit.Framework;

namespace WorkflowSample.Engine.Tests
{
    [TestFixture]
    public class TestTravelRequestWorkflow_SimpleWithConfigurators : TestTravelRequestWorkflow_Base
    {
        protected override ITravelRequestWorkflow CreateTravelRequestWorkflow()
        {
            var stateMachineFactory = new StateMachineFactory(new TravelRequestWorkflowGeneralConfigurator());
            return new TravelRequestWorkflow_SimpleWithConfigurators(stateMachineFactory);
        }
    }
}