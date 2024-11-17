using ContosoCrafts.WebSite.Models; // Importing the namespace containing the Product model definition
using ContosoCrafts.WebSite.Services; // Importing the namespace containing the ProductService
using Microsoft.AspNetCore.Mvc; // Importing ASP.NET Core MVC namespace for controller functionality
using Microsoft.AspNetCore.Mvc.RazorPages; // Importing Razor Pages functionality for managing web pages

// Namespace for ContosoCrafts Website Product Pages
namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// A class which describes the Update Page Model for updating a product
    /// </summary>
    public class UpdateModel : PageModel
    {
        // Read-only field to store the product service, used for interacting with the product data
        private readonly JsonFileProductService _productService;

        /// <summary>
        /// Constructor that initializes the product service
        /// </summary>
        /// <param name="productService">An instance of JsonFileProductService, injected via dependency injection</param>
        public UpdateModel(JsonFileProductService productService)
        {
            _productService = productService; // Assign the injected product service to the private field
        }

        // Bound property to hold the currently selected product, used for data binding with the Razor Page form
        [BindProperty]
        public ProductModel Product { get; set; } // The product being updated

        /// <summary>
        /// HTTP GET Method to fetch and display the product identified by the given ID
        /// </summary>
        /// <param name="id">The ID of the product to fetch</param>
        public void OnGet(string id)
        {
            // Retrieve the product from the service by its ID and assign it to the Product property
            Product = _productService.GetProductById(id.ToString());
        }

        /// <summary>
        /// HTTP POST Method to update the product and redirect the user to the index page
        /// </summary>
        /// <returns>Redirects to the Index page on success or redisplays the form on validation failure</returns>
        public IActionResult OnPost()
        {
            // Check if the ModelState is invalid, indicating form validation errors
            if (!ModelState.IsValid)
            {
                return Page(); // Redisplay the form with validation errors
            }

            // Update the product data using the product service
            _productService.UpdateData(Product);

            // Redirect the user to the Index page after successful update
            return RedirectToPage("./Index");
        }
    }
}
