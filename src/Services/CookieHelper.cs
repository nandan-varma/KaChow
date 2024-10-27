using System;
using Microsoft.AspNetCore.Http;

public class CookieHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieHelper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetCookie(string key)
    {
        return _httpContextAccessor.HttpContext.Request.Cookies[key];
    }

    public void SetCookie(string key, string value, int expirationMinutes)
    {
        var cookieOptions = new CookieOptions
        {
            Expires = DateTime.Now.AddMinutes(expirationMinutes)
        };
        _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, cookieOptions);
    }
}