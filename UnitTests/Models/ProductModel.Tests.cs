using ContosoCrafts.WebSite.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

// Namespace for Model UnitTests
namespace UnitTests.Models
{
    public class ProductModelTest
    {
        #region TestSetup

        private ProductModel _product;

        [SetUp]
        public void TestInitialize()
        {
            // Arrange
            _product = new ProductModel
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

            // Reset
        }

        #endregion TestSetup

        #region PropertyTests_Valid_Properties_Should_Return_Expected_Values

        [Test]
        /// <summary>
        /// Tests that valid properties return the expected values.
        /// </summary>
        public void PropertyTests_Valid_Properties_Should_Return_Expected_Values()
        {
            // Arrange & Act are implicit in property initialization

            // Assert
            Assert.That(_product.Id, Is.EqualTo("1"));
            Assert.That(_product.Image, Is.EqualTo("image.png"));
            Assert.That(_product.Url, Is.EqualTo("http://example.com"));
            Assert.That(_product.Name, Is.EqualTo("Test Product"));
            Assert.That(_product.Model, Is.EqualTo("Model X"));
            Assert.That(_product.Description, Is.EqualTo("This is a test product."));
            Assert.That(new int[] { 5, 4, 3 }, Is.EqualTo(_product.Ratings));
            Assert.That(_product.Price, Is.EqualTo(50));
            Assert.That(_product.CommentList.Count, Is.EqualTo(1));
            Assert.That(_product.CommentList[0].Comment, Is.EqualTo("Great product!"));
        }

        #endregion PropertyTests_Valid_Properties_Should_Return_Expected_Values

        #region ValidationTests_Name_Empty_Should_Return_False

        [Test]
        /// <summary>
        /// Tests that an empty Name property returns false for validation.
        /// </summary>
        public void ValidationTests_Name_Empty_Should_Return_False()
        {
            // Arrange
            var context = new ValidationContext(_product) { MemberName = "Name" };
            var results = new List<ValidationResult>();

            // Act
            _product.Name = ""; // Invalid
            var isValid = Validator.TryValidateProperty(_product.Name, context, results);

            // Reset

            // Assert
            Assert.That(isValid, Is.False);
        }

        #endregion ValidationTests_Name_Empty_Should_Return_False

        #region ValidationTests_Name_Valid_Should_Return_True

        [Test]
        /// <summary>
        /// Tests that a valid Name property returns true for validation.
        /// </summary>
        public void ValidationTests_Name_Valid_Should_Return_True()
        {
            // Arrange
            var context = new ValidationContext(_product) { MemberName = "Name" };
            var results = new List<ValidationResult>();

            // Act
            _product.Name = "Valid Name"; // Valid
            var isValid = Validator.TryValidateProperty(_product.Name, context, results);

            // Reset

            // Assert
            Assert.That(isValid, Is.True);
        }

        #endregion ValidationTests_Name_Valid_Should_Return_True

        #region ValidationTests_Price_Negative_Should_Return_False

        [Test]
        /// <summary>
        /// Tests that a negative Price property returns false for validation.
        /// </summary>
        public void ValidationTests_Price_Negative_Should_Return_False()
        {
            // Arrange
            var context = new ValidationContext(_product) { MemberName = "Price" };
            var results = new List<ValidationResult>();

            // Act
            _product.Price = -2; // Invalid
            var isValid = Validator.TryValidateProperty(_product.Price, context, results);

            // Reset

            // Assert
            Assert.That(isValid, Is.False);
        }

        #endregion ValidationTests_Price_Negative_Should_Return_False

        #region ValidationTests_Price_Valid_Should_Return_True

        [Test]
        /// <summary>
        /// Tests that a valid Price property returns true for validation.
        /// </summary>
        public void ValidationTests_Price_Valid_Should_Return_True()
        {
            // Arrange
            var context = new ValidationContext(_product) { MemberName = "Price" };
            var results = new List<ValidationResult>();

            // Act
            _product.Price = 50; // Valid
            var isValid = Validator.TryValidateProperty(_product.Price, context, results);

            // Reset

            // Assert
            Assert.That(isValid, Is.True);
        }

        #endregion ValidationTests_Price_Valid_Should_Return_True

        #region ToStringTest_Valid_Product_Should_Return_Expected_Json

        [Test]
        /// <summary>
        /// Tests that ToString returns the expected JSON representation of the product.
        /// </summary>
        public void ToStringTest_Valid_Product_Should_Return_Expected_Json()
        {
            // Arrange
            var expectedJson = JsonSerializer.Serialize(_product);

            // Act
            var actualJson = _product.ToString();

            // Reset

            // Assert
            Assert.That(actualJson, Is.EqualTo(expectedJson));
        }

        #endregion ToStringTest_Valid_Product_Should_Return_Expected_Json
    }
}
