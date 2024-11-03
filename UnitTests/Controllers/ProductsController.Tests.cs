using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using ContosoCrafts.WebSite.Controllers;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Moq;

namespace UnitTests.Controllers
{
    public class ProductsControllerTests
    {
        private ProductsController Controller;
        private Mock<IWebHostEnvironment> WebHostEnvironmentMock;
        private JsonFileProductService ProductService;
        private IEnumerable<ProductModel> OriginalProductData;

        [SetUp]
        public void Setup()
        {
            #region TestSetup
            WebHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            WebHostEnvironmentMock.Setup(m => m.WebRootPath).Returns("../../../../src/wwwroot");
            ProductService = new JsonFileProductService(WebHostEnvironmentMock.Object);
            Controller = new ProductsController(ProductService);
            OriginalProductData = ProductService.GetAllData();
            #endregion
        }

        [TearDown]
        public void TearDown()
        {
            ProductService.SaveData(OriginalProductData);
        }

    }
}
