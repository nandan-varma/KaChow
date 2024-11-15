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
using System.Linq;

namespace UnitTests.Pages.Product
{
    public class IndexTests
    {
        #region TestSetup
        private static DefaultHttpContext HttpContextDefault;
        private static ModelStateDictionary ModelState;
        private static PageContext PageContext;
        private static IndexModel PageModel;
        private static JsonFileProductService ProductService;

        [SetUp]
        public void TestInitialize()
        {
            HttpContextDefault = new DefaultHttpContext();
            ModelState = new ModelStateDictionary();
            var actionContext = new ActionContext(HttpContextDefault, HttpContextDefault.GetRouteData(), new PageActionDescriptor(), ModelState);
            var modelMetadataProvider = new EmptyModelMetadataProvider();
            var viewData = new ViewDataDictionary(modelMetadataProvider, ModelState);
            var tempData = new TempDataDictionary(HttpContextDefault, Mock.Of<ITempDataProvider>());

            PageContext = new PageContext(actionContext) { ViewData = viewData };

            ProductService = TestHelper.ProductService;
            PageModel = new IndexModel(ProductService);
        }
        #endregion TestSetup

        #region OnGet
        /// <summary>
        /// Test to check if OnGet method retrieves and returns products correctly.
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_Products()
        {
            // Arrange
            var expectedProductCount = ProductService.GetAllData().Count();

            // Act
            PageModel.OnGet();

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.True);
            Assert.That(PageModel.Products.ToList().Count, Is.EqualTo(expectedProductCount));
        }

        /// <summary>
        /// Test to verify if OnGet method sets the Products property with data.
        /// </summary>
        [Test]
        public void OnGet_Should_Set_Products_Property()
        {
            // Arrange

            // Act
            PageModel.OnGet();

            // Assert
            Assert.That(PageModel.Products, Is.Not.Null);
            Assert.That(PageModel.Products.Any(), Is.True);
        }

        /// <summary>
        /// Test to confirm that OnGet method completes without throwing exceptions.
        /// </summary>
        [Test]
        public void OnGet_Should_Not_Throw_Exception()
        {
            // Arrange

            // Act & Assert
            Assert.DoesNotThrow(() => PageModel.OnGet());
        }
        #endregion OnGet

        [TearDown]
        public void TearDown()
        {
        }
    }
}
