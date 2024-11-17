using ContosoCrafts.WebSite.Models; // Importing the namespace containing the Product model definition
using ContosoCrafts.WebSite.Services; // Importing the namespace containing the ProductService for handling data operations
using Microsoft.AspNetCore.Mvc.RazorPages; // Importing Razor Pages namespace for building page models
using System.Collections.Generic; // Importing namespace for working with collections like IEnumerable

// Namespace for the ContosoCrafts Website Product Page
namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// Index Page Model that returns all data to display to the user
    /// </summary>
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Constructor that initializes the ProductService
        /// </summary>
        /// <param name="productService">An instance of JsonFileProductService injected via dependency injection</param>
        public IndexModel(JsonFileProductService productService)
        {
            ProductService = productService; // Assign the injected product service to the read-only property
        }

        // Property to hold the instance of the product service used for data interactions
        public JsonFileProductService ProductService { get; }

        // Property to store the collection of product data retrieved from the service
        public IEnumerable<ProductModel> Products { get; private set; } // Private setter ensures it can only be modified internally

        /// <summary>
        /// HTTP GET method to retrieve and populate all product data
        /// </summary>
        public void OnGet()
        {
            // Fetch all product data using the ProductService and assign it to the Products property
            Products = ProductService.GetAllData();
        }
    }
}
