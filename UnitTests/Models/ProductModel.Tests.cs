using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using NUnit.Framework;
using ContosoCrafts.WebSite.Models;

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
        /// Tests that valid properties return the correct values.
        /// </summary>
        public void PropertyTests_Valid_Properties_Should_Return_Correct_Values()
        {
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
            var Context = new ValidationContext(Product) { MemberName = "Name" };
            var Results = new List<ValidationResult>();

            Product.Name = ""; // Invalid
            bool IsValid = Validator.TryValidateProperty(Product.Name, Context, Results);
            Assert.That(IsValid, Is.False);
        }

        [Test]
        /// <summary>
        /// Tests that a valid Name property returns true for validation.
        /// </summary>
        public void ValidationTests_Name_Valid_Should_Return_True()
        {
            var Context = new ValidationContext(Product) { MemberName = "Name" };
            var Results = new List<ValidationResult>();

            Product.Name = "Valid Name"; // Valid
            bool IsValid = Validator.TryValidateProperty(Product.Name, Context, Results);
            Assert.That(IsValid, Is.True);
        }

        [Test]
        /// <summary>
        /// Tests that a negative Price property returns false for validation.
        /// </summary>
        public void ValidationTests_Price_Negative_Should_Return_False()
        {
            var Context = new ValidationContext(Product) { MemberName = "Price" };
            var Results = new List<ValidationResult>();

            Product.Price = -2; // Invalid
            bool IsValid = Validator.TryValidateProperty(Product.Price, Context, Results);
            Assert.That(IsValid, Is.False);
        }

        [Test]
        /// <summary>
        /// Tests that a valid Price property returns true for validation.
        /// </summary>
        public void ValidationTests_Price_Valid_Should_Return_True()
        {
            var Context = new ValidationContext(Product) { MemberName = "Price" };
            var Results = new List<ValidationResult>();

            Product.Price = 50; // Valid
            bool IsValid = Validator.TryValidateProperty(Product.Price, Context, Results);
            Assert.That(IsValid, Is.True);
        }

        #endregion ValidationTests

        #region ToStringTest

        [Test]
        /// <summary>
        /// Tests that ToString returns the correct JSON representation of the product.
        /// </summary>
        public void ToStringTest_Valid_Product_Should_Return_Correct_Json()
        {
            string ExpectedJson = JsonSerializer.Serialize(Product);
            Assert.That(Product.ToString(), Is.EqualTo(ExpectedJson));
        }

        #endregion ToStringTest
    }
}
