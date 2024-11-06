using ContosoCrafts.WebSite.Models; // Assuming you have a Product model
using ContosoCrafts.WebSite.Services; // Assuming you have a ProductService
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class UpdateModel : PageModel
    {
        private readonly JsonFileProductService _productService;

        public UpdateModel(JsonFileProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public ProductModel Product { get; set; }

        public void OnGet(string id)
        {
            Product = _productService.GetProductById(id.ToString());
        }

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
