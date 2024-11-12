using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

// Namespace for ContosoCrafts Web Pages
namespace ContosoCrafts.WebSite.Pages
{

    /// <summary>
    /// A Class which describes the Privacy Page's Model
    /// </summary>
    public class PrivacyModel : PageModel
    {
        // Read only logger
        private readonly ILogger<PrivacyModel> _logger;

        /// <summary>
        /// Constructor which sets the logger
        /// </summary>
        /// <param name="logger"></param>
        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Handles GET requests to display the Privacy page.
        /// This method is called when the Privacy page is accessed,
        /// preparing any required data or resources.
        /// </summary>
        public void OnGet()
        {
        }
    }
}
