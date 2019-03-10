using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Orders.OrderStatus
{
    [TestFixture]
    public class OrderStatusSortTest : BaseSeleniumTest
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
        public void SortByName()
        {
            testname = "SortByName";
            VerifyBegin(testname);

            GetGridCell(-1, "StatusName").Click();
            WaitForAjax();
            VerifyAreEqual("Order Status1", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "sort name 1 asc");
            VerifyAreEqual("Order Status107", GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text, "sort name 10 asc");

            GetGridCell(-1, "StatusName").Click();
            WaitForAjax();
            VerifyAreEqual("Order Status99", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "sort name 1 desc");
            VerifyAreEqual("Order Status90", GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text, "sort name 10 desc");

            VerifyFinally(testname);
        }


        [Test]
        public void SortByDefault()
        {
            testname = "SortByDefault";
            VerifyBegin(testname);

            GetGridCell(-1, "IsDefault").Click();
            WaitForAjax();
            VerifyIsFalse(GetGridCell(0, "IsDefault").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "sort IsDefault 1 asc");
            VerifyIsFalse(GetGridCell(9, "IsDefault").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "sort IsDefault 10 asc");

            string ascLine1 = GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text;
            string ascLine10 = GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text;

            VerifyIsFalse(ascLine1.Equals(ascLine10), "asc different statuses");

            GetGridCell(-1, "IsDefault").Click();
            WaitForAjax();
            VerifyIsTrue(GetGridCell(0, "IsDefault").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "sort IsDefault 1 desc");
            VerifyIsFalse(GetGridCell(9, "IsDefault").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "sort IsDefault 10 desc");

            string descLine1 = GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text;
            string descLine10 = GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text;

            VerifyIsFalse(descLine1.Equals(descLine10), "desc different statuses");

            VerifyFinally(testname);
        }


        [Test]
        public void SortByIsCanceled()
        {
            testname = "SortByIsCanceled";
            VerifyBegin(testname);

            GetGridCell(-1, "IsCanceled").Click();
            WaitForAjax();
            VerifyIsFalse(GetGridCell(0, "IsCanceled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "sort IsCanceled 1 asc");
            VerifyIsFalse(GetGridCell(9, "IsCanceled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "sort IsCanceled 10 asc");

            string ascLine1 = GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text;
            string ascLine10 = GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text;

            VerifyIsFalse(ascLine1.Equals(ascLine10), "asc different statuses");

            GetGridCell(-1, "IsCanceled").Click();
            WaitForAjax();
            VerifyIsTrue(GetGridCell(0, "IsCanceled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "sort IsCanceled 1 desc");
            VerifyIsTrue(GetGridCell(9, "IsCanceled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "sort IsCanceled 10 desc");

            string descLine1 = GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text;
            string descLine10 = GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text;

            VerifyIsFalse(descLine1.Equals(descLine10), "desc different statuses");

            VerifyFinally(testname);
        }


        [Test]
        public void SortByIsCompleted()
        {
            testname = "SortByIsCompleted";
            VerifyBegin(testname);

            GetGridCell(-1, "IsCompleted").Click();
            WaitForAjax();
            VerifyIsFalse(GetGridCell(0, "IsCompleted").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "sort IsCompleted 1 asc");
            VerifyIsFalse(GetGridCell(9, "IsCompleted").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "sort IsCompleted 10 asc");

            string ascLine1 = GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text;
            string ascLine10 = GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text;

            VerifyIsFalse(ascLine1.Equals(ascLine10), "asc different statuses");

            GetGridCell(-1, "IsCompleted").Click();
            WaitForAjax();
            VerifyIsTrue(GetGridCell(0, "IsCompleted").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "sort IsCompleted 1 desc");
            VerifyIsTrue(GetGridCell(9, "IsCompleted").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "sort IsCompleted 10 desc");

            string descLine1 = GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text;
            string descLine10 = GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text;

            VerifyIsFalse(descLine1.Equals(descLine10), "desc different statuses");

            VerifyFinally(testname);
        }

        [Test]
        public void SortBySortOrder()
        {
            testname = "SortBySortOrder";
            VerifyBegin(testname);

            GetGridCell(-1, "SortOrder").Click();
            WaitForAjax();
            VerifyAreEqual("1", GetGridCell(0, "SortOrder").Text, "sort SortOrder 1 asc");
            VerifyAreEqual("10", GetGridCell(9, "SortOrder").Text, "sort SortOrder 10 asc");

            GetGridCell(-1, "SortOrder").Click();
            WaitForAjax();
            VerifyAreEqual("125", GetGridCell(0, "SortOrder").Text, "sort SortOrder 1 desc");
            VerifyAreEqual("116", GetGridCell(9, "SortOrder").Text, "sort SortOrder 10 desc");

            VerifyFinally(testname);
        }


        [Test]
        public void SortByCommand()
        {
            testname = "SortByCommand";
            VerifyBegin(testname);

            GetGridCell(-1, "CommandFormatted").Click();
            WaitForAjax();
            VerifyAreEqual("Нет команды", GetGridCell(0, "CommandFormatted").Text, "sort Command 1 asc");
            VerifyAreEqual("Нет команды", GetGridCell(9, "CommandFormatted").Text, "sort Command 10 asc");

            string ascLine1 = GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text;
            string ascLine10 = GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text;

            VerifyIsFalse(ascLine1.Equals(ascLine10), "asc different statuses");

            GetGridCell(-1, "CommandFormatted").Click();
            WaitForAjax();
            VerifyAreEqual("Списание товара со склада", GetGridCell(0, "CommandFormatted").Text, "sort Command 1 desc");
            VerifyAreEqual("Списание товара со склада", GetGridCell(0, "CommandFormatted").Text, "sort Command 10 desc");

            string descLine1 = GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text;
            string descLine10 = GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text;

            VerifyIsFalse(descLine1.Equals(descLine10), "desc different statuses");

            VerifyFinally(testname);
        }
    }
}
