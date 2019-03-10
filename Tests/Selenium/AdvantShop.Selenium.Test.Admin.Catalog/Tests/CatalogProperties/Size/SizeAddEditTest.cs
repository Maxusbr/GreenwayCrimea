using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Catalog.Sizes
{
    [TestFixture]
    public class SizeAddEditTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
            "data\\Admin\\Catalog\\CatalogSize\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\CatalogSize\\Catalog.Brand.csv",
           "data\\Admin\\Catalog\\CatalogSize\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\CatalogSize\\Catalog.Size.csv",
           "data\\Admin\\Catalog\\CatalogSize\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\CatalogSize\\Catalog.ProductCategories.csv"
           );
             
            Init();
        }
        

        [Test]
        public void SizeAdd()
        {
            GoToAdmin("sizes");

            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"SizeName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"SizeName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"SizeName\"]")).SendKeys("NewSizeName");

            driver.FindElement(By.CssSelector("[data-e2e=\"SizeSort\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"SizeSort\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"SizeSort\"]")).SendKeys("10");

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            //check
            GoToAdmin("sizes");
            GetGridFilter().SendKeys("NewSizeName");
            DropFocus("h1");
            WaitForAjax();
            Assert.AreEqual("NewSizeName", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("10", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));
        }
        
        [Test]
        public void SizeAddCancel()
        {
            GoToAdmin("sizes");
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"SizeName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"SizeName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"SizeName\"]")).SendKeys("NewSizeNameCancel");
            driver.FindElement(By.XPath("//button[contains(text(), 'Отмена')]")).Click();
            Thread.Sleep(2000);
            GoToAdmin("sizes");
            GetGridFilter().SendKeys("NewSizeNameCancel");
            DropFocus("h1");
            WaitForAjax();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }

        [Test]
        public void SizeEdit()
        {
            GoToAdmin("sizes");

            GetGridFilter().SendKeys("NewSizeName");
            DropFocus("h1");
            WaitForAjax();

            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Размер", driver.FindElement(By.TagName("h2")).Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"SizeName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"SizeName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"SizeName\"]")).SendKeys("Changed");

            driver.FindElement(By.CssSelector("[data-e2e=\"SizeSort\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"SizeSort\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"SizeSort\"]")).SendKeys("50");

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            //check
            GoToAdmin("sizes");
            GetGridFilter().SendKeys("Changed");
            DropFocus("h1");
            WaitForAjax();
            Assert.AreEqual("Changed", GetGridCell(0, "SizeName").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("50", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("NewSizeName");
            DropFocus("h1");
            WaitForAjax();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }
    }
}