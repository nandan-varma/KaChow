using ContosoCrafts.WebSite.Models; // Assuming you have a Product model
using ContosoCrafts.WebSite.Services; // Assuming you have a ProductService
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

// Namespace for ContosoCrafts Website Product Pages
namespace ContosoCrafts.WebSite.Pages.Product
{

    /// <summary>
    /// A class which describes the Delete Pages Model
    /// </summary>
    public class DeleteModel : PageModel
    {
        // Read only value, Product Service
        private readonly JsonFileProductService _productService;

        /// <summary>
        /// A constructor which sets the product service
        /// </summary>
        /// <param name="productService"></param>
        public DeleteModel(JsonFileProductService productService)   
        {
            _productService = productService;
        }

        // Bound property, currently selected product
        [BindProperty]
        public ProductModel Product { get; set; }

        /// <summary>
        /// HTTP GET Method, selects the product identified by id
        /// </summary>
        /// <param name="id"></param>
        public void OnGet(string id)
        {
            Product = _productService.GetProductById(id.ToString());
        }

        /// <summary>
        /// HTTP POST Method, if the selected product is valid and in the
        /// database, a successful call will delete the page and return the user
        /// back to the index.
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Call DeleteData and check if the deletion was successful
            var isDeleted = _productService.DeleteData(Product.Id);

            if (!isDeleted)
            {
                // Optionally, add an error message to ModelState if deletion fails
                ModelState.AddModelError(string.Empty, "Product not found or could not be deleted.");
                return Page();
            }

            return RedirectToPage("./Index");
        }

    }
}
