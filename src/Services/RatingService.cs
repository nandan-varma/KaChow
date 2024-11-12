using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.Json;

/// <summary>
/// A Class which handles all rating interactions with
/// the cookies in the browser
/// </summary>
/// 

// Namespace for ContosoCrafts Web Services
namespace ContosoCrafts.WebSite.Services
{
    public class RatingService
    {

        // read only HTTP Context Accessor
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Constructor which sets the accessor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public RatingService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// A Method which determines if a user has rated this product before
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public bool HasUserRated(string productId)
        {
            var ratings = GetRatingsFromCookie();
            return ratings.ContainsKey(productId);
        }

        /// <summary>
        /// A method which adds the rating to the cookies if valid
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="stars"></param>
        public virtual void AddRating(string productId, int stars)
        {

            // Get the current ratings
            var ratings = GetRatingsFromCookie();

            // Check if the user has rated before
            if (!ratings.ContainsKey(productId))
            {
                // If they havent rate, then save the ratings to cookies
                ratings[productId] = stars;
                SaveRatingsToCookie(ratings);
            }
        }

        /// <summary>
        /// A method which reads ratings from the cookies
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> GetRatingsFromCookie()
        {

            // Get the cookie from the accessor
            var cookieValue = _httpContextAccessor.HttpContext.Request.Cookies["UserRatings"];

            // if the cookie is null or empty, return a new dictionary
            if (string.IsNullOrEmpty(cookieValue))
                return new Dictionary<string, int>();


            try
            {
                // Try to serialize the dictionary, if success, return it
                return JsonSerializer.Deserialize<Dictionary<string, int>>(cookieValue);
            }
            catch (JsonException)
            {
                // If the serialize was unsuccessful, return a new dictionary
                return new Dictionary<string, int>();
            }
        }

        /// <summary>
        /// A method which serializes the rating dictionary
        /// </summary>
        /// <param name="ratings"></param>
        /// <returns></returns>
        public string SerializeRatings(Dictionary<string, int> ratings)
        {
            return JsonSerializer.Serialize(ratings);
        }

        /// <summary>
        /// A method which saves the ratings to the cookies
        /// </summary>
        /// <param name="ratings"></param>
        public void SaveRatingsToCookie(Dictionary<string, int> ratings)
        {
            // Serialize the dictionary
            var cookieValue = SerializeRatings(ratings);
            // Write the cookie to the browser
            _httpContextAccessor.HttpContext.Items["UserRatings"] = cookieValue;
        }
    }
}