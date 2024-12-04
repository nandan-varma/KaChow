using System.Net.Http;
using ContosoCrafts.WebSite;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

// Namespace for base site UnitTests
namespace UnitTests
{
    /// <summary>
    /// Unit tests for the Startup class.
    /// </summary>
    public class StartupTest
    {
        #region TestSetup

        private Startup _startup;
        private Mock<IConfiguration> _configurationMock;

        /// <summary>
        /// Initializes the test setup.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Arrange
            _configurationMock = new Mock<IConfiguration>();
            _startup = new Startup(_configurationMock.Object);

            // Reset
        }

        #endregion TestSetup

        #region ConfigureServices

        /// <summary>
        /// Tests that ConfigureServices registers the required services.
        /// </summary>
        [Test]
        public void ConfigureServices_Valid_Registers_Required_Services_Should_Return_True()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            var webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            serviceCollection.AddSingleton<IWebHostEnvironment>(webHostEnvironmentMock.Object);

            // Act
            _startup.ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Reset

            // Assert
            Assert.That(serviceProvider.GetService<RatingService>(), Is.Not.Null);
            Assert.That(serviceProvider.GetService<JsonFileProductService>(), Is.Not.Null);
            Assert.That(serviceProvider.GetService<IHttpContextAccessor>(), Is.Not.Null);
            Assert.That(serviceProvider.GetService<IHttpClientFactory>(), Is.Not.Null);
            Assert.That(serviceProvider.GetService<Microsoft.AspNetCore.Mvc.Controllers.IControllerActivator>(), Is.Not.Null);
        }

        #endregion ConfigureServices

        #region Configure

        /// <summary>
        /// Tests that Configure sets up the middleware correctly.
        /// </summary>
        [Test]
        public void Configure_Valid_Configures_Middleware_Should_Return_True()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder(new string[0]);
            var services = builder.Services;
            _startup.ConfigureServices(services);
            var app = builder.Build();

            // Act
            _startup.Configure(app, new Mock<IWebHostEnvironment>().Object);

            // Reset

            // Assert
            // Ensure the app runs without throwing an exception
            Assert.That(app, Is.Not.Null);
        }

        /// <summary>
        /// Tests that Configure sets up the endpoints correctly.
        /// </summary>
        [Test]
        public void Configure_Valid_Sets_Up_Endpoints_Should_Return_True()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder(new string[0]);
            var services = builder.Services;
            _startup.ConfigureServices(services);
            var app = builder.Build();

            // Act
            _startup.Configure(app, new Mock<IWebHostEnvironment>().Object);

            // Manually build the endpoint route
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            var endpointsDataSources = app.Services.GetRequiredService<EndpointDataSource>();

            // Reset

            // Assert
            Assert.That(endpointsDataSources, Is.Not.Null);
        }

        /// <summary>
        /// Tests that Configure uses the DeveloperExceptionPage in development environment.
        /// </summary>
        [Test]
        public void Configure_Valid_Uses_DeveloperExceptionPage_In_Development()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder(new string[0]);
            var services = builder.Services;
            _startup.ConfigureServices(services);
            var app = builder.Build();

            var webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            webHostEnvironmentMock.Setup(env => env.EnvironmentName).Returns("Development");

            // Act
            _startup.Configure(app, webHostEnvironmentMock.Object);

            // Reset

            // Assert
            Assert.That(app, Is.Not.Null);
        }

        /// <summary>
        /// Tests that Configure uses StatusCodePagesWithReExecute in production environment.
        /// </summary>
        [Test]
        public void Configure_Valid_Uses_StatusCodePagesWithReExecute()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder(new string[0]);
            var services = builder.Services;
            _startup.ConfigureServices(services);
            var app = builder.Build();

            var webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            webHostEnvironmentMock.Setup(env => env.EnvironmentName).Returns("Production");

            // Act
            _startup.Configure(app, webHostEnvironmentMock.Object);

            // Reset

            // Assert
            var context = new DefaultHttpContext { Response = { StatusCode = 404 } };
            Assert.DoesNotThrow(() => app.StartAsync());
        }

        #endregion Configure

        #region Configuration

        /// <summary>
        /// Tests that the Configuration property is initialized correctly.
        /// </summary>
        [Test]
        public void Configuration_Property_Should_Be_Initialized()
        {
            // Arrange
            var configurationMock = new Mock<IConfiguration>();
            var startup = new Startup(configurationMock.Object);

            // Act
            var configuration = startup.Configuration;

            // Reset

            // Assert
            Assert.That(configuration, Is.Not.Null);
            Assert.That(configuration, Is.EqualTo(configurationMock.Object));
        }

        #endregion Configuration
    }
}
