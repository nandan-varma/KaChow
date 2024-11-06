using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

// Namespace for Page UnitTests
namespace UnitTests.Pages
{

    [TestFixture]
    public class IndexModelTests
    {
        #region TestSetup
        private IndexModel IndexModel;
        private Mock<ILogger<IndexModel>> LoggerMock;
        private Mock<IWebHostEnvironment> WebHostEnvironment;
        private JsonFileProductService ProductService;
        private IEnumerable<ProductModel> OriginalProductData;

        [SetUp]
        public void Setup()
        {
            // Create mocks for logger and product service
            LoggerMock = new Mock<ILogger<IndexModel>>();
            WebHostEnvironment = new Mock<IWebHostEnvironment>();
            WebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/wwwroot");
            ProductService = new JsonFileProductService(WebHostEnvironment.Object);
            OriginalProductData = ProductService.GetAllData();
            IndexModel = new IndexModel(LoggerMock.Object, ProductService);
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
        public void OnGet_WhenDataIsAvailable_Should_SetProducts()
        {
            // Arrange
            var testProducts = ProductService.GetAllData();

            // Act
            IndexModel.OnGet();

            // Assert
            Assert.That(IndexModel.Products, Is.Not.Null);
            Assert.That(IndexModel.Products.Count(), Is.EqualTo(testProducts.Count()));
        }

        [Test]
        public void OnGet_WhenNoDataAvailable_Should_SetProductsToEmpty()
        {
            // Arrange
            ProductService.SaveData(new List<ProductModel>());

            // Act
            IndexModel.OnGet();

            // Assert
            Assert.That(IndexModel.Products, Is.Not.Null);
            Assert.That(IndexModel.Products, Is.Empty);
        }

        [Test]
        public void MoveNext_WhenNextItemExists_Should_ReturnTrue()
        {
            // Arrange
            IndexModel.OnGet(); // Make sure products are loaded
            var enumerator = IndexModel.Products.GetEnumerator(); // Assuming Products is an IEnumerable<ProductModel>

            // Act
            bool result = enumerator.MoveNext(); // Call MoveNext

            // Assert
            Assert.That(result, Is.True); // Assuming there are products loaded
        }

        [Test]
        public void MoveNext_WhenNoMoreItems_Should_ReturnFalse()
        {
            // Arrange
            ProductService.SaveData(new List<ProductModel>()); // Clear products
            IndexModel.OnGet(); // Refresh the product list

            var enumerator = IndexModel.Products.GetEnumerator(); // Assuming Products is an IEnumerable<ProductModel>

            // Act
            bool result = enumerator.MoveNext(); // Call MoveNext

            // Assert
            Assert.That(result, Is.False); // No items should be available
        }

        [Test]
        public void ProductsEnumerator_WhenNoMoreItems_Should_ReturnFalse()
        {
            // Arrange
            ProductService.SaveData(new List<ProductModel>()); // Clear products
            IndexModel.OnGet(); // Refresh the product list

            // Act
            var enumerator = IndexModel.Products.GetEnumerator();
            bool hasMore = enumerator.MoveNext(); // Attempt to move when no items

            // Assert
            Assert.That(hasMore, Is.False);
        }

        #endregion OnGet

    }
}
