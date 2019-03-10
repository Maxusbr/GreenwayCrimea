using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Orders.OrderStatus
{
    [TestFixture]
    public class OrderStatusPresentTest : BaseSeleniumTest
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
        public void Present10()
        {
            testname = "OrderStatusPresent10";
            VerifyBegin(testname);

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("10");
            VerifyAreEqual("Order Status1", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "present line 1");
            VerifyAreEqual("Order Status10", GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text, "present line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void Present20()
        {
            testname = "OrderStatusPresent20";
            VerifyBegin(testname);

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("20");
            VerifyAreEqual("Order Status1", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "present line 1");
            VerifyAreEqual("Order Status20", GetGridCell(19, "StatusName").FindElement(By.TagName("a")).Text, "present line 20");

            VerifyFinally(testname);
        }

        [Test]
        public void Present50()
        {
            testname = "OrderStatusPresent50";
            VerifyBegin(testname);

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("50");
            VerifyAreEqual("Order Status1", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "present line 1");
            VerifyAreEqual("Order Status50", GetGridCell(49, "StatusName").FindElement(By.TagName("a")).Text, "present line 50");

            VerifyFinally(testname);
        }

        [Test]
        public void Present100()
        {
            testname = "OrderStatusPresent100";
            VerifyBegin(testname);

            ScrollToElements(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("100");
            VerifyAreEqual("Order Status1", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "present line 1");
            VerifyAreEqual("Order Status100", GetGridCell(99, "StatusName").FindElement(By.TagName("a")).Text, "present line 100");

            VerifyFinally(testname);
        }
    }
}
