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

        #region TestSetup

        [SetUp]
        public void Setup()
        {
            ProductService = TestHelper.ProductService;
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
        }

        #endregion TestSetup

        #region HasUserRated

        [Test]
        /// <summary>
        /// Tests that HasUserRated returns true when the user has rated the product.
        /// </summary>
        public void HasUserRated_User_Has_Rated_Should_Return_True()
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
        public void HasUserRated_User_Has_Not_Rated_Should_Return_False()
        {
            // Arrange
            var ProductId = ProductService.GetAllData().First().Id;
            RequestCookieCollectionMock.Setup(x => x["UserRatings"]).Returns((string)null);

            // Act
            var Result = RatingService.HasUserRated(ProductId);

            // Assert
            Assert.That(Result, Is.EqualTo(false));
        }

        #endregion HasUserRated

        #region AddRating

        [Test]
        /// <summary>
        /// Tests that AddRating adds a rating when the product is not already rated.
        /// </summary>
        public void AddRating_Product_Not_Already_Rated_Should_Add_Rating()
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

        #endregion AddRating

        #region GetRatingsFromCookie

        [Test]
        /// <summary>
        /// Tests that GetRatingsFromCookie returns an empty dictionary when the cookie is null.
        /// </summary>
        public void GetRatingsFromCookie_Cookie_Is_Null_Should_Return_Empty_Dictionary()
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
        public void GetRatingsFromCookie_Cookie_Is_Invalid_Should_Return_Empty_Dictionary()
        {
            // Arrange
            RequestCookieCollectionMock.Setup(x => x["UserRatings"]).Returns("invalid_json");

            // Act
            var Result = RatingService.GetRatingsFromCookie();

            // Assert
            Assert.That(Result, Is.Empty);
        }

        #endregion GetRatingsFromCookie

        #region SerializeRatings

        [Test]
        /// <summary>
        /// Tests that SerializeRatings returns a valid JSON string from a valid ratings dictionary.
        /// </summary>
        public void SerializeRatings_Valid_Ratings_Dictionary_Should_Return_Valid_Json_String()
        {
            // Arrange
            var Ratings = new Dictionary<string, int> { { "product1", 5 } };

            // Act
            var Result = RatingService.SerializeRatings(Ratings);

            // Assert
            Assert.That(Result, Is.EqualTo("{\"product1\":5}"));
        }

        #endregion SerializeRatings

        #region SaveRatingsToCookie

        [Test]
        /// <summary>
        /// Tests that SaveRatingsToCookie saves the ratings to the cookie correctly.
        /// </summary>
        public void SaveRatingsToCookie_Valid_Ratings_Dictionary_Should_Save_Ratings_To_Cookie()
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

        #endregion SaveRatingsToCookie
    }
}
