using NUnit.Framework;

namespace WorkflowSample.Engine.Tests
{
    [TestFixture]
    public class TestTravelRequestWorkflow_ReusableWithConfigurators : TestTravelRequestWorkflow_Base
    {
        private TravelRequestWorkflow_ReusableWithConfigurators _travelRequestWorkflowReusableWithConfigurators;

        protected override ITravelRequestWorkflow CreateTravelRequestWorkflow()
        {
            _travelRequestWorkflowReusableWithConfigurators = _travelRequestWorkflowReusableWithConfigurators ?? new TravelRequestWorkflow_ReusableWithConfigurators();
            return _travelRequestWorkflowReusableWithConfigurators;
        }
    }
}