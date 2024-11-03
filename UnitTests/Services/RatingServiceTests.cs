using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace UnitTests.Services
{
    internal class RatingServiceTests
    {
        private RatingService RatingService;
        private JsonFileProductService ProductService;
        private Mock<IHttpContextAccessor> HttpContextAccessorMock;
        private Mock<HttpContext> HttpContextMock;
        private Mock<IRequestCookieCollection> RequestCookieCollectionMock;
        private Mock<IHeaderDictionary> ResponseHeadersMock;
        private IEnumerable<ProductModel> OriginalProductData;

        #region TestSetup
        [SetUp]
        public void Setup()
        {
            var WebHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            WebHostEnvironmentMock.Setup(x => x.WebRootPath).Returns("../../../../src/wwwroot");
            ProductService = new JsonFileProductService(WebHostEnvironmentMock.Object);
            OriginalProductData = ProductService.GetAllData();
            HttpContextAccessorMock = new Mock<IHttpContextAccessor>();
            HttpContextMock = new Mock<HttpContext>();
            RequestCookieCollectionMock = new Mock<IRequestCookieCollection>();
            ResponseHeadersMock = new Mock<IHeaderDictionary>();

            HttpContextAccessorMock.Setup(x => x.HttpContext).Returns(HttpContextMock.Object);
            HttpContextMock.Setup(x => x.Request.Cookies).Returns(RequestCookieCollectionMock.Object);
            HttpContextMock.Setup(x => x.Response.Headers).Returns(ResponseHeadersMock.Object);

            RatingService = new RatingService(HttpContextAccessorMock.Object);
        }

        [TearDown]
        public void TestCleanup()
        {
            ProductService.SaveData(OriginalProductData);
        }
        #endregion

    }
}
