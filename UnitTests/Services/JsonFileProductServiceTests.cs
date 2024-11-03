using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using ContosoCrafts.WebSite.Models;
using System.Collections.Generic;

namespace UnitTests.Services
{
    public class JsonFileProductServiceTests
    {
        #region TestSetup

        private IEnumerable<ProductModel> OriginalProducts;

        [SetUp]
        public void TestInitialize()
        {
            OriginalProducts = TestHelper.ProductService.GetAllData();
        }

        [TearDown]
        public void TestCleanup()
        {
            TestHelper.ProductService.SaveData(OriginalProducts);
        }

        #endregion TestSetup

        #region AddRating
        [Test]
        /// <summary>
        /// Tests that AddRating returns false when the product is null.
        /// </summary>
        public void AddRating_InValid_Product_Null_Should_Return_False()
        {
            // Arrange

            // Act
            var Result = TestHelper.ProductService.AddRating(null, 1);

            // Assert
            Assert.That(Result, Is.EqualTo(false));
        }

        [Test]
        /// <summary>
        /// Tests that AddRating returns false when the product is an empty string.
        /// </summary>
        public void AddRating_InValid_Product_Empty_Should_Return_False()
        {
            // Arrange

            // Act
            var Result = TestHelper.ProductService.AddRating("", 1);

            // Assert
            Assert.That(Result, Is.EqualTo(false));
        }

        [Test]
        /// <summary>
        /// Tests that AddRating returns true when a valid product is rated with 5.
        /// </summary>
        public void AddRating_Valid_Product_Rating_5_Should_Return_True()
        {
            // Arrange
            var Data = TestHelper.ProductService.GetAllData().First();
            var CountOriginal = Data.Ratings.Length;

            // Act
            var Result = TestHelper.ProductService.AddRating(Data.Id, 5);
            var DataNewList = TestHelper.ProductService.GetAllData().First();

            // Assert
            Assert.That(Result, Is.EqualTo(true));
            Assert.That(DataNewList.Ratings.Length, Is.EqualTo(CountOriginal + 1));
            Assert.That(DataNewList.Ratings.Last(), Is.EqualTo(5));
        }

        [Test]
        /// <summary>
        /// Tests that AddRating returns false when the product is not in the database.
        /// </summary>
        public void AddRating_InValid_Product_Not_In_DataBase_Should_Return_False()
        {
            // Arrange
            var ProductId = "non_empty_or_null";

            // Act
            var Result = TestHelper.ProductService.AddRating(ProductId, 5);

            // Assert
            Assert.That(Result, Is.EqualTo(false));
        }

        [Test]
        /// <summary>
        /// Tests that AddRating returns false when the rating is negative.
        /// </summary>
        public void AddRating_InValid_Rating_Negative_Rating_Should_Return_False()
        {
            // Arrange
            var Data = TestHelper.ProductService.GetAllData().First();

            // Act
            var Result = TestHelper.ProductService.AddRating(Data.Id, -1);

            // Assert
            Assert.That(Result, Is.EqualTo(false));
        }

        [Test]
        /// <summary>
        /// Tests that AddRating returns false when the rating is over five.
        /// </summary>
        public void AddRating_InValid_Rating_Rating_Over_Five_Should_Return_False()
        {
            // Arrange
            var Data = TestHelper.ProductService.GetAllData().First();

            // Act
            var Result = TestHelper.ProductService.AddRating(Data.Id, 6);

            // Assert
            Assert.That(Result, Is.EqualTo(false));
        }

        [Test]
        /// <summary>
        /// Tests that AddRating creates a ratings array and returns true when ratings are null.
        /// </summary>
        public void AddRating_Valid_Product_Null_Ratings_Should_Create_Ratings_Array_And_Return_True()
        {
            // Arrange
            var Data = TestHelper.ProductService.CreateData();

            // Act
            var Result = TestHelper.ProductService.AddRating(Data.Id, 5);

            // Reset
            TestHelper.ProductService.DeleteData(Data.Id);

            // Assert
            Assert.That(Result, Is.EqualTo(true));
        }
        #endregion AddRating



    }
}
