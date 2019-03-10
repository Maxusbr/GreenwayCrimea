using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using System.Collections.ObjectModel;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.SettingsSearch
{
    [TestFixture]
    public class SettingsSearchFilterTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.SettingsSearch);
            InitializeService.LoadData(
           "data\\Admin\\SettingsSearch\\Settings.SettingsSearch.csv"

           );

            Init();

            GoToAdmin("settingssearch");
        }

         

        [Test]
        public void SettingsSearchFilterTitle()
        {
            testname = "SettingsSearchFilterTitle";
            VerifyBegin(testname);

            //search by exist name 
            Functions.GridFilterSet(driver, baseURL, name: "Title");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("test title 2");
            DropFocus("h1");
            WaitForAjax();
            Refresh();
            VerifyAreEqual("test title 2", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "filtered items");
            VerifyAreEqual("Найдено записей: 11", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all with filter");
            
            //search by not exist name 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("123123123 title 3");
            DropFocus("h1");
            WaitForAjax();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter not exist settings");
            
            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            DropFocus("h1");
            WaitForAjax();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter settings too much symbols");
            
            //search invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            DropFocus("h1");
            WaitForAjax();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter  invalid symbols");

            //check delete filter
            Functions.GridFilterClose(driver, baseURL, name: "Title");
            VerifyAreEqual("Найдено записей: 150", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after closing filter");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsSearchzFilterTitleDelete()
        {
            testname = "SettingsSearchFilterTitleDelete";
            VerifyBegin(testname);

            Functions.GridFilterSet(driver, baseURL, name: "Title");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("test title 2");
            DropFocus("h1");
            WaitForAjax();
            Refresh();
            VerifyAreEqual("test title 2", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "filtered items");
            VerifyAreEqual("Найдено записей: 11", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all with filter");

            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(500);
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "filter delete items");

            //check delete filter
            Functions.GridFilterClose(driver, baseURL, name: "Title");
            Refresh();
            VerifyAreEqual("Найдено записей: 139", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after closing filter");

            GoToAdmin("settingssearch");
            VerifyAreEqual("Найдено записей: 139", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count 2 all after closing filter");

            VerifyFinally(testname);
        }
        
    }
}