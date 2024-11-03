using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using ContosoCrafts.WebSite.Models;
using System.Collections.Generic;

namespace UnitTests.Services
{
    public class JsonFileProductServiceTests
    {
        #region TestSetup

        private IEnumerable<ProductModel> OriginalProducts;

        [SetUp]
        public void TestInitialize()
        {
            OriginalProducts = TestHelper.ProductService.GetAllData();
        }

        [TearDown]
        public void TestCleanup()
        {
            TestHelper.ProductService.SaveData(OriginalProducts);
        }

        #endregion TestSetup


    }
}
