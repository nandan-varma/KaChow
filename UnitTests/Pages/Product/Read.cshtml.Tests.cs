using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

using ContosoCrafts.WebSite.Pages.Product;
using ContosoCrafts.WebSite.Services;
using ContosoCrafts.Pages.Product;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace UnitTests.Pages.Product.Read
{
    public class ReadTests
    {
        #region TestSetup
        public static IUrlHelperFactory urlHelperFactory;
        public static DefaultHttpContext httpContextDefault;
        public static IWebHostEnvironment webHostEnvironment;
        public static ModelStateDictionary modelState;
        public static ActionContext actionContext;
        public static EmptyModelMetadataProvider modelMetadataProvider;
        public static ViewDataDictionary viewData;
        public static TempDataDictionary tempData;
        public static PageContext pageContext;

        public static DetailsModel pageModel;

        [SetUp]
        public void TestInitialize()
        {
            httpContextDefault = new DefaultHttpContext()
            {
                //RequestServices = serviceProviderMock.Object,
            };

            modelState = new ModelStateDictionary();

            actionContext = new ActionContext(httpContextDefault, httpContextDefault.GetRouteData(), new PageActionDescriptor(), modelState);

            modelMetadataProvider = new EmptyModelMetadataProvider();
            viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
            tempData = new TempDataDictionary(httpContextDefault, Mock.Of<ITempDataProvider>());

            pageContext = new PageContext(actionContext)
            {
                ViewData = viewData,
            };

            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/bin/Debug/net6.0/wwwroot");
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns("./data/");

            var MockLoggerDirect = Mock.Of<ILogger<IndexModel>>();
            JsonFileProductService productService;

            productService = new JsonFileProductService(mockWebHostEnvironment.Object);
            pageModel = new DetailsModel(productService);
        }

        #endregion TestSetup

        #region OnGet

        [Test]
        public void ReadPage_InValid_Product_Id_Should_Return_404()
        {
            // Arrange
            var dataId = "not_empty_or_null";

            // Act
            var result = pageModel.OnGet(dataId);

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.EqualTo(true));
            Assert.That(((IStatusCodeActionResult) result).StatusCode, Is.EqualTo(404));
            Assert.That(pageModel.Product, Is.EqualTo(null));
        }

        [Test]
        public void ReadPage_InValid_Product_Id_Null_String_Should_Return_Not_Found()
        {
            // Arrange

            // Act
            var result = pageModel.OnGet(null);

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.EqualTo(true));
            Assert.That(((IStatusCodeActionResult)result).StatusCode, Is.EqualTo(404));
        }

        [Test]
        public void ReadPage_Valid_Product_Id_Should_Select_Product_And_Return_Non_Null_Page_Result()
        {
            // Arrange
            var dataId = TestHelper.ProductService.GetAllData().First().Id;

            // Act
            var result = (PageResult) pageModel.OnGet(dataId);

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.EqualTo(true));
            Assert.That(result, Is.Not.Null);
            Assert.That(pageModel.Product.Id, Is.EqualTo(dataId));
        }

        #endregion


    }
}