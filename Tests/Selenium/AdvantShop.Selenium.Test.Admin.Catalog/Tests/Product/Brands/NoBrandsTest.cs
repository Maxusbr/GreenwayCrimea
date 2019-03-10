using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.Brand
{
    [TestFixture]
    public class BrandNoTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Catalog\\Brands\\NoBrands\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\Brands\\NoBrands\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\Brands\\NoBrands\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\Brands\\NoBrands\\Catalog.ProductCategories.csv"
           );
             
            Init();
        }
        

        [Test]
        public void OpenNoBrands()
        {
            GoToAdmin("brands");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }

    }
}