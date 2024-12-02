using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;

namespace UnitTests.Controllers
{
    public class ErrorControllerTest
    {
        #region TestSetup

        private ErrorController _controller;

        [SetUp]
        public void TestInitialize()
        {
            // Arrange
            _controller = new ErrorController();

            // Reset
        }

        [TearDown]
        public void TestCleanup()
        {
            // Reset
        }

        #endregion TestSetup

        #region HandleError_ValidStatusCode_Should_Return_ErrorView

        [Test]
        /// <summary>
        /// Tests that HandleError returns the correct view for a given status code.
        /// </summary>
        public void HandleError_ValidStatusCode_Should_Return_ErrorView()
        {
            // Arrange
            var statusCode = 404;

            // Act
            var result = _controller.HandleError(statusCode) as ViewResult;

            // Reset

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Error"));
        }

        #endregion HandleError_ValidStatusCode_Should_Return_ErrorView

        #region HandleError_DifferentStatusCodes_Should_Return_ErrorView

        [Test]
        /// <summary>
        /// Tests that HandleError handles different status codes correctly.
        /// </summary>
        public void HandleError_DifferentStatusCodes_Should_Return_ErrorView()
        {
            // Arrange
            var statusCodes = new[] { 400, 401, 403, 500 };

            foreach (var statusCode in statusCodes)
            {
                // Act
                var result = _controller.HandleError(statusCode) as ViewResult;

                // Reset

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result.ViewName, Is.EqualTo("Error"));
            }
        }

        #endregion HandleError_DifferentStatusCodes_Should_Return_ErrorView

        #region HandleError_InvalidStatusCode_Should_Return_ErrorView

        [Test]
        /// <summary>
        /// Tests that HandleError handles invalid status codes gracefully.
        /// </summary>
        public void HandleError_InvalidStatusCode_Should_Return_ErrorView()
        {
            // Arrange
            var invalidStatusCode = -1;

            // Act
            var result = _controller.HandleError(invalidStatusCode) as ViewResult;

            // Reset

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ViewName, Is.EqualTo("Error"));
        }

        #endregion HandleError_InvalidStatusCode_Should_Return_ErrorView
    }
}
