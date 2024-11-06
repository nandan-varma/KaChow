using ContosoCrafts.WebSite.Models;
using NUnit.Framework;

// Namespace for Model UnitTests
namespace UnitTests.Models
{
    public class CommentModelTests
    {
        [Test]
        /// <summary>
        /// Tests that the constructor initializes Id with a unique Guid.
        /// </summary>
        public void Constructor_Should_Initialize_Id_With_Unique_Guid()
        {
            // Arrange & Act
            var Comment1 = new CommentModel();
            var Comment2 = new CommentModel();

            // Assert
            Assert.That(Comment1.Id, Is.Not.Null);
            Assert.That(Comment2.Id, Is.Not.Null);
            Assert.That(Comment1.Id, Is.Not.EqualTo(Comment2.Id));
        }

        [Test]
        /// <summary>
        /// Tests that the Comment property can be set and retrieved correctly.
        /// </summary>
        public void Comment_Property_Should_Set_And_Get_Value_Correctly()
        {
            // Arrange
            var Comment = new CommentModel();
            var ExpectedComment = "This is a test comment.";

            // Act
            Comment.Comment = ExpectedComment;
            var ActualComment = Comment.Comment;

            // Assert
            Assert.That(ActualComment, Is.EqualTo(ExpectedComment));
        }

        [Test]
        /// <summary>
        /// Tests that the Id property can be set and retrieved correctly.
        /// </summary>
        public void Id_Property_Should_Set_And_Get_Value_Correctly()
        {
            // Arrange
            var Comment = new CommentModel();
            var ExpectedId = "test-id";

            // Act
            Comment.Id = ExpectedId;
            var ActualId = Comment.Id;

            // Assert
            Assert.That(ActualId, Is.EqualTo(ExpectedId));
        }
    }
}
