using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    internal class TestBUnitHelperTests : BunitTestContext
    {
        /// <summary>
        /// Tests that the TestContext is set up and not null.
        /// </summary>
        [Test]
        public void Setup_Sets_TestContext_Should_Not_Be_Null()
        {
            // Arrange
            Setup();

            // Act
            var context = TestContext;

            // Assert
            Assert.That(context, Is.Not.Null);
        }
    }
}
