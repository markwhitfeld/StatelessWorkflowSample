using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WorkflowSample.Engine.Tests
{
    [TestFixture]
    public class TestTravelRequestWorkflow
    {
        private static TravelRequestWorkflow CreateTravelRequestWorkflow()
        {
            var workflow = new TravelRequestWorkflow();
            return workflow;
        }

        [Test]
        public void Init_ShouldSetTravelRequestToCaptured()
        {
            // Arrange
            var travelRequest = new TravelRequest();
            var workflow = CreateTravelRequestWorkflow();

            // Act
            workflow.Init(travelRequest);
            // Assert
            Assert.AreEqual(TravelRequestState.Captured, travelRequest.Status);
        }

        [Test]
        public void Submit_GivenEmployee_ShouldSetToTravelerReview()
        {
            // Arrange
            var travelRequest = new TravelRequest { IsEmployee = true };
            var workflow = CreateTravelRequestWorkflow();
            workflow.Init(travelRequest);
            // Act
            workflow.Submit(travelRequest);
            // Assert
            Assert.AreEqual(TravelRequestState.TravelerReview, travelRequest.Status);
        }
    }
}
