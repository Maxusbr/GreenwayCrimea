using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Orders.OrderStatus
{
    [TestFixture]
    public class OrderStatusTest : BaseSeleniumTest
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
        public void Grid()
        {
            testname = "OrderStatusGrid";
            VerifyBegin(testname);

            VerifyAreEqual("Найдено записей: 125", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "grid count all");
            VerifyAreEqual("color: rgb(240, 128, 128);", GetGridCell(0, "Color").FindElement(By.TagName("i")).GetAttribute("style"), "grid color");
            VerifyAreEqual("Order Status1", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "grid name");
            VerifyIsFalse(GetGridCell(0, "IsDefault").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "grid default");
            VerifyIsFalse(GetGridCell(0, "IsCanceled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "grid IsCanceled");
            VerifyIsFalse(GetGridCell(0, "IsCompleted").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "grid IsCompleted");
            VerifyAreEqual("1", GetGridCell(0, "SortOrder").Text, "grid sort order");
            VerifyAreEqual("Нет команды", GetGridCell(0, "CommandFormatted").Text, "grid status command");

            VerifyFinally(testname);
        }


        [Test]
        public void GoToEditByName()
        {
            testname = "GoToEditByName";
            VerifyBegin(testname);

            GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("Редактирование статуса заказа", driver.FindElement(By.TagName("h2")).Text, "pop up h2");
            XPathContainsText("button", "Отмена");

            VerifyFinally(testname);
        }


        [Test]
        public void GoToEditByServiceCol()
        {
            testname = "GoToEditByServiceCol";
            VerifyBegin(testname);

            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(3000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("Редактирование статуса заказа", driver.FindElement(By.TagName("h2")).Text, "pop up h2");
            XPathContainsText("button", "Отмена");

            VerifyFinally(testname);
        }

        [Test]
        public void OrderStatisezSelectDelete()
        {
            testname = "OrderStatisesSelectDelete";
            VerifyBegin(testname);

            //check delete cancel 
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Order Status1", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "1 grid canсel delete");
            VerifyAreEqual("Найдено записей: 125", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all canсel delete");

            //check delete
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            VerifyAreNotEqual("Order Status1", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "1 grid delete");

            //check select 
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 2 grid");
            VerifyIsTrue(GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 3 grid");
            VerifyIsTrue(GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 4 grid");
            VerifyAreEqual("3", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count selected");

            //check delete selected items
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("Order Status5", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "selected 2 grid delete");
            VerifyAreEqual("Order Status6", GetGridCell(1, "StatusName").FindElement(By.TagName("a")).Text, "selected 3 grid delete");
            VerifyAreEqual("Order Status7", GetGridCell(2, "StatusName").FindElement(By.TagName("a")).Text, "selected 4 grid delete");

            //check select all on page
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 1 grid");
            VerifyIsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 10 grid");

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("Order Status5", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "selected all on page 2 grid delete"); //default status
            VerifyAreEqual("Order Status23", GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text, "selected all on page 10 grid delete");

            //check select all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            VerifyAreEqual("112", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count all selected after deleting");

            //check deselect all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            VerifyIsTrue(!GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 1 grid");
            VerifyIsTrue(!GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 10 grid");

            //check delete all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyAreEqual("Order Status5", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "1 delete all except default");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "1 count all after deleting");

            GoToAdmin("orderstatuses");
            VerifyAreEqual("Order Status5", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "2 delete all except default");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "2 count all after deleting");

            VerifyFinally(testname);
        }
    }
}
