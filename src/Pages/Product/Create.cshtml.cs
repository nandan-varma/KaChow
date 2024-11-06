using ContosoCrafts.WebSite.Models; // Assuming you have a Product model
using ContosoCrafts.WebSite.Services; // Assuming you have a ProductService
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

// Namespace for the ConstosoCrafts Website Product Pages
namespace ContosoCrafts.WebSite.Pages.Product
{

    /// <summary>
    /// A class which describes the Create Page Model
    /// </summary>
    public class CreateModel : PageModel
    {

        // Read only product service resource
        private readonly JsonFileProductService _productService;

        /// <summary>
        /// Constructor which sets the product service
        /// </summary>
        /// <param name="productService"></param>
        public CreateModel(JsonFileProductService productService)
        {
            _productService = productService;
        }

        // Binded property, currently selected product
        [BindProperty]
        public ProductModel Product { get; set; }

        /// <summary>
        /// HTTP GET Method, returns the page
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGet()
        {
            return Page();
        }

        /// <summary>
        /// HTTP POST Method, creates the product in the 
        /// product service
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _productService.CreateProduct(Product);
            return RedirectToPage("./Index");
        }
    }
}
