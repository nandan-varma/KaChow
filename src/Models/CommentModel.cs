
using System.ComponentModel.DataAnnotations;

namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// Comments entered by the user about the Product
    /// </summary>
    public class CommentModel
    {
        // The ID for this comment, use a Guid so it is always unique
        public string Id { get; set; } = System.Guid.NewGuid().ToString();

        [Required(ErrorMessage = "Comment is required")]
        [StringLength(250, MinimumLength = 1)]
        // The Comment
        public string Comment { get; set; }
    }
}