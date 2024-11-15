using Bunit;
using ContosoCrafts.WebSite.Components;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

// Namespace for Component UnitTests
namespace UnitTests.Components
{
    public class ProductListTests : Bunit.TestContext
    {
        private Mock<IHttpContextAccessor> HttpContextAccessorMock;

        public ProductListTests()
        {
            HttpContextAccessorMock = new Mock<IHttpContextAccessor>();

            Services.AddSingleton(TestHelper.ProductService);
            Services.AddSingleton(new RatingService(HttpContextAccessorMock.Object));
        }

        [SetUp]
        public void TestInitialize()
        {
        }

        [TearDown]
        public void TestCleanup()
        {
        }

        [Test]
        /// <summary>
        /// Tests that the SelectProduct method sets the selected product correctly when given a valid product.
        /// </summary>
        public void SelectProduct_ValidProduct_Should_SetSelectedProduct()
        {
            // Arrange
            var Component = RenderComponent<ProductList>();
            var Product = TestHelper.ProductService.GetAllData().First();

            // Act
            Component.Instance.SelectProduct(Product.Id);

            // Assert
            Assert.That(Component.Instance.SelectedProductId, Is.EqualTo(Product.Id));
            Assert.That(Component.Instance.SelectedProduct.Name, Is.EqualTo(Product.Name));
        }

        [Test]
        /// <summary>
        /// Tests that the GetCurrentRating method calculates the correct rating when ratings are provided.
        /// </summary>
        public void GetCurrentRating_WithRatings_Should_CalculateCorrectRating()
        {
            // Arrange
            var Product = TestHelper.ProductService.GetAllData().First();
            Product.Ratings = new int[] { 5, 4, 3 };

            var Component = RenderComponent<ProductList>();
            Component.Instance.SelectedProduct = Product;

            // Act
            Component.Instance.GetCurrentRating();

            // Assert
            Assert.That(Component.Instance.CurrentRating, Is.EqualTo(4));
            Assert.That(Component.Instance.VoteCount, Is.EqualTo(3));
            Assert.That(Component.Instance.VoteLabel, Is.EqualTo("Votes"));
        }

        [Test]
        /// <summary>
        /// Tests that the GetCurrentRating method handles the absence of ratings correctly.
        /// </summary>
        public void GetCurrentRating_NoRatings_Should_HandleNoRatings()
        {
            // Arrange
            var Product = TestHelper.ProductService.GetAllData().First();
            Product.Ratings = null;

            var Component = RenderComponent<ProductList>();
            Component.Instance.SelectedProduct = Product;

            // Act
            Component.Instance.GetCurrentRating();

            // Assert
            Assert.That(Component.Instance.CurrentRating, Is.EqualTo(0));
            Assert.That(Component.Instance.VoteCount, Is.EqualTo(0));
        }

        [Test]
        /// <summary>
        /// Tests that the SubmitRating method updates the rating of a valid product.
        /// </summary>
        public void SubmitRating_ValidProduct_Should_UpdateRating()
        {
            // Arrange
            var Product = TestHelper.ProductService.GetAllData().First();

            var Component = RenderComponent<ProductList>();
            Component.Instance.SelectedProduct = Product;

            // Act
            Component.Instance.SubmitRating(Product.Id, 4);

            // Assert
            Assert.That(Component.Instance.CurrentRating, Is.EqualTo(Component.Instance.GetCurrentRating()));
            Assert.That(Component.Instance.VoteCount, Is.EqualTo(Product.Ratings.Length+1));
            Assert.That(Component.Instance.VoteLabel, Is.EqualTo("Votes"));
        }

        [Test]
        /// <summary>
        /// Tests that the GetCurrentRating method handles a single rating correctly.
        /// </summary>
        public void GetCurrentRating_SingleRating_Should_HandleSingleRating()
        {
            // Arrange
            var Product = TestHelper.ProductService.GetAllData().First();
            Product.Ratings = new int[] { 5 };

            var Component = RenderComponent<ProductList>();
            Component.Instance.SelectedProduct = Product;

            // Act
            Component.Instance.GetCurrentRating();

            // Assert
            Assert.That(Component.Instance.CurrentRating, Is.EqualTo(5));
            Assert.That(Component.Instance.VoteCount, Is.EqualTo(1));
            Assert.That(Component.Instance.VoteLabel, Is.EqualTo("Vote"));
        }

        [Test]
        /// <summary>
        /// Tests that the modal displays the correct vote count and label.
        /// </summary>
        public void Modal_DisplayingVoteCountAndLabel_Should_DisplayVoteCountAndLabel()
        {
            // Arrange
            var Product = TestHelper.ProductService.GetAllData().First();
            Product.Ratings = new int[] { 5, 4, 3 };

            var Component = RenderComponent<ProductList>();
            Component.Instance.SelectedProduct = Product;
            Component.Instance.GetCurrentRating();

            // Act
            Component.Render();
            var Modal = Component.Find("#productModal");

            // Assert
            Assert.That(Modal.InnerHtml.Contains($"{Component.Instance.VoteCount} {Component.Instance.VoteLabel}"), Is.True);
        }

        [Test]
        /// <summary>
        /// Tests that the modal displays the correct star ratings.
        /// </summary>
        public void Modal_DisplayStarRatings_Should_DisplayCorrectStarRatings()
        {
            // Arrange
            var Product = TestHelper.ProductService.GetAllData().First();
            Product.Ratings = new int[] { 5, 4, 3 };

            var Component = RenderComponent<ProductList>();
            Component.Instance.SelectedProduct = Product;
            Component.Instance.GetCurrentRating();

            // Act
            Component.Render();
            var Modal = Component.Find("#productModal");
            var Stars = Modal.QuerySelectorAll(".fa-star");

            // Assert
            for (int i = 0; i < 5; i++)
            {
                if (i < Component.Instance.CurrentRating)
                {
                    Assert.That(Stars[i].ClassList.Contains("checked"), Is.True);
                }
                else
                {
                    Assert.That(Stars[i].ClassList.Contains("checked"), Is.False);
                }
            }
        }

        [Test]
        /// <summary>
        /// Tests that clicking the More Info button sets the selected product correctly.
        /// </summary>
        public void MoreInfoButton_Click_Should_SetSelectedProduct()
        {
            // Arrange
            var Product = TestHelper.ProductService.GetAllData().First();
            var Component = RenderComponent<ProductList>();

            // Act
            var Button = Component.Find("button[data-toggle='modal']");
            Button.Click();
            Button.Click();

            // Assert
            Assert.That(Component.Instance.SelectedProductId, Is.EqualTo(Product.Id));
            Assert.That(Component.Instance.SelectedProduct.Name, Is.EqualTo(Product.Name));
        }

        [Test]
        /// <summary>
        /// Tests that the default vote label is presented when there are no votes.
        /// </summary>
        public void DefaultVoteLabel_NoVotes_Should_PresentDefaultLabel()
        {
            // Arrange
            var Product = TestHelper.ProductService.GetAllData().First();
            Product.Ratings = null;

            var Component = RenderComponent<ProductList>();
            Component.Instance.SelectedProduct = Product;
            Component.Instance.GetCurrentRating();

            // Act
            Component.Render();
            var Modal = Component.Find("#productModal");

            // Assert
            Assert.That(Modal.InnerHtml.Contains("<span>Be the first to vote!</span>"), Is.True);
        }

        [Test]
        /// <summary>
        /// Tests that the ApplyFilters method filters products correctly based on the search term.
        /// </summary>
        public void ApplyFilters_SearchTerm_Should_Filter_Products_Correctly()
        {
            // Arrange
            var component = RenderComponent<ProductList>();
            var searchTerm = "Product";
            component.Instance.SearchTerm = searchTerm;

            // Act
            component.Instance.ApplyFilters();

            // Assert
            Assert.That(component.Instance.FilteredProducts.All(p => p.Name.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) || p.Description.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase)), Is.True);
        }

        [Test]
        /// <summary>
        /// Tests that the ApplyFilters method filters products correctly based on the minimum rating.
        /// </summary>
        public void ApplyFilters_MinRating_Should_Filter_Products_Correctly()
        {
            // Arrange
            var component = RenderComponent<ProductList>();
            var minRating = 3;
            component.Instance.MinRating = minRating;

            // Act
            component.Instance.ApplyFilters();

            // Assert
            Assert.That(component.Instance.FilteredProducts.All(p => p.Ratings != null && p.Ratings.Any() && p.Ratings.Average() >= minRating), Is.True);
        }

        [Test]
        /// <summary>
        /// Tests that the ApplyFilters method filters products correctly based on both the search term and minimum rating.
        /// </summary>
        public void ApplyFilters_SearchTerm_And_MinRating_Should_Filter_Products_Correctly()
        {
            // Arrange
            var component = RenderComponent<ProductList>();
            var searchTerm = "Product";
            var minRating = 3;
            component.Instance.SearchTerm = searchTerm;
            component.Instance.MinRating = minRating;

            // Act
            component.Instance.ApplyFilters();

            // Assert
            Assert.That(component.Instance.FilteredProducts.All(p => (p.Name.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase) || p.Description.Contains(searchTerm, System.StringComparison.OrdinalIgnoreCase)) && p.Ratings != null && p.Ratings.Any() && p.Ratings.Average() >= minRating), Is.True);
        }

        [Test]
        /// <summary>
        /// Tests that the ApplyFilters method returns all products when no filters are applied.
        /// </summary>
        public void ApplyFilters_No_Filters_Should_Return_All_Products()
        {
            // Arrange
            var component = RenderComponent<ProductList>();
            component.Instance.SearchTerm = string.Empty;
            component.Instance.MinRating = 0;

            // Act
            component.Instance.ApplyFilters();

            // Assert
            Assert.That(component.Instance.FilteredProducts.Count(), Is.EqualTo(TestHelper.ProductService.GetAllData().Count()));
        }

        // Additional tests to cover the lambda expressions


        [Test]
        /// <summary>
        /// Tests that the ApplyFilters method correctly handles products with null ratings.
        /// </summary>
        public void ApplyFilters_MinRating_Should_Handle_Null_Ratings()
        {
            // Arrange
            var component = RenderComponent<ProductList>();
            var minRating = 3;
            var productService = TestHelper.ProductService;
            var product = productService.CreateData();
            product.Ratings = null;
            productService.SaveData(productService.GetAllData().Append(product));
            component.Instance.MinRating = minRating;

            // Act
            component.Instance.ApplyFilters();

            // Assert
            Assert.That(component.Instance.FilteredProducts.Any(p => p.Ratings == null), Is.False);

            // Cleanup
            productService.DeleteData(product.Id);
        }
    }
}
