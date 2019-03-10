using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Orders.OrderSource
{
    [TestFixture]
    public class OrderSourcePaginationAndView : BaseSeleniumTest
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
        public void OpenOrders()
        {
            GoToAdmin("ordersources");
            Assert.AreEqual("Источники заказов/лидов",driver.FindElement(By.TagName("h1")).Text);
        }
        [Test]
        public void Present10Page()
        {
            GoToAdmin("ordersources");
            
            Assert.AreEqual("Source1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source10", GetGridCell(9, "Name").Text);
             ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Source11", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source20", GetGridCell(9, "Name").Text);
             ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Source21", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source30", GetGridCell(9, "Name").Text);
        }
        [Test]
        public void Present10PageToNext()
        {
            GoToAdmin("ordersources");

            Assert.AreEqual("Source1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source10", GetGridCell(9, "Name").Text);
             ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Source11", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source20", GetGridCell(9, "Name").Text);
             ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Source21", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source30", GetGridCell(9, "Name").Text);
        }
        [Test]
        public void Present10PageToPrevious()
        {
            GoToAdmin("ordersources");

            Assert.AreEqual("Source1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source10", GetGridCell(9, "Name").Text);
             ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Source11", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source20", GetGridCell(9, "Name").Text);
             ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Source1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source10", GetGridCell(9, "Name").Text);
        }
        [Test]
        public void Present10PageToEnd()
        {
            GoToAdmin("ordersources");

            Assert.AreEqual("Source1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source10", GetGridCell(9, "Name").Text);
            //to end
             ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Source101", GetGridCell(0, "Name").Text);
        }
        [Test]
        public void Present10PageToBegin()
        {
            GoToAdmin("ordersources");

            Assert.AreEqual("Source1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source10", GetGridCell(9, "Name").Text);
             ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Source11", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source20", GetGridCell(9, "Name").Text);
            Thread.Sleep(2000);
             ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Source21", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source30", GetGridCell(9, "Name").Text);

            //to begin
             ScrollTo(By.CssSelector(".version"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Source1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source10", GetGridCell(9, "Name").Text);
        }
    }
}
