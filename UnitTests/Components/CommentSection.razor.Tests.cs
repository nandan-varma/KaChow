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

        /// <summary>
        /// PageNumber * PAGE_SIZE < commentlist.Count should increment
        /// </summary>
        [Test]
        public void IncrementPageNumber_Should_Increment_When_Page_Number_Is_Less_Than_Comment_List_Size_Divided_By_Page_Size_Minus_One()
        {
            // Arrange
            ProductModel model = new ProductModel();
            model.CommentList = new List<CommentModel>();
            for (int i = 0; i < 20; i++)
            {
                CommentModel commentModel = new CommentModel();
                commentModel.Comment = i.ToString();
                model.CommentList.Add(commentModel);
            }
            TestHelper.ProductService.CreateProduct(model);
            var component = RenderComponent<CommentSection>(parameters => parameters.Add(p => p.Id, model.Id));
            var PageNumber = component.Instance.PageNumber;

            // Act
            component.Instance.IncrementPageNumber();

            // Assert
            Assert.That(component.Instance.PageNumber, Is.EqualTo(PageNumber + 1));
        }

        #endregion IncrementPageNumber

        #region DecrementPageNumber
        
        /// <summary>
        /// PageNumber should never decrement lower than 0
        /// </summary>
        [Test]
        public void DecrementPageNumber_Should_Not_Decrement_Past_Zero()
        {
            // Arrange
            var component = RenderComponent<CommentSection>(parameters => parameters.Add(p => p.Id, "1"));
            component.Instance.PageNumber = 0;

            // Act
            component.Instance.DecrementPageNumber();

            // Assert
            Assert.That(component.Instance.PageNumber, Is.EqualTo(0));
        }

        /// <summary>
        /// If PageNumber is positive non zero, PageNumber should decrement
        /// </summary>
        [Test]
        public void DecrementPageNumber_Should_Decrement_If_Page_Number_Is_Greater_Than_Zero()
        {
            // Arrange
            var component = RenderComponent<CommentSection>(parameters => parameters.Add(p => p.Id, "1"));
            component.Instance.PageNumber = 1;

            // Act
            component.Instance.DecrementPageNumber();

            // Assert
            Assert.That(component.Instance.PageNumber, Is.EqualTo(0));
        }

        #endregion DecrementPageNumber

        #region Components

        #endregion Components
    }
}
