using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

// Namespace for ContosoCrafts Web Pages
namespace ContosoCrafts.WebSite.Pages
{

    /// <summary>
    /// A Class which describes the Index Page's Model
    /// </summary>
    public class IndexModel : PageModel
    {

        // Read only logger
        private readonly ILogger<IndexModel> _logger;

        /// <summary>
        /// Constructor which sets the read only logger and 
        /// product service
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="productService"></param>
        public IndexModel(ILogger<IndexModel> logger,
            JsonFileProductService productService)
        {
            _logger = logger;
            ProductService = productService;
        }

        // ProductService Field
        public JsonFileProductService ProductService { get; }
        
        // Iterable of Products
        public IEnumerable<ProductModel> Products { get; private set; }

        /// <summary>
        /// HTTP GET Method which gets all the products in the database
        /// </summary>
        public void OnGet()
        {
            Products = ProductService.GetAllData();
        }
    }
}