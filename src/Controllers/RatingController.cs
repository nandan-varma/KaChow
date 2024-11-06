using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;

// Namespace for ContosoCrafts Website Controllers
namespace ContosoCrafts.WebSite.Controllers
{
    /// <summary>
    /// A controller that handles all product rating interactions
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        // Read only assets
        private readonly RatingService _ratingService;
        private readonly JsonFileProductService _productService;

        public RatingController(RatingService ratingService, JsonFileProductService productService)
        {
            _ratingService = ratingService;
            _productService = productService;
        }


        /// <summary>
        /// An HTTP Post endpoint that adds a rating to a product
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="rating"></param>
        /// <returns>Result of POST request</returns>
        [HttpPost("add")]
        public IActionResult AddRating(string productId, int rating)
        {
            // If the rating is invalid return BadRequest
            if (rating < 1 || rating > 5)
            {
                return BadRequest("Invalid rating value.");
            }

            // If the productId is empty or null return BadRequest
            if (string.IsNullOrEmpty(productId))
            {
                return BadRequest("Product ID cannot be null or empty.");
            }

            var product = _productService.GetProductById(productId);

            // If product is not in the database return 404 NotFound
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            _ratingService.AddRating(productId, rating);

            return Ok();
        }
    }
}