using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.Json;

public class RatingService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RatingService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool HasUserRated(string productId)
    {
        var ratings = GetRatingsFromCookie();
        return ratings.ContainsKey(productId);
    }

    public virtual void AddRating(string productId, int stars)
    {
        var ratings = GetRatingsFromCookie();

        if (!ratings.ContainsKey(productId))
        {
            ratings[productId] = stars;
            SaveRatingsToCookie(ratings);
        }
    }

    public Dictionary<string, int> GetRatingsFromCookie()
    {
        var cookieValue = _httpContextAccessor.HttpContext.Request.Cookies["UserRatings"];
        if (string.IsNullOrEmpty(cookieValue))
            return new Dictionary<string, int>();

        try
        {
            return JsonSerializer.Deserialize<Dictionary<string, int>>(cookieValue);
        }
        catch (JsonException)
        {
            return new Dictionary<string, int>();
        }
    }

    public string SerializeRatings(Dictionary<string, int> ratings)
    {
        return JsonSerializer.Serialize(ratings);
    }

    public void SaveRatingsToCookie(Dictionary<string, int> ratings)
    {
        var cookieValue = SerializeRatings(ratings);
        _httpContextAccessor.HttpContext.Items["UserRatings"] = cookieValue;
    }
}