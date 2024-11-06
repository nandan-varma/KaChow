using ContosoCrafts.WebSite.Models;
using NUnit.Framework;

namespace UnitTests.Models
{
    public class ProductTypeEnumTests
    {
        [Test]
        /// <summary>
        /// Tests that the DisplayName for Amature returns "Hand Made Items".
        /// </summary>
        public void DisplayName_Amature_Should_Return_HandMadeItems()
        {
            // Arrange
            var ProductType = ProductTypeEnum.Amature;

            // Act
            var DisplayName = ProductType.DisplayName();

            // Assert
            Assert.That(DisplayName, Is.EqualTo("Hand Made Items"));
        }

        [Test]
        /// <summary>
        /// Tests that the DisplayName for Antique returns "Antiques".
        /// </summary>
        public void DisplayName_Antique_Should_Return_Antiques()
        {
            // Arrange
            var ProductType = ProductTypeEnum.Antique;

            // Act
            var DisplayName = ProductType.DisplayName();

            // Assert
            Assert.That(DisplayName, Is.EqualTo("Antiques"));
        }

        [Test]
        /// <summary>
        /// Tests that the DisplayName for Collectable returns "Collectables".
        /// </summary>
        public void DisplayName_Collectable_Should_Return_Collectables()
        {
            // Arrange
            var ProductType = ProductTypeEnum.Collectable;

            // Act
            var DisplayName = ProductType.DisplayName();

            // Assert
            Assert.That(DisplayName, Is.EqualTo("Collectables"));
        }

        [Test]
        /// <summary>
        /// Tests that the DisplayName for Commercial returns "Commercial goods".
        /// </summary>
        public void DisplayName_Commercial_Should_Return_CommercialGoods()
        {
            // Arrange
            var ProductType = ProductTypeEnum.Commercial;

            // Act
            var DisplayName = ProductType.DisplayName();

            // Assert
            Assert.That(DisplayName, Is.EqualTo("Commercial goods"));
        }

        [Test]
        /// <summary>
        /// Tests that the DisplayName for Undefined returns an empty string.
        /// </summary>
        public void DisplayName_Undefined_Should_Return_EmptyString()
        {
            // Arrange
            var ProductType = ProductTypeEnum.Undefined;

            // Act
            var DisplayName = ProductType.DisplayName();

            // Assert
            Assert.That(DisplayName, Is.EqualTo(""));
        }

        [Test]
        /// <summary>
        /// Tests that the DisplayName for an unknown enum value returns an empty string.
        /// </summary>
        public void DisplayName_UnknownEnumValue_Should_Return_EmptyString()
        {
            // Arrange
            var ProductType = (ProductTypeEnum)999;

            // Act
            var DisplayName = ProductType.DisplayName();

            // Assert
            Assert.That(DisplayName, Is.EqualTo(""));
        }
    }
}
