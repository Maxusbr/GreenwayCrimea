using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Orders.OrderStatus
{
    [TestFixture]
    public class OrderStatusFilterTest : BaseMultiSeleniumTest
    {
        [SetUp]
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
        public void FilterName()
        {
            testname = "OrderStatusFilterName";
            VerifyBegin(testname);

            //check filter Contact
            Functions.GridFilterSet(driver, baseURL, name: "StatusName");

            //search by not exist Contact
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("not existing Order Status 3");
            Blur();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            Blur();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");

            //search invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            Blur();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "invalid symbols");

            //search by exist contact
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("Order Status1");
            Blur();

            VerifyAreEqual("Order Status1", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "lead id fullname line 1");
            VerifyAreEqual("Order Status18", GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text, "lead id fullname line 10");
            VerifyAreEqual("Найдено записей: 37", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter StatusName");

            //check go to edit and back
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.CssSelector(".modal-content"));
            VerifyAreEqual("Редактирование статуса заказа", driver.FindElement(By.TagName("h2")).Text, "pop up h2");

            XPathContainsText("button", "Отмена");

            VerifyAreEqual("Найдено записей: 37", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter StatusName return");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"StatusName\"]")).Displayed);

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "StatusName");
            VerifyAreEqual("Найдено записей: 88", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter StatusName deleting 1");

            GoToAdmin("orderstatuses");
            VerifyAreEqual("Найдено записей: 88", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter StatusName deleting 2");


            VerifyFinally(testname);
        }

        [Test]
        public void FilterIsDefault()
        {
            testname = "OrderStatusFilterIsDefault";
            VerifyBegin(testname);

            //check filter is default yes
            Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "IsDefault", filterItem: "Да", tag: "h1");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count filter is default yes");
            VerifyAreEqual("Order Status5", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "name filter is default yes");
            VerifyIsTrue(GetGridCell(0, "IsDefault").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "default filter is default yes");

            //check filter is default no
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Нет");
            VerifyAreEqual("Найдено записей: 124", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count filter is default no");

            VerifyAreEqual("Order Status1", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "name line 1 filter is default no");
            VerifyIsFalse(GetGridCell(0, "IsDefault").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "default line 1 filter is default no");
            VerifyAreEqual("Order Status11", GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text, "name line 10 filter is default no");
            VerifyIsFalse(GetGridCell(9, "IsDefault").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "default line 10 filter is default no");
            
            //check go to edit and back
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.CssSelector(".modal-content"));
            VerifyAreEqual("Редактирование статуса заказа", driver.FindElement(By.TagName("h2")).Text, "pop up h2");

            XPathContainsText("button", "Отмена");
            
            VerifyAreEqual("Найдено записей: 124", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter is default return");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"IsDefault\"]")).Displayed, "filter is default Displayed");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "IsDefault");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter is default after deleting 1");

            GoToAdmin("orderstatuses");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter is default after deleting 2");
            VerifyAreEqual("Order Status5", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "name filter is default after deleting 2");

            VerifyFinally(testname);
        }

        [Test]
        public void FilterIsCanceled()
        {
            testname = "OrderStatusFilterIsCanceled";
            VerifyBegin(testname);

            //check filter IsCanceled yes
            Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "IsCanceled", filterItem: "Да", tag: "h1");
            VerifyAreEqual("Найдено записей: 21", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count filter is Canceled yes");
            VerifyAreEqual("Order Status105", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "name line 1 filter is Canceled yes");
            VerifyIsTrue(GetGridCell(0, "IsCanceled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "Canceled line 1 filter is Canceled yes");
            VerifyAreEqual("Order Status114", GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text, "name line 10 filter is Canceled yes");
            VerifyIsTrue(GetGridCell(9, "IsCanceled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "Canceled line 10 filter is Canceled yes");

            //check filter IsCanceled no
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Нет");
            VerifyAreEqual("Найдено записей: 104", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count filter is Canceled no");

            VerifyAreEqual("Order Status1", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "name line 1 filter is Canceled no");
            VerifyIsFalse(GetGridCell(0, "IsCanceled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "Canceled line 1 filter is Canceled no");
            VerifyAreEqual("Order Status10", GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text, "name line 10 filter is Canceled no");
            VerifyIsFalse(GetGridCell(9, "IsCanceled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "Canceled line 10 filter is Canceled no");

            //check go to edit and back
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.CssSelector(".modal-content"));
            VerifyAreEqual("Редактирование статуса заказа", driver.FindElement(By.TagName("h2")).Text, "pop up h2");

            XPathContainsText("button", "Отмена");

            VerifyAreEqual("Найдено записей: 104", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter is Canceled return");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"IsCanceled\"]")).Displayed, "filter is Canceled Displayed");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "delete filtered items except default");
            VerifyAreEqual("Order Status5", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "name delete filtered items except default");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "IsCanceled");
            VerifyAreEqual("Найдено записей: 22", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter is Canceled after deleting 1 except default");

            GoToAdmin("orderstatuses");
            VerifyAreEqual("Найдено записей: 22", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter is Canceled after deleting 2 except default");

            VerifyFinally(testname);
        }

        [Test]
        public void FilterIsCompleted()
        {
            testname = "OrderStatusFilterIsCompleted";
            VerifyBegin(testname);

            //check filter Completed yes
            Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "IsCompleted", filterItem: "Да", tag: "h1");
            VerifyAreEqual("Найдено записей: 20", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count filter is Completed yes");
            VerifyAreEqual("Order Status4", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "name line 1 filter is Completed yes");
            VerifyIsTrue(GetGridCell(0, "IsCompleted").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "Completed line 1 filter is Completed yes");
            VerifyAreEqual("Order Status115", GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text, "name line 10 filter is Completed yes");
            VerifyIsTrue(GetGridCell(9, "IsCompleted").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "Completed line 10 filter is Completed yes");

            //check filter Completed no
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Нет");
            VerifyAreEqual("Найдено записей: 105", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count filter is Completed no");

            VerifyAreEqual("Order Status1", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "name line 1 filter is Completed no");
            VerifyIsFalse(GetGridCell(0, "IsCompleted").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "Completed line 1 filter is Completed no");
            VerifyAreEqual("Order Status11", GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text, "name line 10 filter is Completed no");
            VerifyIsFalse(GetGridCell(9, "IsCompleted").FindElement(By.CssSelector("[data-e2e=\"switchOnOffSelect\"]")).Selected, "Completed line 10 filter is Completed no");

            //check go to edit and back
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.CssSelector(".modal-content"));
            VerifyAreEqual("Редактирование статуса заказа", driver.FindElement(By.TagName("h2")).Text, "pop up h2");

            XPathContainsText("button", "Отмена");

            VerifyAreEqual("Найдено записей: 105", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter is Completed return");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"IsCompleted\"]")).Displayed, "filter is Completed Displayed");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "delete filtered items except default");
            VerifyAreEqual("Order Status5", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "name delete filtered items except default");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "IsCompleted");
            VerifyAreEqual("Найдено записей: 21", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter is Completed after deleting 1 except default");

            GoToAdmin("orderstatuses");
            VerifyAreEqual("Найдено записей: 21", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter is Completed after deleting 2 except default");

            VerifyFinally(testname);
        }


        [Test]
        public void FilterCommand()
        {
            testname = "OrderStatusFilterCommand";
            VerifyBegin(testname);

            //check filter Command to stock
            Functions.GridFilterSelectDropFocus(driver, baseURL, filterName: "CommandFormatted", filterItem: "Возврат товара на склад", tag: "h1");
            VerifyAreEqual("Найдено записей: 22", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count filter Command to stock");
            VerifyAreEqual("Order Status15", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "name line 1 filter Command to stock");
            VerifyAreEqual("Возврат товара на склад", GetGridCell(0, "CommandFormatted").Text, "Command line 1 filter Command to stock");
            VerifyAreEqual("Order Status24", GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text, "name line 10 filter Command to stock");
            VerifyAreEqual("Возврат товара на склад", GetGridCell(9, "CommandFormatted").Text, "Command line 10 filter Command to stock");

            //check filter Command from stock
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Списание товара со склада");
            VerifyAreEqual("Найдено записей: 29", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count filter Command from stock");
            VerifyAreEqual("Order Status34", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "name line 1 filter Command from stock");
            VerifyAreEqual("Списание товара со склада", GetGridCell(0, "CommandFormatted").Text, "Command line 1 filter Command from stock");
            VerifyAreEqual("Order Status43", GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text, "name line 10 filter Command from stock");
            VerifyAreEqual("Списание товара со склада", GetGridCell(9, "CommandFormatted").Text, "Command line 10 filter Command from stock");

            //check filter Command no
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Нет команды");
            VerifyAreEqual("Найдено записей: 74", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count filter Command no");
            VerifyAreEqual("Order Status1", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "name line 1 filter Command no");
            VerifyAreEqual("Нет команды", GetGridCell(0, "CommandFormatted").Text, "Completed line 1 filter Command no");
            VerifyAreEqual("Order Status10", GetGridCell(9, "StatusName").FindElement(By.TagName("a")).Text, "name line 10 filter Command no");
            VerifyAreEqual("Нет команды", GetGridCell(9, "CommandFormatted").Text, "Completed line 10 filter Command no");

            //check go to edit and back
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.CssSelector(".modal-content"));
            VerifyAreEqual("Редактирование статуса заказа", driver.FindElement(By.TagName("h2")).Text, "pop up h2");

            XPathContainsText("button", "Отмена");

            VerifyAreEqual("Найдено записей: 74", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter Command return");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"CommandFormatted\"]")).Displayed, "filter Command Displayed");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "delete filtered items except default");
            VerifyAreEqual("Order Status5", GetGridCell(0, "StatusName").FindElement(By.TagName("a")).Text, "name delete filtered items except default");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "CommandFormatted");
            VerifyAreEqual("Найдено записей: 52", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter Command after deleting 1 except default");

            GoToAdmin("orderstatuses");
            VerifyAreEqual("Найдено записей: 52", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter Command after deleting 2 except default");

            VerifyFinally(testname);
        }
    }
}
