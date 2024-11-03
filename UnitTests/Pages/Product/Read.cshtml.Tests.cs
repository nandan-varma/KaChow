using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NUnit.Framework;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using System.Linq;
using ContosoCrafts.Pages.Product;
using Microsoft.AspNetCore.Hosting;
using Moq;
using System.Collections.Generic;

namespace UnitTests.Pages.Product
{
    [TestFixture]
    public class ReadTests
    {
        private Mock<IWebHostEnvironment> WebHostEnvironment;
        private JsonFileProductService ProductService;
        private DetailsModel DetailsModel;

        private IEnumerable<ProductModel> OriginalProductData;

        [SetUp]
        public void Setup()
        {
            // Initialize the product service with a test data setup
            WebHostEnvironment = new Mock<IWebHostEnvironment>();
            WebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/wwwroot");
            ProductService = new JsonFileProductService(WebHostEnvironment.Object); // Assume this loads data from a test file or a mock setup
            DetailsModel = new DetailsModel(ProductService);

            OriginalProductData = ProductService.GetAllData();
        }

        [TearDown]
        public void TearDown()
        {
            // Restore the original data after each test
            ProductService.SaveData(OriginalProductData);
        }

    }
}
