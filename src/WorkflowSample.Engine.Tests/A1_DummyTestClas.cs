using NUnit.Framework;

namespace WorkflowSample.Engine.Tests
{
    [TestFixture]
    public class A1_DummyTestClas 
    {
        [Test]
        public void DummyTestToAbsorbTestRunnerAndJitCompileWarmup()
        {
            new TravelRequest().GetType().Assembly.GetTypes();
            Assert.Pass();
        }
    }
}