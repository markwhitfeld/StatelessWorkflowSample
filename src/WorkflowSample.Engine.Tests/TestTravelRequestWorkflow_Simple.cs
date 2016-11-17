using NUnit.Framework;

namespace WorkflowSample.Engine.Tests
{
    [TestFixture]
    public class TestTravelRequestWorkflow_Simple : TestTravelRequestWorkflow_Base
    {
        protected override ITravelRequestWorkflow CreateTravelRequestWorkflow(IUserSecurityContext userSecurityContext = null)
        {
            userSecurityContext = userSecurityContext ?? new UserSecurityContext();
            return new TravelRequestWorkflow_Simple(userSecurityContext);
        }
    }
}