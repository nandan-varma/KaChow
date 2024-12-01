using ContosoCrafts.WebSite.Controllers;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

// Namespace for Controller UnitTests
namespace UnitTests.Controllers
{
    public class ProductsControllerTest
    {
        #region TestSetup

        private ProductsController _controller;
        private JsonFileProductService _productService;

        [SetUp]
        public void TestInitialize()
        {
            // Arrange
            _productService = TestHelper.ProductService;
            _controller = new ProductsController(_productService);

            // Reset
        }

        [TearDown]
        public void TestCleanup()
        {
            // Reset
        }

        #endregion TestSetup

        #region Get_Should_Return_All_Products_When_Products_Exist

        [Test]
        /// <summary>
        /// Tests that the Get method returns all products when products exist.
        /// </summary>
        public void Get_Should_Return_All_Products_When_Products_Exist()
        {
            // Arrange

            // Act
            var result = _controller.Get() as IEnumerable<ProductModel>;

            // Reset

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count(), Is.EqualTo(_productService.GetAllData().Count()));
        }

        #endregion Get_Should_Return_All_Products_When_Products_Exist

        #region Patch_ValidRating_Should_Return_Ok

        [Test]
        /// <summary>
        /// Tests that a valid rating update returns an Ok result.
        /// </summary>
        public void Patch_ValidRating_Should_Return_Ok()
        {
            // Arrange
            var validProduct = _productService.CreateData();

            var originalData = _productService.GetAllData().ToList();
            originalData.Add(validProduct);
            _productService.SaveData(originalData);

            var request = new ProductsController.RatingRequest
            {
                ProductId = validProduct.Id,
                Rating = 4
            };

            // Act
            var result = _controller.Patch(request);

            // Reset

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        #endregion Patch_ValidRating_Should_Return_Ok

        #region Patch_NullProductId_Should_Return_BadRequest

        [Test]
        /// <summary>
        /// Tests that a null product ID returns a BadRequest result.
        /// </summary>
        public void Patch_NullProductId_Should_Return_BadRequest()
        {
            // Arrange
            var request = new ProductsController.RatingRequest
            {
                ProductId = null,
                Rating = 3
            };

            // Act
            var result = _controller.Patch(request);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        #endregion Patch_NullProductId_Should_Return_BadRequest

        #region Patch_RatingOutOfBounds_Should_Return_BadRequest

        [Test]
        /// <summary>
        /// Tests that a rating out of bounds returns a BadRequest result.
        /// </summary>
        public void Patch_RatingOutOfBounds_Should_Return_BadRequest()
        {
            // Arrange
            var request = new ProductsController.RatingRequest
            {
                ProductId = "test-product-id",
                Rating = 6
            };

            // Act
            var result = _controller.Patch(request);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        #endregion Patch_RatingOutOfBounds_Should_Return_BadRequest

        #region Patch_InvalidProductId_Should_Return_BadRequest

        [Test]
        /// <summary>
        /// Tests that an invalid product ID returns a BadRequest result.
        /// </summary>
        public void Patch_InvalidProductId_Should_Return_BadRequest()
        {
            // Arrange
            var request = new ProductsController.RatingRequest
            {
                ProductId = "invalid-product-id",
                Rating = 3
            };

            // Act
            var result = _controller.Patch(request);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        #endregion Patch_InvalidProductId_Should_Return_BadRequest

        #region Patch_ValidRating_Should_Increase_Product_Rating

        [Test]
        /// <summary>
        /// Tests that a valid rating increases the product's ratings.
        /// </summary>
        public void Patch_ValidRating_Should_Increase_Product_Rating()
        {
            // Arrange
            var existingProduct = _productService.GetAllData().First();
            var request = new ProductsController.RatingRequest
            {
                ProductId = existingProduct.Id,
                Rating = 5
            };

            // Act
            _controller.Patch(request);
            var updatedProduct = _productService.GetAllData().First(p => p.Id == request.ProductId);

            // Reset

            // Assert
            Assert.That(updatedProduct.Ratings, Does.Contain(5));
        }

        #endregion Patch_ValidRating_Should_Increase_Product_Rating

        #region Patch_InvalidRating_Should_Not_Change_Ratings

        [Test]
        /// <summary>
        /// Tests that an invalid rating does not change the ratings.
        /// </summary>
        public void Patch_InvalidRating_Should_Not_Change_Ratings()
        {
            // Arrange
            var existingProduct = _productService.GetAllData().First();
            var request = new ProductsController.RatingRequest
            {
                ProductId = existingProduct.Id,
                Rating = 7
            };
            var originalRatings = existingProduct.Ratings.ToArray();

            // Act
            _controller.Patch(request);
            var updatedProduct = _productService.GetAllData().First(p => p.Id == request.ProductId);

            // Reset

            // Assert
            Assert.That(updatedProduct.Ratings, Is.EqualTo(originalRatings));
        }

        #endregion Patch_InvalidRating_Should_Not_Change_Ratings

        #region Patch_NonexistentProductId_Should_Not_Add_Rating

        [Test]
        /// <summary>
        /// Tests that a nonexistent product ID does not add a rating.
        /// </summary>
        public void Patch_NonexistentProductId_Should_Not_Add_Rating()
        {
            // Arrange
            var request = new ProductsController.RatingRequest
            {
                ProductId = "nonexistent-product-id",
                Rating = 3
            };

            // Act
            var result = _controller.Patch(request);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        #endregion Patch_NonexistentProductId_Should_Not_Add_Rating

        #region Patch_EmptyProductId_Should_Return_BadRequest

        [Test]
        /// <summary>
        /// Tests that an empty product ID returns a BadRequest result.
        /// </summary>
        public void Patch_EmptyProductId_Should_Return_BadRequest()
        {
            // Arrange
            var request = new ProductsController.RatingRequest
            {
                ProductId = "",
                Rating = 3
            };

            // Act
            var result = _controller.Patch(request);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        #endregion Patch_EmptyProductId_Should_Return_BadRequest
    }
}
