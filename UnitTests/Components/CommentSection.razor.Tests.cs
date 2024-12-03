using Bunit;
using ContosoCrafts.WebSite.Components;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Components
{
    class CommentSectionTest : Bunit.TestContext
    {
        #region TestSetup

        private Mock<IHttpContextAccessor> _httpContextAccessorMock;

        public CommentSectionTest()
        {
            // Arrange
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            Services.AddSingleton(TestHelper.ProductService);
            Services.AddSingleton(new RatingService(_httpContextAccessorMock.Object));

            // Reset
        }

        [SetUp]
        public void TestInitialize()
        {
            // Reset
        }

        [TearDown]
        public void TestCleanup()
        {
            // Reset
        }

        #endregion TestSetup

        #region SubmitComment

        /// <summary>
        /// Submit comment should not throw errors under no circumstances
        /// </summary>
        [Test]
        public void SubmitComment_Should_Not_Throw_Errors()
        {
            // Arrange
            var component = RenderComponent<CommentSection>(parameters => parameters.Add(p => p.Id, "hello"));

            // Act
            component.Instance.SubmitComment();

            // Assert
        }


        #endregion SubmitComment

        #region IncrementPageNumber

        #endregion IncrementPageNumber

        #region DecrementPageNumber

        #endregion DecrementPageNumber
    }
}
