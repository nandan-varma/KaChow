using System.Linq;

using Microsoft.AspNetCore.Mvc;

using NUnit.Framework;

using ContosoCrafts.WebSite.Models;
using Microsoft.Extensions.DependencyModel;

namespace UnitTests.Pages.Product.AddRating
{
    public class JsonFileProductServiceTests
    {
        #region TestSetup

        [SetUp]
        public void TestInitialize()
        {
        }

        #endregion TestSetup

        #region AddRating
        //[Test]
        //public void AddRating_InValid_....()
        //{
        //    // Arrange

        //    // Act
        //    //var result = TestHelper.ProductService.AddRating(null, 1);

        //    // Assert
        //    //Assert.AreEqual(false, result);
        //}

        // ....

        [Test]
        public void AddRating_InValid_Product_Null_Should_Return_False()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.AddRating(null, 1);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void AddRating_InValid_Product_Empty_Should_Return_False()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.AddRating("", 1);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void AddRating_Valid_Product_Rating_5_Should_Return_True()
        {
            // Arrange

            // Get the First data item
            var data = TestHelper.ProductService.GetAllData().First();
            var countOriginal = data.Ratings.Length;

            // Act
            var result = TestHelper.ProductService.AddRating(data.Id, 5);
            var dataNewList = TestHelper.ProductService.GetAllData().First();

            // Assert
            Assert.That(result, Is.EqualTo(true));
            Assert.That(dataNewList.Ratings.Length, Is.EqualTo(countOriginal + 1));
            Assert.That(dataNewList.Ratings.Last(), Is.EqualTo(5));
        }

        [Test]
        public void AddRating_InValid_Product_Not_In_DataBase_Should_Return_False()
        {
            // Arrange
            var productId = "non_empty_or_null";

            // Act
            var result = TestHelper.ProductService.AddRating(productId, 5);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void AddRating_InValid_Rating_Negative_Rating_Should_Return_False()
        {
            // Arrange
            var data = TestHelper.ProductService.GetAllData().First();

            // Act
            var result = TestHelper.ProductService.AddRating(data.Id, -1);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void AddRating_InValid_Rating_Rating_Over_Five_Should_Return_False()
        {
            // Arrange
            var data = TestHelper.ProductService.GetAllData().First();

            // Act
            var result = TestHelper.ProductService.AddRating(data.Id, 6);

            // Assert
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void AddRating_Valid_Product_Null_Ratings_Should_Create_Ratings_Array_And_Return_True()
        {
            // Arrange
            var data = TestHelper.ProductService.CreateData();

            // Act
            var result = TestHelper.ProductService.AddRating(data.Id, 5);

            // Reset
            TestHelper.ProductService.DeleteData(data.Id);

            // Assert
            Assert.That(result, Is.EqualTo(true));

        }
        #endregion AddRating
    }
}

namespace UnitTests.Pages.Product.UpdateData
{
    public class JsonFileProductServiceTests
    {
        #region TestSetup

        [SetUp]
        public void TestInitialize()
        {
        }

        #endregion TestSetup
        #region UpdateData

        [Test]
        public void UpdateData_Valid_Product_In_DataBase_Should_Return_Updated_Model()
        {
            // Arrange
            var data = TestHelper.ProductService.CreateData();
            data.Name = "Skibidi";
            data.Description = "Toilet";
            data.Url = "https://www.google.com";
            data.Image = "https://www.google.com";

            data.Price = 0;
            data.CommentList = null;

            // Act
            var resultData = TestHelper.ProductService.UpdateData(data);

            // Reset
            TestHelper.ProductService.DeleteData(data.Id);

            // Assert
            Assert.That(resultData.Name, Is.EqualTo(data.Name));
            Assert.That(resultData.Description, Is.EqualTo(data.Description));
            Assert.That(resultData.Url, Is.EqualTo(data.Url));
            Assert.That(resultData.Image, Is.EqualTo(data.Image));
            Assert.That(resultData.Price, Is.EqualTo(data.Price));
            Assert.That(resultData.CommentList, Is.EqualTo(data.CommentList));
        }

        [Test]
        public void UpdateData_InValid_Product_Not_In_DataBase_Should_Return_Null()
        {
            // Arrange
            var data = new ProductModel();
            data.Id = "not_null_or_empty";

            // Act
            var resultData = TestHelper.ProductService.UpdateData(data);

            // Assert
            Assert.That(resultData, Is.EqualTo(null));
        }
        #endregion UpdateData
    }
}

namespace UnitTests.Pages.Product.GetProductById
{

    public class JsonFileProductServiceTests
    {
        #region TestSetup

        [SetUp]
        public void TestInitialize()
        {
        }

        #endregion TestSetup

        #region GetProductById

        [Test]
        public void GetProductById_Valid_Product_In_DataBase_Should_Return_Product()
        {
            // Arrange
            var data = TestHelper.ProductService.GetAllData().First();

            // Act
            var resultData = TestHelper.ProductService.GetProductById(data.Id);

            // Assert
            Assert.That(data.Id, Is.EqualTo(resultData.Id));
        }

       

        #endregion GetProductById
    }

}