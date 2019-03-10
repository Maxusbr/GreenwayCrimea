using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.CRM.Leads
{
    [TestFixture]
    public class CRMLeadFilterTest : BaseMultiSeleniumTest
    {

        [SetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CRM | ClearType.Customers | ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\CRM\\Lead\\Catalog.Product.csv",
           "data\\Admin\\CRM\\Lead\\Catalog.Offer.csv",
           "data\\Admin\\CRM\\Lead\\Catalog.Category.csv",
           "data\\Admin\\CRM\\Lead\\Catalog.ProductCategories.csv",
         "data\\Admin\\CRM\\Lead\\Customers.Customer.csv",
           "data\\Admin\\CRM\\Lead\\Customers.CustomerGroup.csv",
               "data\\Admin\\CRM\\Lead\\Customers.CustomerField.csv",
               "data\\Admin\\CRM\\Lead\\Customers.CustomerFieldValue.csv",
               "data\\Admin\\CRM\\Lead\\Customers.CustomerFieldValuesMap.csv",
               "data\\Admin\\CRM\\Lead\\Customers.Managers.csv",
                "data\\Admin\\CRM\\Lead\\CRM.DealStatus.csv",
               //    "data\\Admin\\CRM\\Lead\\CRM.BizProcessRule.csv",
               "data\\Admin\\CRM\\Lead\\[Order].OrderSource.csv",
                "data\\Admin\\CRM\\Lead\\[Order].LeadCurrency.csv",
                 "data\\Admin\\CRM\\Lead\\[Order].LeadEvent.csv",
                    "data\\Admin\\CRM\\Lead\\[Order].LeadItem.csv",
                "data\\Admin\\CRM\\Lead\\[Order].Lead.csv",
             "data\\Admin\\CRM\\Lead\\Customers.TaskGroup.csv",
            "data\\Admin\\CRM\\Lead\\Customers.Task.csv"
           );

            Init();
            GoToAdmin("leads");
        }


         

        [Test]
        public void LeadFilterManager()
        {
            testname = "LeadFilterManager";
            VerifyBegin(testname);

            //check filter manager no
            Functions.GridFilterSet(driver, baseURL, name: "ManagerName");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Выберите пункт");
          //  DropFocus("h1");
            VerifyAreEqual("Найдено записей: 120", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter manager no");

            //check filter manager
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("test testov");
           // DropFocus("h1");
            VerifyAreEqual("Найдено записей: 36", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter manager");

            VerifyAreEqual("120", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "lead id manager line 1");
            VerifyAreEqual("test testov", GetGridCell(0, "ManagerName").Text, "ManagerName line 1");
            VerifyAreEqual("47", GetGridCell(9, "Id").FindElement(By.TagName("a")).Text, "lead id manager line 10");
            VerifyAreEqual("test testov", GetGridCell(9, "ManagerName").Text, "ManagerName line 10");

            //check all managers
            IWebElement selectElem = driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]"));
            SelectElement select = new SelectElement(selectElem);

            IList<IWebElement> allOptions = select.Options;

            VerifyIsTrue(allOptions.Count == 3, "count managers");  //2 managers + null option 

           //check go to edit and back
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            VerifyIsTrue(driver.Url.Contains("edit"), "lead edit");

            GoBack();

            Refresh();

            VerifyAreEqual("Найдено записей: 36", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter manager return");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"ManagerName\"]")).Displayed, "filter manager Displayed");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "ManagerName");
            VerifyAreEqual("Найдено записей: 84", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter manager after deleting 1");

            GoToAdmin("leads");
            VerifyAreEqual("Найдено записей: 84", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter manager after deleting 2");

            VerifyFinally(testname);

        }

        [Test]
        public void LeadFilterContact()
        {
            testname = "LeadFilterContact";
            VerifyBegin(testname);

            //check filter Contact
            Functions.GridFilterSet(driver, baseURL, name: "FullName");

            //search by not exist Contact
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("123123123 name contact 3");
            //DropFocus("h1");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            //DropFocus("h1");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");

            //search invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            //DropFocus("h1");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "invalid symbols");

            //search by exist contact
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("Patron1");
            //DropFocus("h1");

            VerifyAreEqual("19", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "lead id fullname line 1");
            VerifyAreEqual("LastName19 FirstName19 Patron19", GetGridCell(0, "FullName").Text, "fullname line 1");
            VerifyAreEqual("10", GetGridCell(9, "Id").FindElement(By.TagName("a")).Text, "lead id fullname line 10");
            VerifyAreEqual("LastName10 FirstName10 Patron10", GetGridCell(9, "FullName").Text, "fullname line 10");
            VerifyAreEqual("Найдено записей: 11", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter FullName");
            
            //check go to edit and back 
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            Thread.Sleep(4000);
            VerifyIsTrue(driver.Url.Contains("edit"), "lead edit");

            GoBack();

            VerifyAreEqual("Найдено записей: 11", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter FullName return");
            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"FullName\"]")).Displayed, "filter FullName Displayed");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "FullName");
            VerifyAreEqual("Найдено записей: 109", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter FullName deleting 1");

            GoToAdmin("leads");
            VerifyAreEqual("Найдено записей: 109", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter FullName deleting 2");

            VerifyFinally(testname);
        }

        [Test]
        public void LeadFilterSum()
        {
            testname = "LeadFilterSum";
            VerifyBegin(testname);

            Functions.GridFilterSet(driver, baseURL, name: "Sum");
            
            //check min too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("1111111111111111111111111111111");
            DropFocus("h1");
            // VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter sum min many symbols");
            VerifyAreEqual("rgb(241, 89, 89)", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).GetCssValue("border-top-color"), "filter sum min many symbols");

            //check max too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("1111111111111111111111111111111");
            DropFocus("h1");
            VerifyAreEqual("Найдено записей: 120", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter sum max many symbols");
            VerifyAreEqual("rgb(241, 89, 89)", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).GetCssValue("border-top-color"), "filter max many symbols border color");

            //check min and max too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("1111111111111111111111111111111");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("1111111111111111111111111111111");
            DropFocus("h1");
            //VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter sum min/max many symbols");
            VerifyAreEqual("rgb(241, 89, 89)", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).GetCssValue("border-top-color"), "filter sum min many symbols");
            VerifyAreEqual("rgb(241, 89, 89)", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).GetCssValue("border-top-color"), "filter sum max many symbols");

            //check invalid symbols
            GoToAdmin("leads");
            Functions.GridFilterSet(driver, baseURL, name: "Sum");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();

            //check min invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("########@@@@@@@@&&&&&&&******");
            //DropFocus("h1");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Text, "filter sum min imvalid symbols");
            VerifyAreEqual("Найдено записей: 120", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter sum count leads min many symbols");

            GoToAdmin("leads");
            Functions.GridFilterSet(driver, baseURL, name: "Sum");

            //check max invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("########@@@@@@@@&&&&&&&******");
            //DropFocus("h1");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Text, "filter sum max imvalid symbols");
            VerifyAreEqual("Найдено записей: 120", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter sum count leads max many symbols");

            //check min and max invalid symbols

            GoToAdmin("leads");
            Functions.GridFilterSet(driver, baseURL, name: "Sum");
            
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("########@@@@@@@@&&&&&&&******");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("########@@@@@@@@&&&&&&&******");
            //DropFocus("h1");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Text, "filter sum both min imvalid symbols");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Text, "filter sum both max imvalid symbols");
            VerifyAreEqual("Найдено записей: 120", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter sum count leads min/max many symbols");

            GoToAdmin("leads");
            Functions.GridFilterSet(driver, baseURL, name: "Sum");

            //check filter min not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("1000");
            //DropFocus("h1");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter sum min not exist");

            //check max not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("1000");
            //DropFocus("h1");
            VerifyAreEqual("Найдено записей: 120", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter sum max not exist");

            //check min and max not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("1000");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("1000");
            //DropFocus("h1");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter sum min/max not exist");

            //check filter sum
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("40");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("99");
            //DropFocus("h1");
            VerifyAreEqual("Найдено записей: 60", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter sum");

            VerifyAreEqual("99", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "lead id sum line 1");
            VerifyAreEqual("99", GetGridCell(0, "Sum").Text, "sum line 1");
            VerifyAreEqual("90", GetGridCell(9, "Id").FindElement(By.TagName("a")).Text, "lead id sum line 10");
            VerifyAreEqual("90", GetGridCell(9, "Sum").Text, "sum line 10");
            
            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "Sum");
            VerifyAreEqual("Найдено записей: 60", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter Sum after deleting 1");

            GoToAdmin("leads");
            VerifyAreEqual("Найдено записей: 60", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter Sum after deleting 2");

            VerifyFinally(testname);
        }

        [Test]
        public void LeadFilterAddDate()
        {
            testname = "LeadFilterAddDate";
            VerifyBegin(testname);

            Functions.GridFilterSet(driver, baseURL, name: "CreatedDateFormatted");

            //check filter min not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("31.12.2050 00:00");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            //DropFocus("h1");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter add date min not exist");

            //check max not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("31.12.2050 00:00");
            //DropFocus("h1");
            VerifyAreEqual("Найдено записей: 120", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter add date max not exist");

            //check min and max not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("31.12.2050 00:00");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("31.12.2050 00:00");
            //DropFocus("h1"); 
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter sum min/max not exist");

            //check filter sum
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterFrom\"]")).SendKeys("01.01.2015 00:00");
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"datetimeFilterTo\"]")).SendKeys("31.12.2015 18:00");
            //DropFocus("h1");
            VerifyAreEqual("Найдено записей: 87", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter add date");

            VerifyAreEqual("87", GetGridCell(0, "Id").FindElement(By.TagName("a")).Text, "lead id add date line 1");
            VerifyIsTrue(GetGridCell(0, "CreatedDateFormatted").Text.Contains("2015"), "add date line 1");
            VerifyAreEqual("78", GetGridCell(9, "Id").FindElement(By.TagName("a")).Text, "lead id add date line 10");
            VerifyIsTrue(GetGridCell(9, "CreatedDateFormatted").Text.Contains("2015"), "add date line 10");
            
            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "CreatedDateFormatted");
            VerifyAreEqual("Найдено записей: 33", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter add date after deleting 1");

            GoToAdmin("leads");
            VerifyAreEqual("Найдено записей: 33", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter add date after deleting 2");

            VerifyFinally(testname);
        }
    }
}