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
    public class ReadTest
    {
        #region TestSetup

        private JsonFileProductService _productService;
        private DetailsModel _detailsModel;
        private IEnumerable<ProductModel> _originalData;

        [SetUp]
        public void TestInitialize()
        {
            // Arrange
            _productService = TestHelper.ProductService;
            _detailsModel = new DetailsModel(_productService);

            // Reset
        }

        [TearDown]
        public void TestCleanup()
        {
            // Reset
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
            var result = _detailsModel.OnGet(null);

            // Reset

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
            var result = _detailsModel.OnGet(string.Empty);

            // Reset

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
            var id = "non-existent-id";

            // Act
            var result = _detailsModel.OnGet(id);

            // Reset

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
            var product = _productService.GetAllData().First();
            var id = product.Id;

            // Act
            var result = _detailsModel.OnGet(id);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(_detailsModel.Product.Id, Is.EqualTo(product.Id));
        }

        [Test]
        /// <summary>
        /// Tests that OnGet returns NotFound for a whitespace ID.
        /// </summary>
        public void OnGet_WhitespaceId_Should_Return_NotFound()
        {
            // Act
            var result = _detailsModel.OnGet("   ");

            // Reset

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
            var product = _productService.GetAllData().First();
            var id = product.Id;

            // Act
            var result1 = _detailsModel.OnGet(id);
            var result2 = _detailsModel.OnGet(id);

            // Reset

            // Assert
            Assert.That(result1, Is.InstanceOf<PageResult>());
            Assert.That(result2, Is.InstanceOf<PageResult>());
            Assert.That(_detailsModel.Product.Id, Is.EqualTo(product.Id));
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
            var result = _detailsModel.OnGet(id);

            // Reset

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
            var emptyProductList = new ProductModel[] { };
            _originalData = _productService.GetAllData();
            _productService.SaveData(emptyProductList);

            // Act
            var result = _detailsModel.OnGet("any-id");
            _productService.SaveData(_originalData);

            // Reset

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        #endregion OnGet
    }
}
