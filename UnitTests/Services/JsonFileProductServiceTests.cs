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

        [SetUp]
        public void TestInitialize()
        {
        }

        [TearDown]
        public void TestCleanup()
        {
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

            // Act
            var Result = TestHelper.ProductService.AddRating(Data.Id, 5);
            var DataNewList = TestHelper.ProductService.GetAllData().First();

            // Assert
            Assert.That(Result, Is.EqualTo(true));
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

        [Test]
        /// <summary>
        /// Tests that GetProductById returns null when the product ID is empty.
        /// </summary>
        public void GetProductById_ProductId_Is_Empty_Should_Return_Null()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.GetProductById("");

            // Assert
            Assert.That(result, Is.Null);
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

        #region GetAllData

        [Test]
        /// <summary>
        /// Tests that GetAllData returns a non-null collection.
        /// </summary>
        public void GetAllData_Should_Return_NonNull_Collection()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.GetAllData();

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        /// <summary>
        /// Tests that GetAllData returns a collection with at least one product.
        /// </summary>
        public void GetAllData_Should_Return_Collection_With_At_Least_One_Product()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.GetAllData();

            // Assert
            Assert.That(result.Count(), Is.GreaterThan(0));
        }

        #endregion GetAllData

        #region CreateData

        [Test]
        /// <summary>
        /// Tests that CreateData creates a new product with default values.
        /// </summary>
        public void CreateData_Should_Create_New_Product_With_Default_Values()
        {
            // Arrange

            // Act
            var result = TestHelper.ProductService.CreateData();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Enter Name"));
            Assert.That(result.Description, Is.EqualTo("Enter Description"));
            Assert.That(result.Url, Is.EqualTo("Enter URL"));
            Assert.That(result.Image, Is.EqualTo(""));
        }

        #endregion CreateData

        #region SaveData

        [Test]
        /// <summary>
        /// Tests that SaveData saves the product data correctly.
        /// </summary>
        public void SaveData_Should_Save_Product_Data_Correctly()
        {
            // Arrange
            var products = TestHelper.ProductService.GetAllData();
            var newProduct = new ProductModel
            {
                Id = "new_id",
                Name = "New Product",
                Description = "Description of new product",
                Price = 100
            };
            var updatedProducts = products.Append(newProduct);

            // Act
            TestHelper.ProductService.SaveData(updatedProducts);
            var result = TestHelper.ProductService.GetAllData().FirstOrDefault(p => p.Id == newProduct.Id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(newProduct.Name));
            Assert.That(result.Description, Is.EqualTo(newProduct.Description));
        }

        #endregion SaveData

        #region GetProductsByRating

        [Test]
        /// <summary>
        /// Tests that GetProductsByRating returns products with ratings greater than or equal to the specified minimum rating.
        /// </summary>
        public void GetProductsByRating_Should_Return_Products_With_Ratings_Greater_Than_Or_Equal_To_MinRating()
        {
            // Arrange
            var minRating = 3;

            // Act
            var result = TestHelper.ProductService.GetProductsByRating(minRating);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.All(p => p.Ratings.Average() >= minRating), Is.True);
        }

        [Test]
        /// <summary>
        /// Tests that GetProductsByRating returns an empty collection when no products meet the minimum rating.
        /// </summary>
        public void GetProductsByRating_No_Products_Meet_MinRating_Should_Return_Empty_Collection()
        {
            // Arrange
            var minRating = 6;

            // Act
            var result = TestHelper.ProductService.GetProductsByRating(minRating);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        /// <summary>
        /// Tests that GetProductsByRating returns products with non-null ratings.
        /// </summary>
        public void GetProductsByRating_Should_Return_Products_With_NonNull_Ratings()
        {
            // Arrange
            var minRating = 3;

            // Act
            var result = TestHelper.ProductService.GetProductsByRating(minRating);

            // Assert
            Assert.That(result.All(p => p.Ratings != null), Is.True);
        }

        [Test]
        /// <summary>
        /// Tests that GetProductsByRating returns products with non-empty ratings.
        /// </summary>
        public void GetProductsByRating_Should_Return_Products_With_NonEmpty_Ratings()
        {
            // Arrange
            var minRating = 3;

            // Act
            var result = TestHelper.ProductService.GetProductsByRating(minRating);

            // Assert
            Assert.That(result.All(p => p.Ratings.Any()), Is.True);
        }

        [Test]
        /// <summary>
        /// Tests that GetProductsByRating returns products with average ratings greater than or equal to the specified minimum rating.
        /// </summary>
        public void GetProductsByRating_Should_Return_Products_With_Average_Ratings_Greater_Than_Or_Equal_To_MinRating()
        {
            // Arrange
            var minRating = 3;

            // Act
            var result = TestHelper.ProductService.GetProductsByRating(minRating);

            // Assert
            Assert.That(result.All(p => p.Ratings.Average() >= minRating), Is.True);
        }

        #endregion GetProductsByRating

        #region SearchProducts

        [Test]
        /// <summary>
        /// Tests that SearchProducts returns all products when the search term is empty.
        /// </summary>
        public void SearchProducts_With_Empty_SearchTerm_Should_Return_All_Products()
        {
            // Arrange
            var searchTerm = string.Empty;
            var products = TestHelper.ProductService.GetAllData();

            // Act
            var result = TestHelper.ProductService.SearchProducts(searchTerm);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(products.Count()));
        }

        #endregion SearchProducts

        #region FilterProductsByRating

        [Test]
        /// <summary>
        /// Tests that FilterProductsByRating returns all products when the minimum rating is zero.
        /// </summary>
        public void FilterProductsByRating_With_Zero_MinRating_Should_Return_All_Products()
        {
            // Arrange
            var minRating = 0;
            var products = TestHelper.ProductService.GetAllData();

            // Act
            var result = TestHelper.ProductService.FilterProductsByRating(products, minRating);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(products.Count()));
        }

        [Test]
        /// <summary>
        /// Tests that FilterProductsByRating returns products with non-null ratings.
        /// </summary>
        public void FilterProductsByRating_Should_Return_Products_With_NonNull_Ratings()
        {
            // Arrange
            var minRating = 3;
            var products = TestHelper.ProductService.GetAllData();

            // Act
            var result = TestHelper.ProductService.FilterProductsByRating(products, minRating);

            // Assert
            Assert.That(result.All(p => p.Ratings != null), Is.True);
        }

        [Test]
        /// <summary>
        /// Tests that FilterProductsByRating returns products with non-empty ratings.
        /// </summary>
        public void FilterProductsByRating_Should_Return_Products_With_NonEmpty_Ratings()
        {
            // Arrange
            var minRating = 3;
            var products = TestHelper.ProductService.GetAllData();

            // Act
            var result = TestHelper.ProductService.FilterProductsByRating(products, minRating);

            // Assert
            Assert.That(result.All(p => p.Ratings.Any()), Is.True);
        }

        [Test]
        /// <summary>
        /// Tests that FilterProductsByRating returns products with average ratings greater than or equal to the specified minimum rating.
        /// </summary>
        public void FilterProductsByRating_Should_Return_Products_With_Average_Ratings_Greater_Than_Or_Equal_To_MinRating()
        {
            // Arrange
            var minRating = 3;
            var products = TestHelper.ProductService.GetAllData();

            // Act
            var result = TestHelper.ProductService.FilterProductsByRating(products, minRating);

            // Assert
            Assert.That(result.All(p => p.Ratings.Average() >= minRating), Is.True);
        }

        #endregion FilterProductsByRating

        #region GetPageOfComments
        /// <summary>
        /// Null Product Id should return null Comment List
        /// </summary>
        [Test]
        public void GetPageOfComments_InValid_Null_Id_Should_Return_Null_List()
        {
            // Arrange
            string Id = null;

            // Act
            var CommentPage = TestHelper.ProductService.GetPageOfComments(Id, 0);

            // Assert
            Assert.That(CommentPage, Is.EqualTo(null));
        }

        /// <summary>
        /// Empty Id should return null Comment List
        /// </summary>
        [Test]
        public void GetPageOfComments_InValid_Empty_Id_Should_Return_Null_List()
        {
            // Arrange
            string Id = string.Empty;

            // Act
            var CommentPage = TestHelper.ProductService.GetPageOfComments(Id, 0);

            // Assert
            Assert.That(CommentPage, Is.EqualTo(null));
        }

        /// <summary>
        /// Non null or Empty Id not in DataBase should return Null List
        /// </summary>
        [Test]
        public void GetPageOfComments_InValid_Non_Null_Or_Empty_Id_Should_Return_Null_List()
        {
            // Arrange
            string Id = "Random_Non_Null_Or_Empty";

            // Act
            var CommentPage = TestHelper.ProductService.GetPageOfComments(Id, 0);

            // Assert
            Assert.That(CommentPage, Is.EqualTo(null));
        }

        /// <summary>
        /// Negative Page number, Valid Product Id should return null Comment List
        /// </summary>
        [Test]
        public void GetPageOfComments_InValid_Negative_Page_Number_Should_Return_Null_List()
        {
            // Arrange
            string Id = TestHelper.ProductService.GetAllData().First().Id;

            // Act
            var Comments = TestHelper.ProductService.GetPageOfComments(Id, -1);

            // Assert
            Assert.That(Comments, Is.EqualTo(null));
        }

        /// <summary>
        /// Non initialized Comment List Product should return null list
        /// </summary>
        [Test]
        public void GetPageOfComments_Valid_Product_No_Comments_Should_Return_Null_List()
        {
            // Arrange
            var Product = TestHelper.ProductService.CreateData();

            // Act
            var Comments = TestHelper.ProductService.GetPageOfComments(Product.Id, 0);

            // Assert
            Assert.That(Comments, Is.EqualTo(null));
        }

        /// <summary>
        /// Valid Product, PageNumber > (Product.Comments.Count / 10) should return null list
        /// </summary>
        [Test]
        public void GetPageOfComments_Valid_Product_Has_Comments_InValid_Page_Number_Greater_Than_Number_Of_Comments_Should_Return_Null_List()
        {
            // Arrange
            ProductModel model = new ProductModel();
            model.CommentList = new List<CommentModel>(); 
            model.CommentList.Add(new CommentModel());
            TestHelper.ProductService.CreateProduct(model);

            // Act
            var Comments = TestHelper.ProductService.GetPageOfComments(model.Id, model.CommentList.Count);

            // Assert
            Assert.That(Comments, Is.EqualTo(null));
        }

        /// <summary>
        /// Asking for page of comments on a product with valid page data should return a comment list
        /// </summary>
        [Test]
        public void GetPageOfComments_Valid_Id_Valid_Product_With_Comments_Valid_Page_Number_Should_Return_Non_Null_List()
        {
            // Arrange
            ProductModel product = new ProductModel();
            product.CommentList = new List<CommentModel>();
            product.CommentList.Add(new CommentModel());
            TestHelper.ProductService.CreateProduct(product);

            // Act
            var CommentList = TestHelper.ProductService.GetPageOfComments(product.Id, 0);

            // Assert
            Assert.That(CommentList.Count, Is.GreaterThan(0));

        }

        #endregion GetPageOfComments

        #region GetCommentListSize

        /// <summary>
        /// Null Id should return 0
        /// </summary>
        [Test]
        public void GetCommentListSize_Null_Id_Should_Return_Zero()
        {
            // Arrange
            string Id = null;


            // Act
            var ListSize = TestHelper.ProductService.GetCommentListSize(Id);

            // Assert
            Assert.That(ListSize, Is.EqualTo(0));
        }

        /// <summary>
        /// Empty string id should return 0
        /// </summary>
        [Test]
        public void GetCommentListSize_Empty_Id_Should_Return_Zero()
        {
            // Arrange
            string Id = string.Empty;

            // Act
            var ListSize = TestHelper.ProductService.GetCommentListSize(Id);

            // Assert
            Assert.That(ListSize, Is.EqualTo(0));

        }

        /// <summary>
        /// Valid Id, non product, should return 0
        /// </summary>
        [Test]
        public void GetCommentListSize_Valid_Id_InValid_Product_Should_Return_Zero()
        {
            // Arrange
            var Id = "Random_Non_Null_Or_Empty";

            // Act
            var ListSize = TestHelper.ProductService.GetCommentListSize(Id);

            // Assert
            Assert.That(ListSize, Is.EqualTo(0));
        }

        /// <summary>
        /// Non initialized comment list should return 0
        /// </summary>
        [Test]
        public void GetCommentListSize_Non_Iniitialized_Comment_List_Should_Return_Zero()
        {
            // Arrange
            var Id = TestHelper.ProductService.GetAllData().Last().Id;

            // Act
            var ListSize = TestHelper.ProductService.GetCommentListSize(Id);

            // Assert
            Assert.That(ListSize, Is.EqualTo(0));
        }

        /// <summary>
        /// List with comments should reutrn positive non zero number
        /// </summary>
        [Test]
        public void GetCommentListSize_Product_With_Comments_Should_Return_Positive_Non_Zero_Count()
        {
            // Arrange
            var Product = new ProductModel();
            Product.CommentList = new List<CommentModel>();
            Product.CommentList.Add(new CommentModel());
            TestHelper.ProductService.CreateProduct(Product);
            var Id = Product.Id;

            // Act
            var ListSize = TestHelper.ProductService.GetCommentListSize(Id);

            // Assert
            Assert.That(ListSize, Is.GreaterThan(0));
        }

        #endregion GetCommentListSize

        #region AddComment

        /// <summary>
        /// Null id should return false
        /// </summary>
        [Test]
        public void AddComment_Null_Id_Should_return_False()
        {
            // Arrange
            string id = null;

            // Act
            var result = TestHelper.ProductService.AddComment(id, new CommentModel());

            // Assert
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// If id is empty result should be false
        /// </summary>
        [Test]
        public void AddComment_Empty_Id_Should_Return_False()
        {
            // Arrange
            string id = string.Empty;

            // Act
            var result = TestHelper.ProductService.AddComment(id, new CommentModel());

            // Assert
            Assert.That(result, Is.False);
        }
        #endregion AddComment
    }
}
