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
    public class IndexTest
    {
        #region TestSetup

        private static DefaultHttpContext _httpContextDefault;
        private static ModelStateDictionary _modelState;
        private static PageContext _pageContext;
        private static IndexModel _pageModel;
        private static JsonFileProductService _productService;

        [SetUp]
        public void TestInitialize()
        {
            // Arrange
            _httpContextDefault = new DefaultHttpContext();
            _modelState = new ModelStateDictionary();
            var actionContext = new ActionContext(_httpContextDefault, _httpContextDefault.GetRouteData(), new PageActionDescriptor(), _modelState);
            var modelMetadataProvider = new EmptyModelMetadataProvider();
            var viewData = new ViewDataDictionary(modelMetadataProvider, _modelState);
            var tempData = new TempDataDictionary(_httpContextDefault, Mock.Of<ITempDataProvider>());

            _pageContext = new PageContext(actionContext) { ViewData = viewData };

            _productService = TestHelper.ProductService;
            _pageModel = new IndexModel(_productService);

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
        public void OnGet_Valid_Should_Return_Products()
        {
            // Arrange
            var expectedProductCount = _productService.GetAllData().Count();

            // Act
            _pageModel.OnGet();

            // Reset

            // Assert
            Assert.That(_pageModel.ModelState.IsValid, Is.True);
            Assert.That(_pageModel.Products.ToList().Count, Is.EqualTo(expectedProductCount));
        }

        [Test]
        public void OnGet_Should_Set_Products_Property()
        {
            // Act
            _pageModel.OnGet();

            // Reset

            // Assert
            Assert.That(_pageModel.Products, Is.Not.Null);
            Assert.That(_pageModel.Products.Any(), Is.True);
        }

        [Test]
        public void OnGet_Should_Not_Throw_Exception()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => _pageModel.OnGet());
        }

        #endregion OnGet
    }
}
