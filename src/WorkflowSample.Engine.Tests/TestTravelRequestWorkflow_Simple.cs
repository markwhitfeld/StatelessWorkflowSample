using NUnit.Framework;

namespace WorkflowSample.Engine.Tests
{
    [TestFixture]
    public class TestTravelRequestWorkflow_Simple : TestTravelRequestWorkflow_Base
    {
        protected override ITravelRequestWorkflow CreateTravelRequestWorkflow()
        {
            return new TravelRequestWorkflow_Simple();
        }
    }
}