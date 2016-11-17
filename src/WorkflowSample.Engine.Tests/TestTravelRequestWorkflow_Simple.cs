using NSubstitute;
using NUnit.Framework;

namespace WorkflowSample.Engine.Tests
{
    [TestFixture]
    public class TestTravelRequestWorkflow_Simple : TestTravelRequestWorkflow_Base
    {
        protected override ITravelRequestWorkflow CreateTravelRequestWorkflow(IUserSecurityContext userSecurityContext = null, INotifier notifier = null)
        {
            userSecurityContext = userSecurityContext ?? new UserSecurityContext();
            notifier = notifier ?? Substitute.For<INotifier>();
            return new TravelRequestWorkflow_Simple(userSecurityContext, notifier);
        }
    }
}