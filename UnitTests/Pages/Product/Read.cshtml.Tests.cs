using ContosoCrafts.WebSite.Pages.Product;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UnitTests.Pages.Product
{
    [TestFixture]
    public class ReadTests
    {
        #region TestSetup
        private JsonFileProductService ProductService;
        private DetailsModel DetailsModel;
        private IEnumerable<ProductModel> OriginalData;

        [SetUp]
        public void Setup()
        {
            ProductService = TestHelper.ProductService;
            DetailsModel = new DetailsModel(ProductService);
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion TestSetup

        #region OnGet

        /// <summary>
        /// Tests that OnGet returns NotFound when ID is null.
        /// </summary>
        [Test]
        public void OnGet_NullId_Should_Return_NotFound()
        {
            // Act
            var result = DetailsModel.OnGet(null);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        /// <summary>
        /// Tests that OnGet returns NotFound when ID is empty.
        /// </summary>
        [Test]
        public void OnGet_EmptyId_Should_Return_NotFound()
        {
            // Act
            var result = DetailsModel.OnGet(string.Empty);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        /// <summary>
        /// Tests that OnGet returns NotFound for a non-existent product ID.
        /// </summary>
        [Test]
        public void OnGet_NonExistentProductId_Should_Return_NotFound()
        {
            // Arrange
            var id = "non-existent-id";

            // Act
            var result = DetailsModel.OnGet(id);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        /// <summary>
        /// Tests that OnGet returns the page when the product ID exists.
        /// </summary>
        [Test]
        public void OnGet_ExistingProductId_Should_Return_Page()
        {
            // Arrange
            var product = ProductService.GetAllData().First();
            var id = product.Id;

            // Act
            var result = DetailsModel.OnGet(id);

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(DetailsModel.Product.Id, Is.EqualTo(product.Id));
        }

        /// <summary>
        /// Tests that OnGet returns NotFound for a whitespace ID.
        /// </summary>
        [Test]
        public void OnGet_WhitespaceId_Should_Return_NotFound()
        {
            // Act
            var result = DetailsModel.OnGet("   ");

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        /// <summary>
        /// Tests that multiple calls with the same valid ID return the correct product.
        /// </summary>
        [Test]
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

        /// <summary>
        /// Tests that OnGet returns NotFound for an invalid ID.
        /// </summary>
        [Test]
        public void OnGet_InvalidId_Should_Return_NotFound()
        {
            // Arrange
            var id = "invalid-id";

            // Act
            var result = DetailsModel.OnGet(id);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        /// <summary>
        /// Tests that OnGet returns NotFound when the product list is empty.
        /// </summary>
        [Test]
        public void OnGet_EmptyProductList_Should_Return_NotFound()
        {
            // Arrange
            var emptyProductList = new ProductModel[] { };
            OriginalData = ProductService.GetAllData();
            ProductService.SaveData(emptyProductList);

            // Act
            var result = DetailsModel.OnGet("any-id");
            ProductService.SaveData(OriginalData);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        #endregion OnGet
    }
}
