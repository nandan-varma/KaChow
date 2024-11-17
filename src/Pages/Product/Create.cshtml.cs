using ContosoCrafts.WebSite.Models; // Importing the namespace containing the Product model definition
using ContosoCrafts.WebSite.Services; // Importing the namespace containing the ProductService
using Microsoft.AspNetCore.Mvc; // Importing ASP.NET Core MVC namespace for controller functionality
using Microsoft.AspNetCore.Mvc.RazorPages; // Importing Razor Pages functionality

// Namespace for the ContosoCrafts Website Product Pages
namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// A class which describes the Create Page Model for creating a product
    /// </summary>
    public class CreateModel : PageModel
    {
        // Read-only product service resource used for interacting with the product data
        private readonly JsonFileProductService _productService;

        /// <summary>
        /// Constructor which initializes the product service
        /// </summary>
        /// <param name="productService">An instance of JsonFileProductService injected via dependency injection</param>
        public CreateModel(JsonFileProductService productService)
        {
            _productService = productService; // Assigning the injected product service to the private field
        }

        // Binded property representing the currently selected product in the form
        [BindProperty]
        public ProductModel Product { get; set; } // The product object bound to the Razor Page form

        /// <summary>
        /// HTTP GET Method that renders the page
        /// </summary>
        /// <returns>Returns the Page result to render the Create product page</returns>
        public IActionResult OnGet()
        {
            return Page(); // Render and return the current Razor Page
        }

        /// <summary>
        /// HTTP POST Method that creates a product using the product service
        /// </summary>
        /// <returns>Redirects to the Index page on success, or redisplays the form on failure</returns>
        public IActionResult OnPost()
        {
            // Check if the ModelState is invalid, indicating form validation errors
            if (!ModelState.IsValid)
            {
                return Page(); // Redisplay the form with validation errors
            }

            // Use the product service to create a new product with the provided data
            _productService.CreateProduct(Product);

            // Redirect the user to the Index page after successful creation
            return RedirectToPage("./Index");
        }
    }
}
