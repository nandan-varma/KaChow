using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages.Product;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

// Namespace for Product Page UnitTests
namespace UnitTests.Pages.Product
{
    public class CreateTests
    {
        #region TestSetup
        private static DefaultHttpContext HttpContextDefault;
        private static ModelStateDictionary ModelState;
        private static ActionContext ActionContext;
        private static ViewDataDictionary ViewData;
        private static TempDataDictionary TempData;
        private static PageContext PageContext;
        private static CreateModel PageModel;
        private static JsonFileProductService ProductService;

        [SetUp]
        public void TestInitialize()
        {
            HttpContextDefault = new DefaultHttpContext();
            ModelState = new ModelStateDictionary();
            ActionContext = new ActionContext(HttpContextDefault, new Microsoft.AspNetCore.Routing.RouteData(), new PageActionDescriptor(), ModelState);
            ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), ModelState);
            TempData = new TempDataDictionary(HttpContextDefault, Mock.Of<ITempDataProvider>());
            PageContext = new PageContext(ActionContext) { ViewData = ViewData };

            ProductService = TestHelper.ProductService;
            PageModel = new CreateModel(ProductService) { PageContext = PageContext };
        }

        [TearDown]
        public void TestCleanup()
        {
        }

        #endregion TestSetup

        #region OnGet

        [Test]
        /// <summary>
        /// Tests that OnGet returns a PageResult.
        /// </summary>
        public void OnGet_Should_Return_Page()
        {
            // Act
            var result = PageModel.OnGet();

            // Assert
            Assert.That(result, Is.TypeOf<PageResult>());
        }

        #endregion OnGet

        #region OnPost

        [Test]
        /// <summary>
        /// Tests that OnPost creates a product and redirects to Index when model state is valid.
        /// </summary>
        public void OnPost_Valid_ModelState_Should_Create_Product_And_Redirect_To_Index()
        {
            // Arrange
            PageModel.Product = ProductService.CreateData();
            PageModel.ModelState.Clear(); // Ensure model state is valid

            // Act
            var result = (RedirectToPageResult)PageModel.OnPost();

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.True);
            Assert.That(result.PageName, Is.EqualTo("./Index"));
        }

        [Test]
        /// <summary>
        /// Tests that OnPost returns a PageResult when model state is invalid.
        /// </summary>
        public void OnPost_Invalid_ModelState_Should_Return_PageResult()
        {
            // Arrange
            PageModel.Product = new ProductModel();
            PageModel.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = PageModel.OnPost();

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.False);
            Assert.That(result, Is.TypeOf<PageResult>());
        }

        #endregion OnPost
    }
}
