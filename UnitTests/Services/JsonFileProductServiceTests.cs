using ContosoCrafts.WebSite.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

// Namespace for Service UnitTests
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
        public void AddRating_Product_Is_Null_Should_Return_False()
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
        public void AddRating_Product_Is_Empty_Should_Return_False()
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
        public void AddRating_Product_Is_Valid_Should_Return_True()
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
        public void AddRating_Product_Not_In_DataBase_Should_Return_False()
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
        public void AddRating_Rating_Is_Negative_Should_Return_False()
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
        public void AddRating_Rating_Over_Five_Should_Return_False()
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
        public void AddRating_Ratings_Null_Should_Create_Ratings_Array_And_Return_True()
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

        #region UpdateData

        [Test]
        /// <summary>
        /// Tests that UpdateData returns the updated model for a valid product in the database.
        /// </summary>
        public void UpdateData_Product_In_DataBase_Should_Return_Updated_Model()
        {
            // Arrange
            var Data = TestHelper.ProductService.CreateData();
            Data.Name = "Skibidi";
            Data.Description = "Toilet";
            Data.Url = "https://www.google.com";
            Data.Image = "https://www.google.com";
            Data.Price = 0;
            Data.CommentList = null;

            // Act
            var ResultData = TestHelper.ProductService.UpdateData(Data);

            // Reset
            TestHelper.ProductService.DeleteData(Data.Id);

            // Assert
            Assert.That(ResultData.Name, Is.EqualTo(Data.Name));
            Assert.That(ResultData.Description, Is.EqualTo(Data.Description));
            Assert.That(ResultData.Url, Is.EqualTo(Data.Url));
            Assert.That(ResultData.Image, Is.EqualTo(Data.Image));
            Assert.That(ResultData.Price, Is.EqualTo(Data.Price));
            Assert.That(ResultData.CommentList, Is.EqualTo(Data.CommentList));
        }

        [Test]
        /// <summary>
        /// Tests that UpdateData returns null for an invalid product not in the database.
        /// </summary>
        public void UpdateData_Product_Not_In_DataBase_Should_Return_Null()
        {
            // Arrange
            var Data = new ProductModel();
            Data.Id = "not_null_or_empty";

            // Act
            var ResultData = TestHelper.ProductService.UpdateData(Data);

            // Assert
            Assert.That(ResultData, Is.EqualTo(null));
        }

        #endregion UpdateData

        #region GetProductById

        [Test]
        /// <summary>
        /// Tests that GetProductById returns a product for a valid product in the database.
        /// </summary>
        public void GetProductById_Product_In_DataBase_Should_Return_Product()
        {
            // Arrange
            var Data = TestHelper.ProductService.GetAllData().First();

            // Act
            var ResultData = TestHelper.ProductService.GetProductById(Data.Id);

            // Assert
            Assert.That(Data.Id, Is.EqualTo(ResultData.Id));
        }

        [Test]
        /// <summary>
        /// Tests that GetProductById returns null for an invalid product not in the database.
        /// </summary>
        public void GetProductById_Product_Not_In_DataBase_Should_Return_Null()
        {
            // Arrange
            var DataId = "Skibidi";

            // Act
            var ResultData = TestHelper.ProductService.GetProductById(DataId);

            // Assert
            Assert.That(ResultData, Is.EqualTo(null));
        }

        #endregion GetProductById

        #region DeleteData

        [Test]
        /// <summary>
        /// Tests that DeleteData successfully removes a valid product from the database.
        /// </summary>
        public void DeleteData_Product_In_DataBase_Should_Remove_Product()
        {
            // Arrange
            var Data = TestHelper.ProductService.CreateData();
            Data.Name = "Skibidi";
            Data.Description = "Toilet";
            Data.Url = "https://www.google.com";
            Data.Image = "https://www.google.com";
            Data.Price = 0;
            Data.CommentList = null;

            // Act
            var DeleteResult = TestHelper.ProductService.DeleteData(Data.Id);

            // Assert that the delete operation was successful
            Assert.That(DeleteResult, Is.True);

            // Act again to confirm product is no longer in the database
            var ResultData = TestHelper.ProductService.GetProductById(Data.Id);

            // Assert
            Assert.That(ResultData, Is.Null);
        }

        [Test]
        /// <summary>
        /// Tests that DeleteData returns false for an invalid product not in the database.
        /// </summary>
        public void DeleteData_Product_Not_In_DataBase_Should_Return_False()
        {
            // Arrange
            var DataId = "not_null_or_empty"; // an ID that doesn't exist in the database

            // Act
            var DeleteResult = TestHelper.ProductService.DeleteData(DataId);

            // Assert
            Assert.That(DeleteResult, Is.False);
        }

        #endregion DeleteData

        #region CreateProduct

        [Test]
        /// <summary>
        /// Tests that CreateProduct adds a new product.
        /// </summary>
        public void CreateProduct_Product_Is_Valid_Should_Add_Product()
        {
            // Arrange
            var newProduct = new ProductModel
            {
                Id = "new_id",
                Name = "New Product",
                Description = "Description of new product",
                Price = 100
            };

            // Act
            TestHelper.ProductService.CreateProduct(newProduct);
            var createdProduct = TestHelper.ProductService.GetAllData().FirstOrDefault(p => p.Id == newProduct.Id);

            // Assert
            Assert.That(createdProduct, Is.Not.Null);
            Assert.That(createdProduct.Name, Is.EqualTo(newProduct.Name));
            Assert.That(createdProduct.Description, Is.EqualTo(newProduct.Description));
        }

        [Test]
        /// <summary>
        /// Tests that CreateProduct does not add a null product.
        /// </summary>
        public void CreateProduct_Product_Is_Null_Should_Not_Add_Product()
        {
            // Arrange
            var originalProductCount = TestHelper.ProductService.GetAllData().Count();

            // Act
            var result = TestHelper.ProductService.CreateProduct(null);

            // Assert
            Assert.That(result, Is.False);
            var productCountAfterCreate = TestHelper.ProductService.GetAllData().Count();
            Assert.That(productCountAfterCreate, Is.EqualTo(originalProductCount));
        }

        #endregion CreateProduct
    }
}
