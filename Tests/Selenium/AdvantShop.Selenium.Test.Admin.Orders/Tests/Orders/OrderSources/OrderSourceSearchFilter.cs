using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Orders.OrderSource
{
    [TestFixture]
    public class OrderSourceSearchFilter : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Orders);
            InitializeService.LoadData(
             "Data\\Admin\\Orders\\OrderSources\\[Order].OrderSource.csv"
          );
             
            Init();
        }
    
        [Test]
        public void SearchCorrectSource()
        {
            GoToAdmin("ordersources");
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterSearch\"]")).SendKeys("Source2");
            driver.FindElement(By.TagName("h1")).Click();
            WaitForAjax();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"0\"]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Source2", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source28", GetGridCell(9, "Name").Text);
        }
        [Test]
        public void SearchCorrectOneSource()
        {
            GoToAdmin("ordersources");
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterSearch\"]")).SendKeys("Source100");
            driver.FindElement(By.TagName("h1")).Click();
            Thread.Sleep(3000);
            WaitForAjax();
            Assert.AreEqual("Source100", GetGridCell(0, "Name").Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 1);
        }
        [Test]
        public void SearchCorrectNoSource()
        {
            GoToAdmin("ordersources");
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterSearch\"]")).SendKeys("Source1000");
            driver.FindElement(By.TagName("h1")).Click();
            Thread.Sleep(3000);
            WaitForAjax();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 0);
        }
        [Test]
        public void SearchUncorrectSource()
        {
            GoToAdmin("ordersources");
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterSearch\"]")).SendKeys("123123@#$$^%&%&^&$%");
            driver.FindElement(By.TagName("h1")).Click();
            Thread.Sleep(3000);
            WaitForAjax();
            Assert.IsFalse(driver.FindElements(By.CssSelector("[data-e2e=\"gridRow\"]")).Count > 0);
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }
        [Test]
        public void SearchLongSource()
        {
            GoToAdmin("ordersources");
            Thread.Sleep(3000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterSearch\"]")).SendKeys("11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111");
            driver.FindElement(By.TagName("h1")).Click();
            Thread.Sleep(3000);
            WaitForAjax();
            Assert.IsTrue(driver.FindElements(By.CssSelector("[data-e2e=\"gridCell\"][data-e2e-col-index=\"1\"]")).Count == 0);
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }
    }
}
