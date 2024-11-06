using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models; // Assuming you have a Product model
using ContosoCrafts.WebSite.Services; // Assuming you have a ProductService
namespace ContosoCrafts.WebSite.Pages.Product
{
    public class CreateModel : PageModel
    {
        private readonly JsonFileProductService _productService;
        public CreateModel(JsonFileProductService productService)
        {
            _productService = productService;
        }
        [BindProperty]
        public ProductModel Product { get; set; }



        public IActionResult OnGet()
        {

            return Page();
        }

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
