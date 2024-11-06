using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

// Namespace for ContosoCrafts Website Controllers
namespace ContosoCrafts.WebSite.Controllers
{
    /// <summary>
    /// A controller class which governs the actions peformed on the Product Database
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        /// <summary>
        /// Constructor for the Products Controller class. Takes in a 
        /// JsonFileProductService which the controller will utilize as
        /// its database connection.
        /// </summary>
        /// <param name="productService"></param>
        public ProductsController(JsonFileProductService productService)
        {
            ProductService = productService;
        }

        /// <summary>
        /// Getter for the "database" connection
        /// </summary>
        public JsonFileProductService ProductService { get; }

        /// <summary>
        /// An HTTP GET endpoint which retrieves all the Products in the database
        /// </summary>
        /// <returns>List of all Products in database</returns>
        [HttpGet]
        public IEnumerable<ProductModel> Get()
        {
            return ProductService.GetAllData();
        }

        /// <summary>
        /// An HTTP PATCH endpoint which changes the ratings of a product in the database
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Result of the Patch</returns>
        [HttpPatch]
        public ActionResult Patch([FromBody] RatingRequest request)
        {
            // Check if the product exists
            var product = ProductService.GetAllData().FirstOrDefault(p => p.Id == request.ProductId);
            if (product == null || request.Rating < 1 || request.Rating > 5)
            {
                return BadRequest(); // Return bad request if product does not exist or rating is out of bounds
            }

            ProductService.AddRating(request.ProductId, request.Rating);

            return Ok();
        }

        /// <summary>
        /// A subclass of the product controller which contains information 
        /// about product ratings
        /// </summary>
        public class RatingRequest
        {
            public string ProductId { get; set; }
            public int Rating { get; set; }
        }
    }
}