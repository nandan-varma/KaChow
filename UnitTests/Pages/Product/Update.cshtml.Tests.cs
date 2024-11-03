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
using System.Collections;
using ContosoCrafts.WebSite.Models;
using System.Collections.Generic;

namespace UnitTests.Pages.Product
{
    public class UpdateTests
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

        public static UpdateModel PageModel;
        public static JsonFileProductService ProductService;
        public static IEnumerable<ProductModel> OriginalProductData;

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

            ProductService = new JsonFileProductService(MockWebHostEnvironment.Object);
            OriginalProductData = ProductService.GetAllData();

            PageModel = new UpdateModel(ProductService) { };
        }

        [TearDown]
        public void TestCleanup()
        {
            ProductService.SaveData(OriginalProductData);
        }

        #endregion TestSetup


    }
}
