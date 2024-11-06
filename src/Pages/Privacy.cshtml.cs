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

        public void OnGet()
        {
        }
    }
}
