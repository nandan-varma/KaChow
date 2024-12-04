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
    public class ProductListTest : Bunit.TestContext
    {
        #region TestSetup

        private Mock<IHttpContextAccessor> _httpContextAccessorMock;

        public ProductListTest()
        {
            // Arrange
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            Services.AddSingleton(TestHelper.ProductService);
            Services.AddSingleton(new RatingService(_httpContextAccessorMock.Object));

            // Reset
        }

        [SetUp]
        public void TestInitialize()
        {
            // Reset
        }

        [TearDown]
        public void TestCleanup()
        {
            // Reset
        }

        #endregion TestSetup

        #region SelectProduct

        [Test]
        /// <summary>
        /// Tests that the SelectProduct method sets the selected product correctly when given a valid product.
        /// </summary>
        public void SelectProduct_ValidProduct_Should_SetSelectedProduct()
        {
            // Arrange
            var component = RenderComponent<ProductList>();
            var product = TestHelper.ProductService.GetAllData().First();

            // Act
            component.Instance.SelectProduct(product.Id);

            // Reset

            // Assert
            Assert.That(component.Instance.SelectedProductId, Is.EqualTo(product.Id));
            Assert.That(component.Instance.SelectedProduct.Name, Is.EqualTo(product.Name));
        }

        #endregion SelectProduct

        #region GetCurrentRating

        [Test]
        /// <summary>
        /// Tests that the GetCurrentRating method calculates the correct rating when ratings are provided.
        /// </summary>
        public void GetCurrentRating_WithRatings_Should_CalculateCorrectRating()
        {
            // Arrange
            var product = TestHelper.ProductService.GetAllData().First();
            product.Ratings = new int[] { 5, 4, 3 };

            var component = RenderComponent<ProductList>();
            component.Instance.SelectedProduct = product;

            // Act
            component.Instance.GetCurrentRating();

            // Reset

            // Assert
            Assert.That(component.Instance.CurrentRating, Is.EqualTo(4));
            Assert.That(component.Instance.VoteCount, Is.EqualTo(3));
            Assert.That(component.Instance.VoteLabel, Is.EqualTo("Votes"));
        }

        [Test]
        /// <summary>
        /// Tests that the GetCurrentRating method handles the absence of ratings correctly.
        /// </summary>
        public void GetCurrentRating_NoRatings_Should_HandleNoRatings()
        {
            // Arrange
            var product = TestHelper.ProductService.GetAllData().First();
            product.Ratings = null;

            var component = RenderComponent<ProductList>();
            component.Instance.SelectedProduct = product;

            // Act
            component.Instance.GetCurrentRating();

            // Reset

            // Assert
            Assert.That(component.Instance.CurrentRating, Is.EqualTo(0));
            Assert.That(component.Instance.VoteCount, Is.EqualTo(0));
        }

        [Test]
        /// <summary>
        /// Tests that the GetCurrentRating method handles a single rating correctly.
        /// </summary>
        public void GetCurrentRating_SingleRating_Should_HandleSingleRating()
        {
            // Arrange
            var product = TestHelper.ProductService.GetAllData().First();
            product.Ratings = new int[] { 5 };

            var component = RenderComponent<ProductList>();
            component.Instance.SelectedProduct = product;

            // Act
            component.Instance.GetCurrentRating();

            // Reset

            // Assert
            Assert.That(component.Instance.CurrentRating, Is.EqualTo(5));
            Assert.That(component.Instance.VoteCount, Is.EqualTo(1));
            Assert.That(component.Instance.VoteLabel, Is.EqualTo("Vote"));
        }

        #endregion GetCurrentRating

        #region SubmitRating

        [Test]
        /// <summary>
        /// Tests that the SubmitRating method updates the rating of a valid product.
        /// </summary>
        public void SubmitRating_ValidProduct_Should_UpdateRating()
        {
            // Arrange
            var product = TestHelper.ProductService.GetAllData().First();

            var component = RenderComponent<ProductList>();
            component.Instance.SelectedProduct = product;

            // Act
            component.Instance.SubmitRating(product.Id, 4);

            // Reset

            // Assert
            Assert.That(component.Instance.CurrentRating, Is.EqualTo(component.Instance.GetCurrentRating()));
            Assert.That(component.Instance.VoteCount, Is.EqualTo(product.Ratings.Length + 1));
            Assert.That(component.Instance.VoteLabel, Is.EqualTo("Votes"));
        }

        [Test]
        /// <summary>
        /// Tests that the SubmitRating method is called when a star is clicked.
        /// </summary>
        public void SubmitRating_Should_Be_Called_On_Star_Click()
        {
            // Arrange
            var product = TestHelper.ProductService.GetAllData().First();
            var component = RenderComponent<ProductList>();
            component.Instance.SelectProduct(product.Id);
            component.Instance.GetCurrentRating();

            // Act
            component.Render();
            var button = component.Find("button[data-toggle='modal']");
            button.Click();
            var star = component.Find(".fa-star");
            star.Click();

            // Reset

            // Assert
            Assert.That(component.Instance.CurrentRating, Is.EqualTo(component.Instance.GetCurrentRating()));
        }

        #endregion SubmitRating

        #region Modal

        [Test]
        /// <summary>
        /// Tests that the modal displays the correct vote count and label.
        /// </summary>
        public void Modal_DisplayingVoteCountAndLabel_Should_DisplayVoteCountAndLabel()
        {
            // Arrange
            var product = TestHelper.ProductService.GetAllData().First();
            product.Ratings = new int[] { 5, 4, 3 };
            TestHelper.ProductService.UpdateData(product);

            var component = RenderComponent<ProductList>();
            component.Instance.SelectProduct(product.Id);
            component.Instance.GetCurrentRating();

            // Act
            component.Render();
            var modal = component.Find("#productModal");

            // Reset

            // Assert
            Assert.That(modal.InnerHtml.Contains($"{component.Instance.VoteCount} {component.Instance.VoteLabel}"), Is.True);
        }

        [Test]
        /// <summary>
        /// Tests that the modal displays the correct star ratings.
        /// </summary>
        public void Modal_DisplayStarRatings_Should_DisplayCorrectStarRatings()
        {
            // Arrange
            var product = TestHelper.ProductService.GetAllData().First();
            product.Ratings = new int[] { 5, 4, 3 };
            TestHelper.ProductService.UpdateData(product);

            var component = RenderComponent<ProductList>();
            component.Instance.SelectProduct(product.Id);
            component.Instance.GetCurrentRating();

            // Act
            component.Render();
            var modal = component.Find("#productModal");
            var stars = modal.QuerySelectorAll(".fa-star");

            // Reset

            // Assert
            for (int i = 0; i < 5; i++)
            {
                if (i < component.Instance.CurrentRating)
                {
                    Assert.That(stars[i].ClassList.Contains("checked"), Is.True);
                }
                else
                {
                    Assert.That(stars[i].ClassList.Contains("checked"), Is.False);
                }
            }
        }

        [Test]
        /// <summary>
        /// Tests that the default vote label is presented when there are no votes.
        /// </summary>
        public void DefaultVoteLabel_NoVotes_Should_PresentDefaultLabel()
        {
            // Arrange
            var product = TestHelper.ProductService.GetAllData().First();
            product.Ratings = null;
            TestHelper.ProductService.UpdateData(product);

            var component = RenderComponent<ProductList>();
            component.Instance.SelectProduct(product.Id);
            component.Instance.GetCurrentRating();

            // Act
            component.Render();
            var modal = component.Find("#productModal");

            // Reset

            // Assert
            Assert.That(modal.InnerHtml.Contains("<span>Be the first to vote!</span>"), Is.True);
        }

        #endregion Modal

        #region ApplyFilters

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

            // Reset

            // Assert
            Assert.That(component.Instance.FilteredProducts.Count(), Is.EqualTo(TestHelper.ProductService.GetAllData().Count()));
        }

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

            // Reset

            // Assert
            Assert.That(component.Instance.FilteredProducts.Any(p => p.Ratings == null), Is.False);

            // Cleanup
            productService.DeleteData(product.Id);
        }

        [Test]
        /// <summary>
        /// Tests that the ApplyFilters method returns an empty list when no products match the search term.
        /// </summary>
        public void ApplyFilters_NoMatchSearchTerm_Should_Return_EmptyList()
        {
            // Arrange
            var component = RenderComponent<ProductList>();
            var searchTerm = "NonExistingTerm";
            component.Instance.SearchTerm = searchTerm;

            // Act
            component.Instance.ApplyFilters();

            // Reset

            // Assert
            Assert.That(component.Instance.FilteredProducts.Any(), Is.False);
        }

        [Test]
        /// <summary>
        /// Tests that the ApplyFilters method excludes products below the minimum rating.
        /// </summary>
        public void ApplyFilters_BelowMinRating_Should_ExcludeProducts()
        {
            // Arrange
            var component = RenderComponent<ProductList>();
            var minRating = 4;
            var productService = TestHelper.ProductService;
            var product = productService.CreateData();
            product.Ratings = new int[] { 2, 3 };
            productService.SaveData(productService.GetAllData().Append(product));
            component.Instance.MinRating = minRating;

            // Act
            component.Instance.ApplyFilters();

            // Reset

            // Assert
            Assert.That(component.Instance.FilteredProducts.Any(p => p.Id == product.Id), Is.False);

            // Cleanup
            productService.DeleteData(product.Id);
        }

        [Test]
        /// <summary>
        /// Tests that the ApplyFilters method returns filtered products when the search term and minimum rating match.
        /// </summary>
        public void ApplyFilters_MatchSearchTermAndMinRating_Should_ReturnFilteredProducts()
        {
            // Arrange
            var component = RenderComponent<ProductList>();
            var searchTerm = "Product";
            var minRating = 3;
            var product = TestHelper.ProductService.CreateData();
            product.Name = "Product Match";
            product.Ratings = new int[] { 5, 4, 4 };
            TestHelper.ProductService.UpdateData(product);
            component.Instance.SearchTerm = searchTerm;
            component.Instance.MinRating = minRating;

            // Act
            component.Instance.ApplyFilters();

            // Reset

            // Assert
            Assert.That(component.Instance.FilteredProducts.Any(p => p.Id == product.Id), Is.True);

            // Cleanup
            TestHelper.ProductService.DeleteData(product.Id);
        }

        [Test]
        /// <summary>
        /// Tests that the ApplyFilters method includes a product when the search term matches the product name.
        /// </summary>
        public void ApplyFilters_SearchTerm_MatchOnName_Should_IncludeProduct()
        {
            // Arrange
            var component = RenderComponent<ProductList>();
            var searchTerm = "MatchInName";
            var productService = TestHelper.ProductService;
            var product = productService.CreateData();
            product.Name = "MatchInName";
            product.Description = "Unrelated Description";
            productService.UpdateData(product);
            component.Instance.SearchTerm = searchTerm;

            // Act
            component.Instance.ApplyFilters();

            // Reset

            // Assert
            Assert.That(component.Instance.FilteredProducts.Any(p => p.Id == product.Id), Is.True);

            // Cleanup
            productService.DeleteData(product.Id);
        }

        [Test]
        public void ApplyFilters_SearchTerm_MatchOnDescription_Should_IncludeProduct()
        {
            // Arrange
            var component = RenderComponent<ProductList>();
            var searchTerm = "MatchInDescription";
            var productService = TestHelper.ProductService;
            var product = productService.CreateData();
            product.Name = "Unrelated Name";
            product.Description = "MatchInDescription";
            productService.UpdateData(product);
            component.Instance.SearchTerm = searchTerm;

            // Act
            component.Instance.ApplyFilters();

            // Reset

            // Assert
            Assert.That(component.Instance.FilteredProducts.Any(p => p.Id == product.Id), Is.True);

            // Cleanup
            productService.DeleteData(product.Id);
        }

        #endregion ApplyFilters

    }
}
