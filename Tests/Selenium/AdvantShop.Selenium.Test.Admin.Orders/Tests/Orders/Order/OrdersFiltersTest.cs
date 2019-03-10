using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace AdvantShop.SeleniumTest.Admin.Orders
{
    [TestFixture]
    public class OrdersFilterStatusTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Orders);
            InitializeService.LoadData(
             "Data\\Admin\\Orders\\OrderGrid\\Catalog.Product.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Offer.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Category.csv",
          "Data\\Admin\\Orders\\OrderGrid\\Catalog.ProductCategories.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Customers.CustomerGroup.csv",
           "data\\Admin\\Orders\\OrderGrid\\Customers.Country.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Region.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.City.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Customer.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Contact.csv",
              "data\\Admin\\Orders\\OrderGrid\\Customers.Managers.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCustomer.csv",
            "Data\\Admin\\Orders\\OrderGrid\\[Order].OrderSource.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderContact.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCurrency.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderItems.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderStatus.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].[Order].csv"
          );

            Init();
            GoToAdmin("orders");
        }


        [Test]
        public void FilterStatus()
        {
            testname = "OrderFilterStatus";
            VerifyBegin(testname);

            Functions.GridFilterSet(driver, baseURL, "_noopColumnStatuses");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Новый");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 5", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all new filtered");
            VerifyAreEqual("5", GetGridCell(0, "Number").Text, "line 1 status new filtered");
            VerifyAreEqual("1", GetGridCell(4, "Number").Text, "line 5 status new filtered");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("В обработке");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 5", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all in progress filtered");
            VerifyAreEqual("10", GetGridCell(0, "Number").Text, "line 1 status in progress filtered");
            VerifyAreEqual("6", GetGridCell(4, "Number").Text, "line 5 status in progress filtered");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Отправлен");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 5", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all sent filtered");
            VerifyAreEqual("15", GetGridCell(0, "Number").Text, "line 1 status sent filtered");
            VerifyAreEqual("11", GetGridCell(4, "Number").Text, "line 5 status sent filtered");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Доставлен");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 10", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all delivered filtered");
            VerifyAreEqual("25", GetGridCell(0, "Number").Text, "line 1 status delivered filtered");
            VerifyAreEqual("16", GetGridCell(9, "Number").Text, "line 10 status delivered filtered");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Отменён");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all canceled filtered");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no orders with canceled status filter");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Отменен навсегда");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 74", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all canceled forever filtered");
            VerifyAreEqual("96", GetGridCell(0, "Number").Text, "line 1 status canceled forever filtered");
            VerifyAreEqual("92", GetGridCell(9, "Number").Text, "line 10 status canceled forever filtered");

            //close
            Functions.GridFilterClose(driver, baseURL, "_noopColumnStatuses");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 99", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");
            VerifyAreEqual("96", GetGridCell(0, "Number").Text, "line 1 filter closed");
            VerifyAreEqual("92", GetGridCell(9, "Number").Text, "line 10 filter closed");

            VerifyFinally(testname);
        }


        [Test]
        public void FilterStatusDelete()
        {
            testname = "OrderFilterStatusDelete";
            VerifyBegin(testname);

            Functions.GridFilterSet(driver, baseURL, "_noopColumnStatuses");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Отменен навсегда");
            WaitForAjax();

            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete all filtered");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all filtered after deleting");

            //close
            Functions.GridFilterClose(driver, baseURL, "_noopColumnStatuses");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 25", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after deleting");
            VerifyAreEqual("25", GetGridCell(0, "Number").Text, "line 1 after deleting");
            VerifyAreEqual("16", GetGridCell(9, "Number").Text, "line 10 after deleting");

            GoToAdmin("orders");
            VerifyAreEqual("Найдено записей: 25", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "deleting filtered items count all after refreshing");

            VerifyFinally(testname);
        }
    }

    [TestFixture]
    public class OrdersFilterNumberTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Orders);
            InitializeService.LoadData(
             "Data\\Admin\\Orders\\OrderGrid\\Catalog.Product.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Offer.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Category.csv",
          "Data\\Admin\\Orders\\OrderGrid\\Catalog.ProductCategories.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Customers.CustomerGroup.csv",
           "data\\Admin\\Orders\\OrderGrid\\Customers.Country.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Region.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.City.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Customer.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Contact.csv",
              "data\\Admin\\Orders\\OrderGrid\\Customers.Managers.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCustomer.csv",
            "Data\\Admin\\Orders\\OrderGrid\\[Order].OrderSource.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderContact.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCurrency.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderItems.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderStatus.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].[Order].csv"
          );

            Init();
            GoToAdmin("orders");
        }
        [Test]
        public void FilterNumber()
        {
            testname = "OrderFilterNumber";
            VerifyBegin(testname);

            Functions.GridFilterSet(driver, baseURL, "_noopColumnNumber");

            //search by not exist 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("6754");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("111111111122222222222222222222222222222");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");

            //search by exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("2");

            VerifyAreEqual("92", GetGridCell(0, "Number").Text, "line 1 number filtered");
            VerifyAreEqual("27", GetGridCell(9, "Number").Text, "line 10 number filtered");
            VerifyAreEqual("Найдено записей: 19", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "number filter count");

            //check go to edit and back 
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".link-invert.ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.Name("orderCustomerForm"));

            VerifyIsTrue(driver.Url.Contains("edit"), "filtered order edit");
            GoBack();

            VerifyAreEqual("Найдено записей: 19", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "number filter return");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"_noopColumnNumber\"]")).Displayed, "filter displayed");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all filtered after deleting");

            //check delete filter
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnNumber");
            VerifyAreEqual("Найдено записей: 80", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "deleting filtered items count all");

            GoToAdmin("orders");
            VerifyAreEqual("Найдено записей: 80", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "deleting filtered items count all after refreshing");

            VerifyFinally(testname);
        }
    }


    [TestFixture]
    public class OrdersFilterSumTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Orders);
            InitializeService.LoadData(
             "Data\\Admin\\Orders\\OrderGrid\\Catalog.Product.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Offer.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Category.csv",
          "Data\\Admin\\Orders\\OrderGrid\\Catalog.ProductCategories.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Customers.CustomerGroup.csv",
           "data\\Admin\\Orders\\OrderGrid\\Customers.Country.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Region.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.City.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Customer.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Contact.csv",
              "data\\Admin\\Orders\\OrderGrid\\Customers.Managers.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCustomer.csv",
            "Data\\Admin\\Orders\\OrderGrid\\[Order].OrderSource.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderContact.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCurrency.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderItems.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderStatus.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].[Order].csv"
          );

            Init();
            GoToAdmin("orders");
        }

        [Test]
        public void FilterSum()
        {
            testname = "OrderFilterSum";
            VerifyBegin(testname);

            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnSum");

            //check min too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("111111111");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter min many symbols");

            //check max too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("111111111");
            VerifyAreEqual("Найдено записей: 99", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter max many symbols");

            //check min and max too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("111111111");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("111111111");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter min/max many symbols");

            //check invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();

            //check min invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("########@@@@@@@@&&&&&&&******");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Text, "filter min imvalid symbols");
            VerifyAreEqual("Найдено записей: 99", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter count min many symbols");

            GoToAdmin("orders");
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnSum");

            //check max invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("########@@@@@@@@&&&&&&&******");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Text, "filter max imvalid symbols");
            VerifyAreEqual("Найдено записей: 99", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter count max many symbols");

            //check min and max invalid symbols

            GoToAdmin("orders");
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnSum");

            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("########@@@@@@@@&&&&&&&******");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("########@@@@@@@@&&&&&&&******");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Text, "filter both min imvalid symbols");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Text, "filter both max imvalid symbols");
            DropFocus("h1");
            VerifyAreEqual("rgb(241, 89, 89)", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).GetCssValue("border-top-color"), "filter sum min many symbols border color");
            VerifyAreEqual("rgb(241, 89, 89)", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).GetCssValue("border-top-color"), "filter sum max many symbols border color");
            VerifyAreEqual("Найдено записей: 99", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter count min/max many symbols");

            GoToAdmin("orders");
            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnSum");

            //check filter min not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("90000");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter min not exist");

            //check max not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("90000");
            VerifyAreEqual("Найдено записей: 99", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter max not exist");

            //check min and max not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("90000");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("90000");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter min/max not exist");

            //check filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("30");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("35");
            VerifyAreEqual("Найдено записей: 6", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter count");

            VerifyAreEqual("33", GetGridCell(0, "Number").Text, "filter number line 1");
            VerifyAreEqual("33", GetGridCell(0, "SumFormatted").Text, "filter number line 1");
            VerifyAreEqual("32", GetGridCell(5, "Number").Text, "filter checkbox line 6");
            VerifyAreEqual("32", GetGridCell(5, "SumFormatted").Text, "filter checkbox line 6");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnSum");
            VerifyAreEqual("Найдено записей: 93", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "deleting filtered items count all");

            GoToAdmin("orders");
            VerifyAreEqual("Найдено записей: 93", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "deleting filtered items count all after refreshing");

            VerifyFinally(testname);
        }
    }

    [TestFixture]
    public class OrdersFilterPaidTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Orders);
            InitializeService.LoadData(
             "Data\\Admin\\Orders\\OrderGrid\\Catalog.Product.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Offer.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Category.csv",
          "Data\\Admin\\Orders\\OrderGrid\\Catalog.ProductCategories.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Customers.CustomerGroup.csv",
           "data\\Admin\\Orders\\OrderGrid\\Customers.Country.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Region.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.City.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Customer.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Contact.csv",
              "data\\Admin\\Orders\\OrderGrid\\Customers.Managers.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCustomer.csv",
            "Data\\Admin\\Orders\\OrderGrid\\[Order].OrderSource.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderContact.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCurrency.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderItems.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderStatus.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].[Order].csv"
          );

            Init();
            GoToAdmin("orders");
        }

        [Test]
        public void FilterPaid()
        {
            testname = "OrderFilterPaid";
            VerifyBegin(testname);

            Functions.GridFilterSet(driver, baseURL, name: "_noopColumnIsPaid");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Да");
            VerifyAreEqual("Найдено записей: 85", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter yes count");
            VerifyAreEqual("96", GetGridCell(0, "Number").Text, "filter number line 1");
            VerifyIsTrue(GetGridCell(0, "IsPaid").FindElement(By.TagName("input")).Selected, "filter checkbox line 1");
            VerifyAreEqual("92", GetGridCell(9, "Number").Text, "filter number line 10");
            VerifyIsTrue(GetGridCell(9, "IsPaid").FindElement(By.TagName("input")).Selected, "filter checkbox line 10");

            //check filter not paid
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Нет");
            VerifyAreEqual("Найдено записей: 14", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter no count");
            VerifyAreEqual("14", GetGridCell(0, "Number").Text, "filter no number line 1");
            VerifyAreEqual("5", GetGridCell(9, "Number").Text, "filter no number line 10");
            VerifyIsFalse(GetGridCell(0, "IsPaid").FindElement(By.CssSelector("input")).Selected, "filter no checkbox line 1");
            VerifyIsFalse(GetGridCell(9, "IsPaid").FindElement(By.CssSelector("input")).Selected, "filter no checkbox line 10");
            
            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnIsPaid");
            VerifyAreEqual("Найдено записей: 99", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "close filter count all");
            
            VerifyFinally(testname);
        }


        [Test]
        public void FilterPaidDelete()
        {
            testname = "OrderFilterPaidDelete";
            VerifyBegin(testname);

            Functions.GridFilterSet(driver, baseURL, "_noopColumnIsPaid");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Да");
            WaitForAjax();

            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete all filtered");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all filtered after deleting");

            //close
            Functions.GridFilterClose(driver, baseURL, "_noopColumnIsPaid");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 14", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after deleting");
            VerifyAreEqual("14", GetGridCell(0, "Number").Text, "line 1 after deleting");
            VerifyAreEqual("5", GetGridCell(9, "Number").Text, "line 10 after deleting");

            GoToAdmin("orders");
            VerifyAreEqual("Найдено записей: 14", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "deleting filtered items count all after refreshing");

            VerifyFinally(testname);
        }
    }

    [TestFixture]
    public class OrdersFilterFIOTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Orders);
            InitializeService.LoadData(
             "Data\\Admin\\Orders\\OrderGrid\\Catalog.Product.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Offer.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Category.csv",
          "Data\\Admin\\Orders\\OrderGrid\\Catalog.ProductCategories.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Customers.CustomerGroup.csv",
           "data\\Admin\\Orders\\OrderGrid\\Customers.Country.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Region.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.City.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Customer.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Contact.csv",
              "data\\Admin\\Orders\\OrderGrid\\Customers.Managers.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCustomer.csv",
            "Data\\Admin\\Orders\\OrderGrid\\[Order].OrderSource.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderContact.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCurrency.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderItems.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderStatus.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].[Order].csv"
          );

            Init();
            GoToAdmin("orders");
        }

        [Test]
        public void FilterFIO()
        {
            testname = "OrderFilterFIO";
            VerifyBegin(testname);

            Functions.GridFilterSet(driver, baseURL, "_noopColumnName");

            //search by not exist 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("FirstName3 LastName5");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("111111111122222222222222222222222222222");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");

            //search by exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("FirstName3");

            VerifyAreEqual("98", GetGridCell(0, "Number").Text, "line 1 number FIO filtered items");
            VerifyAreEqual("33", GetGridCell(9, "Number").Text, "line 10 number FIO filtered items");
            VerifyAreEqual("FirstName3 LastName3", GetGridCell(0, "BuyerName").Text, "line 1 customer FIO filtered items");
            VerifyAreEqual("FirstName3 LastName3", GetGridCell(9, "BuyerName").Text, "line 10 customer FIO filtered items");
            VerifyAreEqual("Найдено записей: 14", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "FIO filter count");
            
            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all filtered after deleting");

            //check delete filter
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnName");
            VerifyAreEqual("Найдено записей: 85", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "deleting filtered items count all");

            GoToAdmin("orders");
            VerifyAreEqual("Найдено записей: 85", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "deleting filtered items count all after refreshing");

            VerifyFinally(testname);
        }
    }

    [TestFixture]
    public class OrdersFilterPhoneTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Orders);
            InitializeService.LoadData(
             "Data\\Admin\\Orders\\OrderGrid\\Catalog.Product.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Offer.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Category.csv",
          "Data\\Admin\\Orders\\OrderGrid\\Catalog.ProductCategories.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Customers.CustomerGroup.csv",
           "data\\Admin\\Orders\\OrderGrid\\Customers.Country.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Region.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.City.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Customer.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Contact.csv",
              "data\\Admin\\Orders\\OrderGrid\\Customers.Managers.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCustomer.csv",
            "Data\\Admin\\Orders\\OrderGrid\\[Order].OrderSource.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderContact.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCurrency.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderItems.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderStatus.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].[Order].csv"
          );

            Init();
            GoToAdmin("orders");
        }

        [Test]
        public void FilterPhone()
        {
            testname = "OrderFilterPhone";
            VerifyBegin(testname);

            Functions.GridFilterSet(driver, baseURL, "_noopColumnPhone");

            //search by not exist 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("+7 495 900 900 99");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("111111111122222222222222222222222222222");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");

            //search by exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("+7 495 800 200 05");

            VerifyAreEqual("95", GetGridCell(0, "Number").Text, "line 1 number phone filtered items");
            VerifyAreEqual("80", GetGridCell(9, "Number").Text, "line 10 number phone filtered items");
            VerifyAreEqual("FirstName5 LastName5", GetGridCell(0, "BuyerName").Text, "line 1 customer phone filtered items");
            VerifyAreEqual("FirstName5 LastName5", GetGridCell(9, "BuyerName").Text, "line 10 customer phone filtered items");
            VerifyAreEqual("Найдено записей: 43", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "phone filter count");
            
            //check close filter
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnPhone");
            VerifyAreEqual("Найдено записей: 99", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "close filter count all");
            
            VerifyFinally(testname);
        }

        [Test]
        public void FilterPhoneDelete()
        {
            testname = "OrderFilterPhoneDelete";
            VerifyBegin(testname);

            GoToAdmin("orders");
            Functions.GridFilterSet(driver, baseURL, "_noopColumnPhone");

            //search by not exist 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("+7 495 800 200 02");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all filtered after deleting");

            //check delete filter
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnPhone");
            VerifyAreEqual("Найдено записей: 85", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "deleting filtered items count all");

            GoToAdmin("orders");
            VerifyAreEqual("Найдено записей: 85", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "deleting filtered items count all after refreshing");

            VerifyFinally(testname);
        }
    }

    [TestFixture]
    public class OrdersFilterEmailTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Orders);
            InitializeService.LoadData(
             "Data\\Admin\\Orders\\OrderGrid\\Catalog.Product.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Offer.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Category.csv",
          "Data\\Admin\\Orders\\OrderGrid\\Catalog.ProductCategories.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Customers.CustomerGroup.csv",
           "data\\Admin\\Orders\\OrderGrid\\Customers.Country.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Region.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.City.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Customer.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Contact.csv",
              "data\\Admin\\Orders\\OrderGrid\\Customers.Managers.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCustomer.csv",
            "Data\\Admin\\Orders\\OrderGrid\\[Order].OrderSource.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderContact.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCurrency.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderItems.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderStatus.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].[Order].csv"
          );

            Init();
            GoToAdmin("orders");
        }

        [Test]
        public void FilterEmail()
        {
            testname = "OrderFilterEmail";
            VerifyBegin(testname);

            Functions.GridFilterSet(driver, baseURL, "_noopColumnEmail");

            //search by not exist 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("test555@mail.ru4");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("111111111122222222222222222222222222222");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");

            //search by exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("test@mail.ru3");

            VerifyAreEqual("98", GetGridCell(0, "Number").Text, "line 1 number email filtered items");
            VerifyAreEqual("33", GetGridCell(9, "Number").Text, "line 10 number email filtered items");
            VerifyAreEqual("FirstName3 LastName3", GetGridCell(0, "BuyerName").Text, "line 1 customer email filtered items");
            VerifyAreEqual("FirstName3 LastName3", GetGridCell(9, "BuyerName").Text, "line 10 customer email filtered items");
            VerifyAreEqual("Найдено записей: 14", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "email filter count");

            //check close filter
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnEmail");
            VerifyAreEqual("Найдено записей: 99", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "close filter count all");

            VerifyFinally(testname);
        }

        [Test]
        public void FilterEmailDelete()
        {
            testname = "OrderFilterEmailDelete";
            VerifyBegin(testname);

            GoToAdmin("orders");
            Functions.GridFilterSet(driver, baseURL, "_noopColumnEmail");

            //search by not exist 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("test@mail.ru4");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all filtered after deleting");

            //check delete filter
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnEmail");
            VerifyAreEqual("Найдено записей: 85", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "deleting filtered items count all");

            GoToAdmin("orders");
            VerifyAreEqual("Найдено записей: 85", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "deleting filtered items count all after refreshing");

            VerifyFinally(testname);
        }
    }

    [TestFixture]
    public class OrdersFilterCityTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Orders);
            InitializeService.LoadData(
             "Data\\Admin\\Orders\\OrderGrid\\Catalog.Product.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Offer.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Category.csv",
          "Data\\Admin\\Orders\\OrderGrid\\Catalog.ProductCategories.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Customers.CustomerGroup.csv",
           "data\\Admin\\Orders\\OrderGrid\\Customers.Country.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Region.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.City.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Customer.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Contact.csv",
              "data\\Admin\\Orders\\OrderGrid\\Customers.Managers.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCustomer.csv",
            "Data\\Admin\\Orders\\OrderGrid\\[Order].OrderSource.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderContact.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCurrency.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderItems.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderStatus.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].[Order].csv"
          );

            Init();
            GoToAdmin("orders");
        }

        [Test]
        public void FilterCity()
        {
            testname = "OrderFilterCity";
            VerifyBegin(testname);

            Functions.GridFilterSet(driver, baseURL, "_noopColumnCity");

            //search by not exist 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("Самара");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("111111111122222222222222222222222222222");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");

            //search by exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("Москва");

            VerifyAreEqual("96", GetGridCell(0, "Number").Text, "line 1 number city filtered items");
            VerifyAreEqual("73", GetGridCell(9, "Number").Text, "line 10 number city filtered items");
            VerifyAreEqual("FirstName1 LastName1", GetGridCell(0, "BuyerName").Text, "line 1 customer city filtered items");
            VerifyAreEqual("FirstName3 LastName3", GetGridCell(9, "BuyerName").Text, "line 10 customer city filtered items");
            VerifyAreEqual("Найдено записей: 42", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "city filter count");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all filtered after deleting");

            //check delete filter
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnCity");
            VerifyAreEqual("Найдено записей: 57", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "deleting filtered items count all");

            GoToAdmin("orders");
            VerifyAreEqual("Найдено записей: 57", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "deleting filtered items count all after refreshing");

            VerifyFinally(testname);
        }
    }

    [TestFixture]
    public class OrdersFilterManagerTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Orders);
            InitializeService.LoadData(
             "Data\\Admin\\Orders\\OrderGrid\\Catalog.Product.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Offer.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Category.csv",
          "Data\\Admin\\Orders\\OrderGrid\\Catalog.ProductCategories.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Customers.CustomerGroup.csv",
           "data\\Admin\\Orders\\OrderGrid\\Customers.Country.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Region.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.City.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Customer.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Contact.csv",
              "data\\Admin\\Orders\\OrderGrid\\Customers.Managers.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCustomer.csv",
            "Data\\Admin\\Orders\\OrderGrid\\[Order].OrderSource.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderContact.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCurrency.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderItems.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderStatus.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].[Order].csv"
          );

            Init();
            GoToAdmin("orders");
        }


        [Test]
        public void FilterManager()
        {
            testname = "OrderFilterManager";
            VerifyBegin(testname);

            Functions.GridFilterSet(driver, baseURL, "_noopColumnManager");

            IWebElement selectElem = driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]"));
            SelectElement select = new SelectElement(selectElem);

            IList<IWebElement> allOptions = select.Options;

            VerifyIsTrue(allOptions.Count == 3, "count all managers in filter"); //2 managers + null select

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Manager Name");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 68", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all manager 1 filtered");
            VerifyAreEqual("83", GetGridCell(0, "Number").Text, "line 1 number manager 1 filtered");
            VerifyAreEqual("79", GetGridCell(9, "Number").Text, "line 10 number manager 1 filtered");
            VerifyAreEqual("Manager Name", GetGridCell(0, "ManagerName").Text, "line 1 manager 1 filtered");
            VerifyAreEqual("Manager Name", GetGridCell(9, "ManagerName").Text, "line 10 manager 1 filtered");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Manager2 Name2");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 28", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all manager 2 filtered"); //exept 3 drafts among all
            VerifyAreEqual("96", GetGridCell(0, "Number").Text, "line 1 number manager 2 filtered");
            VerifyAreEqual("92", GetGridCell(9, "Number").Text, "line 10 number manager 1 filtered");
            VerifyAreEqual("Manager2 Name2", GetGridCell(0, "ManagerName").Text, "line 1 manager 1 filtered");
            VerifyAreEqual("Manager2 Name2", GetGridCell(9, "ManagerName").Text, "line 10 manager 1 filtered");

            //close
            Functions.GridFilterClose(driver, baseURL, "_noopColumnManager");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 99", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");
            VerifyAreEqual("96", GetGridCell(0, "Number").Text, "line 1 filter closed");
            VerifyAreEqual("92", GetGridCell(9, "Number").Text, "line 10 filter closed");

            VerifyFinally(testname);
        }


        [Test]
        public void FilterManagerDelete()
        {
            testname = "OrderFilterManagerDelete";
            VerifyBegin(testname);

            Functions.GridFilterSet(driver, baseURL, "_noopColumnManager");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Manager Name");
            WaitForAjax();

            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete all filtered");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all filtered after deleting");

            //close
            Functions.GridFilterClose(driver, baseURL, "_noopColumnManager");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 31", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after deleting");
            VerifyAreEqual("96", GetGridCell(0, "Number").Text, "line 1 number after deleting");
            VerifyAreEqual("92", GetGridCell(9, "Number").Text, "line 10 number after deleting");
            VerifyAreEqual("Manager2 Name2", GetGridCell(0, "ManagerName").Text, "line 1 manager after deleting");
            VerifyAreEqual("Manager2 Name2", GetGridCell(9, "ManagerName").Text, "line 10 manager after deleting");

            GoToAdmin("orders");
            VerifyAreEqual("Найдено записей: 31", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "deleting filtered items count all after refreshing");

            VerifyFinally(testname);
        }
    }

    [TestFixture]
    public class OrdersFilterShippingTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Orders);
            InitializeService.LoadData(
             "Data\\Admin\\Orders\\OrderGrid\\Catalog.Product.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Offer.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Category.csv",
          "Data\\Admin\\Orders\\OrderGrid\\Catalog.ProductCategories.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Customers.CustomerGroup.csv",
           "data\\Admin\\Orders\\OrderGrid\\Customers.Country.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Region.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.City.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Customer.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Contact.csv",
              "data\\Admin\\Orders\\OrderGrid\\Customers.Managers.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCustomer.csv",
            "Data\\Admin\\Orders\\OrderGrid\\[Order].OrderSource.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderContact.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCurrency.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderItems.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderStatus.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].[Order].csv"
          );

            Init();
            GoToAdmin("orders");
        }

        [Test]
        public void FilterShipping()
        {
            testname = "OrderFilterShipping";
            VerifyBegin(testname);

            Functions.GridFilterSet(driver, baseURL, "_noopColumnShippings");

            //search by not exist 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("Пункт выдачи в постаматах PickPoint");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("111111111122222222222222222222222222222");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");

            //search by exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("Курьером по Москве");

            VerifyAreEqual("96", GetGridCell(0, "Number").Text, "line 1 number shipping filtered items");
            VerifyAreEqual("92", GetGridCell(9, "Number").Text, "line 10 number shipping filtered items");
            VerifyAreEqual("Найдено записей: 81", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "shipping filter count");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all filtered after deleting");

            //check delete filter
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnShippings");
            VerifyAreEqual("Найдено записей: 18", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "deleting filtered items count all");

            GoToAdmin("orders");
            VerifyAreEqual("Найдено записей: 18", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "deleting filtered items count all after refreshing");

            VerifyFinally(testname);
        }
    }

    [TestFixture]
    public class OrdersFilterPaymentTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Orders);
            InitializeService.LoadData(
             "Data\\Admin\\Orders\\OrderGrid\\Catalog.Product.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Offer.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Category.csv",
          "Data\\Admin\\Orders\\OrderGrid\\Catalog.ProductCategories.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Customers.CustomerGroup.csv",
           "data\\Admin\\Orders\\OrderGrid\\Customers.Country.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Region.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.City.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Customer.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Contact.csv",
              "data\\Admin\\Orders\\OrderGrid\\Customers.Managers.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCustomer.csv",
            "Data\\Admin\\Orders\\OrderGrid\\[Order].OrderSource.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderContact.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCurrency.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderItems.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderStatus.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].[Order].csv"
          );

            Init();
            GoToAdmin("orders");
        }

        [Test]
        public void FilterPayment()
        {
            testname = "OrderFilterPayment";
            VerifyBegin(testname);

            Functions.GridFilterSet(driver, baseURL, "_noopColumnPayments");

            IWebElement selectElem = driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]"));
            SelectElement select = new SelectElement(selectElem);

            IList<IWebElement> allOptions = select.Options;

            VerifyIsTrue(allOptions.Count == 10, "count all payment methods in filter"); //9 payment methods + null select

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Подарочный сертификат");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 2", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all certificate payment filtered");
            VerifyAreEqual("16", GetGridCell(0, "Number").Text, "line 1 certificate payment filtered");
            VerifyAreEqual("15", GetGridCell(1, "Number").Text, "line 2 certificate payment filtered");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Наличными курьеру");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 2", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all cash payment filtered");
            VerifyAreEqual("22", GetGridCell(0, "Number").Text, "line 1 cash payment filtered");
            VerifyAreEqual("21", GetGridCell(1, "Number").Text, "line 2 cash payment filtered");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Терминалы оплаты");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 70", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all wallet payment filtered");
            VerifyAreEqual("96", GetGridCell(0, "Number").Text, "line 1 wallet payment filtered");
            VerifyAreEqual("92", GetGridCell(9, "Number").Text, "line 10 wallet payment filtered");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all filtered after deleting");

            //check delete filter
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnPayments");
            VerifyAreEqual("Найдено записей: 29", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "deleting filtered items count all");

            GoToAdmin("orders");
            VerifyAreEqual("Найдено записей: 29", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "deleting filtered items count all after refreshing");

            VerifyFinally(testname);
        }
    }

    [TestFixture]
    public class OrdersFilterOrderSourceTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Orders);
            InitializeService.LoadData(
             "Data\\Admin\\Orders\\OrderGrid\\Catalog.Product.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Offer.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Category.csv",
          "Data\\Admin\\Orders\\OrderGrid\\Catalog.ProductCategories.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Customers.CustomerGroup.csv",
           "data\\Admin\\Orders\\OrderGrid\\Customers.Country.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Region.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.City.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Customer.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Contact.csv",
              "data\\Admin\\Orders\\OrderGrid\\Customers.Managers.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCustomer.csv",
            "Data\\Admin\\Orders\\OrderGrid\\[Order].OrderSource.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderContact.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCurrency.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderItems.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderStatus.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].[Order].csv"
          );

            Init();
            GoToAdmin("orders");
        }

        [Test]
        public void FilterOrderSource()
        {
            testname = "OrderFilterOrderSource";
            VerifyBegin(testname);

            Functions.GridFilterSet(driver, baseURL, "_noopColumnSources");

            IWebElement selectElem = driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]"));
            SelectElement select = new SelectElement(selectElem);

            IList<IWebElement> allOptions = select.Options;

            VerifyIsTrue(allOptions.Count == 13, "count all order sources in filter"); //12 order sources + null select

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("В один клик");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 8", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all one click order source filtered");
            VerifyAreEqual("96", GetGridCell(0, "Number").Text, "line 1 one click order source filtered");
            VerifyAreEqual("97", GetGridCell(1, "Number").Text, "line 2 one click order source filtered");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Нашли дешевле");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 3", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all find cheaper order source filtered");
            VerifyAreEqual("22", GetGridCell(0, "Number").Text, "line 1 find cheaper order source filtered");
            VerifyAreEqual("21", GetGridCell(1, "Number").Text, "line 2 find cheaper order source filtered");
            VerifyAreEqual("20", GetGridCell(2, "Number").Text, "line 3 find cheaper order source filtered");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Обратный звонок");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all callback source filtered");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "callback order source no orders filtered");

            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Корзина интернет магазина");
            WaitForAjax();
            VerifyAreEqual("Найдено записей: 44", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all shopping cart order source filtered");
            VerifyAreEqual("63", GetGridCell(0, "Number").Text, "line 1 shopping cart order source filtered");
            VerifyAreEqual("54", GetGridCell(9, "Number").Text, "line 10 shopping cart order source filtered");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all filtered after deleting");

            //check delete filter
            Functions.GridFilterClose(driver, baseURL, name: "_noopColumnSources");
            VerifyAreEqual("Найдено записей: 55", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "deleting filtered items count all");

            GoToAdmin("orders");
            VerifyAreEqual("Найдено записей: 55", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "deleting filtered items count all after refreshing");

            VerifyFinally(testname);
        }
    }

    [TestFixture]
    public class OrdersFilterDateTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.Customers | ClearType.Orders);
            InitializeService.LoadData(
             "Data\\Admin\\Orders\\OrderGrid\\Catalog.Product.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Offer.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Catalog.Category.csv",
          "Data\\Admin\\Orders\\OrderGrid\\Catalog.ProductCategories.csv",
           "Data\\Admin\\Orders\\OrderGrid\\Customers.CustomerGroup.csv",
           "data\\Admin\\Orders\\OrderGrid\\Customers.Country.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Region.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.City.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Customer.csv",
            "data\\Admin\\Orders\\OrderGrid\\Customers.Contact.csv",
              "data\\Admin\\Orders\\OrderGrid\\Customers.Managers.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCustomer.csv",
            "Data\\Admin\\Orders\\OrderGrid\\[Order].OrderSource.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderContact.csv",
            "data\\Admin\\Orders\\OrderGrid\\[Order].OrderCurrency.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderItems.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].OrderStatus.csv",
             "data\\Admin\\Orders\\OrderGrid\\[Order].[Order].csv"
          );

            Init();
            GoToAdmin("orders");
        }

        [Test]
        public void FilterDate()
        {
            testname = "OrderFilterDate";
            VerifyBegin(testname);

            Functions.GridFilterSet(driver, baseURL, "OrderDateFormatted");

            //check filter min not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("31.12.2050 00:00");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter min not exist");

            //check max not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("31.12.2050 00:00");
            VerifyAreEqual("Найдено записей: 99", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter max not exist");

            //check min and max not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("31.12.2050 00:00");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("31.12.2050 00:00");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter sum min/max not exist");

            //check filter
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("04.08.2016 00:00");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("14.08.2016 23:00");
            VerifyAreEqual("Найдено записей: 11", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filtered items count");

            VerifyAreEqual("11", GetGridCell(0, "Number").Text, "filter order number line 1");
            VerifyAreEqual("14.08.2016 00:00", GetGridCell(0, "OrderDateFormatted").Text, "filter order date line 1");
            VerifyAreEqual("2", GetGridCell(9, "Number").Text, "filter order number line 10");
            VerifyAreEqual("05.08.2016 00:00", GetGridCell(9, "OrderDateFormatted").Text, "filter order date line 10");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "OrderDateFormatted");
            VerifyAreEqual("Найдено записей: 88", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "deleting filtered items count all");

            GoToAdmin("orders");
            VerifyAreEqual("Найдено записей: 88", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "deleting filtered items count all after refreshing");

            VerifyFinally(testname);
        }
    }
}