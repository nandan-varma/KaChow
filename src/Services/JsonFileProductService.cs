using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

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
            WebHostEnvironment = webHostEnvironment;
            SaveDataLock = new Mutex();
        }

        // Web environment field
        public IWebHostEnvironment WebHostEnvironment { get; }

        private Mutex SaveDataLock;

        // FileName Field and Getter
        private string JsonFileName
        {
            get { return Path.Combine(WebHostEnvironment.WebRootPath, "data", "plushies.json"); }
        }

        /// <summary>
        /// Method which retrieves all the data in the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProductModel> GetAllData()
        {
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                return JsonSerializer.Deserialize<ProductModel[]>(jsonFileReader.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
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
            var products = GetAllData();
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
        public bool AddRating(string productId, int rating)
        {
            // If the ProductID is invalid, return
            if (string.IsNullOrEmpty(productId))
            {
                return false;
            }

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

        /// <summary>
        /// Find the data record
        /// Update the fields
        /// Save to the data store
        /// </summary>
        /// <param name="data"></param>
        public ProductModel UpdateData(ProductModel data)
        {
            var products = GetAllData();
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
                JsonSerializer.Serialize<IEnumerable<ProductModel>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        SkipValidation = true,
                        Indented = true
                    }),
                    products
                );
                outputStream.Close();
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

            SaveData(dataSet);

            return data;
        }

        public bool CreateProduct(ProductModel data)
        {
            if (data == null || data.Price < 0) return false;
            // Get the current set, and append the new record to it because IEnumerable does not have Add
            data.Id = Guid.NewGuid().ToString();
            var dataSet = GetAllData();
            dataSet = dataSet.Append(data);

            SaveData(dataSet);

            return true;
        }


        /// <summary>
        /// Remove the item from the system
        /// </summary>
        /// <returns></returns>
        public bool DeleteData(string id)
        {
            // Get the current data set
            var dataSet = GetAllData();
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