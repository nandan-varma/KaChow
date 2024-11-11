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
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Pages.Product
{
    public class UpdateTests
    {
        #region TestSetup
        public static DefaultHttpContext HttpContextDefault;
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

            ProductService = new JsonFileProductService(MockWebHostEnvironment.Object);
            OriginalProductData = ProductService.GetAllData();

            PageModel = new UpdateModel(ProductService);
        }

        [TearDown]
        public void TestCleanup()
        {
            ProductService.SaveData(OriginalProductData);
        }

        #endregion TestSetup

        #region UpdateTests

        /// <summary>
        /// Tests that OnGet returns null product when given an invalid product ID.
        /// </summary>
        [Test]
        public void Update_OnGet_Invalid_Product_Id_Should_Have_Null_Product()
        {
            // Arrange

            // Act
            PageModel.OnGet("non_null_or_empty");

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.True);
            Assert.That(PageModel.Product, Is.Null);
        }

        /// <summary>
        /// Tests that OnGet returns a non-null product when given a valid product ID.
        /// </summary>
        [Test]
        public void Update_OnGet_Valid_Product_Id_Should_Have_Non_Null_Product()
        {
            // Arrange
            var Data = ProductService.GetAllData().First();

            // Act
            PageModel.OnGet(Data.Id);

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.True);
            Assert.That(PageModel.Product.Id, Is.EqualTo(Data.Id));
        }

        /// <summary>
        /// Tests that OnPost redirects to the index page after a successful update.
        /// </summary>
        [Test]
        public void Update_OnPost_Should_Redirect_To_Index_Page()
        {
            // Arrange
            PageModel.OnGet(ProductService.GetAllData().First().Id);
            var originalPrice = PageModel.Product.Price;
            PageModel.Product.Price = -originalPrice;

            // Act
            var Result = (RedirectToPageResult)PageModel.OnPost();

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.True);
            Assert.That(PageModel.Product.Price, Is.Not.EqualTo(originalPrice));
            Assert.That(Result.PageName, Is.EqualTo("./Index"));
        }

        /// <summary>
        /// Tests that OnPost returns PageResult when the model state is invalid.
        /// </summary>
        [Test]
        public void Update_OnPost_Invalid_ModelState_Should_Return_Page_Result()
        {
            // Arrange
            PageModel.Product = ProductService.GetAllData().First();
            PageModel.ModelState.AddModelError("Sigma", "You must be Sigma");

            // Act
            var Result = (PageResult)PageModel.OnPost();

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.False);
        }

        #endregion UpdateTests
    }
}
