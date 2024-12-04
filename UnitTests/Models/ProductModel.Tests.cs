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
                Material = ProductTypeEnum.Cotton,
                Size = ProductSizeEnum.M,
                CommentList = new List<CommentModel>
                {
                    new CommentModel { Comment = "Great product!" }
                }
            };

            // Reset
        }

        #endregion TestSetup

        #region Property

        /// <summary>
        /// Tests that valid properties return the expected values.
        /// </summary>
        [Test]
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
            Assert.That(_product.Material, Is.EqualTo(ProductTypeEnum.Cotton));
            Assert.That(_product.Size, Is.EqualTo(ProductSizeEnum.M));
        }

        #endregion Property

        #region Validation

        /// <summary>
        /// Tests that an empty Name property returns false for validation.
        /// </summary>
        [Test]
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

        /// <summary>
        /// Tests that a valid Name property returns true for validation.
        /// </summary>
        [Test]
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

        /// <summary>
        /// Tests that a negative Price property returns false for validation.
        /// </summary>
        [Test]
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

        /// <summary>
        /// Tests that a valid Price property returns true for validation.
        /// </summary>
        [Test]
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

        /// <summary>
        /// Tests that a valid Material property returns true for validation.
        /// </summary>
        [Test]
        public void ValidationTests_Material_Valid_Should_Return_True()
        {
            // Arrange
            var context = new ValidationContext(_product) { MemberName = "Material" };
            var results = new List<ValidationResult>();

            // Act
            _product.Material = ProductTypeEnum.Wool; // Valid
            var isValid = Validator.TryValidateProperty(_product.Material, context, results);

            // Assert
            Assert.That(isValid, Is.True);
        }

        /// <summary>
        /// Tests that a valid Size property returns true for validation.
        /// </summary>
        [Test]
        public void ValidationTests_Size_Valid_Should_Return_True()
        {
            // Arrange
            var context = new ValidationContext(_product) { MemberName = "Size" };
            var results = new List<ValidationResult>();

            // Act
            _product.Size = ProductSizeEnum.L; // Valid
            var isValid = Validator.TryValidateProperty(_product.Size, context, results);

            // Assert
            Assert.That(isValid, Is.True);
        }

        /// <summary>
        /// Tests that an empty Model property returns false for validation.
        /// </summary>
        [Test]
        public void ValidationTests_Model_Empty_Should_Return_False()
        {
            // Arrange
            var context = new ValidationContext(_product) { MemberName = "Model" };
            var results = new List<ValidationResult>();

            // Act
            _product.Model = ""; // Invalid
            var isValid = Validator.TryValidateProperty(_product.Model, context, results);

            // Assert
            Assert.That(isValid, Is.False);
        }

        /// <summary>
        /// Tests that a valid Model property returns true for validation.
        /// </summary>
        [Test]
        public void ValidationTests_Model_Valid_Should_Return_True()
        {
            // Arrange
            var context = new ValidationContext(_product) { MemberName = "Model" };
            var results = new List<ValidationResult>();

            // Act
            _product.Model = "Valid Model"; // Valid
            var isValid = Validator.TryValidateProperty(_product.Model, context, results);

            // Assert
            Assert.That(isValid, Is.True);
        }

        /// <summary>
        /// Tests that a too long Description property returns false for validation.
        /// </summary>
        [Test]
        public void ValidationTests_Description_Too_Long_Should_Return_False()
        {
            // Arrange
            var context = new ValidationContext(_product) { MemberName = "Description" };
            var results = new List<ValidationResult>();

            // Act
            _product.Description = new string('a', 201); // Invalid
            var isValid = Validator.TryValidateProperty(_product.Description, context, results);

            // Assert
            Assert.That(isValid, Is.False);
        }

        /// <summary>
        /// Tests that a valid Description property returns true for validation.
        /// </summary>
        [Test]
        public void ValidationTests_Description_Valid_Should_Return_True()
        {
            // Arrange
            var context = new ValidationContext(_product) { MemberName = "Description" };
            var results = new List<ValidationResult>();

            // Act
            _product.Description = "Valid Description"; // Valid
            var isValid = Validator.TryValidateProperty(_product.Description, context, results);

            // Assert
            Assert.That(isValid, Is.True);
        }

        /// <summary>
        /// Tests that a valid Ratings property returns true for validation.
        /// </summary>
        [Test]
        public void ValidationTests_Ratings_Valid_Should_Return_True()
        {
            // Arrange
            var context = new ValidationContext(_product) { MemberName = "Ratings" };
            var results = new List<ValidationResult>();

            // Act
            _product.Ratings = new int[] { 3 }; // Valid
            var isValid = Validator.TryValidateProperty(_product.Ratings, context, results);

            // Assert
            Assert.That(isValid, Is.True);
        }

        /// <summary>
        /// Tests that a negative Stock property returns false for validation.
        /// </summary>
        [Test]
        public void ValidationTests_Stock_Negative_Should_Return_False()
        {
            // Arrange
            var context = new ValidationContext(_product) { MemberName = "Stock" };
            var results = new List<ValidationResult>();

            // Act
            _product.Stock = -1; // Invalid
            var isValid = Validator.TryValidateProperty(_product.Stock, context, results);

            // Assert
            Assert.That(isValid, Is.False);
        }

        /// <summary>
        /// Tests that a valid Stock property returns true for validation.
        /// </summary>
        [Test]
        public void ValidationTests_Stock_Valid_Should_Return_True()
        {
            // Arrange
            var context = new ValidationContext(_product) { MemberName = "Stock" };
            var results = new List<ValidationResult>();

            // Act
            _product.Stock = 10; // Valid
            var isValid = Validator.TryValidateProperty(_product.Stock, context, results);

            // Assert
            Assert.That(isValid, Is.True);
        }

        #endregion Validation

        #region ToString

        /// <summary>
        /// Tests that ToString returns the expected JSON representation of the product.
        /// </summary>
        [Test]
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

        #endregion ToString
    }
}
