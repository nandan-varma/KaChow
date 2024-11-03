using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using NUnit.Framework;
using ContosoCrafts.WebSite;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace UnitTests
{
    public class StartupTests
    {
        private Startup _startup;
        private Mock<IConfiguration> _configurationMock;

        [SetUp]
        public void Setup()
        {
            _configurationMock = new Mock<IConfiguration>();
            _startup = new Startup(_configurationMock.Object);
        }

        /// <summary>
        /// ConfigureServices Valid Registers Required Services Should Return True
        /// </summary>
        [Test]
        public void ConfigureServices_Valid_Registers_Required_Services_Should_Return_True()
        {
            // Arrange
            var ServiceCollection = new ServiceCollection();
            var WebHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            ServiceCollection.AddSingleton<IWebHostEnvironment>(WebHostEnvironmentMock.Object);

            // Act
            _startup.ConfigureServices(ServiceCollection);

            // Assert
            var ServiceProvider = ServiceCollection.BuildServiceProvider();

            // Check if services are registered
            Assert.That(ServiceProvider.GetService<RatingService>(), Is.Not.Null);
            Assert.That(ServiceProvider.GetService<JsonFileProductService>(), Is.Not.Null);
            Assert.That(ServiceProvider.GetService<IHttpContextAccessor>(), Is.Not.Null);
            Assert.That(ServiceProvider.GetService<IHttpClientFactory>(), Is.Not.Null);
            Assert.That(ServiceProvider.GetService<Microsoft.AspNetCore.Mvc.Controllers.IControllerActivator>(), Is.Not.Null);
        }

        /// <summary>
        /// Configure Valid Configures Middleware Should Return True
        /// </summary>
        [Test]
        public void Configure_Valid_Configures_Middleware_Should_Return_True()
        {
            // Arrange
            var Builder = WebApplication.CreateBuilder(new string[0]);
            var Services = Builder.Services;
            _startup.ConfigureServices(Services);
            var App = Builder.Build();

            // Act
            _startup.Configure(App, new Mock<IWebHostEnvironment>().Object);

            // Assert
            // Ensure the app runs without throwing an exception
            Assert.That(App, Is.Not.Null);
        }

        /// <summary>
        /// Configure Valid Sets Up Endpoints Should Return True
        /// </summary>
        [Test]
        public void Configure_Valid_Sets_Up_Endpoints_Should_Return_True()
        {
            // Arrange
            var Builder = WebApplication.CreateBuilder(new string[0]);
            var Services = Builder.Services;
            _startup.ConfigureServices(Services);
            var App = Builder.Build();

            // Act
            _startup.Configure(App, new Mock<IWebHostEnvironment>().Object);

            // Manually build the endpoint route
            App.UseRouting();
            App.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Assert
            var EndpointsDataSources = App.Services.GetRequiredService<EndpointDataSource>();
            Assert.That(EndpointsDataSources, Is.Not.Null);
        }

        [Test]
        public void Configure_Valid_Uses_DeveloperExceptionPage_In_Development()
        {
            // Arrange
            var Builder = WebApplication.CreateBuilder(new string[0]);
            var Services = Builder.Services;
            _startup.ConfigureServices(Services);
            var App = Builder.Build();

            var WebHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            WebHostEnvironmentMock.Setup(env => env.EnvironmentName).Returns("Development");

            // Act
            _startup.Configure(App, WebHostEnvironmentMock.Object);

            // Assert
            // Ensure the app runs without throwing an exception
            Assert.That(App, Is.Not.Null);
        }

        [Test]
        public void Configuration_Property_Should_Be_Initialized()
        {
            // Arrange
            var ConfigurationMock = new Mock<IConfiguration>();
            var Startup = new Startup(ConfigurationMock.Object);

            // Act
            var Configuration = Startup.Configuration;

            // Assert
            Assert.That(Configuration, Is.Not.Null);
            Assert.That(Configuration, Is.EqualTo(ConfigurationMock.Object));
        }
    }
}
