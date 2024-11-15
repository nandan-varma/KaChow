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
using System;
using System.Collections.Generic;
using System.Linq;

// Namespace for Product Page UnitTests
namespace UnitTests.Pages.Product
{
    public class DeleteTests
    {
        #region TestSetup
        private static DefaultHttpContext HttpContextDefault;
        private static IWebHostEnvironment WebHostEnvironment;
        private static ModelStateDictionary ModelState;
        private static PageContext PageContext;

        private static DeleteModel PageModel;
        private static JsonFileProductService ProductService;

        [SetUp]
        public void TestInitialize()
        {
            HttpContextDefault = new DefaultHttpContext();
            ModelState = new ModelStateDictionary();

            var ActionContext = new ActionContext(HttpContextDefault, new RouteData(), new PageActionDescriptor(), ModelState);
            var ModelMetadataProvider = new EmptyModelMetadataProvider();
            var ViewData = new ViewDataDictionary(ModelMetadataProvider, ModelState);
            var TempData = new TempDataDictionary(HttpContextDefault, Mock.Of<ITempDataProvider>());

            PageContext = new PageContext(ActionContext)
            {
                ViewData = ViewData,
            };

            ProductService = TestHelper.ProductService;

            PageModel = new DeleteModel(ProductService);
        }

        [TearDown]
        public void TestCleanup()
        {
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
            var data = ProductService.GetAllData().First();

            // Act
            PageModel.OnGet(data.Id);

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.True);
            Assert.That(PageModel.Product.Id, Is.EqualTo(data.Id));
        }

        [Test]
        /// <summary>
        /// Tests that OnPost redirects to the index page after a successful delete.
        /// </summary>
        public void Delete_OnPost_Should_Redirect_To_Index_Page()
        {
            // Arrange
            PageModel.OnGet(ProductService.GetAllData().First().Id);

            // Act
            var result = (RedirectToPageResult)PageModel.OnPost();

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.True);
            Assert.That(result.PageName, Is.EqualTo("./Index"));
        }

        [Test]
        /// <summary>
        /// Tests that OnPost returns PageResult when the model state is invalid.
        /// </summary>
        public void Delete_OnPost_Invalid_ModelState_Should_Return_Page_Result()
        {
            // Arrange
            PageModel.Product = ProductService.GetAllData().First();
            PageModel.ModelState.AddModelError("Error", "Invalid Model State");

            // Act
            var result = (PageResult)PageModel.OnPost();

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
            var product = ProductService.CreateData();
            PageModel = new DeleteModel(ProductService) { Product = product };
            ProductService.DeleteData(product.Id);

            // Act
            var result = PageModel.OnPost();

            // Assert
            Assert.That(PageModel.ModelState.IsValid, Is.False);
            Assert.That(PageModel.ModelState[string.Empty].Errors.First().ErrorMessage, Is.EqualTo("Product not found or could not be deleted."));
            Assert.That(result, Is.TypeOf<PageResult>());
        }

        [Test]
        /// <summary>
        /// Tests that CreateProduct assigns a unique GUID and returns true when valid data is provided.
        /// </summary>
        public void CreateProduct_Valid_Data_Should_Assign_Guid_And_Return_True()
        {
            // Arrange
            var newProduct = ProductService.CreateData();

            // Act
            var result = ProductService.CreateProduct(newProduct);

            // Assert
            Assert.That(result, Is.True, "CreateProduct should return true for valid data");
            Assert.That(newProduct.Id, Is.Not.Null.And.Not.Empty, "Product ID should not be null or empty");
            Assert.That(Guid.TryParse(newProduct.Id, out _), Is.True, "Assigned ID should be a valid GUID");
        }

        #endregion DeleteTests
    }
}
