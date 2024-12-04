using ContosoCrafts.WebSite.Models;
using NUnit.Framework;

// Namespace for Model UnitTests
namespace UnitTests.Models
{
    public class ProductTypeEnumTest
    {
        #region DisplayName_ProductTypeEnum_Should_Return_Correct_DisplayName

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

        #endregion DisplayName_ProductTypeEnum_Should_Return_Correct_DisplayName

        #region DisplayName_ProductSizeEnum_Should_Return_Correct_DisplayName

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

        #endregion DisplayName_ProductSizeEnum_Should_Return_Correct_DisplayName
    }
}