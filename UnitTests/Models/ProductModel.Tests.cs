using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using NUnit.Framework;
using ContosoCrafts.WebSite.Models;

namespace UnitTests.Models
{
    public class ProductModelTests
    {
        #region TestSetup

        private ProductModel Product;

        [SetUp]
        public void TestInitialize()
        {
            Product = new ProductModel
            {
                Id = "1",
                Image = "image.png",
                Url = "http://example.com",
                Name = "Test Product",
                Model = "Model X",
                Description = "This is a test product.",
                Ratings = new int[] { 5, 4, 3 },
                Price = 50,
                CommentList = new List<CommentModel>
                {
                    new CommentModel { Comment = "Great product!" }
                }
            };
        }

        #endregion TestSetup



    }
}
