using ContosoCrafts.WebSite.Models; // Assuming you have a Product model
using ContosoCrafts.WebSite.Services; // Assuming you have a ProductService
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

// Namespace for ContosoCrafts Website Product Pages
namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// A class which describes the Update Pages Model
    /// </summary>
    public class UpdateModel : PageModel
    {
        // Read only attribute, Product Service
        private readonly JsonFileProductService _productService;

        /// <summary>
        /// Constructor which sets the product service
        /// </summary>
        /// <param name="productService"></param>
        public UpdateModel(JsonFileProductService productService)
        {
            _productService = productService;
        }

        // Bound property, currently selected product
        [BindProperty]
        public ProductModel Product { get; set; }

        /// <summary>
        /// HTTP GET Method, Selects the product identified
        /// by the Id, id
        /// </summary>
        /// <param name="id"></param>
        public void OnGet(string id)
        {
            Product = _productService.GetProductById(id.ToString());
        }

        /// <summary>
        /// HTTP POST Method, Updates the product and redirects the user
        /// to index.
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _productService.UpdateData(Product);
            return RedirectToPage("./Index");
        }
    }
}
