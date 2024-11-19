using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;

namespace UnitTests.Controllers
{
    public class ErrorControllerTests
    {
        private ErrorController Controller;

        [SetUp]
        public void SetUp()
        {
            Controller = new ErrorController();
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        /// <summary>
        /// Tests that HandleError returns the correct view for a given status code.
        /// </summary>
        public void HandleError_ValidStatusCode_Should_Return_ErrorView()
        {
            // Arrange
            var statusCode = 404;

            // Act
            var Result = Controller.HandleError(statusCode) as ViewResult;

            // Assert
            Assert.That(Result, Is.Not.Null);
            Assert.That(Result.ViewName, Is.EqualTo("Error"));
        }

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
                var Result = Controller.HandleError(statusCode) as ViewResult;

                // Assert
                Assert.That(Result, Is.Not.Null);
                Assert.That(Result.ViewName, Is.EqualTo("Error"));
            }
        }

        [Test]
        /// <summary>
        /// Tests that HandleError handles invalid status codes gracefully.
        /// </summary>
        public void HandleError_InvalidStatusCode_Should_Return_ErrorView()
        {
            // Arrange
            var invalidStatusCode = -1;

            // Act
            var Result = Controller.HandleError(invalidStatusCode) as ViewResult;

            // Assert
            Assert.That(Result, Is.Not.Null);
            Assert.That(Result.ViewName, Is.EqualTo("Error"));
        }
    }
}
