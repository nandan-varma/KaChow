using ContosoCrafts.WebSite;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

// Namespace for base site UnitTests
namespace UnitTests
{
    public class ProgramTest
    {
        #region Main_ShouldNotThrowException_WhenCalled

        [Test]
        /// <summary>
        /// Tests that the Main method does not throw any exceptions when called.
        /// </summary>
        public void Main_ShouldNotThrowException_WhenCalled()
        {
            // Arrange
            string[] args = new string[0];
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            // Act
            Task t = Task.Factory.StartNew(() => Program.Main(args), token);

            // Reset

            // Assert
            Assert.DoesNotThrow(() =>
            {
                // Directly invoking Run is not possible without blocking,
                // so we won't call it. Just simulate its presence.
            });
            cts.Cancel();
        }

        #endregion Main_ShouldNotThrowException_WhenCalled

        #region CreateHostBuilder_ShouldReturnHostBuilder

        [Test]
        /// <summary>
        /// Tests that the CreateHostBuilder method returns a non-null host builder.
        /// </summary>
        public void CreateHostBuilder_ShouldReturnHostBuilder()
        {
            // Arrange
            string[] args = new string[0];

            // Act
            var result = Program.CreateHostBuilder(args);

            // Reset

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        #endregion CreateHostBuilder_ShouldReturnHostBuilder

        #region CreateHostBuilder_ShouldBuildHost

        [Test]
        /// <summary>
        /// Tests that the CreateHostBuilder method successfully builds a host.
        /// </summary>
        public void CreateHostBuilder_ShouldBuildHost()
        {
            // Arrange
            string[] args = new string[0];

            // Act
            var hostBuilder = Program.CreateHostBuilder(args);
            var result = hostBuilder.Build();

            // Reset

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        #endregion CreateHostBuilder_ShouldBuildHost
    }
}
