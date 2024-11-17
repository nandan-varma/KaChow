using ContosoCrafts.WebSite.Models; // Importing the namespace containing the Product model definition
using ContosoCrafts.WebSite.Services; // Importing the namespace containing the ProductService
using Microsoft.AspNetCore.Mvc; // Importing ASP.NET Core MVC for controller functionality
using Microsoft.AspNetCore.Mvc.RazorPages; // Importing Razor Pages functionality for managing web pages

// Namespace for ContosoCrafts Product Pages
namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// A class that defines the Details Page Model for viewing product details
    /// </summary>
    public class DetailsModel : PageModel
    {
        // Read-only field for the product service used to interact with product data
        private readonly JsonFileProductService _productService;

        /// <summary>
        /// Constructor that initializes the product service
        /// </summary>
        /// <param name="productService">An instance of JsonFileProductService injected via dependency injection</param>
        public DetailsModel(JsonFileProductService productService)
        {
            _productService = productService; // Assign the injected product service to the private field
        }

        // Property to store the currently selected product
        public ProductModel Product { get; set; } // Holds the product details to display on the page

        /// <summary>
        /// HTTP GET Method to retrieve a product by ID
        /// A successful call will return the page with product details; an unsuccessful call returns 404 NotFound
        /// </summary>
        /// <param name="id">The ID of the product to fetch</param>
        /// <returns>Returns a Page result if successful, or a NotFound result if the product is not found</returns>
        public IActionResult OnGet(string id)
        {
            // Check if the provided ID is null or empty
            if (string.IsNullOrEmpty(id))
            {
                return NotFound(); // Return 404 NotFound if the ID is invalid
            }

            // Use the product service to retrieve the product by its ID
            Product = _productService.GetProductById(id);

            // Check if the product was not found in the database
            if (Product == null)
            {
                return NotFound(); // Return 404 NotFound if the product does not exist
            }

            return Page(); // Return the page with the product details
        }
    }
}
