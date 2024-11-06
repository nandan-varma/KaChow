using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ContosoCrafts.WebSite.Models; // Assuming you have a Product model
using ContosoCrafts.WebSite.Services; // Assuming you have a ProductService

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class DeleteModel : PageModel
    {
        private readonly JsonFileProductService _productService;

        public DeleteModel(JsonFileProductService productService)   
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
