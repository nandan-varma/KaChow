using Microsoft.AspNetCore.Mvc.RazorPages; // Importing the Razor Pages framework for building web pages
using Microsoft.Extensions.Logging; // Importing the logging framework

namespace ContosoCrafts.WebSite.Pages
{
    // Defines the AboutModel class, which represents the data and behavior for the About page
    public class AboutModel : PageModel
    {
        // Private field for logging within the About page
        private readonly ILogger<AboutModel> _logger;

        // Constructor that initializes the AboutModel with a logger instance
        public AboutModel(ILogger<AboutModel> logger)
        {
            _logger = logger;
        }

        // Method called on HTTP GET requests to the About page
        public void OnGet()
        {
        }
    }
}
