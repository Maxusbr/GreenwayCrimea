using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.CustomerFields.Values
{
    [TestFixture]
    public class SettingsCustomerFieldValueTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
            "data\\Admin\\Settings\\CustomerFieldValues\\Customers.Customer.csv",
           "data\\Admin\\Settings\\CustomerFieldValues\\Customers.CustomerGroup.csv",
               "data\\Admin\\Settings\\CustomerFieldValues\\Customers.CustomerField.csv",
               "data\\Admin\\Settings\\CustomerFieldValues\\Customers.CustomerFieldValue.csv"

           );

            Init();

            GoToAdmin("settingscustomers#?tab=customerFields");
        }

         


        [Test]
        public void CustomerFieldValuesGridTest()
        {
            testname = "CustomerFieldValuesGridTest";
            VerifyBegin(testname);
            
            VerifyIsTrue(GetGridCell(0, "Name", "CustomerFields").FindElements(By.TagName("span")).Count > 0, "Customer Fields grid");
            VerifyIsFalse(driver.FindElements(By.XPath("//span[contains(text(), 'Value 1')]")).Count > 0, "no Customer Field Values grid");

            GetGridCell(0, "HasValues", "CustomerFields").Click();

            VerifyIsTrue(driver.PageSource.Contains("Значения поля \"Customer Field 1\""), "h1 values page");

            VerifyIsFalse(driver.FindElements(By.XPath("//span[contains(text(), 'Customer Field 1')]")).Count > 0, "no Customer Fields grid");
            VerifyIsTrue(GetGridCell(0, "Value", "CustomerFieldValues").FindElements(By.TagName("span")).Count > 0, "Customer Field Values grid");

            VerifyAreEqual("Value 1", GetGridCell(0, "Value", "CustomerFieldValues").Text, "Value");
            VerifyAreEqual("1", GetGridCell(0, "SortOrder", "CustomerFieldValues").FindElement(By.TagName("input")).GetAttribute("value"), "SortOrder");

            VerifyAreEqual("Найдено записей: 140", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            driver.FindElement(By.LinkText("Вернуться к списку полей")).Click();

            VerifyIsTrue(GetGridCell(0, "Name", "CustomerFields").FindElements(By.TagName("span")).Count > 0, "Customer Fields grid back");
            VerifyIsFalse(driver.FindElements(By.XPath("//span[contains(text(), 'Value 1')]")).Count > 0, "no Customer Field Values grid back");
            
            VerifyFinally(testname);
        }

        [Test]
        public void CustomerFieldValuesInplaceTest()
        {
            testname = "CustomerFieldValuesInplaceTest";
            VerifyBegin(testname);

            GetGridCell(0, "HasValues", "CustomerFields").Click();

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Value 50");
            XPathContainsText("h1", "Значения поля \"Customer Field 1\"");
            
            VerifyAreEqual("50", GetGridCell(0, "SortOrder", "CustomerFieldValues").FindElement(By.TagName("input")).GetAttribute("value"), "before edit");

            GetGridCell(0, "SortOrder", "CustomerFieldValues").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "SortOrder", "CustomerFieldValues").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "SortOrder", "CustomerFieldValues").FindElement(By.TagName("input")).SendKeys("200");

            XPathContainsText("h1", "Значения поля \"Customer Field 1\"");

            Refresh();

            GetGridCell(0, "HasValues", "CustomerFields").Click();

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Value 50");

            XPathContainsText("h1", "Значения поля \"Customer Field 1\"");

            VerifyAreEqual("200", GetGridCell(0, "SortOrder", "CustomerFieldValues").FindElement(By.TagName("input")).GetAttribute("value"), "edited inplace");

            //back
            GetGridCell(0, "SortOrder", "CustomerFieldValues").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "SortOrder", "CustomerFieldValues").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "SortOrder", "CustomerFieldValues").FindElement(By.TagName("input")).SendKeys("50");

            XPathContainsText("h1", "Значения поля \"Customer Field 1\"");
            
            VerifyFinally(testname);
        }

        [Test]
        public void CustomerFieldValuezSelectDelete()
        {
            testname = "CustomerFieldValuesSelectDelete";
            VerifyBegin(testname);

            Refresh();
            GetGridCell(0, "HasValues", "CustomerFields").Click();
            Thread.Sleep(3000);

            VerifyAreEqual("Value 1", GetGridCell(0, "Value", "CustomerFieldValues").Text, "open Values page");

            //check delete cancel 
            GetGridCell(0, "_serviceColumn", "CustomerFieldValues").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Value 1", GetGridCell(0, "Value", "CustomerFieldValues").Text, "1 grid canсel delete");

            //check delete
            GetGridCell(0, "_serviceColumn", "CustomerFieldValues").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Value 2", GetGridCell(0, "Value", "CustomerFieldValues").Text, "1 grid delete");

            //check select 
            GetGridCell(0, "selectionRowHeaderCol", "CustomerFieldValues").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol", "CustomerFieldValues").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol", "CustomerFieldValues").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol", "CustomerFieldValues").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 1 grid");
            VerifyIsTrue(GetGridCell(1, "selectionRowHeaderCol", "CustomerFieldValues").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 2 grid");
            VerifyIsTrue(GetGridCell(2, "selectionRowHeaderCol", "CustomerFieldValues").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 3 grid");
            VerifyAreEqual("3", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count selected");

            //check delete selected items
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("Value 5", GetGridCell(0, "Value", "CustomerFieldValues").Text, "selected 1 grid delete");
            VerifyAreEqual("Value 6", GetGridCell(1, "Value", "CustomerFieldValues").Text, "selected 2 grid delete");
            VerifyAreEqual("Value 7", GetGridCell(2, "Value", "CustomerFieldValues").Text, "selected 3 grid delete");

            //check select all on page
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol", "CustomerFieldValues").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 1 grid");
            VerifyIsTrue(GetGridCell(9, "selectionRowHeaderCol", "CustomerFieldValues").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 10 grid");

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("Value 15", GetGridCell(0, "Value", "CustomerFieldValues").Text, "selected all on page 1 grid delete");
            VerifyAreEqual("Value 24", GetGridCell(9, "Value", "CustomerFieldValues").Text, "selected all on page 10 grid delete");

            //check select all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("126", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count all selected after deleting");

            //check deselect all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(!GetGridCell(0, "selectionRowHeaderCol", "CustomerFieldValues").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 1 grid");
            VerifyIsTrue(!GetGridCell(9, "selectionRowHeaderCol", "CustomerFieldValues").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 10 grid");

            //check delete all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete all");
            Refresh();
            GetGridCell(0, "HasValues", "CustomerFields").Click();
            Thread.Sleep(3000);
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after deleting 1");

            GoToAdmin("settingscustomers#?tab=customerFields");
            GetGridCell(0, "HasValues", "CustomerFields").Click();
            Thread.Sleep(3000);
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete all 2");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after deleting 2");

            VerifyFinally(testname);
        }
    }
}