using Bunit;
using NUnit.Framework;

// Namespace for base site UnitTests
namespace UnitTests
{
    /// <summary>
    /// Test Context used by bUnit
    /// </summary>
    public abstract class BunitTestContext : TestContextWrapper
    {
        // The Setup sets the context
        [SetUp]
        public void Setup() => TestContext = new Bunit.TestContext();

        // When done displose removes it, to free up system resources
        [TearDown]
        public void TearDown() => TestContext.Dispose();
    }
}