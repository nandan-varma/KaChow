using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;
using System.Collections.Generic;
using ContosoCrafts.WebSite.Controllers;
using ContosoCrafts.WebSite.Services;
using Moq;
using System.Linq;
using ContosoCrafts.WebSite.Models;

namespace UnitTests.Controllers
{
    public class RatingControllerTests
    {
        private RatingService RatingService;
        private JsonFileProductService ProductService;
        private RatingController Controller;
        private DefaultHttpContext HttpContext;
        private Mock<IWebHostEnvironment> WebHostEnvironmentMock;
        private IHttpContextAccessor HttpContextAccessor;
        private IEnumerable<ProductModel> OriginalProducts;

        [SetUp]
        public void SetUp()
        {
            #region TestSetup
            HttpContext = new DefaultHttpContext();
            HttpContextAccessor = new HttpContextAccessor { HttpContext = HttpContext };
            WebHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            WebHostEnvironmentMock.Setup(m => m.WebRootPath).Returns("../../../../src/wwwroot");
            RatingService = new RatingService(HttpContextAccessor);
            ProductService = new JsonFileProductService(WebHostEnvironmentMock.Object);
            Controller = new RatingController(RatingService, ProductService);
            OriginalProducts = ProductService.GetAllData();
            #endregion
        }

        [TearDown]
        public void TearDown()
        {
            ProductService.SaveData(OriginalProducts);
        }

        
    }
}
