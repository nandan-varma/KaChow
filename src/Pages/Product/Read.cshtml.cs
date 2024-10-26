using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.Pages.Product
{
    public class DetailsModel : PageModel
    {
        private readonly JsonFileProductService _productService;

        public DetailsModel(JsonFileProductService productService)
        {
            _productService = productService;
        }

        public ProductModel Product { get; set; }

        public IActionResult OnGet(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            Product = _productService.GetProductById(id.Trim());

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
