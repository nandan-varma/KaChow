using ContosoCrafts.Pages.Product;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

// Namespace for Product Page UnitTests
namespace UnitTests.Pages.Product
{
    [TestFixture]
    public class ReadTests
    {
        #region TestSetup
        private Mock<IWebHostEnvironment> WebHostEnvironment;
        private JsonFileProductService ProductService;
        private DetailsModel DetailsModel;

        private IEnumerable<ProductModel> OriginalProductData;

        [SetUp]
        public void Setup()
        {
            // Initialize the product service with a test data setup
            WebHostEnvironment = new Mock<IWebHostEnvironment>();
            WebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/wwwroot");
            ProductService = new JsonFileProductService(WebHostEnvironment.Object); // Assume this loads data from a test file or a mock setup
            DetailsModel = new DetailsModel(ProductService);

            OriginalProductData = ProductService.GetAllData();
        }

        [TearDown]
        public void TearDown()
        {
            // Restore the original data after each test
            ProductService.SaveData(OriginalProductData);
        }

        #endregion TestSetup

        #region OnGet
        [Test]
        /// <summary>
        /// Tests that OnGet returns NotFound when ID is null.
        /// </summary>
        public void OnGet_NullId_Should_Return_NotFound()
        {
            // Act
            var result = DetailsModel.OnGet(null);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        /// <summary>
        /// Tests that OnGet returns NotFound when ID is empty.
        /// </summary>
        public void OnGet_EmptyId_Should_Return_NotFound()
        {
            // Act
            var result = DetailsModel.OnGet(string.Empty);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        /// <summary>
        /// Tests that OnGet returns NotFound for a non-existent product ID.
        /// </summary>
        public void OnGet_NonExistentProductId_Should_Return_NotFound()
        {
            // Arrange
            var id = "non-existent-id"; // ID that does not exist in the dataset

            // Act
            var result = DetailsModel.OnGet(id);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        /// <summary>
        /// Tests that OnGet returns the page when the product ID exists.
        /// </summary>
        public void OnGet_ExistingProductId_Should_Return_Page()
        {
            // Arrange
            var product = ProductService.GetAllData().First();
            var id = product.Id; // Use the ID of the first product

            // Act
            var result = DetailsModel.OnGet(id);

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(DetailsModel.Product.Id, Is.EqualTo(product.Id));
        }

        [Test]
        /// <summary>
        /// Tests that OnGet returns NotFound for a whitespace ID.
        /// </summary>
        public void OnGet_WhitespaceId_Should_Return_NotFound()
        {
            // Act
            var result = DetailsModel.OnGet("   ");

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        /// <summary>
        /// Tests that multiple calls with the same valid ID return the correct product.
        /// </summary>
        public void OnGet_MultipleCalls_Should_Return_CorrectProduct()
        {
            // Arrange
            var product = ProductService.GetAllData().First();
            var id = product.Id;

            // Act
            var result1 = DetailsModel.OnGet(id);
            var result2 = DetailsModel.OnGet(id);

            // Assert
            Assert.That(result1, Is.InstanceOf<PageResult>());
            Assert.That(result2, Is.InstanceOf<PageResult>());
            Assert.That(DetailsModel.Product.Id, Is.EqualTo(product.Id));
        }

        [Test]
        /// <summary>
        /// Tests that OnGet returns NotFound for an invalid ID.
        /// </summary>
        public void OnGet_InvalidId_Should_Return_NotFound()
        {
            // Arrange
            var id = "invalid-id";

            // Act
            var result = DetailsModel.OnGet(id);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        /// <summary>
        /// Tests that OnGet returns NotFound when the product list is empty.
        /// </summary>
        public void OnGet_EmptyProductList_Should_Return_NotFound()
        {
            // Arrange
            // Here you should ensure that GetAllData returns an empty list
            var emptyProductList = new ProductModel[] { };
            ProductService.SaveData(emptyProductList); // Hypothetical method to clear data for this test

            // Act
            var result = DetailsModel.OnGet("any-id");

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        #endregion OnGet
    }
}
