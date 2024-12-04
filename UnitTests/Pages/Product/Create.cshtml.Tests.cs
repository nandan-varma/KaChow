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

// Namespace for Product Page UnitTests
namespace UnitTests.Pages.Product
{
    public class CreateTest
    {
        #region TestSetup

        private static DefaultHttpContext _httpContextDefault;
        private static ModelStateDictionary _modelState;
        private static ActionContext _actionContext;
        private static ViewDataDictionary _viewData;
        private static TempDataDictionary _tempData;
        private static PageContext _pageContext;
        private static CreateModel _pageModel;
        private static JsonFileProductService _productService;

        [SetUp]
        public void TestInitialize()
        {
            // Arrange
            _httpContextDefault = new DefaultHttpContext();
            _modelState = new ModelStateDictionary();
            _actionContext = new ActionContext(_httpContextDefault, new Microsoft.AspNetCore.Routing.RouteData(), new PageActionDescriptor(), _modelState);
            _viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), _modelState);
            _tempData = new TempDataDictionary(_httpContextDefault, Mock.Of<ITempDataProvider>());
            _pageContext = new PageContext(_actionContext) { ViewData = _viewData };

            _productService = TestHelper.ProductService;
            _pageModel = new CreateModel(_productService) { PageContext = _pageContext };

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
        public void OnGet_Should_Return_Page()
        {
            // Act
            var result = _pageModel.OnGet();

            // Reset

            // Assert
            Assert.That(result, Is.TypeOf<PageResult>());
        }

        #endregion OnGet

        #region OnPost

        [Test]
        public void OnPost_Valid_ModelState_Should_Create_Product_And_Redirect_To_Index()
        {
            // Arrange
            _pageModel.Product = _productService.CreateData();
            _pageModel.ModelState.Clear(); // Ensure model state is valid

            // Act
            var result = (RedirectToPageResult)_pageModel.OnPost();

            // Reset

            // Assert
            Assert.That(_pageModel.ModelState.IsValid, Is.True);
            Assert.That(result.PageName, Is.EqualTo("./Index"));
        }

        [Test]
        public void OnPost_Invalid_ModelState_Should_Return_PageResult()
        {
            // Arrange
            _pageModel.Product = new ProductModel();
            _pageModel.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = _pageModel.OnPost();

            // Reset

            // Assert
            Assert.That(_pageModel.ModelState.IsValid, Is.False);
            Assert.That(result, Is.TypeOf<PageResult>());
        }

        [Test]
        public void OnPost_Valid_ModelState_Should_Create_Product_With_Material_And_Size()
        {
            // Arrange
            var product = _productService.CreateData();
            product.Material = ProductTypeEnum.Cotton;
            product.Size = ProductSizeEnum.M;
            _pageModel.Product = product;
            _pageModel.ModelState.Clear(); // Ensure model state is valid

            // Act
            var result = (RedirectToPageResult)_pageModel.OnPost();

            // Assert
            Assert.That(_pageModel.ModelState.IsValid, Is.True);
            Assert.That(_pageModel.Product.Material, Is.EqualTo(ProductTypeEnum.Cotton));
            Assert.That(_pageModel.Product.Size, Is.EqualTo(ProductSizeEnum.M));
            Assert.That(result.PageName, Is.EqualTo("./Index"));
        }

        #endregion OnPost
    }
}
