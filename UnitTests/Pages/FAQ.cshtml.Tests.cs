using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContosoCrafts.WebSite.Pages;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace UnitTests.Pages
{
    public class FAQTests
    {
        #region TestSetup

        public static FAQModel pageModel;

        [SetUp]
        public void TestInitialize()
        {
            var logger = new LoggerFactory().CreateLogger<FAQModel>();
            pageModel = new FAQModel(logger);
        }

        #endregion TestSetup

        #region OnGet

        [Test]
        /// <summary>
        /// Tests that OnGet method initializes the page model correctly.
        /// </summary>
        public void OnGet_Should_Initialize_PageModel()
        {
            // Arrange

            // Act
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel, Is.Not.Null);
        }

        #endregion OnGet
    }
}
