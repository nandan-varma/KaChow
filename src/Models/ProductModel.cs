using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// A class which models a product
    /// </summary>
    public class ProductModel
    {   
        // Id Field
        public string Id { get; set; }

        // Image URL Field
        [JsonPropertyName("img")]
        [RegularExpression(@"^(https?:\/\/).*(\.(jpeg|png|webp|JPEG|PNG|WEBP))", 
            ErrorMessage = "Image URL must be a Jpeg, PNG, or WebP file and must start with http:// or https://")]
        public string Image { get; set; }

        // URL Field
        [RegularExpression(@"^(https?:\/\/).*(\/.*)?$", ErrorMessage = "URL must start with http:// or https://")]
        public string Url { get; set; }

        // Name Field
        [StringLength(maximumLength: 33, MinimumLength = 1, ErrorMessage = "The Name should have a length of more than {2} and less than {1}")]
        public string Name { get; set; }

        // Model Name Field
        [Required(ErrorMessage = "Model is required")]
        public string Model { get; set; }

        // Description Field
        [StringLength(maximumLength: 200, ErrorMessage = "Description cannot exceed {1} characters")]
        public string Description { get; set; }

        // Ratings Field
        public int[] Ratings { get; set; }

        // Price Field
        [Range(0, 100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Price { get; set; }

        // Stock Field
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative number")]
        public int Stock { get; set; }

        // Material Field
        public ProductTypeEnum Material { get; set; }

        // Size Field
        public ProductSizeEnum Size { get; set; }
        // Store the Comments entered by the users on this product
        public List<CommentModel> CommentList { get; set; }

        // ToString Override method which converts the object into JSON
        public override string ToString() => JsonSerializer.Serialize<ProductModel>(this);


    }
}