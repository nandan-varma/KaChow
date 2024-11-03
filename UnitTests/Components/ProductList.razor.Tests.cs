using System.Collections.Generic;
using System.Linq;
using Bunit;
using ContosoCrafts.WebSite.Components;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace UnitTests.Components
{
    public class ProductListTests : Bunit.TestContext
    {
        private Mock<IWebHostEnvironment> WebHostEnvironmentMock;
        private Mock<IHttpContextAccessor> HttpContextAccessorMock;
        private IEnumerable<ProductModel> OriginalProductData;

        public ProductListTests()
        {
            #region TestSetup
            WebHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            WebHostEnvironmentMock.Setup(x => x.WebRootPath).Returns("../../../../src/wwwroot");
            HttpContextAccessorMock = new Mock<IHttpContextAccessor>();

            Services.AddSingleton(new JsonFileProductService(WebHostEnvironmentMock.Object));
            Services.AddSingleton(new RatingService(HttpContextAccessorMock.Object));
            #endregion
        }

        [SetUp]
        public void TestInitialize()
        {
            OriginalProductData = Services.GetService<JsonFileProductService>().GetAllData();
        }

        [TearDown]
        public void TestCleanup()
        {
            Services.GetService<JsonFileProductService>().SaveData(OriginalProductData);
        }

    }
}
