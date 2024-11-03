using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using ContosoCrafts.WebSite.Controllers;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Moq;

namespace UnitTests.Controllers
{
    public class ProductsControllerTests
    {
        private ProductsController Controller;
        private Mock<IWebHostEnvironment> WebHostEnvironmentMock;
        private JsonFileProductService ProductService;
        private IEnumerable<ProductModel> OriginalProductData;

        [SetUp]
        public void Setup()
        {
            #region TestSetup
            WebHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            WebHostEnvironmentMock.Setup(m => m.WebRootPath).Returns("../../../../src/wwwroot");
            ProductService = new JsonFileProductService(WebHostEnvironmentMock.Object);
            Controller = new ProductsController(ProductService);
            OriginalProductData = ProductService.GetAllData();
            #endregion
        }

        [TearDown]
        public void TearDown()
        {
            ProductService.SaveData(OriginalProductData);
        }

        [Test]
        /// <summary>
        /// Tests that the Get method returns all products when products exist.
        /// </summary>
        public void Get_Should_Return_All_Products_When_Products_Exist()
        {
            // Arrange
            // Act
            var Result = Controller.Get() as IEnumerable<ProductModel>;

            // Assert
            Assert.That(Result, Is.Not.Null);
            Assert.That(Result, Is.Not.Empty);
            Assert.That(Result.Count(), Is.EqualTo(ProductService.GetAllData().Count()));
        }

        [Test]
        /// <summary>
        /// Tests that a valid rating update returns an Ok result.
        /// </summary>
        public void Patch_ValidRating_Should_Return_Ok()
        {
            // Arrange
            var ValidProduct = new ProductModel
            {
                Id = "valid-product-id",
                Name = "Test Product",
                Ratings = new int[] { }
            };

            var OriginalData = ProductService.GetAllData().ToList();
            OriginalData.Add(ValidProduct);
            ProductService.SaveData(OriginalData);

            var Request = new ProductsController.RatingRequest
            {
                ProductId = ValidProduct.Id,
                Rating = 4
            };

            // Act
            var Result = Controller.Patch(Request);

            // Assert
            Assert.That(Result, Is.Not.Null);
            Assert.That(Result, Is.InstanceOf<OkResult>());
        }

        [Test]
        /// <summary>
        /// Tests that a null product ID returns a BadRequest result.
        /// </summary>
        public void Patch_NullProductId_Should_Return_BadRequest()
        {
            // Arrange
            var Request = new ProductsController.RatingRequest
            {
                ProductId = null,
                Rating = 3
            };

            // Act
            var Result = Controller.Patch(Request);

            // Assert
            Assert.That(Result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        /// <summary>
        /// Tests that a rating out of bounds returns a BadRequest result.
        /// </summary>
        public void Patch_RatingOutOfBounds_Should_Return_BadRequest()
        {
            // Arrange
            var Request = new ProductsController.RatingRequest
            {
                ProductId = "test-product-id",
                Rating = 6
            };

            // Act
            var Result = Controller.Patch(Request);

            // Assert
            Assert.That(Result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        /// <summary>
        /// Tests that an invalid product ID returns a BadRequest result.
        /// </summary>
        public void Patch_InvalidProductId_Should_Return_BadRequest()
        {
            // Arrange
            var Request = new ProductsController.RatingRequest
            {
                ProductId = "invalid-product-id",
                Rating = 3
            };

            // Act
            var Result = Controller.Patch(Request);

            // Assert
            Assert.That(Result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        /// <summary>
        /// Tests that a valid rating increases the product's ratings.
        /// </summary>
        public void Patch_ValidRating_Should_Increase_Product_Rating()
        {
            // Arrange
            var ExistingProduct = ProductService.GetAllData().First();
            var Request = new ProductsController.RatingRequest
            {
                ProductId = ExistingProduct.Id,
                Rating = 5
            };

            // Act
            Controller.Patch(Request);
            var UpdatedProduct = ProductService.GetAllData().First(p => p.Id == Request.ProductId);

            // Assert
            Assert.That(UpdatedProduct.Ratings, Does.Contain(5));
        }

        [Test]
        /// <summary>
        /// Tests that an invalid rating does not change the ratings.
        /// </summary>
        public void Patch_InvalidRating_Should_Not_Change_Ratings()
        {
            // Arrange
            var ExistingProduct = ProductService.GetAllData().First();
            var Request = new ProductsController.RatingRequest
            {
                ProductId = ExistingProduct.Id,
                Rating = 7
            };
            var OriginalRatings = ExistingProduct.Ratings.ToArray();

            // Act
            Controller.Patch(Request);
            var UpdatedProduct = ProductService.GetAllData().First(p => p.Id == Request.ProductId);

            // Assert
            Assert.That(UpdatedProduct.Ratings, Is.EqualTo(OriginalRatings));
        }

        [Test]
        /// <summary>
        /// Tests that a nonexistent product ID does not add a rating.
        /// </summary>
        public void Patch_NonexistentProductId_Should_Not_Add_Rating()
        {
            // Arrange
            var Request = new ProductsController.RatingRequest
            {
                ProductId = "nonexistent-product-id",
                Rating = 3
            };

            // Act
            var Result = Controller.Patch(Request);

            // Assert
            Assert.That(Result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        /// <summary>
        /// Tests that an empty product ID returns a BadRequest result.
        /// </summary>
        public void Patch_EmptyProductId_Should_Return_BadRequest()
        {
            // Arrange
            var Request = new ProductsController.RatingRequest
            {
                ProductId = "",
                Rating = 3
            };

            // Act
            var Result = Controller.Patch(Request);

            // Assert
            Assert.That(Result, Is.InstanceOf<BadRequestResult>());
        }
    }
}
