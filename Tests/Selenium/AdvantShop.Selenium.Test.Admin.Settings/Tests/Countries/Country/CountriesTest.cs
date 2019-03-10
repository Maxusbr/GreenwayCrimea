using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.SystemSettings.Countries.Country
{
    [TestFixture]
    public class CountriesTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
             "data\\Admin\\Settings\\Countries\\Customers.Country.csv",
               "data\\Admin\\Settings\\Countries\\Customers.Region.csv",
              "data\\Admin\\Settings\\Countries\\Customers.City.csv",
            "data\\Admin\\Settings\\Countries\\Customers.Customer.csv",
           "data\\Admin\\Settings\\Countries\\Customers.CustomerGroup.csv"

           );

            Init();
        }

        [Test]
        public void CountriesGrid()
        {
            testname = "CountriesGrid";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");

            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "name");
            VerifyAreEqual("AA", GetGridCell(0, "Iso2", "Country").FindElement(By.TagName("input")).GetAttribute("value"), "Iso2");
            VerifyAreEqual("AA1", GetGridCell(0, "Iso3", "Country").FindElement(By.TagName("input")).GetAttribute("value"), "Iso3");
            VerifyAreEqual("111", GetGridCell(0, "DialCode", "Country").FindElement(By.TagName("input")).GetAttribute("value"), "DialCode");
            VerifyAreEqual("1", GetGridCell(0, "SortOrder", "Country").FindElement(By.TagName("input")).GetAttribute("value"), "SortOrder");
            VerifyIsFalse(GetGridCell(0, "DisplayInPopup", "Country").FindElement(By.TagName("input")).Selected, "DisplayInPopup");

            VerifyAreEqual("Найдено записей: 101", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");
            
            VerifyFinally(testname);
        }

        [Test]
        public void CountriesInplaceDisplay()
        {
            testname = "CountriesInplaceDisplay";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "name");
            VerifyIsFalse(GetGridCell(0, "DisplayInPopup", "Country").FindElement(By.TagName("input")).Selected, "DisplayInPopup");

            GetGridCell(0, "DisplayInPopup", "Country").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(GetGridCell(0, "DisplayInPopup", "Country").FindElement(By.TagName("input")).Selected, "inplace DisplayInPopup 1");

            GoToAdmin("settingssystem#?systemTab=countries");

            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "name");
            VerifyIsTrue(GetGridCell(0, "DisplayInPopup", "Country").FindElement(By.TagName("input")).Selected, "inplace DisplayInPopup 2");

            VerifyFinally(testname);
        }
        [Test]
        public void CountriesInplaceIso()
        {
            testname = "CountriesInplaceIso";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");

            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "name");
            VerifyAreEqual("AA", GetGridCell(0, "Iso2", "Country").FindElement(By.TagName("input")).GetAttribute("value"), "Iso2");
            VerifyAreEqual("AA1", GetGridCell(0, "Iso3", "Country").FindElement(By.TagName("input")).GetAttribute("value"), "Iso3");

            GetGridCell(0, "Iso2", "Country").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "Iso2", "Country").FindElement(By.TagName("input")).SendKeys("ZZ");

            GetGridCell(0, "Iso3", "Country").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "Iso3", "Country").FindElement(By.TagName("input")).SendKeys("ZZZ");
            Thread.Sleep(2000);
            XPathContainsText("h1", "Список стран");
            VerifyAreEqual("ZZ", GetGridCell(0, "Iso2", "Country").FindElement(By.TagName("input")).GetAttribute("value"), "inplace Iso2 1");
            VerifyAreEqual("ZZZ", GetGridCell(0, "Iso3", "Country").FindElement(By.TagName("input")).GetAttribute("value"), "inplace Iso3 1");

            GoToAdmin("settingssystem#?systemTab=countries");
            
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "name");
            VerifyAreEqual("ZZ", GetGridCell(0, "Iso2", "Country").FindElement(By.TagName("input")).GetAttribute("value"), "inplace Iso2 2");
            VerifyAreEqual("ZZZ", GetGridCell(0, "Iso3", "Country").FindElement(By.TagName("input")).GetAttribute("value"), "inplace Iso3 2");

           
            VerifyFinally(testname);
        }
        [Test]
        public void CountriesInplaceSortCod()
        {
            testname = "CountriesInplaSortCod";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestCountry100");
            WaitForAjax();
            XPathContainsText("h1", "Список стран");
            WaitForAjax();
            VerifyAreEqual("TestCountry100", GetGridCell(0, "Name", "Country").Text, "name");
            VerifyAreEqual("210", GetGridCell(0, "DialCode", "Country").FindElement(By.TagName("input")).GetAttribute("value"), "DialCode");
            VerifyAreEqual("100", GetGridCell(0, "SortOrder", "Country").FindElement(By.TagName("input")).GetAttribute("value"), "SortOrder");

            GetGridCell(0, "DialCode", "Country").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "DialCode", "Country").FindElement(By.TagName("input")).SendKeys("999");

            GetGridCell(0, "SortOrder", "Country").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "SortOrder", "Country").FindElement(By.TagName("input")).SendKeys("1000");
            Thread.Sleep(2000);
            XPathContainsText("h1", "Список стран");
            VerifyAreEqual("999", GetGridCell(0, "DialCode", "Country").FindElement(By.TagName("input")).GetAttribute("value"), "inplace DialCode1");
            VerifyAreEqual("1000", GetGridCell(0, "SortOrder", "Country").FindElement(By.TagName("input")).GetAttribute("value"), "inplace SortOrder1");

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestCountry100");
            XPathContainsText("h1", "Список стран");
            WaitForAjax();
            VerifyAreEqual("TestCountry100", GetGridCell(0, "Name", "Country").Text, "name");
            VerifyAreEqual("999", GetGridCell(0, "DialCode", "Country").FindElement(By.TagName("input")).GetAttribute("value"), "inplace DialCode2");
            VerifyAreEqual("1000", GetGridCell(0, "SortOrder", "Country").FindElement(By.TagName("input")).GetAttribute("value"), "inplace SortOrder2");

            VerifyFinally(testname);
        }

        [Test]
        public void CountriesRedirect()
        {
            testname = "CountriesRedirect";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);

            VerifyAreEqual("TestCountry1 - Список регионов", driver.FindElement(By.CssSelector("[data-e2e=\"h1-region\"]")).Text, "h1 region");
            VerifyAreEqual("TestRegion1", GetGridCell(0, "Name", "Region").Text, "name region");
            VerifyAreEqual("11", GetGridCell(0, "RegionCode", "Region").FindElement(By.TagName("input")).GetAttribute("value"), "cod region");
            VerifyAreEqual("1", GetGridCell(0, "SortOrder", "Region").FindElement(By.TagName("input")).GetAttribute("value"), "sort region");
          
            VerifyAreEqual("Найдено записей: 29", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all region");

            driver.FindElement(By.CssSelector("[data-e2e=\"GoToCountry\"]")).Click();
            Thread.Sleep(1000);

            VerifyAreEqual("Список стран", driver.FindElement(By.CssSelector("[data-e2e=\"h1-country\"]")).Text, "h1 country");
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "name country");

            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"GoToCity\"]")).Click();
            Thread.Sleep(1000);

            VerifyAreEqual("TestCountry1 - Список городов", driver.FindElement(By.CssSelector("[data-e2e=\"h1-city\"]")).Text, "h1 city");
            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "name city");

            VerifyAreEqual("Найдено записей: 99", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all city");

            driver.FindElement(By.CssSelector("[data-e2e=\"GoToRegion\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("TestCountry1 - Список регионов", driver.FindElement(By.CssSelector("[data-e2e=\"h1-region\"]")).Text, "h1 region 2");

            VerifyFinally(testname);
        }

        [Test]
        public void CountrieszSelectDelete()
        {
            testname = "CountrieszSelectDelete";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");

            //check delete cancel 
            GetGridCell(0, "_serviceColumn", "Country").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "1 grid canсel delete");

            //check delete
            GetGridCell(0, "_serviceColumn", "Country").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCountry2", GetGridCell(0, "Name", "Country").Text, "1 grid delete");

            //check select 
            GetGridCell(0, "selectionRowHeaderCol", "Country").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol", "Country").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol", "Country").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol", "Country").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 1 grid");
            VerifyIsTrue(GetGridCell(1, "selectionRowHeaderCol", "Country").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 2 grid");
            VerifyIsTrue(GetGridCell(2, "selectionRowHeaderCol", "Country").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 3 grid");
            VerifyAreEqual("3", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count selected");

            //check delete selected items
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("TestCountry5", GetGridCell(0, "Name", "Country").Text, "selected 1 grid delete");
            VerifyAreEqual("TestCountry6", GetGridCell(1, "Name", "Country").Text, "selected 2 grid delete");
            VerifyAreEqual("TestCountry7", GetGridCell(2, "Name", "Country").Text, "selected 3 grid delete");

            //check select all on page
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol", "Country").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 1 grid");
            VerifyIsTrue(GetGridCell(9, "selectionRowHeaderCol", "Country").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 10 grid");

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("TestCountry15", GetGridCell(0, "Name", "Country").Text, "selected all on page 1 grid delete");
            VerifyAreEqual("TestCountry24", GetGridCell(9, "Name", "Country").Text, "selected all on page 10 grid delete");

            //check select all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("87", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count all selected after deleting");

            //check deselect all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(!GetGridCell(0, "selectionRowHeaderCol", "Country").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 1 grid");
            VerifyIsTrue(!GetGridCell(9, "selectionRowHeaderCol", "Country").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 10 grid");

            //check delete all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete all");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after deleting");

            GoToAdmin("settingssystem#?systemTab=countries");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete all 2");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after deleting 2");

            VerifyFinally(testname);
        }
    }
}
