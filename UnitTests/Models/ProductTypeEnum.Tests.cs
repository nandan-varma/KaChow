using ContosoCrafts.WebSite.Models;
using NUnit.Framework;

// Namespace for Model UnitTests
namespace UnitTests.Models
{
    public class ProductTypeEnumTest
    {
        #region ProductTypeEnum_DisplayName

        /// <summary>
        /// Tests that the display name for Cotton is returned correctly.
        /// </summary>
        [Test]
        public void DisplayName_ProductTypeEnum_Cotton_Should_Return_Cotton()
        {
            // Arrange
            var productType = ProductTypeEnum.Cotton;

            // Act
            var displayName = productType.DisplayName();

            // Assert
            Assert.That(displayName, Is.EqualTo("Cotton"));
        }

        /// <summary>
        /// Tests that the display name for Wool is returned correctly.
        /// </summary>
        [Test]
        public void DisplayName_ProductTypeEnum_Wool_Should_Return_Wool()
        {
            // Arrange
            var productType = ProductTypeEnum.Wool;

            // Act
            var displayName = productType.DisplayName();

            // Assert
            Assert.That(displayName, Is.EqualTo("Wool"));
        }

        /// <summary>
        /// Tests that the display name for Nylon is returned correctly.
        /// </summary>
        [Test]
        public void DisplayName_ProductTypeEnum_Nylon_Should_Return_Nylon()
        {
            // Arrange
            var productType = ProductTypeEnum.Nylon;

            // Act
            var displayName = productType.DisplayName();

            // Assert
            Assert.That(displayName, Is.EqualTo("Nylon"));
        }

        /// <summary>
        /// Tests that the display name for Plastic is returned correctly.
        /// </summary>
        [Test]
        public void DisplayName_ProductTypeEnum_Plastic_Should_Return_Plastic()
        {
            // Arrange
            var productType = ProductTypeEnum.Plastic;

            // Act
            var displayName = productType.DisplayName();

            // Assert
            Assert.That(displayName, Is.EqualTo("Plastic"));
        }

        /// <summary>
        /// Tests that the display name for an unknown product type is returned as "Unknown".
        /// </summary>
        [Test]
        public void DisplayName_ProductTypeEnum_Unknown_Should_Return_Unknown()
        {
            // Arrange
            var productType = (ProductTypeEnum)999;

            // Act
            var displayName = productType.DisplayName();

            // Assert
            Assert.That(displayName, Is.EqualTo("Unknown"));
        }

        #endregion ProductTypeEnum_DisplayName
        #region ProductSizeEnum_DisplayName

        /// <summary>
        /// Tests that the display name for Extra Small size is returned correctly.
        /// </summary>
        [Test]
        public void DisplayName_ProductSizeEnum_XS_Should_Return_ExtraSmall()
        {
            // Arrange
            var size = ProductSizeEnum.XS;

            // Act
            var displayName = size.DisplayName();

            // Assert
            Assert.That(displayName, Is.EqualTo("Extra Small"));
        }

        /// <summary>
        /// Tests that the display name for Small size is returned correctly.
        /// </summary>
        [Test]
        public void DisplayName_ProductSizeEnum_S_Should_Return_Small()
        {
            // Arrange
            var size = ProductSizeEnum.S;

            // Act
            var displayName = size.DisplayName();

            // Assert
            Assert.That(displayName, Is.EqualTo("Small"));
        }

        /// <summary>
        /// Tests that the display name for Medium size is returned correctly.
        /// </summary>
        [Test]
        public void DisplayName_ProductSizeEnum_M_Should_Return_Medium()
        {
            // Arrange
            var size = ProductSizeEnum.M;

            // Act
            var displayName = size.DisplayName();

            // Assert
            Assert.That(displayName, Is.EqualTo("Medium"));
        }

        /// <summary>
        /// Tests that the display name for Large size is returned correctly.
        /// </summary>
        [Test]
        public void DisplayName_ProductSizeEnum_L_Should_Return_Large()
        {
            // Arrange
            var size = ProductSizeEnum.L;

            // Act
            var displayName = size.DisplayName();

            // Assert
            Assert.That(displayName, Is.EqualTo("Large"));
        }

        /// <summary>
        /// Tests that the display name for Extra Large size is returned correctly.
        /// </summary>
        [Test]
        public void DisplayName_ProductSizeEnum_XL_Should_Return_ExtraLarge()
        {
            // Arrange
            var size = ProductSizeEnum.XL;

            // Act
            var displayName = size.DisplayName();

            // Assert
            Assert.That(displayName, Is.EqualTo("Extra Large"));
        }

        /// <summary>
        /// Tests that the display name for an unknown size is returned as "Unknown".
        /// </summary>
        [Test]
        public void DisplayName_ProductSizeEnum_Unknown_Should_Return_Unknown()
        {
            // Arrange
            var size = (ProductSizeEnum)999;

            // Act
            var displayName = size.DisplayName();

            // Assert
            Assert.That(displayName, Is.EqualTo("Unknown"));
        }

        #endregion ProductSizeEnum_DisplayName
    }
}