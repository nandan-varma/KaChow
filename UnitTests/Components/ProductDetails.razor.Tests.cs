using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bunit;
using ContosoCrafts.WebSite.Components;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace UnitTests.Components
{
    public class ProductDetailsTests : Bunit.TestContext
    {
        private Mock<IWebHostEnvironment> WebHostEnvironment;
        private JsonFileProductService ProductService;
        private IEnumerable<ProductModel> OriginalProductData;

        #region TestSetup
        public ProductDetailsTests()
        {
            WebHostEnvironment = new Mock<IWebHostEnvironment>();
            WebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/wwwroot");
            ProductService = new JsonFileProductService(WebHostEnvironment.Object);
            Services.AddSingleton(ProductService);
        }

        [SetUp]
        public void TestInitialize()
        {
            OriginalProductData = ProductService.GetAllData();
        }

        [TearDown]
        public void TestCleanup()
        {
            ProductService.SaveData(OriginalProductData);
        }
        #endregion

    }
}
