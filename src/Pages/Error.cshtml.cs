using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

// Namespace for ContosoCrafts Web Pages
namespace ContosoCrafts.WebSite.Pages
{

    /// <summary>
    /// A class which describes the Error Pages Model
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        // RequestId Field
        public string RequestId { get; set; }

        // A lambda function which determines if we are able to show the request Id
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        // Read only logger
        private readonly ILogger<ErrorModel> _logger;

        /// <summary>
        /// Constructor which sets the logger
        /// </summary>
        /// <param name="logger"></param>
        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// HTTP GET Method, sets the request id
        /// </summary>
        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}