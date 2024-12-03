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

        /// <summary>
        /// PageNumber = Count / PAGE_SIZE should not increment when IncrementPageNumber() is called
        /// </summary>
        [Test]
        public void IncrementPageNumber_Should_Not_Increment_Past_Comment_List_Size_Divided_By_Page_Size()
        {
            // Arrange
            ProductModel model = new ProductModel();
            model.CommentList = new List<CommentModel>();
            for (int i = 0; i < 10; i++)
            {
                CommentModel commentModel = new CommentModel();
                commentModel.Comment = i.ToString();
                model.CommentList.Add(commentModel);
            }
            TestHelper.ProductService.CreateProduct(model);
            var component = RenderComponent<CommentSection>(parameters => parameters.Add(p => p.Id, model.Id));
            component.Instance.PageNumber = model.CommentList.Count / 10;

            // Act
            component.Instance.IncrementPageNumber();

            // Assert
            Assert.That(component.Instance.PageNumber, Is.EqualTo(model.CommentList.Count / 10));
        }

        #endregion IncrementPageNumber

        #region DecrementPageNumber

        #endregion DecrementPageNumber
    }
}
