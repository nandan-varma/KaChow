using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

// Namespace for Service UnitTests
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

        #region HasUserRated
        [Test]
        /// <summary>
        /// Tests that HasUserRated returns true when the user has rated the product.
        /// </summary>
        public void HasUserRated_UserHasRated_ShouldReturnTrue()
        {
            // Arrange
            var ProductId = ProductService.GetAllData().First().Id;
            var Ratings = new Dictionary<string, int> { { ProductId, 5 } };
            var SerializedRatings = JsonSerializer.Serialize(Ratings);

            RequestCookieCollectionMock.Setup(x => x["UserRatings"]).Returns(SerializedRatings);

            // Act
            var Result = RatingService.HasUserRated(ProductId);

            // Assert
            Assert.That(Result, Is.EqualTo(true));
        }

        [Test]
        /// <summary>
        /// Tests that HasUserRated returns false when the user has not rated the product.
        /// </summary>
        public void HasUserRated_UserHasNotRated_ShouldReturnFalse()
        {
            // Arrange
            var ProductId = ProductService.GetAllData().First().Id;
            RequestCookieCollectionMock.Setup(x => x["UserRatings"]).Returns((string)null);

            // Act
            var Result = RatingService.HasUserRated(ProductId);

            // Assert
            Assert.That(Result, Is.EqualTo(false));
        }
        #endregion

        #region AddRating
        [Test]
        /// <summary>
        /// Tests that AddRating adds a rating when the product is not already rated.
        /// </summary>
        public void AddRating_ProductNotAlreadyRated_ShouldAddRating()
        {
            // Arrange
            var ProductId = ProductService.GetAllData().First().Id;
            var Ratings = new Dictionary<string, int>();
            var SerializedRatings = JsonSerializer.Serialize(Ratings);
            var Items = new Dictionary<object, object>();

            RequestCookieCollectionMock.Setup(x => x["UserRatings"]).Returns(SerializedRatings);
            HttpContextMock.Setup(x => x.Items).Returns(Items);

            // Act
            RatingService.AddRating(ProductId, 5);

            // Assert
            Assert.That(Items.ContainsKey("UserRatings"), Is.True);
            Assert.That(Items["UserRatings"], Is.EqualTo($"{{\"{ProductId}\":5}}"));
        }
        #endregion

        #region GetRatingsFromCookie
        [Test]
        /// <summary>
        /// Tests that GetRatingsFromCookie returns an empty dictionary when the cookie is null.
        /// </summary>
        public void GetRatingsFromCookie_CookieIsNull_ShouldReturnEmptyDictionary()
        {
            // Arrange
            RequestCookieCollectionMock.Setup(x => x["UserRatings"]).Returns((string)null);

            // Act
            var Result = RatingService.GetRatingsFromCookie();

            // Assert
            Assert.That(Result, Is.Empty);
        }

        [Test]
        /// <summary>
        /// Tests that GetRatingsFromCookie returns an empty dictionary when the cookie is invalid.
        /// </summary>
        public void GetRatingsFromCookie_CookieIsInvalid_ShouldReturnEmptyDictionary()
        {
            // Arrange
            RequestCookieCollectionMock.Setup(x => x["UserRatings"]).Returns("invalid_json");

            // Act
            var Result = RatingService.GetRatingsFromCookie();

            // Assert
            Assert.That(Result, Is.Empty);
        }
        #endregion

        #region SerializeRatings
        [Test]
        /// <summary>
        /// Tests that SerializeRatings returns a valid JSON string from a valid ratings dictionary.
        /// </summary>
        public void SerializeRatings_ValidRatingsDictionary_ShouldReturnValidJsonString()
        {
            // Arrange
            var Ratings = new Dictionary<string, int> { { "product1", 5 } };

            // Act
            var Result = RatingService.SerializeRatings(Ratings);

            // Assert
            Assert.That(Result, Is.EqualTo("{\"product1\":5}"));
        }
        #endregion

        #region SaveRatingsToCookie
        [Test]
        /// <summary>
        /// Tests that SaveRatingsToCookie saves the ratings to the cookie correctly.
        /// </summary>
        public void SaveRatingsToCookie_ValidRatingsDictionary_ShouldSaveRatingsToCookie()
        {
            // Arrange
            var Ratings = new Dictionary<string, int> { { "product1", 5 } };
            var Items = new Dictionary<object, object>();
            HttpContextMock.Setup(x => x.Items).Returns(Items);

            // Act
            RatingService.SaveRatingsToCookie(Ratings);

            // Assert
            Assert.That(Items.ContainsKey("UserRatings"), Is.True);
            Assert.That(Items["UserRatings"], Is.EqualTo("{\"product1\":5}"));
        }
        #endregion
    }
}
