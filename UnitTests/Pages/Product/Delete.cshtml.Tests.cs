using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages.Product;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

// Namespace for Product Page UnitTests
namespace UnitTests.Pages.Product
{
    public class DeleteTest
    {
        #region TestSetup

        private static DefaultHttpContext _httpContextDefault;
        private static ModelStateDictionary _modelState;
        private static PageContext _pageContext;
        private static DeleteModel _pageModel;
        private static JsonFileProductService _productService;

        [SetUp]
        public void TestInitialize()
        {
            // Arrange
            _httpContextDefault = new DefaultHttpContext();
            _modelState = new ModelStateDictionary();

            var actionContext = new ActionContext(_httpContextDefault, new RouteData(), new PageActionDescriptor(), _modelState);
            var modelMetadataProvider = new EmptyModelMetadataProvider();
            var viewData = new ViewDataDictionary(modelMetadataProvider, _modelState);
            var tempData = new TempDataDictionary(_httpContextDefault, Mock.Of<ITempDataProvider>());

            _pageContext = new PageContext(actionContext) { ViewData = viewData };

            _productService = TestHelper.ProductService;
            _pageModel = new DeleteModel(_productService);

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
        public void Delete_OnGet_Invalid_Product_Id_Should_Have_Null_Product()
        {
            // Act
            _pageModel.OnGet("non_null_or_empty");

            // Reset

            // Assert
            Assert.That(_pageModel.ModelState.IsValid, Is.True);
            Assert.That(_pageModel.Product, Is.Null);
        }

        [Test]
        public void Delete_OnGet_Valid_Product_Id_Should_Have_Non_Null_Product()
        {
            // Arrange
            var data = _productService.GetAllData().First();

            // Act
            _pageModel.OnGet(data.Id);

            // Reset

            // Assert
            Assert.That(_pageModel.ModelState.IsValid, Is.True);
            Assert.That(_pageModel.Product.Id, Is.EqualTo(data.Id));
        }

        #endregion OnGet

        #region OnPost

        [Test]
        public void Delete_OnPost_Should_Redirect_To_Index_Page()
        {
            // Arrange
            _pageModel.OnGet(_productService.GetAllData().First().Id);

            // Act
            var result = (RedirectToPageResult)_pageModel.OnPost();

            // Reset

            // Assert
            Assert.That(_pageModel.ModelState.IsValid, Is.True);
            Assert.That(result.PageName, Is.EqualTo("./Index"));
        }

        [Test]
        public void Delete_OnPost_Invalid_ModelState_Should_Return_Page_Result()
        {
            // Arrange
            _pageModel.Product = _productService.GetAllData().First();
            _pageModel.ModelState.AddModelError("Error", "Invalid Model State");

            // Act
            var result = (PageResult)_pageModel.OnPost();

            // Reset

            // Assert
            Assert.That(_pageModel.ModelState.IsValid, Is.False);
        }

        [Test]
        public void Delete_OnPost_Delete_Fails_Should_Add_Error_To_ModelState_And_Return_Page()
        {
            // Arrange
            var product = _productService.CreateData();
            _pageModel = new DeleteModel(_productService) { Product = product };
            _productService.DeleteData(product.Id);

            // Act
            var result = _pageModel.OnPost();

            // Reset

            // Assert
            Assert.That(_pageModel.ModelState.IsValid, Is.False);
            Assert.That(_pageModel.ModelState[string.Empty].Errors.First().ErrorMessage, Is.EqualTo("Product not found or could not be deleted."));
            Assert.That(result, Is.TypeOf<PageResult>());
        }

        #endregion OnPost

        #region CreateProduct

        [Test]
        public void CreateProduct_Valid_Data_Should_Assign_Guid_And_Return_True()
        {
            // Arrange
            var newProduct = _productService.CreateData();

            // Act
            var result = _productService.CreateProduct(newProduct);

            // Reset

            // Assert
            Assert.That(result, Is.True, "CreateProduct should return true for valid data");
            Assert.That(newProduct.Id, Is.Not.Null.And.Not.Empty, "Product ID should not be null or empty");
            Assert.That(Guid.TryParse(newProduct.Id, out _), Is.True, "Assigned ID should be a valid GUID");
        }

        #endregion CreateProduct
    }
}
