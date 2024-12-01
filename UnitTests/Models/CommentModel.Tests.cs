using ContosoCrafts.WebSite.Models;
using NUnit.Framework;

// Namespace for Model UnitTests
namespace UnitTests.Models
{
    public class CommentModelTest
    {
        #region Constructor_Id_Should_Initialize_With_Unique_Guid
        [Test]
        /// <summary>
        /// Tests that the constructor initializes Id with a unique Guid.
        /// </summary>
        public void Constructor_Id_Should_Initialize_With_Unique_Guid()
        {
            // Arrange & Act
            var result1 = new CommentModel();
            var result2 = new CommentModel();

            // Reset

            // Assert
            Assert.That(result1.Id, Is.Not.Null);
            Assert.That(result2.Id, Is.Not.Null);
            Assert.That(result1.Id, Is.Not.EqualTo(result2.Id));
        }
        #endregion Constructor_Id_Should_Initialize_With_Unique_Guid

        #region Comment_Property_Set_Get_Value_Should_Return_ExpectedValue
        [Test]
        /// <summary>
        /// Tests that the Comment property can be set and retrieved with the expected value.
        /// </summary>
        public void Comment_Property_Set_Get_Value_Should_Return_ExpectedValue()
        {
            // Arrange
            var result = new CommentModel();
            var expectedComment = "This is a test comment.";

            // Act
            result.Comment = expectedComment;
            var actualComment = result.Comment;

            // Reset

            // Assert
            Assert.That(actualComment, Is.EqualTo(expectedComment));
        }
        #endregion Comment_Property_Set_Get_Value_Should_Return_ExpectedValue

        #region Id_Property_Set_Get_Value_Should_Return_ExpectedValue
        [Test]
        /// <summary>
        /// Tests that the Id property can be set and retrieved with the expected value.
        /// </summary>
        public void Id_Property_Set_Get_Value_Should_Return_ExpectedValue()
        {
            // Arrange
            var result = new CommentModel();
            var expectedId = "test-id";

            // Act
            result.Id = expectedId;
            var actualId = result.Id;

            // Reset

            // Assert
            Assert.That(actualId, Is.EqualTo(expectedId));
        }
        #endregion Id_Property_Set_Get_Value_Should_Return_ExpectedValue
    }
}
