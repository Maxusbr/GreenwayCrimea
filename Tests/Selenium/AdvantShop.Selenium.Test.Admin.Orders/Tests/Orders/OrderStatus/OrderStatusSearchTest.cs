using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Orders.OrderStatus
{
    [TestFixture]
    public class OrderStatusSearchTest : BaseSeleniumTest
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
        }

        [Test]
        public void SearchExist()
        {
            testname = "SearchExist";
            VerifyBegin(testname);

            GoToAdmin("orderstatuses");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Order Status114");

            VerifyAreEqual("Order Status114", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "search exist status");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }



        [Test]
        public void SearchNotExist()
        {
            testname = "SearchNotExist";
            VerifyBegin(testname);

            GoToAdmin("orderstatuses");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Order Status5543");

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search not exist status");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void SearchMuchSymbols()
        {
            testname = "SearchMuchSymbols";
            VerifyBegin(testname);

            GoToAdmin("orderstatuses");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search too much symbols");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void SearchInvalidSymbols()
        {
            testname = "SearchInvalidSymbols";
            VerifyBegin(testname);

            GoToAdmin("orderstatuses");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("########@@@@@@@@&&&&&&&******,,,,..");

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search invalid symbols");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }
    }
}
