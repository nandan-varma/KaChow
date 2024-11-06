using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

// Namespace for ContosoCrafts Product Pages
namespace ContosoCrafts.Pages.Product
{
    /// <summary>
    /// A class which describes the Read Pages Model
    /// </summary>
    public class DetailsModel : PageModel
    {
        // Read only value, Product Service
        private readonly JsonFileProductService _productService;

        /// <summary>
        /// Constructor which sets the product service
        /// </summary>
        /// <param name="productService"></param>
        public DetailsModel(JsonFileProductService productService)
        {
            _productService = productService;
        }

        // Currently selected product
        public ProductModel Product { get; set; }

        /// <summary>
        /// HTTP GET Method, a successful call will return
        /// the page, an unsuccessful call will return 404 NotFound
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult OnGet(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            Product = _productService.GetProductById(id);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
