using System;
using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Mobile
{
    [TestFixture]
    public class MobileTestAdmin : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
             InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
              "Data\\Mobile\\Catalog.Category.csv",
              "Data\\Mobile\\Catalog.Product.csv",
              "Data\\Mobile\\Catalog.ProductCategories.csv",
              "Data\\Mobile\\Catalog.Offer.csv"
                );

            Init();
        }

        [Test]
        public void AEntry()
        {
            driver.Navigate().GoToUrl(baseURL + "/adminv2/settings/mobileversion");
            Thread.Sleep(1000);
            if (!driver.FindElement(By.CssSelector("[data-e2e=\"mobileEnabled\"]")).FindElement(By.Id("Enabled")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"mobileEnabled\"]")).FindElement(By.TagName("span")).Click();
                Thread.Sleep(2000);
                driver.FindElement(By.CssSelector("[data-e2e=\"mobileSave\"]")).Click();
                Thread.Sleep(2000);
            }
            
            GoToClient("/?forcedMobile=true");
            Thread.Sleep(3000);
            Assert.AreEqual("Главная", driver.FindElement(By.CssSelector(".cs-bg-1 div")).Text);
        }

    }
}
