using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.CustomerFields.Values
{
    [TestFixture]
    public class SettingsCustomerFieldValueSearchTest : BaseSeleniumTest
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
        public void CustomerFieldValueSearchExistName()
        {
            testname = "CustomerFieldValueSearchExistName";
            VerifyBegin(testname);
            
            Refresh();

            GetGridCell(0, "HasValues", "CustomerFields").Click();
            
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Value 111");

            XPathContainsText("h1", "Значения поля \"Customer Field 1\"");
            WaitForAjax();

            VerifyAreEqual("Value 111", GetGridCell(0, "Value", "CustomerFieldValues").Text, "search exist name");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }
        
        [Test]
        public void CustomerFieldValueSearchNotExist()
        {
            testname = "CustomerFieldValueSearchNotExist";
            VerifyBegin(testname);
            
            Refresh();

            GetGridCell(0, "HasValues", "CustomerFields").Click();

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("test value 5000");

            XPathContainsText("h1", "Значения поля \"Customer Field 1\"");
            WaitForAjax();

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search not exist name");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void CustomerFieldValueSearchMuchSymbols()
        {
            testname = "CustomerFieldValueSearchMuchSymbols";
            VerifyBegin(testname);
            
            Refresh();

            GetGridCell(0, "HasValues", "CustomerFields").Click();

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");

            XPathContainsText("h1", "Значения поля \"Customer Field 1\"");
            WaitForAjax();

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search too much symbols");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void CustomerFieldValueSearchInvalidSymbols()
        {
            testname = "CustomerFieldValueSearchInvalidSymbols";
            VerifyBegin(testname);
            
            Refresh();

            GetGridCell(0, "HasValues", "CustomerFields").Click();

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("########@@@@@@@@&&&&&&&******,,,,..");

            XPathContainsText("h1", "Значения поля \"Customer Field 1\"");
            WaitForAjax();

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search invalid symbols");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void CustomerFieldValueFilterName()
        {
            testname = "CustomerFieldValueFilterName";
            VerifyBegin(testname);

            Refresh();

            GetGridCell(0, "HasValues", "CustomerFields").Click();

            //check filter name
            Functions.GridFilterSet(driver, baseURL, name: "Value");

            //search by not exist name
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("123123123 name test 3");
            XPathContainsText("h1", "Значения поля \"Customer Field 1\"");
            WaitForAjax();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no such element");

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            XPathContainsText("h1", "Значения поля \"Customer Field 1\"");
            WaitForAjax();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "too much symbols");

            //search invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            XPathContainsText("h1", "Значения поля \"Customer Field 1\"");
            WaitForAjax();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "invalid symbols");

            //search by exist name
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("Value 2");
            XPathContainsText("h1", "Значения поля \"Customer Field 1\"");
            WaitForAjax();

            VerifyAreEqual("Value 2", GetGridCell(0, "Value", "CustomerFieldValues").Text, "Value");

            VerifyAreEqual("Найдено записей: 11", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            //check delete with filter
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(500);
            Functions.GridDropdownDelete(driver, baseURL);
            
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after deleting");

            //check delete filter
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterClose(driver, baseURL, name: "Value");
            VerifyAreEqual("Найдено записей: 129", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter name deleting 1");

            Refresh();
            GetGridCell(0, "HasValues", "CustomerFields").Click();

            VerifyAreEqual("Найдено записей: 129", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "filter name deleting 2");

            VerifyFinally(testname);
        }
    }
}