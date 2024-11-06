using Bunit;
using ContosoCrafts.WebSite.Components;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        #region ProductDetails
        [Test]
        /// <summary>
        /// Tests that the ProductDetails component displays loading message when product is null.
        /// </summary>
        public void ProductDetails_Product_Is_Null_Should_Display_Loading()
        {
            // Arrange
            var ProductService = Services.GetService<JsonFileProductService>();

            // Act
            var Component = RenderComponent<ProductDetails>(parameters => parameters.Add(p => p.Id, "wrong-id"));

            // Assert
            Component.MarkupMatches("<p>Loading...</p>");
        }

        [Test]
        /// <summary>
        /// Tests that the ProductDetails component displays product details when product is not null.
        /// </summary>
        public async Task ProductDetails_Product_Is_Not_Null_Should_Display_Product_Details()
        {
            // Arrange
            var Product = new ProductModel
            {
                Id = "test-id",
                Name = "Test Product",
                Description = "Test Description",
                Price = 99,
                Image = "test-image.jpg",
                Url = "http://example.com",
                Ratings = new int[] { 3 }
            };
            var NewProductData = OriginalProductData.ToList();
            NewProductData.Add(Product);
            ProductService.SaveData(NewProductData);

            // Act
            var Component = RenderComponent<ProductDetails>(parameters => parameters.Add(p => p.Id, Product.Id));
            await Task.Delay(100);
            Component.Render();

            // Assert
            Component.Find(".product-title").MarkupMatches("<h1 class=\"product-title\">" + Product.Name + "</h1>");
            Component.Find(".product-description").MarkupMatches("<p class=\"product-description\">" + Product.Description + "</p>");
            Component.Find(".product-price").MarkupMatches("<p class=\"product-price\">Price: $" + Product.Price + "</p>");
            Component.Find(".product-image img").MarkupMatches("<img src=\"" + Product.Image + "\" alt=\"" + Product.Name + "\" />");

            var Stars = Component.FindAll(".star");
            Assert.That(Stars.Count, Is.EqualTo(5));
            Assert.That(Stars[0].TextContent, Is.EqualTo("★"));
            Assert.That(Stars[1].TextContent, Is.EqualTo("★"));
            Assert.That(Stars[2].TextContent, Is.EqualTo("★"));
            Assert.That(Stars[3].TextContent, Is.EqualTo("☆"));
            Assert.That(Stars[4].TextContent, Is.EqualTo("☆"));

            var RatingCount = Component.Find(".rating-count");
            Assert.That(RatingCount.TextContent, Is.EqualTo("(1)"));
        }

        [Test]
        /// <summary>
        /// Tests that the ProductDetails component displays product details when product has no ratings.
        /// </summary>
        public async Task ProductDetails_Product_Has_No_Ratings_Should_Display_Product_Details()
        {
            // Arrange
            var Product = new ProductModel
            {
                Id = "test-id",
                Name = "Test Product",
                Description = "Test Description",
                Price = 99,
                Image = "test-image.jpg",
                Url = "http://example.com",
                Ratings = new int[] { }
            };
            var NewProductData = OriginalProductData.ToList();
            NewProductData.Add(Product);
            ProductService.SaveData(NewProductData);

            // Act
            var Component = RenderComponent<ProductDetails>(parameters => parameters.Add(p => p.Id, Product.Id));
            await Task.Delay(100);
            Component.Render();

            // Assert
            Component.Find(".product-title").MarkupMatches("<h1 class=\"product-title\">" + Product.Name + "</h1>");
            Component.Find(".product-description").MarkupMatches("<p class=\"product-description\">" + Product.Description + "</p>");
            Component.Find(".product-price").MarkupMatches("<p class=\"product-price\">Price: $" + Product.Price + "</p>");
            Component.Find(".product-image img").MarkupMatches("<img src=\"" + Product.Image + "\" alt=\"" + Product.Name + "\" />");

            var Stars = Component.FindAll(".star");
            Assert.That(Stars.Count, Is.EqualTo(5));
            Assert.That(Stars[0].TextContent, Is.EqualTo("☆"));
            Assert.That(Stars[1].TextContent, Is.EqualTo("☆"));
            Assert.That(Stars[2].TextContent, Is.EqualTo("☆"));
            Assert.That(Stars[3].TextContent, Is.EqualTo("☆"));
            Assert.That(Stars[4].TextContent, Is.EqualTo("☆"));

            var RatingCount = Component.Find(".rating-count");
            Assert.That(RatingCount.TextContent, Is.EqualTo("(0)"));
        }

        [Test]
        /// <summary>
        /// Tests that the ProductDetails component displays filled and empty stars when product has ratings.
        /// </summary>
        public async Task ProductDetails_Product_Has_Ratings_Should_Display_Filled_And_Empty_Stars()
        {
            // Arrange
            var Product = new ProductModel
            {
                Id = "test-id",
                Name = "Rated Product",
                Ratings = new int[] { 5, 4, 4, 3 }
            };
            var NewProductData = OriginalProductData.ToList();
            NewProductData.Add(Product);
            ProductService.SaveData(NewProductData);

            // Act
            var Component = RenderComponent<ProductDetails>(parameters => parameters.Add(p => p.Id, Product.Id));
            await Task.Delay(100);
            Component.Render();

            // Assert
            var Stars = Component.FindAll(".star");
            Assert.That(Stars[0].ClassList.Contains("filled"));
            Assert.That(Stars[1].ClassList.Contains("filled"));
            Assert.That(Stars[2].ClassList.Contains("filled"));
            Assert.That(Stars[3].ClassList.Contains("filled"));
            Assert.That(!Stars[4].ClassList.Contains("filled"));

            var RatingCount = Component.Find(".rating-count");
            Assert.That(RatingCount.TextContent, Is.EqualTo("(4)"));
        }

        [Test]
        /// <summary>
        /// Tests that the ProductDetails component displays all empty stars when product has no ratings.
        /// </summary>
        public async Task ProductDetails_Product_Has_No_Ratings_Should_Display_All_Empty_Stars()
        {
            // Arrange
            var Product = new ProductModel
            {
                Id = "no-ratings",
                Name = "Unrated Product",
                Ratings = new int[] { }
            };
            var NewProductData = OriginalProductData.ToList();
            NewProductData.Add(Product);
            ProductService.SaveData(NewProductData);

            // Act
            var Component = RenderComponent<ProductDetails>(parameters => parameters.Add(p => p.Id, Product.Id));
            await Task.Delay(100);
            Component.Render();

            // Assert
            var Stars = Component.FindAll(".star");
            Assert.That(!Stars[0].ClassList.Contains("filled"));
            Assert.That(!Stars[1].ClassList.Contains("filled"));
            Assert.That(!Stars[2].ClassList.Contains("filled"));
            Assert.That(!Stars[3].ClassList.Contains("filled"));
            Assert.That(!Stars[4].ClassList.Contains("filled"));

            var RatingCount = Component.Find(".rating-count");
            Assert.That(RatingCount.TextContent, Is.EqualTo("(0)"));
        }

        [Test]
        /// <summary>
        /// Tests that the ProductDetails component displays all empty stars when product ratings is null.
        /// </summary>
        public async Task ProductDetails_Product_Ratings_Is_Null_Should_Display_All_Empty_Stars()
        {
            // Arrange
            var Product = new ProductModel
            {
                Id = "null-ratings",
                Name = "Null Ratings Product",
                Ratings = null
            };
            var NewProductData = OriginalProductData.ToList();
            NewProductData.Add(Product);
            ProductService.SaveData(NewProductData);

            // Act
            var Component = RenderComponent<ProductDetails>(parameters => parameters.Add(p => p.Id, Product.Id));
            await Task.Delay(100);
            Component.Render();

            // Assert
            var Stars = Component.FindAll(".star");
            Assert.That(Stars.Count, Is.EqualTo(5));
            Assert.That(!Stars[0].ClassList.Contains("filled"));
            Assert.That(!Stars[1].ClassList.Contains("filled"));
            Assert.That(!Stars[2].ClassList.Contains("filled"));
            Assert.That(!Stars[3].ClassList.Contains("filled"));
            Assert.That(!Stars[4].ClassList.Contains("filled"));

            var RatingCount = Component.Find(".rating-count");
            Assert.That(RatingCount.TextContent, Is.EqualTo("(0)"));
        }
        #endregion
    }
}
