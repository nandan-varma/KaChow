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
using System.Linq;

namespace UnitTests.Pages.Product
{
    public class UpdateTest
    {
        #region TestSetup

        private DefaultHttpContext _httpContextDefault;
        private ModelStateDictionary _modelState;
        private ActionContext _actionContext;
        private ViewDataDictionary _viewData;
        private TempDataDictionary _tempData;
        private PageContext _pageContext;

        private UpdateModel _pageModel;
        private JsonFileProductService _productService;

        [SetUp]
        public void TestInitialize()
        {
            // Arrange
            _httpContextDefault = new DefaultHttpContext();
            _modelState = new ModelStateDictionary();
            _actionContext = new ActionContext(_httpContextDefault, _httpContextDefault.GetRouteData(), new PageActionDescriptor(), _modelState);
            var modelMetadataProvider = new EmptyModelMetadataProvider();
            _viewData = new ViewDataDictionary(modelMetadataProvider, _modelState);
            _tempData = new TempDataDictionary(_httpContextDefault, Mock.Of<ITempDataProvider>());

            _pageContext = new PageContext(_actionContext) { ViewData = _viewData };

            _productService = TestHelper.ProductService;
            _pageModel = new UpdateModel(_productService);

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
        public void Update_OnGet_Invalid_Product_Id_Should_Have_Null_Product()
        {
            // Act
            _pageModel.OnGet("non_null_or_empty");

            // Reset

            // Assert
            Assert.That(_pageModel.ModelState.IsValid, Is.True);
            Assert.That(_pageModel.Product, Is.Null);
        }

        [Test]
        public void Update_OnGet_Valid_Product_Id_Should_Have_Non_Null_Product()
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
        public void Update_OnPost_Should_Redirect_To_Index_Page()
        {
            // Arrange
            _pageModel.OnGet(_productService.GetAllData().First().Id);
            var product = _productService.CreateData();
            _pageModel.Product = product;

            // Act
            var result = (RedirectToPageResult)_pageModel.OnPost();

            // Reset

            // Assert
            Assert.That(_pageModel.ModelState.IsValid, Is.True);
            Assert.That(_pageModel.Product, Is.EqualTo(product));
            Assert.That(result.PageName, Is.EqualTo("./Index"));
        }

        [Test]
        public void Update_OnPost_Invalid_ModelState_Should_Return_Page_Result()
        {
            // Arrange
            _pageModel.Product = _productService.GetAllData().First();
            _pageModel.ModelState.AddModelError("Error", "Invalid model state");

            // Act
            var result = (PageResult)_pageModel.OnPost();

            // Reset

            // Assert
            Assert.That(_pageModel.ModelState.IsValid, Is.False);
        }

        #endregion OnPost
    }
}
