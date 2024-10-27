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
        public IActionResult AddRating([FromQuery] string productId, [FromQuery] int rating)
        {
            var ratings = _ratingService.GetRatingsFromCookie();

            if (ratings.ContainsKey(productId))
            {
                return BadRequest("User has already rated this product.");
            }

            _ratingService.AddRating(productId, rating);
            _productService.AddRating(productId, rating);

            ratings[productId] = rating;
            var cookieValue = _ratingService.SerializeRatings(ratings);
            var options = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                HttpOnly = true
            };
            Response.Cookies.Append("UserRatings", cookieValue, options);

            return Ok();
        }
    }
}