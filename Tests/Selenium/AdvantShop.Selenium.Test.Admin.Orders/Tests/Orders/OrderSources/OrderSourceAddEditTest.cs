using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Orders.OrderSource
{
    [TestFixture]
    public class OrderSourceAddEditTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Orders);
            InitializeService.LoadData(
          "data\\Admin\\Orders\\OrderSources\\OrderSourceAdd\\Catalog.Product.csv",
           "data\\Admin\\Orders\\OrderSources\\OrderSourceAdd\\Catalog.Offer.csv",
           "data\\Admin\\Orders\\OrderSources\\OrderSourceAdd\\Catalog.Category.csv",
           "data\\Admin\\Orders\\OrderSources\\OrderSourceAdd\\Catalog.ProductCategories.csv",
            "data\\Admin\\Orders\\OrderSources\\OrderSourceAdd\\[Order].OrderContact.csv",
            "data\\Admin\\Orders\\OrderSources\\OrderSourceAdd\\[Order].OrderCurrency.csv",
             "data\\Admin\\Orders\\OrderSources\\OrderSourceAdd\\[Order].OrderItems.csv",
             "data\\Admin\\Orders\\OrderSources\\OrderSourceAdd\\[Order].OrderSource.csv",
             "data\\Admin\\Orders\\OrderSources\\OrderSourceAdd\\[Order].OrderStatus.csv",
             "data\\Admin\\Orders\\OrderSources\\OrderSourceAdd\\[Order].[Order].csv"/*,
             "data\\Admin\\Orders\\OrderSources\\OrderSourceAdd\\[Order].LeadItem.csv",
             "data\\Admin\\Orders\\OrderSources\\OrderSourceAdd\\[Order].LeadCurrency.csv",
             "data\\Admin\\Orders\\OrderSources\\OrderSourceAdd\\[Order].Lead.csv"*/
           );
             
            Init();
        }
        
        [Test]
        public void OrderSourceAdd()
        {
            GoToAdmin("ordersources");
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Источники заказов/лидов", driver.FindElement(By.TagName("h1")).Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceName\"]")).SendKeys("NewOrderSourceName");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceType\"]")))).SelectByValue("1");

            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceTypeSelect\"]")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceSort\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceSort\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceSort\"]")).SendKeys("1");

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            //check
            GoToAdmin("ordersources");
            Thread.Sleep(1000);
            GetGridFilter().SendKeys("NewOrderSourceName");
            DropFocus("h1");
            Assert.AreEqual("NewOrderSourceName", GetGridCell(0, "Name").Text);
            Assert.AreEqual("1", GetGridCell(0, "SortOrder").Text);
            Assert.IsTrue(GetGridCell(0, "TypeFormatted").Text.Contains("Корзина интернет магазина"));
            Assert.IsTrue(GetGridCell(0, "Main").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
        }

        [Test]
        public void OrderSourceEdit()
        {
            GoToAdmin("ordersources");
            GetGridFilter().SendKeys("NewOrderSourceName");
            DropFocus("h1");
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Источники заказов/лидов", driver.FindElement(By.TagName("h1")).Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceName\"]")).SendKeys("Changed");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceType\"]")))).SelectByValue("5");

            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceTypeSelect\"]")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceSort\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceSort\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceSort\"]")).SendKeys("5");

            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            //check
            GoToAdmin("ordersources");
            Thread.Sleep(1000);
            GetGridFilter().SendKeys("Changed");
            DropFocus("h1");
            Assert.AreEqual("Changed", GetGridCell(0, "Name").Text);
            Assert.AreEqual("5", GetGridCell(0, "SortOrder").Text);
            Assert.IsTrue(GetGridCell(0, "TypeFormatted").Text.Contains("Мобильная версия"));
            Assert.IsFalse(GetGridCell(0, "Main").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
        }
        /*
        [Test]
        public void OrderSourceAddSaveName()
        {
            GoToAdmin("ordersources");
            Thread.Sleep(4000);
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceName\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceName\"]")).SendKeys("NewOrderSourceName");
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(4000);
            driver.Navigate().Refresh();
            Thread.Sleep(4000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterSearch\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterSearch\"]")).SendKeys("NewOrderSourceName");
            Thread.Sleep(5000);
            driver.FindElement(By.TagName("body")).Click();
            Thread.Sleep(5000);
            Assert.AreEqual("NewOrderSourceName", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'Name\']\"]")).Text);
            Thread.Sleep(1000);
        }
        
        [Test]
        public void OrderSourceAddSaveType()
        {
            GoToAdmin("ordersources");
            Thread.Sleep(4000);
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceName\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceName\"]")).SendKeys("NewOrderSourceType");
            Thread.Sleep(3000);
            IWebElement elem = driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceType\"]"));
            SelectElement select = new SelectElement(elem);
            select.SelectByValue("1");
            Thread.Sleep(4000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(4000);
            driver.Navigate().Refresh();
            Thread.Sleep(4000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterSearch\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterSearch\"]")).SendKeys("NewOrderSourceType");
            Thread.Sleep(5000);
            driver.FindElement(By.TagName("body")).Click();
            Thread.Sleep(5000);
            Assert.AreEqual("NewOrderSourceType", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'Name\']\"]")).Text);
            Thread.Sleep(1000);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'TypeFormatted\']\"]")).Text.Contains("Корзина интернет магазина"));
            Thread.Sleep(1000);
        }
        
        [Test]
        public void OrderSourceAddSaveMain()
        {
            GoToAdmin("ordersources");
            Thread.Sleep(4000);
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceName\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceName\"]")).SendKeys("NewOrderSourceMain");
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceTypeSelect\"]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(4000);
            driver.Navigate().Refresh();
            Thread.Sleep(4000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterSearch\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterSearch\"]")).SendKeys("NewOrderSourceMain");
            Thread.Sleep(5000);
            driver.FindElement(By.TagName("body")).Click();
            Thread.Sleep(5000);
            Assert.AreEqual("NewOrderSourceMain", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'Name\']\"]")).Text);
            Thread.Sleep(1000);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'Main\']\"] [data-e2e=\"switchOnOffInput\"]")).Selected);
            Thread.Sleep(1000);
        }*/

        [Test]
        public void OrderSourceAddSaveNotMain()
        {
            GoToAdmin("ordersources");
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceName\"]")).SendKeys("NewOrderSourceNotMain");
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("ordersources");

            GetGridFilter().SendKeys("NewOrderSourceNotMain");
            DropFocus("h1");
            Assert.AreEqual("NewOrderSourceNotMain", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'Name\']\"]")).Text);
            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'Main\']\"] [data-e2e=\"switchOnOffInput\"]")).Selected);
        }
        /*
        [Test]
        public void OrderSourceAddSaveSort()
        {
            GoToAdmin("ordersources");
            Thread.Sleep(4000);
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceName\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceName\"]")).SendKeys("NewOrderSourceSort");
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceSort\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceSort\"]")).Clear();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"OrderSourceSort\"]")).SendKeys("1");
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Сохранить')]")).Click();
            Thread.Sleep(4000);
            driver.Navigate().Refresh();
            Thread.Sleep(4000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterSearch\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterSearch\"]")).SendKeys("NewOrderSourceSort");
            Thread.Sleep(5000);
            driver.FindElement(By.TagName("body")).Click();
            Thread.Sleep(5000);
            Assert.AreEqual("NewOrderSourceSort", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'Name\']\"]")).Text);
            Thread.Sleep(1000);
            Assert.AreEqual("1", driver.FindElement(By.CssSelector("[data-e2e-grid-cell=\"grid[0][\'SortOrder\']\"]")).Text);
            Thread.Sleep(1000);
        }*/
    }
}