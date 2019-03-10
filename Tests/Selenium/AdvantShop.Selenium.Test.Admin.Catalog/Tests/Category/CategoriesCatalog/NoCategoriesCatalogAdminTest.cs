using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Catalog
{
    [TestFixture]
    public class CategoriesNoTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Catalog\\NoCategoryTest\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\NoCategoryTest\\Catalog.Offer.csv"
           );
             
            Init();
        }

       

        //[Test]
        //public void OpenNoCategories()
        //{
        //    GoToAdmin("catalog");
        //    Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        //}

    }
}