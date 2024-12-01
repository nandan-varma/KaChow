using ContosoCrafts.WebSite.Controllers;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Controllers
{
    public class RatingControllerTest
    {
        #region TestSetup

        private RatingService _ratingService;
        private JsonFileProductService _productService;
        private RatingController _controller;
        private DefaultHttpContext _httpContext;
        private IHttpContextAccessor _httpContextAccessor;

        [SetUp]
        public void TestInitialize()
        {
            // Arrange
            _httpContext = new DefaultHttpContext();
            _httpContextAccessor = new HttpContextAccessor { HttpContext = _httpContext };
            _ratingService = new RatingService(_httpContextAccessor);
            _productService = TestHelper.ProductService;
            _controller = new RatingController(_ratingService, _productService);

            // Reset
        }

        [TearDown]
        public void TestCleanup()
        {
            // Reset
        }

        #endregion TestSetup

        #region AddRating_InvalidRating_Should_Return_BadRequest

        [Test]
        /// <summary>
        /// Tests that adding an invalid rating returns a BadRequest result.
        /// </summary>
        public void AddRating_InvalidRating_Should_Return_BadRequest()
        {
            // Arrange
            var productId = "testProductId";
            var rating = -1;

            // Act
            var result = _controller.AddRating(productId, rating);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        #endregion AddRating_InvalidRating_Should_Return_BadRequest

        #region AddRating_NonExistentProduct_Should_Return_NotFound

        [Test]
        /// <summary>
        /// Tests that adding a rating for a non-existent product returns a NotFound result.
        /// </summary>
        public void AddRating_NonExistentProduct_Should_Return_NotFound()
        {
            // Arrange
            var productId = "nonExistentProductId";
            var rating = 3;

            // Act
            var result = _controller.AddRating(productId, rating);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(((NotFoundObjectResult)result).Value, Is.EqualTo("Product not found."));
        }

        #endregion AddRating_NonExistentProduct_Should_Return_NotFound

        #region AddRating_ValidRating_Should_Return_Ok

        [Test]
        /// <summary>
        /// Tests that adding a valid rating returns an Ok result.
        /// </summary>
        public void AddRating_ValidRating_Should_Return_Ok()
        {
            // Arrange
            var productId = _productService.GetAllData().First().Id;
            var rating = 5;

            // Act
            var result = _controller.AddRating(productId, rating);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        #endregion AddRating_ValidRating_Should_Return_Ok

        #region AddRating_NullOrEmptyProductId_Should_Return_BadRequest

        [Test]
        /// <summary>
        /// Tests that adding a rating with a null or empty product ID returns a BadRequest result.
        /// </summary>
        public void AddRating_NullOrEmptyProductId_Should_Return_BadRequest()
        {
            // Arrange
            string productId = null;
            var rating = 3;

            // Act
            var result = _controller.AddRating(productId, rating);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            Assert.That(((BadRequestObjectResult)result).Value, Is.EqualTo("Product ID cannot be null or empty."));

            // Arrange with empty string
            productId = string.Empty;

            // Act
            result = _controller.AddRating(productId, rating);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            Assert.That(((BadRequestObjectResult)result).Value, Is.EqualTo("Product ID cannot be null or empty."));
        }

        #endregion AddRating_NullOrEmptyProductId_Should_Return_BadRequest

        #region AddRating_UserAlreadyRatedProduct_Should_Return_NotFound

        [Test]
        /// <summary>
        /// Tests that adding a rating when the user already rated the product returns a NotFound result.
        /// </summary>
        public void AddRating_UserAlreadyRatedProduct_Should_Return_NotFound()
        {
            // Arrange
            var productId = "testProductId";
            var rating = 5;

            var userRatings = new Dictionary<string, int> { { productId, rating } };
            _httpContext.Items["UserRatings"] = userRatings;

            // Act
            var result = _controller.AddRating(productId, rating);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        #endregion AddRating_UserAlreadyRatedProduct_Should_Return_NotFound

        #region AddRating_ValidCall_Should_Return_Ok_And_AddRating

        [Test]
        /// <summary>
        /// Tests that adding a valid rating succeeds and returns an Ok result.
        /// </summary>
        public void AddRating_ValidCall_Should_Return_Ok_And_AddRating()
        {
            // Arrange
            var productId = _productService.GetAllData().First().Id;
            var rating = 5;

            var controller = new RatingController(new RatingService(new HttpContextAccessor()), _productService);

            // Act
            var result = controller.AddRating(productId, rating) as OkResult;

            // Reset

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }

        #endregion AddRating_ValidCall_Should_Return_Ok_And_AddRating

        #region AddRating_UserAlreadyRated_Should_Return_NotFound

        [Test]
        /// <summary>
        /// Tests that adding a rating when the user has already rated returns a NotFound result.
        /// </summary>
        public void AddRating_UserAlreadyRated_Should_Return_NotFound()
        {
            // Arrange
            var productId = "testProductId";
            var rating = 5;

            var userRatings = new Dictionary<string, int> { { productId, rating } };
            _httpContext.Items["UserRatings"] = userRatings;

            // Act
            var result = _controller.AddRating(productId, rating);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(((NotFoundObjectResult)result).Value, Is.EqualTo("Product not found."));
        }

        #endregion AddRating_UserAlreadyRated_Should_Return_NotFound
    }
}
