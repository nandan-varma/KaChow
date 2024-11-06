using ContosoCrafts.WebSite.Pages.Product;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace UnitTests.Pages.Product
{
    public class IndexTests
    {
        #region TestSetup
        public static IUrlHelperFactory UrlHelperFactory;
        public static DefaultHttpContext HttpContextDefault;
        public static IWebHostEnvironment WebHostEnvironment;
        public static ModelStateDictionary ModelState;
        public static ActionContext ActionContext;
        public static EmptyModelMetadataProvider ModelMetadataProvider;
        public static ViewDataDictionary ViewData;
        public static TempDataDictionary TempData;
        public static PageContext PageContext;

        public static IndexModel PageModel;

        [SetUp]
        public void TestInitialize()
        {
            HttpContextDefault = new DefaultHttpContext();

            ModelState = new ModelStateDictionary();

            ActionContext = new ActionContext(HttpContextDefault, HttpContextDefault.GetRouteData(), new PageActionDescriptor(), ModelState);

            ModelMetadataProvider = new EmptyModelMetadataProvider();
            ViewData = new ViewDataDictionary(ModelMetadataProvider, ModelState);
            TempData = new TempDataDictionary(HttpContextDefault, Mock.Of<ITempDataProvider>());

            PageContext = new PageContext(ActionContext)
            {
                ViewData = ViewData,
            };

            var MockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            MockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/wwwroot");

            var MockLoggerDirect = Mock.Of<ILogger<IndexModel>>();
            JsonFileProductService ProductService;

            ProductService = new JsonFileProductService(MockWebHostEnvironment.Object);

            PageModel = new IndexModel(ProductService)
            {
            };
        }
        #endregion TestSetup

        #region OnGet
        /// <summary>
        /// Test to check if OnGet method returns valid products
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_Products()
        {
            // Arrange

            // Act
            PageModel.OnGet();

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.EqualTo(true));
            Assert.That(PageModel.Products.ToList().Count, Is.EqualTo(TestHelper.ProductService.GetAllData().Count()));
        }

        /// <summary>
        /// Test to check if OnGet method sets Products property
        /// </summary>
        [Test]
        public void OnGet_Should_Set_Products_Property()
        {
            // Arrange

            // Act
            PageModel.OnGet();

            // Assert
            Assert.That(PageModel.Products, Is.Not.Null);
        }

        /// <summary>
        /// Test to check if OnGet method does not throw any exceptions
        /// </summary>
        [Test]
        public void OnGet_Should_Not_Throw_Exception()
        {
            // Arrange

            // Act & Assert
            Assert.DoesNotThrow(() => PageModel.OnGet());
        }
        #endregion OnGet
    }
}