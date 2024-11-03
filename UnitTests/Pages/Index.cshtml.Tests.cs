using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using System.Collections.Generic;
using System.Linq;
using ContosoCrafts.WebSite.Pages;
using Moq;
using Microsoft.AspNetCore.Hosting;

namespace UnitTests.Pages
{
    [TestFixture]
    public class IndexModelTests
    {
        private IndexModel IndexModel;
        private Mock<ILogger<IndexModel>> LoggerMock;
        private Mock<IWebHostEnvironment> WebHostEnvironment;
        private JsonFileProductService ProductService;
        private IEnumerable<ProductModel> OriginalProductData;

        [SetUp]
        public void Setup()
        {
            // Create mocks for logger and product service
            LoggerMock = new Mock<ILogger<IndexModel>>();
            WebHostEnvironment = new Mock<IWebHostEnvironment>();
            WebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/wwwroot");
            ProductService = new JsonFileProductService(WebHostEnvironment.Object);
            OriginalProductData = ProductService.GetAllData();
            IndexModel = new IndexModel(LoggerMock.Object, ProductService);
        }

        [TearDown]
        public void TearDown()
        {
            // Restore the original data after each test
            ProductService.SaveData(OriginalProductData);
        }

    }
}
