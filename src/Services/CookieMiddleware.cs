using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

public class CookieMiddleware
{
    private readonly RequestDelegate _next;

    public CookieMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.OnStarting(() =>
        {
            // Set cookies here
            if (context.Items.ContainsKey("UserRatings"))
            {
                var cookieValue = context.Items["UserRatings"] as string;
                var options = new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(30),
                    HttpOnly = true
                };
                context.Response.Cookies.Append("UserRatings", cookieValue, options);
            }
            return Task.CompletedTask;
        });

        await _next(context);
    }
}