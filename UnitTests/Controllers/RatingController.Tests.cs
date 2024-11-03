using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;
using System.Collections.Generic;
using ContosoCrafts.WebSite.Controllers;
using ContosoCrafts.WebSite.Services;
using Moq;
using System.Linq;
using ContosoCrafts.WebSite.Models;

namespace UnitTests.Controllers
{
    public class RatingControllerTests
    {
        private RatingService RatingService;
        private JsonFileProductService ProductService;
        private RatingController Controller;
        private DefaultHttpContext HttpContext;
        private Mock<IWebHostEnvironment> WebHostEnvironmentMock;
        private IHttpContextAccessor HttpContextAccessor;
        private IEnumerable<ProductModel> OriginalProducts;

        [SetUp]
        public void SetUp()
        {
            #region TestSetup
            HttpContext = new DefaultHttpContext();
            HttpContextAccessor = new HttpContextAccessor { HttpContext = HttpContext };
            WebHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            WebHostEnvironmentMock.Setup(m => m.WebRootPath).Returns("../../../../src/wwwroot");
            RatingService = new RatingService(HttpContextAccessor);
            ProductService = new JsonFileProductService(WebHostEnvironmentMock.Object);
            Controller = new RatingController(RatingService, ProductService);
            OriginalProducts = ProductService.GetAllData();
            #endregion
        }

        [TearDown]
        public void TearDown()
        {
            ProductService.SaveData(OriginalProducts);
        }

        [Test]
        /// <summary>
        /// Tests that adding an invalid rating returns a BadRequest result.
        /// </summary>
        public void AddRating_InvalidRating_Should_Return_BadRequest()
        {
            // Arrange
            var ProductId = "testProductId";
            var Rating = -1;

            // Act
            var Result = Controller.AddRating(ProductId, Rating);

            // Assert
            Assert.That(Result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        /// <summary>
        /// Tests that adding a rating for a non-existent product returns a NotFound result.
        /// </summary>
        public void AddRating_NonExistentProduct_Should_Return_NotFound()
        {
            // Arrange
            var ProductId = "nonExistentProductId";
            var Rating = 3;

            // Act
            var Result = Controller.AddRating(ProductId, Rating);

            // Assert
            Assert.That(Result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(((NotFoundObjectResult)Result).Value, Is.EqualTo("Product not found."));
        }

        [Test]
        /// <summary>
        /// Tests that adding a valid rating returns an Ok result.
        /// </summary>
        public void AddRating_ValidRating_Should_Return_Ok()
        {
            // Arrange
            var ProductId = ProductService.GetAllData().First().Id;
            var Rating = 5;

            // Act
            var Result = Controller.AddRating(ProductId, Rating);

            // Assert
            Assert.That(Result, Is.InstanceOf<OkResult>());
        }

        [Test]
        /// <summary>
        /// Tests that adding a rating with a null or empty product ID returns a BadRequest result.
        /// </summary>
        public void AddRating_NullOrEmptyProductId_Should_Return_BadRequest()
        {
            // Arrange
            string ProductId = null;
            var Rating = 3;

            // Act
            var Result = Controller.AddRating(ProductId, Rating);

            // Assert
            Assert.That(Result, Is.InstanceOf<BadRequestObjectResult>());
            Assert.That(((BadRequestObjectResult)Result).Value, Is.EqualTo("Product ID cannot be null or empty."));

            // Arrange with empty string
            ProductId = string.Empty;

            // Act
            Result = Controller.AddRating(ProductId, Rating);

            // Assert
            Assert.That(Result, Is.InstanceOf<BadRequestObjectResult>());
            Assert.That(((BadRequestObjectResult)Result).Value, Is.EqualTo("Product ID cannot be null or empty."));
        }

        [Test]
        /// <summary>
        /// Tests that adding a rating when the user already rated the product returns a NotFound result.
        /// </summary>
        public void AddRating_UserAlreadyRatedProduct_Should_Return_NotFound()
        {
            // Arrange
            var ProductId = "testProductId";
            var Rating = 5;

            var UserRatings = new Dictionary<string, int> { { ProductId, Rating } };
            HttpContext.Items["UserRatings"] = UserRatings;

            // Act
            var Result = Controller.AddRating(ProductId, Rating);

            // Assert
            Assert.That(Result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        /// <summary>
        /// Tests that adding a valid rating succeeds and returns an Ok result.
        /// </summary>
        public void AddRating_ValidCall_Should_Return_Ok_And_AddRating()
        {
            // Arrange
            var ProductId = ProductService.GetAllData().First().Id;
            var Rating = 5;

            var Controller = new RatingController(new RatingService(new HttpContextAccessor()), ProductService);

            // Act
            var Result = Controller.AddRating(ProductId, Rating) as OkResult;

            // Assert
            Assert.That(Result, Is.Not.Null);
            Assert.That(Result.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        }

        [Test]
        /// <summary>
        /// Tests that adding a rating when the user has already rated returns a NotFound result.
        /// </summary>
        public void AddRating_UserAlreadyRated_Should_Return_NotFound()
        {
            // Arrange
            var ProductId = "testProductId";
            var Rating = 5;

            var UserRatings = new Dictionary<string, int> { { ProductId, Rating } };
            HttpContext.Items["UserRatings"] = UserRatings;

            // Act
            var Result = Controller.AddRating(ProductId, Rating);

            // Assert
            Assert.That(Result, Is.InstanceOf<NotFoundObjectResult>());
            Assert.That(((NotFoundObjectResult)Result).Value, Is.EqualTo("Product not found."));
        }
    }
}
