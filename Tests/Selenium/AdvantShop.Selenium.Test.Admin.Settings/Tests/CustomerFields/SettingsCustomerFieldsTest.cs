using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.CustomerFields
{
    [TestFixture]
    public class SettingsCustomerFieldsTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
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
        public void CustomerFieldsGridTest()
        {
            testname = "CustomerFieldsGridTest";
            VerifyBegin(testname);
            
            VerifyAreEqual("Customer Field 1", GetGridCell(0, "Name", "CustomerFields").Text, "name");
            VerifyAreEqual("Выбор", GetGridCell(0, "FieldTypeFormatted", "CustomerFields").Text, "type");
            VerifyAreEqual("Список значений", GetGridCell(0, "HasValues", "CustomerFields").Text, "values");
            VerifyIsFalse(GetGridCell(0, "Required", "CustomerFields").FindElement(By.TagName("input")).Selected, "Required");
            VerifyAreEqual("1", GetGridCell(0, "SortOrder", "CustomerFields").FindElement(By.TagName("input")).GetAttribute("value"), "SortOrder");
            VerifyIsFalse(GetGridCell(0, "Enabled", "CustomerFields").FindElement(By.TagName("input")).Selected, "enabled");
            VerifyIsFalse(GetGridCell(0, "ShowInClient", "CustomerFields").FindElement(By.TagName("input")).Selected, "show in client");

            VerifyAreEqual("Найдено записей: 150", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");
            
            VerifyFinally(testname);
        }

        [Test]
        public void CustomerFieldsInplaceEnabledTest()
        {
            testname = "CustomerFieldsInplaceEnabledTest";
            VerifyBegin(testname);

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Customer Field 1");
            XPathContainsText("h1", "Дополнительные поля покупателя");
            WaitForAjax();
            VerifyAreEqual("Customer Field 1", GetGridCell(0, "Name", "CustomerFields").Text, "pre check name");

            GetGridCell(0, "Enabled", "CustomerFields").Click();
            VerifyIsTrue(GetGridCell(0, "Enabled", "CustomerFields").FindElement(By.TagName("input")).Selected, "inplace enabled 1");

            GoToAdmin("settingscustomers#?tab=customerFields");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Customer Field 1");
            XPathContainsText("h1", "Дополнительные поля покупателя");
            WaitForAjax();
            VerifyAreEqual("Customer Field 1", GetGridCell(0, "Name", "CustomerFields").Text, "name");

            VerifyIsTrue(GetGridCell(0, "Enabled", "CustomerFields").FindElement(By.TagName("input")).Selected, "inplace enabled 2");

            VerifyFinally(testname);
        }

        [Test]
        public void CustomerFieldsInplaceShowInClientTest()
        {
            testname = "CustomerFieldsInplaceShowInClientTest";
            VerifyBegin(testname);

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Customer Field 2");
            XPathContainsText("h1", "Дополнительные поля покупателя");
            WaitForAjax();
            VerifyAreEqual("Customer Field 2", GetGridCell(0, "Name", "CustomerFields").Text, "pre check name");

            GetGridCell(0, "ShowInClient", "CustomerFields").Click();
            VerifyIsTrue(GetGridCell(0, "ShowInClient", "CustomerFields").FindElement(By.TagName("input")).Selected, "inplace ShowInClient 1");

            GoToAdmin("settingscustomers#?tab=customerFields");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Customer Field 2");
            XPathContainsText("h1", "Дополнительные поля покупателя");
            WaitForAjax();
            VerifyAreEqual("Customer Field 2", GetGridCell(0, "Name", "CustomerFields").Text, "name");

            VerifyIsTrue(GetGridCell(0, "ShowInClient", "CustomerFields").FindElement(By.TagName("input")).Selected, "inplace ShowInClient 2");

            VerifyFinally(testname);
        }

        [Test]
        public void CustomerFieldsInplaceRequiredTest()
        {
            testname = "CustomerFieldsInplaceRequiredTest";
            VerifyBegin(testname);

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Customer Field 3");
            XPathContainsText("h1", "Дополнительные поля покупателя");
            WaitForAjax();
            VerifyAreEqual("Customer Field 3", GetGridCell(0, "Name", "CustomerFields").Text, "pre check name");

            GetGridCell(0, "Required", "CustomerFields").Click();
            VerifyIsTrue(GetGridCell(0, "Required", "CustomerFields").FindElement(By.TagName("input")).Selected, "inplace Required 1");

            GoToAdmin("settingscustomers#?tab=customerFields");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Customer Field 3");
            XPathContainsText("h1", "Дополнительные поля покупателя");
            WaitForAjax();
            VerifyAreEqual("Customer Field 3", GetGridCell(0, "Name", "CustomerFields").Text, "name");

            VerifyIsTrue(GetGridCell(0, "Required", "CustomerFields").FindElement(By.TagName("input")).Selected, "inplace Required 2");

            VerifyFinally(testname);
        }


        [Test]
        public void CustomerFieldsInplaceDisabledTest()
        {
            testname = "CustomerFieldsInplaceDisabledTest";
            VerifyBegin(testname);

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Customer Field 21");
            XPathContainsText("h1", "Дополнительные поля покупателя");
            WaitForAjax();
            VerifyAreEqual("Customer Field 21", GetGridCell(0, "Name", "CustomerFields").Text, "pre check name");

            VerifyIsTrue(GetGridCell(0, "Enabled", "CustomerFields").FindElement(By.TagName("input")).Selected, "pre check inplace enabled");

            GetGridCell(0, "Enabled", "CustomerFields").Click();
            VerifyIsFalse(GetGridCell(0, "Enabled", "CustomerFields").FindElement(By.TagName("input")).Selected, "inplace enabled 1");

            GoToAdmin("settingscustomers#?tab=customerFields");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Customer Field 21");
            XPathContainsText("h1", "Дополнительные поля покупателя");
            WaitForAjax();
            VerifyAreEqual("Customer Field 21", GetGridCell(0, "Name", "CustomerFields").Text, "name");

            VerifyIsFalse(GetGridCell(0, "Enabled", "CustomerFields").FindElement(By.TagName("input")).Selected, "inplace enabled 2");

            VerifyFinally(testname);
        }

        [Test]
        public void CustomerFieldsInplaceSortTest()
        {
            testname = "CustomerFieldsInplaceSortTest";
            VerifyBegin(testname);

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Customer Field 100");
            XPathContainsText("h1", "Дополнительные поля покупателя");
            WaitForAjax();
            VerifyAreEqual("Customer Field 100", GetGridCell(0, "Name", "CustomerFields").Text, "pre check name");
            VerifyAreEqual("100", GetGridCell(0, "SortOrder", "CustomerFields").FindElement(By.TagName("input")).GetAttribute("value"), "pre check SortOrder");

            GetGridCell(0, "SortOrder", "CustomerFields").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "SortOrder", "CustomerFields").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "SortOrder", "CustomerFields").FindElement(By.TagName("input")).SendKeys("1");
            XPathContainsText("h1", "Дополнительные поля покупателя");

            GoToAdmin("settingscustomers#?tab=customerFields");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Customer Field 100");
            XPathContainsText("h1", "Дополнительные поля покупателя");
            WaitForAjax();
            VerifyAreEqual("Customer Field 100", GetGridCell(0, "Name", "CustomerFields").Text, "name");
            VerifyAreEqual("1", GetGridCell(0, "SortOrder", "CustomerFields").FindElement(By.TagName("input")).GetAttribute("value"), "SortOrder");

            //return default
            GetGridCell(0, "SortOrder", "CustomerFields").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "SortOrder", "CustomerFields").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "SortOrder", "CustomerFields").FindElement(By.TagName("input")).SendKeys("100");
            XPathContainsText("h1", "Дополнительные поля покупателя");

            VerifyFinally(testname);
        }

        [Test]
        public void CustomerFieldzSelectDelete()
        {
            testname = "CustomerFieldsSelectDelete";
            VerifyBegin(testname);

            GoToAdmin("settingscustomers#?tab=customerFields");

            //check delete cancel 
            GetGridCell(0, "_serviceColumn", "CustomerFields").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Customer Field 1", GetGridCell(0, "Name", "CustomerFields").Text, "1 grid canсel delete");

            //check delete
            GetGridCell(0, "_serviceColumn", "CustomerFields").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Customer Field 2", GetGridCell(0, "Name", "CustomerFields").Text, "1 grid delete");

            //check select 
            GetGridCell(0, "selectionRowHeaderCol", "CustomerFields").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol", "CustomerFields").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol", "CustomerFields").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol", "CustomerFields").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 1 grid");
            VerifyIsTrue(GetGridCell(1, "selectionRowHeaderCol", "CustomerFields").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 2 grid");
            VerifyIsTrue(GetGridCell(2, "selectionRowHeaderCol", "CustomerFields").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 3 grid");
            VerifyAreEqual("3", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count selected");

            //check delete selected items
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("Customer Field 5", GetGridCell(0, "Name", "CustomerFields").Text, "selected 1 grid delete");
            VerifyAreEqual("Customer Field 6", GetGridCell(1, "Name", "CustomerFields").Text, "selected 2 grid delete");
            VerifyAreEqual("Customer Field 7", GetGridCell(2, "Name", "CustomerFields").Text, "selected 3 grid delete");

            //check select all on page
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol", "CustomerFields").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 1 grid");
            VerifyIsTrue(GetGridCell(9, "selectionRowHeaderCol", "CustomerFields").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 10 grid");

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("Customer Field 15", GetGridCell(0, "Name", "CustomerFields").Text, "selected all on page 1 grid delete");
            VerifyAreEqual("Customer Field 24", GetGridCell(9, "Name", "CustomerFields").Text, "selected all on page 10 grid delete");

            //check select all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("136", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count all selected after deleting");

            //check deselect all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(!GetGridCell(0, "selectionRowHeaderCol", "CustomerFields").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 1 grid");
            VerifyIsTrue(!GetGridCell(9, "selectionRowHeaderCol", "CustomerFields").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 10 grid");

            //check delete all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);
            
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete all");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after deleting");

            GoToAdmin("settingscustomers#?tab=customerFields");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete all 2");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after deleting 2");

            VerifyFinally(testname);
        }
    }
}