using System;
using NUnit.Framework;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.ProductLists
{
    [TestFixture]
    public class ProductListsNoTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\ProductListsNo\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\ProductListsNo\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\ProductListsNo\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\ProductListsNo\\Catalog.ProductCategories.csv"
           );
             
            Init();
        }
        
        [Test]
        public void ProductListNo()
        {
            GoToAdmin("productlists");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }
    }
}