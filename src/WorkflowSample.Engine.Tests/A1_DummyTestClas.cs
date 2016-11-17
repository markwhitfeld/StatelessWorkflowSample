using NUnit.Framework;

namespace WorkflowSample.Engine.Tests
{
    [TestFixture]
    public class A1_DummyTestClas 
    {
        [Test]
        public void DummyTestToAbsorbTestRunnerAndJitCompileWarmup()
        {
            new TravelRequest();
            new TravelRequestWorkflow_Simple();
            new TravelRequestWorkflow_ReusableWithConfigurators();
            new TravelRequestWorkflow_SimpleWithConfigurators(new StateMachineFactory(new TravelRequestWorkflowGeneralConfigurator()));
            Assert.Pass();
        }
    }
}