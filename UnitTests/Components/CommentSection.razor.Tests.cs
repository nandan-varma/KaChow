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

        /// <summary>
        /// Empty comment string should not submit comment
        /// </summary>
        [Test]
        public void Edit_Form_Should_Not_Submit_InValid_Empty_Comment()
        {
            // Arrange
            var product = TestHelper.ProductService.GetAllData().First();
            var component = RenderComponent<CommentSection>(parameters => parameters.Add(p => p.Id, product.Id));
            var textArea = component.Find("textarea");
            var value = textArea.GetAttribute("value");
            var button = component.Find("#submit-button");
            var commentListSize = TestHelper.ProductService.GetCommentListSize(product.Id);

            // Act
            button.Click();

            // Assert
            Assert.That(value, Is.EqualTo(null));
            Assert.That(TestHelper.ProductService.GetCommentListSize(product.Id), Is.EqualTo(commentListSize));
        }

        /// <summary>
        /// Valid non empty comment should be submitted when form submits
        /// </summary>
        [Test]
        public void EditForm_Should_Submit_Valid_Non_Empty_Comment()
        {
            // Arrange
            var product = TestHelper.ProductService.GetAllData().First();
            var component = RenderComponent<CommentSection>(parameters => parameters.Add(p => p.Id, product.Id));
            var textArea = component.Find("textarea");
            textArea.SetAttribute("value", "Comment");
            textArea.Change("Comment");
            var value = textArea.GetAttribute("value");
            var button = component.Find("#submit-button");
            var commentListSize = TestHelper.ProductService.GetCommentListSize(product.Id);

            // Act
            button.Click();

            // Assert
            Assert.That(value, Is.EqualTo("Comment"));
            Assert.That(TestHelper.ProductService.GetProductById(product.Id).CommentList.Last().Comment, Is.EqualTo("Comment"));
            Assert.That(TestHelper.ProductService.GetCommentListSize(product.Id), Is.EqualTo(commentListSize + 1));
        }

        /// <summary>
        /// PageNumber should not be incremented when button is clicked and on last page
        /// </summary>
        [Test]
        public void IncrementButton_Should_Not_Increment_Past_Comment_List_Size_Divided_By_Page_Size()
        {
            // Arrange
            var product = TestHelper.ProductService.GetAllData().First();
            var component = RenderComponent<CommentSection>(parameters => parameters.Add(p => p.Id, product.Id));
            var button = component.Find("#Increment-button");
            product.CommentList.Add(new CommentModel());
            TestHelper.ProductService.UpdateData(product);
            var originalPageNumber = component.Instance.PageNumber;
            var commentListSize = TestHelper.ProductService.GetCommentListSize(product.Id);

            // Act
            button.Click();

            // Assert
            Assert.That(commentListSize, Is.EqualTo(1));
            Assert.That(originalPageNumber, Is.EqualTo(component.Instance.PageNumber));
        }

        /// <summary>
        /// PageNumber should not be incremented when button is clicked and on last page
        /// </summary>
        [Test]
        public void IncrementButton_Should_Increment_When_Comment_List_Size_Is_Greater_Than_Page_Size()
        {
            // Arrange
            var product = TestHelper.ProductService.GetAllData().First();
            var component = RenderComponent<CommentSection>(parameters => parameters.Add(p => p.Id, product.Id));
            var button = component.Find("#Increment-button");
            for (int i = 0; i < 11; i++)
            {
                product.CommentList.Add(new CommentModel());
            }
            TestHelper.ProductService.UpdateData(product);
            var originalPageNumber = component.Instance.PageNumber;
            var commentListSize = TestHelper.ProductService.GetCommentListSize(product.Id);

            // Act
            button.Click();

            // Assert
            Assert.That(commentListSize, Is.EqualTo(11));
            Assert.That(originalPageNumber + 1, Is.EqualTo(component.Instance.PageNumber));
        }

        /// <summary>
        /// Decrement button should not decrement past 0 when clicked
        /// </summary>
        [Test]
        public void DecrementButton_Should_Not_Decrement_Lower_Than_Zero()
        {
            // Arrange
            var product = TestHelper.ProductService.GetAllData().First();
            var component = RenderComponent<CommentSection>(parameters => parameters.Add(p => p.Id, product.Id));
            var button = component.Find("#Decrement-button");
            component.Instance.PageNumber = 0;

            // Act
            button.Click();

            // Assert
            Assert.That(component.Instance.PageNumber, Is.EqualTo(0));
        }

        /// <summary>
        /// Decrement button should decrement when PageNumber > 0
        /// </summary>
        [Test]
        public void DecrementButton_Should_Decrement_When_PageNumber_Greater_Than_Zero()
        {
            // Arrange
            var product = TestHelper.ProductService.GetAllData().First();
            var component = RenderComponent<CommentSection>(parameters => parameters.Add(p => p.Id, product.Id));
            var button = component.Find("#Decrement-button");
            component.Instance.PageNumber = 1;
            var originalPageNumber = component.Instance.PageNumber;

            // Act
            button.Click();

            // Assert
            Assert.That(component.Instance.PageNumber, Is.EqualTo(originalPageNumber - 1));
        }

        #endregion Components
    }
}
