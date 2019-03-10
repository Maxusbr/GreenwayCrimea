using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Settings.CustomerFields
{
    [TestFixture]
    public class SettingsCustomerFieldsFilterTest : BaseMultiSeleniumTest
    {
        [SetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
            "data\\Admin\\Settings\\CustomerFields\\Customers.Customer.csv",
           "data\\Admin\\Settings\\CustomerFields\\Customers.CustomerGroup.csv",
               "data\\Admin\\Settings\\CustomerFields\\Customers.CustomerField.csv",
               "data\\Admin\\Settings\\CustomerFields\\Customers.CustomerFieldValue.csv"

           );

            Init();

            GoToAdmin("settingscustomers#?tab=customerFields");
        }

         
        
        [Test]
        public void FilterName()
        {
            testname = "SettingsCustomerFieldsFilterName";
            VerifyBegin(testname);

            //check filter full name
            Functions.GridFilterSet(driver, baseURL, name: "Name");

            //search by not exist name
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("123123123 field test 3");
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

            //search by exist name
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("Customer Field 2");
            Blur();
            Thread.Sleep(1000);

            VerifyAreEqual("Customer Field 2", GetGridCell(0, "Name", "CustomerFields").Text, "Name");

            VerifyAreEqual("Найдено записей: 11", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter name");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(500);
            Functions.GridDropdownDelete(driver, baseURL);
            
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter name after deleting");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "Name");
            VerifyAreEqual("Найдено записей: 139", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter name deleting 1");

            Refresh();

            VerifyAreEqual("Найдено записей: 139", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter name deleting 2");

            VerifyFinally(testname);
            
        }

        [Test]
        public void FilterEnabled()
        {
            testname = "SettingsCustomerFieldsFilterEnabled";
            VerifyBegin(testname);

            //check filter enabled
            Functions.GridFilterSet(driver, baseURL, name: "Enabled");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Активные");
            VerifyAreEqual("Найдено записей: 130", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter enabled");

            VerifyAreEqual("Customer Field 21", GetGridCell(0, "Name", "CustomerFields").Text, "filter enabled 1");
            VerifyAreEqual("Customer Field 30", GetGridCell(9, "Name", "CustomerFields").Text, "filter enabled 10");
            VerifyIsTrue(GetGridCell(0, "Enabled", "CustomerFields").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected, "filter enabled select 1");
            VerifyIsTrue(GetGridCell(9, "Enabled", "CustomerFields").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected, "filter enabled select 10");

            //check filter disabled
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Неактивные");
            VerifyAreEqual("Найдено записей: 20", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter disabled");

            VerifyAreEqual("Customer Field 1", GetGridCell(0, "Name", "CustomerFields").Text, "filter disabled 1");
            VerifyAreEqual("Customer Field 10", GetGridCell(9, "Name", "CustomerFields").Text, "filter disabled 10");
            VerifyIsFalse(GetGridCell(0, "Enabled", "CustomerFields").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected, "filter disabled select 1");
            VerifyIsFalse(GetGridCell(9, "Enabled", "CustomerFields").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected, "filter disabled select 10");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(500);
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "Enabled");
            VerifyAreEqual("Найдено записей: 130", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter enabled after deleting 1");

            Refresh();

            VerifyAreEqual("Найдено записей: 130", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter enabled after deleting 2");

            VerifyFinally(testname);

            
        }

        [Test]
        public void FilterRequired()
        {
            testname = "SettingsCustomerFieldsFilterRequired";
            VerifyBegin(testname);

            //check filter Required
            Functions.GridFilterSet(driver, baseURL, name: "Required");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Да");
            VerifyAreEqual("Найдено записей: 50", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter Required");

            VerifyAreEqual("Customer Field 101", GetGridCell(0, "Name", "CustomerFields").Text, "filter Required 1");
            VerifyAreEqual("Customer Field 110", GetGridCell(9, "Name", "CustomerFields").Text, "filter Required 10");
            VerifyIsTrue(GetGridCell(0, "Required", "CustomerFields").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected, "filter Required select 1");
            VerifyIsTrue(GetGridCell(9, "Required", "CustomerFields").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected, "filter Required select 10");

            //check filter no Required
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Нет");
            VerifyAreEqual("Найдено записей: 100", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter no Required");

            VerifyAreEqual("Customer Field 1", GetGridCell(0, "Name", "CustomerFields").Text, "filter no Required 1");
            VerifyAreEqual("Customer Field 10", GetGridCell(9, "Name", "CustomerFields").Text, "filter no Required 10");
            VerifyIsFalse(GetGridCell(0, "Required", "CustomerFields").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected, "filter no Required select 1");
            VerifyIsFalse(GetGridCell(9, "Required", "CustomerFields").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected, "filter no Required select 10");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(500);
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "Required");
            VerifyAreEqual("Найдено записей: 50", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter Required after deleting 1");

            Refresh();

            VerifyAreEqual("Найдено записей: 50", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter Required after deleting 2");

            VerifyFinally(testname);

            
        }
        [Test]
        public void FilterType()
        {
            testname = "SettingsCustomerFieldsFilterType";
            VerifyBegin(testname);

            //check filter enabled
            Functions.GridFilterSet(driver, baseURL, name: "FieldTypeFormatted");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Выбор");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter Type1");

            VerifyAreEqual("Customer Field 1", GetGridCell(0, "Name", "CustomerFields").Text, "filter Type1 line 1");
            VerifyAreEqual("Выбор", GetGridCell(0, "FieldTypeFormatted", "CustomerFields").Text, "filter Type2 select");

            //check filter disabled
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Текстовое поле");
            VerifyAreEqual("Найдено записей: 17", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter Type2");

            VerifyAreEqual("Customer Field 2", GetGridCell(0, "Name", "CustomerFields").Text, "filter Type2 line 1");
            VerifyAreEqual("Текстовое поле", GetGridCell(0, "FieldTypeFormatted", "CustomerFields").Text, "filter Type2 select");

            //check filter type 3
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Числовое поле");
            VerifyAreEqual("Найдено записей: 100", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter Type3");

            VerifyAreEqual("Customer Field 3", GetGridCell(0, "Name", "CustomerFields").Text, "filter Type3 line 1");
            VerifyAreEqual("Числовое поле", GetGridCell(0, "FieldTypeFormatted", "CustomerFields").Text, "filter Type3 select");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(500);
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "FieldTypeFormatted");
            VerifyAreEqual("Найдено записей: 50", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter enabled after deleting 1");

            Refresh();

            VerifyAreEqual("Найдено записей: 50", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter enabled after deleting 2");

            VerifyFinally(testname);
        }

        [Test]
        public void FilterShowInClient()
        {
            testname = "SettingsCustomerFieldsFilterShowInClient";
            VerifyBegin(testname);

            //check filter ShowInClient
            Functions.GridFilterSet(driver, baseURL, name: "ShowInClient");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Нет");
            VerifyAreEqual("Найдено записей: 39", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter ShowInClient");

            VerifyAreEqual("Customer Field 1", GetGridCell(0, "Name", "CustomerFields").Text, "filter ShowInClient 1");
            VerifyAreEqual("Customer Field 10", GetGridCell(9, "Name", "CustomerFields").Text, "filter ShowInClient 10");
            VerifyIsFalse(GetGridCell(0, "ShowInClient", "CustomerFields").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected, "filter ShowInClient select 1");
            VerifyIsFalse(GetGridCell(9, "ShowInClient", "CustomerFields").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected, "filter ShowInClient select 10");

            //check filter no ShowInClient
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Да");
            VerifyAreEqual("Найдено записей: 111", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter no ShowInClient");

            VerifyAreEqual("Customer Field 40", GetGridCell(0, "Name", "CustomerFields").Text, "filter no ShowInClient 1");
            VerifyAreEqual("Customer Field 49", GetGridCell(9, "Name", "CustomerFields").Text, "filter no ShowInClient 10");
            VerifyIsTrue(GetGridCell(0, "ShowInClient", "CustomerFields").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected, "filter no ShowInClient select 1");
            VerifyIsTrue(GetGridCell(9, "ShowInClient", "CustomerFields").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected, "filter no ShowInClient select 10");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(500);
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete filtered items");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "ShowInClient");
            VerifyAreEqual("Найдено записей: 39", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter ShowInClient after deleting 1");

            Refresh();

            VerifyAreEqual("Найдено записей: 39", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter ShowInClient after deleting 2");

            VerifyFinally(testname);


        }
    }
}