using ContosoCrafts.WebSite.Models;
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
using System.Collections.Generic;
using System.Linq;

// Namespace for Product Page UnitTests
namespace UnitTests.Pages.Product
{
    public class DeleteTests
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

        public static DeleteModel PageModel;
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

            PageModel = new DeleteModel(ProductService) { };
        }

        [TearDown]
        public void TestCleanup()
        {
            ProductService.SaveData(OriginalProductData);
        }

        #endregion TestSetup

        #region DeleteTests

        [Test]
        /// <summary>
        /// Tests that OnGet returns null product when given an invalid product ID.
        /// </summary>
        public void Delete_OnGet_Invalid_Product_Id_Should_Have_Null_Product()
        {
            // Arrange

            // Act
            PageModel.OnGet("non_null_or_empty");

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.True);
            Assert.That(PageModel.Product, Is.Null);
        }

        [Test]
        /// <summary>
        /// Tests that OnGet returns a non-null product when given a valid product ID.
        /// </summary>
        public void Delete_OnGet_Valid_Product_Id_Should_Have_Non_Null_Product()
        {
            // Arrange
            var Data = TestHelper.ProductService.GetAllData().First();

            // Act
            PageModel.OnGet(Data.Id);

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.True);
            Assert.That(PageModel.Product.Id, Is.EqualTo(Data.Id));
        }

        [Test]
        /// <summary>
        /// Tests that OnPost redirects to the index page after a successful delete.
        /// </summary>
        public void Delete_OnPost_Should_Redirect_To_Index_Page()
        {
            // Arrange
            PageModel.OnGet(TestHelper.ProductService.GetAllData().First().Id);

            // Act
            var Result = (RedirectToPageResult)PageModel.OnPost();

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.True);
            Assert.That(Result.PageName, Is.EqualTo("./Index"));
        }

        [Test]
        /// <summary>
        /// Tests that OnPost returns PageResult when the model state is invalid.
        /// </summary>
        public void Delete_OnPost_Invalid_ModelState_Should_Return_Page_Result()
        {
            // Arrange
            PageModel.Product = TestHelper.ProductService.GetAllData().First();
            PageModel.ModelState.AddModelError("Error", "Invalid Model State");

            // Act
            var Result = (PageResult)PageModel.OnPost();

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.False);
        }
        
        [Test]
        /// <summary>
        /// Tests that OnPost adds an error message to ModelState and returns Page when deletion fails.
        /// </summary>
        public void Delete_OnPost_Delete_Fails_Should_Add_Error_To_ModelState_And_Return_Page()
        {
            // Arrange
            var Product = ProductService.CreateData();
            PageModel = new DeleteModel(productService: ProductService) { Product = Product };

            ProductService.DeleteData(Product.Id);


            // Act
            var Result = PageModel.OnPost();

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.False);
            Assert.That(PageModel.ModelState[string.Empty].Errors.First().ErrorMessage, Is.EqualTo("Product not found or could not be deleted."));
            Assert.That(Result, Is.TypeOf<PageResult>());
        }

        #endregion DeleteTests
    }
}
