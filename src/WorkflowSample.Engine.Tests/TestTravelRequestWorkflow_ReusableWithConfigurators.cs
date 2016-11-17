using NUnit.Framework;

namespace WorkflowSample.Engine.Tests
{
    [TestFixture]
    public class TestTravelRequestWorkflow_ReusableWithConfigurators : TestTravelRequestWorkflow_Base
    {
        protected override ITravelRequestWorkflow CreateTravelRequestWorkflow()
        {
            return new TravelRequestWorkflow_ReusableWithConfigurators();
        }
    }
}