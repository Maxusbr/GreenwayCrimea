using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Orders.OrderSource
{
    [TestFixture]
    public class OrderSourceSort : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Orders);
            InitializeService.LoadData(
             "Data\\Admin\\Orders\\OrderSources\\[Order].OrderSource.csv",
             "data\\Admin\\Orders\\Catalog.Product.csv",
           "data\\Admin\\Orders\\Catalog.Offer.csv",
           "data\\Admin\\Orders\\Catalog.Category.csv",
           "data\\Admin\\Orders\\Catalog.ProductCategories.csv",
            "data\\Admin\\Orders\\[Order].OrderContact.csv",
            "data\\Admin\\Orders\\[Order].OrderCurrency.csv",
             "data\\Admin\\Orders\\[Order].OrderItems.csv",
             "data\\Admin\\Orders\\[Order].OrderStatus.csv",
            // "data\\Admin\\Orders\\[Order].ShippingMethod.csv",
            // "data\\Admin\\Orders\\[Order].PaymentMethod.csv",
             "data\\Admin\\Orders\\[Order].[Order].csv"
        //     "data\\Admin\\Orders\\[Order].LeadItem.csv",
        //     "data\\Admin\\Orders\\[Order].LeadCurrency.csv",
          //   "data\\Admin\\Orders\\[Order].Lead.csv"
          );
             
            Init();
            GoToAdmin("ordersources");
        }

        [Test]
        public void SortByName()
        {
           GetGridCell(-1, "Name").Click();
            WaitForAjax();
            Assert.AreEqual("Source1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source16", GetGridCell(9, "Name").Text);

            GetGridCell(-1, "Name").Click();
            WaitForAjax();
            Assert.AreEqual("Source99", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source90", GetGridCell(9, "Name").Text);
        }

        [Test]
        public void SortByGroup()
        {
            GetGridCell(-1, "TypeFormatted").Click();
            WaitForAjax();
            Assert.AreEqual("Source10", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source100", GetGridCell(9, "Name").Text);
            Assert.AreEqual("Забытая корзина", GetGridCell(9, "TypeFormatted").Text);

            GetGridCell(-1, "TypeFormatted").Click();
            WaitForAjax();
            Assert.AreEqual("Source8", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source98", GetGridCell(9, "Name").Text);
            Assert.AreEqual("Социальные сети", GetGridCell(1, "TypeFormatted").Text);
        }

        [Test]
        public void SortByMain()
        {
            GetGridCell(-1, "Main").Click();
            WaitForAjax();
            Assert.AreEqual("Source2", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source11", GetGridCell(9, "Name").Text);
            Assert.IsFalse(GetGridCell(0, "Main").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.IsTrue(GetGridCell(2, "Main").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "Main").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            GetGridCell(-1, "Main").Click();
            WaitForAjax();
            Assert.AreEqual("Source3", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source12", GetGridCell(9, "Name").Text);
            Assert.IsTrue(GetGridCell(0, "Main").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "Main").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
        }

        [Test]
        public void SortBySortOrder()
        {
            GetGridCell(-1, "SortOrder").Click();
            WaitForAjax();
            Assert.AreEqual("Source1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source10", GetGridCell(9, "Name").Text);

            GetGridCell(-1, "SortOrder").Click();
            WaitForAjax();
            Assert.AreEqual("Source101", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source92", GetGridCell(9, "Name").Text);
        }

        [Test]
        public void SortByCountOrders()
        {
            GetGridCell(-1, "OrdersCount").Click();
            WaitForAjax();
            Assert.AreEqual("Source10", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source19", GetGridCell(9, "Name").Text);

            GetGridCell(-1, "OrdersCount").Click();
            WaitForAjax();
            Assert.AreEqual("Source1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source14", GetGridCell(9, "Name").Text);
        }
        /*
        [Test]
        public void SortByCountLid()
        {
            GoToAdmin("ordersources");
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"5\"]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Source2", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source11", GetGridCell(9, "Name").Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"5\"]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Source1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source12", GetGridCell(1, "Name").Text);
            Assert.AreEqual("Source9", GetGridCell(9, "Name").Text);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCell\"][data-e2e-col-index=\"5\"]")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Source2", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Source11", GetGridCell(9, "Name").Text);
        }  */     
    }
}
