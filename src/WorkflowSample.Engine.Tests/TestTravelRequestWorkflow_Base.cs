using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using NUnit.Framework;

namespace WorkflowSample.Engine.Tests
{
    [TestFixture]
    public abstract class TestTravelRequestWorkflow_Base
    {
        protected abstract ITravelRequestWorkflow CreateTravelRequestWorkflow(IUserSecurityContext userSecurityContext = null);

        private static void SetTravelRequestStatus(TravelRequest travelRequest, TravelRequestState travelRequestState)
        {
            ((ISupportWorkflowState<TravelRequestState>) travelRequest).SetStatus(travelRequestState);
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
        public void GetAllowedActions_GivenNew_ShouldBe_Init()
        {
            // Arrange
            var travelRequest = new TravelRequest();
            var workflow = CreateTravelRequestWorkflow();
            // Act
            var allowedActions = workflow.GetAllowedActions(travelRequest);
            // Assert
            CollectionAssert.AreEquivalent(new [] { TravelRequestAction.Init }, allowedActions);
        }

        [Test]
        public void Submit_WhenCaptured_GivenEmployee_ShouldSetToTravelerReview()
        {
            // Arrange
            var travelRequest = new TravelRequest { IsEmployee = true };
            SetTravelRequestStatus(travelRequest, TravelRequestState.Captured);
            var workflow = CreateTravelRequestWorkflow();
            // Act
            workflow.Submit(travelRequest);
            // Assert
            Assert.AreEqual(TravelRequestState.TravelerReview, travelRequest.Status);
        }

        [Test]
        public void Submit_WhenCaptured_GivenNonEmployee_ShouldSetToHRApproval()
        {
            // Arrange
            var travelRequest = new TravelRequest { IsEmployee = false };
            SetTravelRequestStatus(travelRequest, TravelRequestState.Captured);
            var workflow = CreateTravelRequestWorkflow();
            // Act
            workflow.Submit(travelRequest);
            // Assert
            Assert.AreEqual(TravelRequestState.HRApproval, travelRequest.Status);
        }

        [Test]
        public void GetAllowedActions_GivenCaptured_ShouldBe_Submit()
        {
            // Arrange
            var travelRequest = new TravelRequest();
            SetTravelRequestStatus(travelRequest, TravelRequestState.Captured);
            var workflow = CreateTravelRequestWorkflow();
            // Act
            var allowedActions = workflow.GetAllowedActions(travelRequest);
            // Assert
            CollectionAssert.AreEquivalent(new[] { TravelRequestAction.Submit }, allowedActions);
        }

        [Test]
        public void Accept_WhenTravellerReview_ShouldSetToManagerApproval()
        {
            // Arrange
            var travelRequest = new TravelRequest { IsEmployee = false };
            SetTravelRequestStatus(travelRequest, TravelRequestState.TravelerReview);
            var workflow = CreateTravelRequestWorkflow();
            // Act
            workflow.Accept(travelRequest);
            // Assert
            Assert.AreEqual(TravelRequestState.ManagerApproval, travelRequest.Status);
        }

        [Test]
        public void GetAllowedActions_GivenTravelerReview_ShouldBe_Accept()
        {
            // Arrange
            var travelRequest = new TravelRequest();
            SetTravelRequestStatus(travelRequest, TravelRequestState.TravelerReview);
            var workflow = CreateTravelRequestWorkflow();
            // Act
            var allowedActions = workflow.GetAllowedActions(travelRequest);
            // Assert
            CollectionAssert.AreEquivalent(new[] { TravelRequestAction.Accept }, allowedActions);
        }

        [Test]
        public void GetAllowedActions_GivenTravelerReview_AndNotTraveller_ShouldBeEmpty()
        {
            // Arrange
            var traveller = new User("bob");
            var currentUser = new User("jim");
            var travelRequest = new TravelRequest
            {
                Traveller = traveller
            };
            var userSecurityContext = new UserSecurityContext() {CurrentUser = currentUser};
            SetTravelRequestStatus(travelRequest, TravelRequestState.TravelerReview);
            var workflow = CreateTravelRequestWorkflow(userSecurityContext);
            // Act
            var allowedActions = workflow.GetAllowedActions(travelRequest);
            // Assert
            CollectionAssert.IsEmpty(allowedActions);
        }

        [Test]
        public void GetAllowedActions_GivenTravelerReview_AndTravellerIsCurrentUser_ShouldBe_Accept()
        {
            // Arrange
            var traveller = new User("bob");
            var currentUser = traveller;
            var travelRequest = new TravelRequest
            {
                Traveller = traveller
            };
            var userSecurityContext = new UserSecurityContext() { CurrentUser = currentUser };
            SetTravelRequestStatus(travelRequest, TravelRequestState.TravelerReview);
            var workflow = CreateTravelRequestWorkflow(userSecurityContext);
            // Act
            var allowedActions = workflow.GetAllowedActions(travelRequest);
            // Assert
            CollectionAssert.AreEquivalent(new[] { TravelRequestAction.Accept }, allowedActions);
        }

        [Test]
        public void Approve_WhenHRApproval_ShouldSetToProcurementApproval()
        {
            // Arrange
            var travelRequest = new TravelRequest { IsEmployee = false };
            SetTravelRequestStatus(travelRequest, TravelRequestState.HRApproval);
            var workflow = CreateTravelRequestWorkflow();
            // Act
            workflow.Approve(travelRequest);
            // Assert
            Assert.AreEqual(TravelRequestState.ProcurementApproval, travelRequest.Status);
        }

        [Test]
        public void GetAllowedActions_GivenHRApproval_ShouldBe_Approve()
        {
            // Arrange
            var travelRequest = new TravelRequest();
            SetTravelRequestStatus(travelRequest, TravelRequestState.HRApproval);
            var workflow = CreateTravelRequestWorkflow();
            // Act
            var allowedActions = workflow.GetAllowedActions(travelRequest);
            // Assert
            CollectionAssert.AreEquivalent(new[] { TravelRequestAction.Approve }, allowedActions);
        }

        [Test]
        public void GetAllowedActions_GivenHRApproval_AndNotApprover_ShouldBeEmpty()
        {
            // Arrange
            var approver = new User("bob");
            var currentUser = new User("jim");
            var travelRequest = new TravelRequest
            {
                Approver = approver
            };
            var userSecurityContext = new UserSecurityContext() { CurrentUser = currentUser };
            SetTravelRequestStatus(travelRequest, TravelRequestState.HRApproval);
            var workflow = CreateTravelRequestWorkflow(userSecurityContext);
            // Act
            var allowedActions = workflow.GetAllowedActions(travelRequest);
            // Assert
            CollectionAssert.IsEmpty(allowedActions);
        }

        [Test]
        public void GetAllowedActions_GivenHRApproval_AndApproverIsCurrentUser_ShouldBe_Approve()
        {
            // Arrange
            var approver = new User("bob");
            var currentUser = approver;
            var travelRequest = new TravelRequest
            {
                Approver = approver
            };
            var userSecurityContext = new UserSecurityContext() { CurrentUser = currentUser };
            SetTravelRequestStatus(travelRequest, TravelRequestState.HRApproval);
            var workflow = CreateTravelRequestWorkflow(userSecurityContext);
            // Act
            var allowedActions = workflow.GetAllowedActions(travelRequest);
            // Assert
            CollectionAssert.AreEquivalent(new[] { TravelRequestAction.Approve }, allowedActions);
        }

        [Test]
        public void Approve_WhenManagerApproval_ShouldSetToProcurementApproval()
        {
            // Arrange
            var travelRequest = new TravelRequest();
            SetTravelRequestStatus(travelRequest, TravelRequestState.ManagerApproval);
            var workflow = CreateTravelRequestWorkflow();
            // Act
            workflow.Approve(travelRequest);
            // Assert
            Assert.AreEqual(TravelRequestState.ProcurementApproval, travelRequest.Status);
        }

        [Test]
        public void GetAllowedActions_GivenManagerApproval_ShouldBe_Approve()
        {
            // Arrange
            var travelRequest = new TravelRequest();
            SetTravelRequestStatus(travelRequest, TravelRequestState.ManagerApproval);
            var workflow = CreateTravelRequestWorkflow();
            // Act
            var allowedActions = workflow.GetAllowedActions(travelRequest);
            // Assert
            CollectionAssert.AreEquivalent(new[] { TravelRequestAction.Approve }, allowedActions);
        }

        [Test]
        public void GetAllowedActions_GivenManagerApproval_AndNotApprover_ShouldBeEmpty()
        {
            // Arrange
            var approver = new User("bob");
            var currentUser = new User("jim");
            var travelRequest = new TravelRequest
            {
                Approver = approver
            };
            var userSecurityContext = new UserSecurityContext() { CurrentUser = currentUser };
            SetTravelRequestStatus(travelRequest, TravelRequestState.ManagerApproval);
            var workflow = CreateTravelRequestWorkflow(userSecurityContext);
            // Act
            var allowedActions = workflow.GetAllowedActions(travelRequest);
            // Assert
            CollectionAssert.IsEmpty(allowedActions);
        }

        [Test]
        public void GetAllowedActions_GivenManagerApproval_AndApproverIsCurrentUser_ShouldBe_Approve()
        {
            // Arrange
            var approver = new User("bob");
            var currentUser = approver;
            var travelRequest = new TravelRequest
            {
                Approver = approver
            };
            var userSecurityContext = new UserSecurityContext() { CurrentUser = currentUser };
            SetTravelRequestStatus(travelRequest, TravelRequestState.ManagerApproval);
            var workflow = CreateTravelRequestWorkflow(userSecurityContext);
            // Act
            var allowedActions = workflow.GetAllowedActions(travelRequest);
            // Assert
            CollectionAssert.AreEquivalent(new[] { TravelRequestAction.Approve }, allowedActions);
        }

        [Test]
        public void Approve_WhenProcurementApproval_GivenNonEmployee_ShouldSetToHODApproval()
        {
            // Arrange
            var travelRequest = new TravelRequest { IsEmployee = false };
            SetTravelRequestStatus(travelRequest, TravelRequestState.ProcurementApproval);
            var workflow = CreateTravelRequestWorkflow();
            // Act
            workflow.Approve(travelRequest);
            // Assert
            Assert.AreEqual(TravelRequestState.HODApproval, travelRequest.Status);
        }

        [Test]
        public void Approve_WhenProcurementApproval_GivenEmployee_ShouldAutoApproveForHOD_AndSetToBookTickets()
        {
            // Arrange
            var travelRequest = new TravelRequest { IsEmployee = true };
            SetTravelRequestStatus(travelRequest, TravelRequestState.ProcurementApproval);
            var workflow = CreateTravelRequestWorkflow();
            // Act
            workflow.Approve(travelRequest);
            // Assert
            Assert.AreEqual(TravelRequestState.BookTickets, travelRequest.Status);
        }

        [Test]
        public void GetAllowedActions_GivenProcurementApproval_ShouldBe_Approve()
        {
            // Arrange
            var travelRequest = new TravelRequest();
            SetTravelRequestStatus(travelRequest, TravelRequestState.ProcurementApproval);
            var workflow = CreateTravelRequestWorkflow();
            // Act
            var allowedActions = workflow.GetAllowedActions(travelRequest);
            // Assert
            CollectionAssert.AreEquivalent(new[] { TravelRequestAction.Approve }, allowedActions);
        }

        [Test]
        public void GetAllowedActions_GivenProcurementApproval_AndNotApprover_ShouldBeEmpty()
        {
            // Arrange
            var approver = new User("bob");
            var currentUser = new User("jim");
            var travelRequest = new TravelRequest
            {
                Approver = approver
            };
            var userSecurityContext = new UserSecurityContext() { CurrentUser = currentUser };
            SetTravelRequestStatus(travelRequest, TravelRequestState.ProcurementApproval);
            var workflow = CreateTravelRequestWorkflow(userSecurityContext);
            // Act
            var allowedActions = workflow.GetAllowedActions(travelRequest);
            // Assert
            CollectionAssert.IsEmpty(allowedActions);
        }

        [Test]
        public void GetAllowedActions_GivenProcurementApproval_AndApproverIsCurrentUser_ShouldBe_Approve()
        {
            // Arrange
            var approver = new User("bob");
            var currentUser = approver;
            var travelRequest = new TravelRequest
            {
                Approver = approver
            };
            var userSecurityContext = new UserSecurityContext() { CurrentUser = currentUser };
            SetTravelRequestStatus(travelRequest, TravelRequestState.ProcurementApproval);
            var workflow = CreateTravelRequestWorkflow(userSecurityContext);
            // Act
            var allowedActions = workflow.GetAllowedActions(travelRequest);
            // Assert
            CollectionAssert.AreEquivalent(new[] { TravelRequestAction.Approve }, allowedActions);
        }

        [Test]
        public void Approve_WhenHODApproval_GivenNonEmployee_ShouldSetToBookTickets()
        {
            // Arrange
            var travelRequest = new TravelRequest { IsEmployee = false };
            SetTravelRequestStatus(travelRequest, TravelRequestState.HODApproval);
            var workflow = CreateTravelRequestWorkflow();
            // Act
            workflow.Approve(travelRequest);
            // Assert
            Assert.AreEqual(TravelRequestState.BookTickets, travelRequest.Status);
        }

        [Test]
        public void Approve_WhenHODApproval_GivenEmployee_ShouldSetToBookTickets()
        {
            // Arrange
            var travelRequest = new TravelRequest { IsEmployee = true };
            SetTravelRequestStatus(travelRequest, TravelRequestState.HODApproval);
            var workflow = CreateTravelRequestWorkflow();
            // Act
            workflow.Approve(travelRequest);
            // Assert
            Assert.AreEqual(TravelRequestState.BookTickets, travelRequest.Status);
        }

        [Test]
        public void GetAllowedActions_GivenHODApproval_ShouldBe_Approve()
        {
            // Arrange
            var travelRequest = new TravelRequest();
            SetTravelRequestStatus(travelRequest, TravelRequestState.HODApproval);
            var workflow = CreateTravelRequestWorkflow();
            // Act
            var allowedActions = workflow.GetAllowedActions(travelRequest);
            // Assert
            CollectionAssert.AreEquivalent(new[] { TravelRequestAction.Approve }, allowedActions);
        }

        [Test]
        public void GetAllowedActions_GivenHODApproval_AndNotApprover_ShouldBeEmpty()
        {
            // Arrange
            var approver = new User("bob");
            var currentUser = new User("jim");
            var travelRequest = new TravelRequest
            {
                Approver = approver
            };
            var userSecurityContext = new UserSecurityContext() { CurrentUser = currentUser };
            SetTravelRequestStatus(travelRequest, TravelRequestState.HODApproval);
            var workflow = CreateTravelRequestWorkflow(userSecurityContext);
            // Act
            var allowedActions = workflow.GetAllowedActions(travelRequest);
            // Assert
            CollectionAssert.IsEmpty(allowedActions);
        }

        [Test]
        public void GetAllowedActions_GivenHODApproval_AndApproverIsCurrentUser_ShouldBe_Approve()
        {
            // Arrange
            var approver = new User("bob");
            var currentUser = approver;
            var travelRequest = new TravelRequest
            {
                Approver = approver
            };
            var userSecurityContext = new UserSecurityContext() { CurrentUser = currentUser };
            SetTravelRequestStatus(travelRequest, TravelRequestState.HODApproval);
            var workflow = CreateTravelRequestWorkflow(userSecurityContext);
            // Act
            var allowedActions = workflow.GetAllowedActions(travelRequest);
            // Assert
            CollectionAssert.AreEquivalent(new[] { TravelRequestAction.Approve }, allowedActions);
        }

        [Test]
        public void Finish_WhenBookTickets_ShouldSetToBookingComplete()
        {
            // Arrange
            var travelRequest = new TravelRequest();
            SetTravelRequestStatus(travelRequest, TravelRequestState.BookTickets);
            var workflow = CreateTravelRequestWorkflow();
            // Act
            workflow.Finish(travelRequest);
            // Assert
            Assert.AreEqual(TravelRequestState.BookingComplete, travelRequest.Status);
        }

        [Test]
        public void GetAllowedActions_GivenBookTickets_ShouldBe_Finish()
        {
            // Arrange
            var travelRequest = new TravelRequest();
            SetTravelRequestStatus(travelRequest, TravelRequestState.BookTickets);
            var workflow = CreateTravelRequestWorkflow();
            // Act
            var allowedActions = workflow.GetAllowedActions(travelRequest);
            // Assert
            CollectionAssert.AreEquivalent(new[] { TravelRequestAction.Finish }, allowedActions);
        }
    }
}
