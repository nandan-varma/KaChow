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

            var mockWebHostEnvironment = new Mock<Microsoft.AspNetCore.Hosting.IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/wwwroot");

            ProductService = new JsonFileProductService(mockWebHostEnvironment.Object);
            PageModel = new CreateModel(ProductService) { PageContext = PageContext };
        }
        #endregion TestSetup

        #region OnGet
        [Test]
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
        public void OnPost_Valid_ModelState_Should_Create_Product_And_Redirect_To_Index()
        {
            // Arrange
            PageModel.Product = new ProductModel
            {
                Id = "test_id",
                Name = "Test Product",
                Description = "This is a test product."
            };
            PageModel.ModelState.Clear(); // Ensure model state is valid

            // Act
            var result = (RedirectToPageResult)PageModel.OnPost();

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.True);
            Assert.That(result.PageName, Is.EqualTo("./Index"));
        }

        [Test]
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
