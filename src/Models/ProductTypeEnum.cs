// Namespace for ContosoCrafts Website Models
namespace ContosoCrafts.WebSite.Models
{
    /// <summary>
    /// An enumeration class describing the 
    /// product codes
    /// </summary>
    public enum ProductTypeEnum
    {
        Cotton = 1,
        Wool = 2,
        Nylon = 3,
        Plastic = 4
    }

    public enum ProductSizeEnum
    {
        XS = 1,
        S = 2,
        M = 3,
        L = 4,
        XL = 5
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
                ProductTypeEnum.Cotton => "Cotton",
                ProductTypeEnum.Wool => "Wool",
                ProductTypeEnum.Nylon => "Nylon",
                ProductTypeEnum.Plastic => "Plastic",
                _ => "Unknown"
            };
        }

        public static string DisplayName(this ProductSizeEnum data)
        {
            return data switch
            {
                ProductSizeEnum.XS => "Extra Small",
                ProductSizeEnum.S => "Small",
                ProductSizeEnum.M => "Medium",
                ProductSizeEnum.L => "Large",
                ProductSizeEnum.XL => "Extra Large",
                _ => "Unknown"
            };
        }
    }
}