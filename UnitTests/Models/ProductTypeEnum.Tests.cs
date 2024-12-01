using ContosoCrafts.WebSite.Models;
using NUnit.Framework;

// Namespace for Model UnitTests
namespace UnitTests.Models
{
    public class ProductTypeEnumTest
    {
        #region DisplayName_Amature_Should_Return_HandMadeItems

        [Test]
        /// <summary>
        /// Tests that the DisplayName for Amature returns "Hand Made Items".
        /// </summary>
        public void DisplayName_Amature_Should_Return_HandMadeItems()
        {
            // Arrange
            var productType = ProductTypeEnum.Amature;

            // Act
            var displayName = productType.DisplayName();

            // Reset

            // Assert
            Assert.That(displayName, Is.EqualTo("Hand Made Items"));
        }

        #endregion DisplayName_Amature_Should_Return_HandMadeItems

        #region DisplayName_Antique_Should_Return_Antiques

        [Test]
        /// <summary>
        /// Tests that the DisplayName for Antique returns "Antiques".
        /// </summary>
        public void DisplayName_Antique_Should_Return_Antiques()
        {
            // Arrange
            var productType = ProductTypeEnum.Antique;

            // Act
            var displayName = productType.DisplayName();

            // Reset

            // Assert
            Assert.That(displayName, Is.EqualTo("Antiques"));
        }

        #endregion DisplayName_Antique_Should_Return_Antiques

        #region DisplayName_Collectable_Should_Return_Collectables

        [Test]
        /// <summary>
        /// Tests that the DisplayName for Collectable returns "Collectables".
        /// </summary>
        public void DisplayName_Collectable_Should_Return_Collectables()
        {
            // Arrange
            var productType = ProductTypeEnum.Collectable;

            // Act
            var displayName = productType.DisplayName();

            // Reset

            // Assert
            Assert.That(displayName, Is.EqualTo("Collectables"));
        }

        #endregion DisplayName_Collectable_Should_Return_Collectables

        #region DisplayName_Commercial_Should_Return_CommercialGoods

        [Test]
        /// <summary>
        /// Tests that the DisplayName for Commercial returns "Commercial goods".
        /// </summary>
        public void DisplayName_Commercial_Should_Return_CommercialGoods()
        {
            // Arrange
            var productType = ProductTypeEnum.Commercial;

            // Act
            var displayName = productType.DisplayName();

            // Reset

            // Assert
            Assert.That(displayName, Is.EqualTo("Commercial goods"));
        }

        #endregion DisplayName_Commercial_Should_Return_CommercialGoods

        #region DisplayName_Undefined_Should_Return_EmptyString

        [Test]
        /// <summary>
        /// Tests that the DisplayName for Undefined returns an empty string.
        /// </summary>
        public void DisplayName_Undefined_Should_Return_EmptyString()
        {
            // Arrange
            var productType = ProductTypeEnum.Undefined;

            // Act
            var displayName = productType.DisplayName();

            // Reset

            // Assert
            Assert.That(displayName, Is.EqualTo(""));
        }

        #endregion DisplayName_Undefined_Should_Return_EmptyString

        #region DisplayName_UnknownEnumValue_Should_Return_EmptyString

        [Test]
        /// <summary>
        /// Tests that the DisplayName for an unknown enum value returns an empty string.
        /// </summary>
        public void DisplayName_UnknownEnumValue_Should_Return_EmptyString()
        {
            // Arrange
            var productType = (ProductTypeEnum)999;

            // Act
            var displayName = productType.DisplayName();

            // Reset

            // Assert
            Assert.That(displayName, Is.EqualTo(""));
        }

        #endregion DisplayName_UnknownEnumValue_Should_Return_EmptyString
    }
}
