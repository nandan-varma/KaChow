// Namespace for ContosoCrafts Website Models
namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// An enumeration class describing the 
    /// product codes
    /// </summary>
    public enum ProductTypeEnum
    {
        Undefined = 0,
        Amature = 1,
        Antique = 5,
        Collectable = 130,
        Commercial = 55,
    }

    /// <summary>
    /// A class which helps the program process ProductTypeEnums
    /// </summary>
    public static class ProductTypeEnumExtensions
    {
        /// <summary>
        /// Returns the display name for the product type input
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string DisplayName(this ProductTypeEnum data)
        {
            return data switch
            {
                ProductTypeEnum.Amature => "Hand Made Items",
                ProductTypeEnum.Antique => "Antiques",
                ProductTypeEnum.Collectable => "Collectables",
                ProductTypeEnum.Commercial => "Commercial goods",
 
                // Default, Unknown
                _ => "",
            };
        }
    }
}