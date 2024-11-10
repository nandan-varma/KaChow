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
        /// <summary>
        /// Tests that OnGet sets Products when data is available.
        /// </summary>
        public void OnGet_When_Data_Is_Available_Should_Set_Products()
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
        /// <summary>
        /// Tests that OnGet sets Products to empty when no data is available.
        /// </summary>
        public void OnGet_When_No_Data_Available_Should_Set_Products_To_Empty()
        {
            // Arrange
            ProductService.SaveData(new List<ProductModel>());

            // Act
            IndexModel.OnGet();

            // Assert
            Assert.That(IndexModel.Products, Is.Not.Null);
            Assert.That(IndexModel.Products, Is.Empty);
        }

        #endregion OnGet
    }
}
