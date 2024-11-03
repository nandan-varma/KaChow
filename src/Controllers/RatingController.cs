using Microsoft.AspNetCore.Mvc;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace ContosoCrafts.WebSite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly RatingService _ratingService;
        private readonly JsonFileProductService _productService;

        public RatingController(RatingService ratingService, JsonFileProductService productService)
        {
            _ratingService = ratingService;
            _productService = productService;
        }

        [HttpPost("add")]
        public IActionResult AddRating(string productId, int rating)
        {
            if (rating < 1 || rating > 5)
            {
                return BadRequest("Invalid rating value.");
            }

            if (string.IsNullOrEmpty(productId))
            {
                return BadRequest("Product ID cannot be null or empty.");
            }

            var product = _productService.GetProductById(productId);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            _ratingService.AddRating(productId, rating);

            return Ok();
        }
    }
}