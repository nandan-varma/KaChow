using ContosoCrafts.WebSite.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

// Namespace for Model UnitTests
namespace UnitTests.Models
{
    public class ProductModelTests
    {
        #region TestSetup

        private ProductModel Product;

        [SetUp]
        public void TestInitialize()
        {
            Product = new ProductModel
            {
                Id = "1",
                Image = "image.png",
                Url = "http://example.com",
                Name = "Test Product",
                Model = "Model X",
                Description = "This is a test product.",
                Ratings = new int[] { 5, 4, 3 },
                Price = 50,
                CommentList = new List<CommentModel>
                {
                    new CommentModel { Comment = "Great product!" }
                }
            };
        }

        #endregion TestSetup

        #region PropertyTests

        [Test]
        /// <summary>
        /// Tests that valid properties return the expected values.
        /// </summary>
        public void PropertyTests_Valid_Properties_Should_Return_Expected_Values()
        {
            // Arrange & Act are implicit in property initialization

            // Assert
            Assert.That(Product.Id, Is.EqualTo("1"));
            Assert.That(Product.Image, Is.EqualTo("image.png"));
            Assert.That(Product.Url, Is.EqualTo("http://example.com"));
            Assert.That(Product.Name, Is.EqualTo("Test Product"));
            Assert.That(Product.Model, Is.EqualTo("Model X"));
            Assert.That(Product.Description, Is.EqualTo("This is a test product."));
            Assert.That(new int[] { 5, 4, 3 }, Is.EqualTo(Product.Ratings));
            Assert.That(Product.Price, Is.EqualTo(50));
            Assert.That(Product.CommentList.Count, Is.EqualTo(1));
            Assert.That(Product.CommentList[0].Comment, Is.EqualTo("Great product!"));
        }

        #endregion PropertyTests

        #region ValidationTests

        [Test]
        /// <summary>
        /// Tests that an empty Name property returns false for validation.
        /// </summary>
        public void ValidationTests_Name_Empty_Should_Return_False()
        {
            // Arrange
            var Context = new ValidationContext(Product) { MemberName = "Name" };
            var Results = new List<ValidationResult>();

            // Act
            Product.Name = ""; // Invalid
            bool IsValid = Validator.TryValidateProperty(Product.Name, Context, Results);

            // Assert
            Assert.That(IsValid, Is.False);
        }

        [Test]
        /// <summary>
        /// Tests that a valid Name property returns true for validation.
        /// </summary>
        public void ValidationTests_Name_Valid_Should_Return_True()
        {
            // Arrange
            var Context = new ValidationContext(Product) { MemberName = "Name" };
            var Results = new List<ValidationResult>();

            // Act
            Product.Name = "Valid Name"; // Valid
            bool IsValid = Validator.TryValidateProperty(Product.Name, Context, Results);

            // Assert
            Assert.That(IsValid, Is.True);
        }

        [Test]
        /// <summary>
        /// Tests that a negative Price property returns false for validation.
        /// </summary>
        public void ValidationTests_Price_Negative_Should_Return_False()
        {
            // Arrange
            var Context = new ValidationContext(Product) { MemberName = "Price" };
            var Results = new List<ValidationResult>();

            // Act
            Product.Price = -2; // Invalid
            bool IsValid = Validator.TryValidateProperty(Product.Price, Context, Results);

            // Assert
            Assert.That(IsValid, Is.False);
        }

        [Test]
        /// <summary>
        /// Tests that a valid Price property returns true for validation.
        /// </summary>
        public void ValidationTests_Price_Valid_Should_Return_True()
        {
            // Arrange
            var Context = new ValidationContext(Product) { MemberName = "Price" };
            var Results = new List<ValidationResult>();

            // Act
            Product.Price = 50; // Valid
            bool IsValid = Validator.TryValidateProperty(Product.Price, Context, Results);

            // Assert
            Assert.That(IsValid, Is.True);
        }

        #endregion ValidationTests

        #region ToStringTest

        [Test]
        /// <summary>
        /// Tests that ToString returns the expected JSON representation of the product.
        /// </summary>
        public void ToStringTest_Valid_Product_Should_Return_Expected_Json()
        {
            // Arrange
            string ExpectedJson = JsonSerializer.Serialize(Product);

            // Act
            string ActualJson = Product.ToString();

            // Assert
            Assert.That(ActualJson, Is.EqualTo(ExpectedJson));
        }

        #endregion ToStringTest
    }
}
