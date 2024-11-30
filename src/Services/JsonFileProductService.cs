using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;

// Namespace for ContosoCrafts Web Services
namespace ContosoCrafts.WebSite.Services
{

    /// <summary>
    /// A Service class which handles all interactions between
    /// the application and the JSON database
    /// </summary>
    public class JsonFileProductService
    {

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        public JsonFileProductService(IWebHostEnvironment webHostEnvironment)
        {
            // Assign the WebHostEnvironment instance to the property
            WebHostEnvironment = webHostEnvironment;

            // Initialize a mutex to manage concurrent writes
            SaveDataLock = new Mutex();
        }

        // Web environment field
        public IWebHostEnvironment WebHostEnvironment { get; }

        // Mutex lock for saving data
        private static Mutex SaveDataLock;

        public const int PAGE_SIZE = 10;
        
        // Property to retrieve the JSON file path
        private string JsonFileName
        {
            // Combine path to the "plushies.json" file in the web root directory
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "plushies.json"); }
        }

        /// <summary>
        /// Method which retrieves all the data in the database
        /// </summary>
        /// <returns></returns>
        /// /// <returns>Enumerable list of ProductModel objects</returns>
        public IEnumerable<ProductModel> GetAllData()
        {
            // Open the JSON file for reading
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                // Deserialize JSON content into ProductModel array
                return JsonSerializer.Deserialize<ProductModel[]>(jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // Ignore property case in JSON
                    });
            }
        }

        /// <summary>
        /// A method which retrieves the product with Id, id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductModel GetProductById(string id)
        {
            // Fetch all products
            var products = GetAllData();

            // Return the product with matching ID or null if not found
            return products.FirstOrDefault(x => x.Id.Equals(id.Trim()));
        }

        /// <summary>
        /// Add Rating
        /// 
        /// Take in the product ID and the rating
        /// If the rating does not exist, add it
        /// Save the update
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="rating"></param>
        /// /// <returns>True if successful, otherwise false</returns>
        public bool AddRating(string productId, int rating)
        {
            // If the ProductID is invalid, return
            if (string.IsNullOrEmpty(productId))
            {
                return false;
            }

            // Get all products
            var products = GetAllData();

            // Look up the product, if it does not exist, return
            var data = products.FirstOrDefault(x => x.Id.Equals(productId));
            if (data == null)
            {
                return false;
            }

            // Check Rating for boundaries, do not allow ratings below 0
            if (rating < 0)
            {
                return false;
            }

            // Check Rating for boundaries, do not allow ratings above 5
            if (rating > 5)
            {
                return false;
            }

            // Check to see if the rating exist, if there are none, then create the array
            if (data.Ratings == null)
            {
                data.Ratings = new int[] { };
            }

            // Add the Rating to the Array
            var ratings = data.Ratings.ToList();
            ratings.Add(rating);
            data.Ratings = ratings.ToArray();

            // Save the data back to the data store
            SaveData(products);

            return true;
        }

        public bool AddComment(string productId, CommentModel comment)
        {
            // If the comment is null or empty return false
            if (string.IsNullOrEmpty(comment.Comment))
            {
                return false;
            }

            // If the ProductID is invalid, return
            if (string.IsNullOrEmpty(productId))
            {
                return false;
            }

            // Get all products
            var products = GetAllData();

            // Look up the product, if it does not exist, return
            var data = products.FirstOrDefault(x => x.Id.Equals(productId));
            if (data == null)
            {
                return false;
            }

            // If the comment list is null
            if (data.CommentList == null)
            {
                data.CommentList = new List<CommentModel>();
            }

            // Get the comments
            var comments = data.CommentList;

            // Extract the comment from the view
            CommentModel DeepCopyComment = new CommentModel();
            DeepCopyComment.Comment = comment.Comment;

            // Add the deep copy to the comment list
            comments.Add(DeepCopyComment);

            // Reset the view model comment
            comment.Comment = "";

            // Save the data back to the data store
            SaveData(products);

            return true;
        }

        /// <summary>
        /// Gets the length of the comment list for the given product
        /// </summary>
        /// <param name="ProductId"></param>
        /// <returns></returns>
        public int GetCommentListSize(string ProductId)
        {
            if (string.IsNullOrEmpty(ProductId))
            {
                return 0;
            }

            var products = GetAllData();

            var product = products.FirstOrDefault(x=> x.Id.Equals(ProductId));

            return product.CommentList.Count;
        }

        public List<string> GetPageOfComments(string productId, int pageNumber)
        {
            if (string.IsNullOrEmpty(productId))
            {
                return null;
            }

            var products = GetAllData();

            var product = products.FirstOrDefault(x=>x.Id.Equals(productId));

            if (product == null)
            {
                return null;
            }

            if (pageNumber < 0)
            {
                return null;
            }

            if ((pageNumber * PAGE_SIZE) > product.CommentList.Count) {
                return null;
            }

            var comments = product.CommentList;

            List<string> result = new List<string>();

            int startIndex = pageNumber * PAGE_SIZE, endIndex = (pageNumber + 1) * PAGE_SIZE;

            if (endIndex > comments.Count)
            {
                endIndex = comments.Count;
            }

            for (int i = startIndex; i < endIndex; i++)
            {
                var comment = comments[i];
                result.Add(comment.Comment);
            }

            return result;
        }

        /// <summary>
        /// Find the data record
        /// Update the fields
        /// Save to the data store
        /// </summary>
        /// <param name="data"></param>
        public ProductModel UpdateData(ProductModel data)
        {
            // Retrieve all products
            var products = GetAllData();

            // Find the product to update
            var productData = products.FirstOrDefault(x => x.Id.Equals(data.Id));
            if (productData == null)
            {
                return null;
            }

            // Update the data to the new passed in values
            productData.Name = data.Name;
            productData.Description = data.Description.Trim();
            productData.Url = data.Url;
            productData.Image = data.Image;

            productData.Price = data.Price;

            productData.CommentList = data.CommentList;

            // Save the updated product list
            SaveData(products);

            return productData;
        }

        /// <summary>
        /// Save All products data to storage
        /// </summary>
        public void SaveData(IEnumerable<ProductModel> products)
        {

            // Wait to aquire the mutex, "Wait your turn to open the file and write
            SaveDataLock.WaitOne();
            using (var outputStream = File.Create(JsonFileName))
            {
                // Serialize the product list into the JSON file
                JsonSerializer.Serialize<IEnumerable<ProductModel>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true // Format JSON for readability
                    }),
                    products
                );
                outputStream.Close(); // Close the file stream
            }

            // When done writing, release the Mutex, "My turn is over"
            SaveDataLock.ReleaseMutex();
        }

        /// <summary>
        /// Create a new product using default values
        /// After create the user can update to set values
        /// </summary>
        /// <returns></returns>
        public ProductModel CreateData()
        {
            // Create a new product with default values
            var data = new ProductModel()
            {
                Id = System.Guid.NewGuid().ToString(),
                Name = "Enter Name",
                Description = "Enter Description",
                Url = "Enter URL",
                Image = "",
            };

            // Get the current set, and append the new record to it because IEnumerable does not have Add
            var dataSet = GetAllData();
            dataSet = dataSet.Append(data);

            // Save the updated data set
            SaveData(dataSet);

            return data;
        }

        public bool CreateProduct(ProductModel data)
        {
            // Check if the product data is null or if the price is invalid
            if (data == null || data.Price < 0) return false;

            // Get the current set, and append the new record to it because IEnumerable does not have Add
            data.Id = Guid.NewGuid().ToString();
            // Retrieve the current dataset of products
            var dataSet = GetAllData();
            // Append the new product to the dataset
            dataSet = dataSet.Append(data);
            // Save the updated dataset back to the storage
            SaveData(dataSet);
            // Return true indicating successful creation
            return true;
        }

        /// <summary>
        /// Method to filter products by minimum rating
        /// </summary>
        /// <param name="minRating"></param>
        /// <returns></returns>
        public IEnumerable<ProductModel> GetProductsByRating(int minRating)
        {
            // Retrieve all products from the database
            var products = GetAllData();
            // Filter products with non-null ratings and an average rating greater than or equal to the specified minimum
            return products.Where(p => p.Ratings != null && p.Ratings.Any() && p.Ratings.Average() >= minRating);
        }

        /// <summary>
        /// Retrieves products that meet or exceed a specified minimum rating
        /// </summary>
        /// <param name="minRating">The minimum average rating to filter products</param>
        /// <returns>A collection of products matching the criteria</returns>
        public IEnumerable<ProductModel> SearchProducts(string searchTerm)
        {
            // Retrieve all products from the database
            var products = GetAllData();

            // If the search term is empty or null, return all products
            if (string.IsNullOrEmpty(searchTerm))
            {
                return products;
            }
            // Filter products where the name or description contains the search term (case insensitive)
            return products.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                       p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Searches for products matching a given search term in their name or description
        /// </summary>
        /// <param name="searchTerm">The search term to match</param>
        /// <returns>A collection of matching products</returns>
        public IEnumerable<ProductModel> FilterProductsByRating(IEnumerable<ProductModel> products, int minRating)
        {
            // If the minimum rating is zero or less, return the original collection
            if (minRating <= 0)
            {
                return products;
            }
            // Filter products with non-null ratings and an average rating greater than or equal to the specified minimum
            return products.Where(p => p.Ratings != null && p.Ratings.Any() && p.Ratings.Average() >= minRating);
        }

        /// <summary>
        /// Remove the item from the system
        /// </summary>
        /// <returns></returns>
        public bool DeleteData(string id)
        {
            // Get the current data set
            var dataSet = GetAllData();
            // Find the product with the matching ID
            var data = dataSet.FirstOrDefault(m => m.Id.Equals(id));

            // If data is null (ID not found), return false
            if (data == null)
            {
                return false;
            }

            // Filter out the item to delete
            var newDataSet = dataSet.Where(m => !m.Id.Equals(id));

            // Save the new data set without the deleted item
            SaveData(newDataSet);

            // Return true indicating successful deletion
            return true;
        }


    }
}