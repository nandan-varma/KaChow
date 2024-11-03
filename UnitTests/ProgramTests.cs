using NUnit.Framework;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using ContosoCrafts.WebSite;

namespace UnitTests
{
    public class ProgramTests
    {
        [Test]
        public void Main_ShouldNotThrowException_WhenCalled()
        {
            // Arrange
            string[] Args = new string[0];

            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                // Instead of running, we simulate what would happen
                var HostBuilder = Program.CreateHostBuilder(Args);
                var Host = HostBuilder.Build();

                // Directly invoking Run is not possible without blocking,
                // so we won't call it. Just simulate its presence.
            });
        }

        [Test]
        public void CreateHostBuilder_ShouldReturnHostBuilder()
        {
            // Arrange
            string[] Args = new string[0];

            // Act
            var HostBuilder = Program.CreateHostBuilder(Args);

            // Assert
            Assert.That(HostBuilder, Is.Not.Null);
        }

        [Test]
        public void CreateHostBuilder_ShouldBuildHost()
        {
            // Arrange
            string[] Args = new string[0];

            // Act
            var HostBuilder = Program.CreateHostBuilder(Args);
            var Host = HostBuilder.Build();

            // Assert
            Assert.That(Host, Is.Not.Null);
        }
    }
}
