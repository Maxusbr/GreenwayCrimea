using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Orders.OrderStatus
{
    [TestFixture]
    public class OrderStatusPageTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Orders);
            InitializeService.LoadData(
             "data\\Admin\\Orders\\OrderStatus\\[Order].OrderStatus.csv"
          );

            Init();
            GoToAdmin("orderstatuses");
        }


        [Test]
        public void Page()
        {
            testname = "OrderStatusPage";
            VerifyBegin(testname);

            VerifyAreEqual("Order Status1", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "page 1 line 1");
            VerifyAreEqual("Order Status10", GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            VerifyAreEqual("Order Status11", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "page 2 line 1");
            VerifyAreEqual("Order Status20", GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            VerifyAreEqual("Order Status21", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "page 3 line 1");
            VerifyAreEqual("Order Status30", GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            VerifyAreEqual("Order Status31", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "page 4 line 1");
            VerifyAreEqual("Order Status40", GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text, "page 4 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("Order Status41", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "page 5 line 1");
            VerifyAreEqual("Order Status50", GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text, "page 5 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("Order Status51", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "page 6 line 1");
            VerifyAreEqual("Order Status60", GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text, "page 6 line 10");

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            VerifyAreEqual("Order Status1", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "page 1 line 1");
            VerifyAreEqual("Order Status10", GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text, "page 1 line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void PageToPrevious()
        {
            testname = "OrderStatusPageToPrevious";
            VerifyBegin(testname);

            VerifyAreEqual("Order Status1", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "page 1 line 1");
            VerifyAreEqual("Order Status10", GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("Order Status11", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "page 2 line 1");
            VerifyAreEqual("Order Status20", GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("Order Status21", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "page 3 line 1");
            VerifyAreEqual("Order Status30", GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("Order Status11", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "page 2 line 1");
            VerifyAreEqual("Order Status20", GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("Order Status1", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "page 1 line 1");
            VerifyAreEqual("Order Status10", GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text, "page 1 line 10");

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            VerifyAreEqual("Order Status121", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "last page line 1");
            VerifyAreEqual("Order Status125", GetGridCell(4, "StatusName").FindElement(By.TagName("a")).Text, "last page line 10");

            VerifyFinally(testname);
        }
    }
}
