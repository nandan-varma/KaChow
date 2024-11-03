using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using System.Linq;

namespace ContosoCrafts.WebSite.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        public ProductsController(JsonFileProductService productService)
        {
            ProductService = productService;
        }

        public JsonFileProductService ProductService { get; }

        [HttpGet]
        public IEnumerable<ProductModel> Get()
        {
            return ProductService.GetAllData();
        }

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


        public class RatingRequest
        {
            public string ProductId { get; set; }
            public int Rating { get; set; }
        }
    }
}