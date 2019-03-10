using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.SystemSettings.Countries.Region
{
    [TestFixture]
    public class SettingsRegionTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
             "data\\Admin\\Settings\\Countries\\Region\\Customers.Country.csv",
               "data\\Admin\\Settings\\Countries\\Region\\Customers.Region.csv",
              "data\\Admin\\Settings\\Countries\\Region\\Customers.City.csv",
            "data\\Admin\\Settings\\Countries\\Region\\Customers.Customer.csv",
           "data\\Admin\\Settings\\Countries\\Region\\Customers.CustomerGroup.csv"

           );

            Init();
        }

        [Test]
        public void RegionGrid()
        {
            testname = "RegionGrid";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);

            VerifyAreEqual("TestCountry1 - Список регионов", driver.FindElement(By.CssSelector("[data-e2e=\"h1-region\"]")).Text, "h1 region");
            VerifyAreEqual("TestRegion1", GetGridCell(0, "Name", "Region").Text, "name region");
            VerifyAreEqual("11", GetGridCell(0, "RegionCode", "Region").FindElement(By.TagName("input")).GetAttribute("value"), "cod region");
            VerifyAreEqual("1", GetGridCell(0, "SortOrder", "Region").FindElement(By.TagName("input")).GetAttribute("value"), "sort region");

            VerifyAreEqual("Найдено записей: 101", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all region");

            VerifyFinally(testname);
        }
        
        [Test]
        public void RegionInplaceSortCode()
        {
            testname = "RegionInplaSortCode";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestRegion100");
            WaitForAjax();
            XPathContainsText("h1", "Список регионов");
            WaitForAjax();
            VerifyAreEqual("TestRegion100", GetGridCell(0, "Name", "Region").Text, "name");
            VerifyAreEqual("110", GetGridCell(0, "RegionCode", "Region").FindElement(By.TagName("input")).GetAttribute("value"), "DialCode");
            VerifyAreEqual("100", GetGridCell(0, "SortOrder", "Region").FindElement(By.TagName("input")).GetAttribute("value"), "SortOrder");

            GetGridCell(0, "RegionCode", "Region").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "RegionCode", "Region").FindElement(By.TagName("input")).SendKeys("999");

            GetGridCell(0, "SortOrder", "Region").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "SortOrder", "Region").FindElement(By.TagName("input")).SendKeys("1000");
            Thread.Sleep(2000);
            XPathContainsText("h1", "Список регионов");
            VerifyAreEqual("999", GetGridCell(0, "RegionCode", "Region").FindElement(By.TagName("input")).GetAttribute("value"), "inplace DialCode1");
            VerifyAreEqual("1000", GetGridCell(0, "SortOrder", "Region").FindElement(By.TagName("input")).GetAttribute("value"), "inplace SortOrder1");

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestRegion100");
            XPathContainsText("h1", "Список регионов");
            WaitForAjax();
            VerifyAreEqual("TestRegion100", GetGridCell(0, "Name", "Region").Text, "name");
            VerifyAreEqual("999", GetGridCell(0, "RegionCode", "Region").FindElement(By.TagName("input")).GetAttribute("value"), "inplace DialCode2");
            VerifyAreEqual("1000", GetGridCell(0, "SortOrder", "Region").FindElement(By.TagName("input")).GetAttribute("value"), "inplace SortOrder2");

            VerifyFinally(testname);
        }

        [Test]
        public void RegionRedirect()
        {
            testname = "RegionRedirect";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(1, "Name", "Country").Click();
            Thread.Sleep(2000);

            VerifyAreEqual("TestCountry2 - Список регионов", driver.FindElement(By.CssSelector("[data-e2e=\"h1-region\"]")).Text, "h1 region");
            VerifyAreEqual("TestRegion102", GetGridCell(0, "Name", "Region").Text, "name region");
            VerifyAreEqual("112", GetGridCell(0, "RegionCode", "Region").FindElement(By.TagName("input")).GetAttribute("value"), "cod region");
            VerifyAreEqual("102", GetGridCell(0, "SortOrder", "Region").FindElement(By.TagName("input")).GetAttribute("value"), "sort region");

            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all region");

            GetGridCell(0, "Name", "Region").Click();
            Thread.Sleep(1000);
            VerifyAreEqual("TestRegion102 - Список городов", driver.FindElement(By.CssSelector("[data-e2e=\"h1-city\"]")).Text, "h1 city 1");
            VerifyAreEqual("TestCity16", GetGridCell(0, "Name", "City").Text, "name city1");

            VerifyAreEqual("Найдено записей: 30", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all city");
            
            driver.FindElement(By.CssSelector("[data-e2e=\"GoToCountry\"]")).Click();
            Thread.Sleep(1000);

            VerifyAreEqual("Список стран", driver.FindElement(By.CssSelector("[data-e2e=\"h1-country\"]")).Text, "h1 country");
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "name country");

            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);

            VerifyAreEqual("TestCountry1 - Список регионов", driver.FindElement(By.CssSelector("[data-e2e=\"h1-region\"]")).Text, "h1 region 2 ");
            VerifyAreEqual("TestRegion1", GetGridCell(0, "Name", "Region").Text, "name region 2");
            VerifyAreEqual("11", GetGridCell(0, "RegionCode", "Region").FindElement(By.TagName("input")).GetAttribute("value"), "cod region 2");
            VerifyAreEqual("1", GetGridCell(0, "SortOrder", "Region").FindElement(By.TagName("input")).GetAttribute("value"), "sort region 2");

            VerifyAreEqual("Найдено записей: 101", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all region 2");

            driver.FindElement(By.CssSelector("[data-e2e=\"GoToCity\"]")).Click();
            Thread.Sleep(1000);

            VerifyAreEqual("TestCountry1 - Список городов", driver.FindElement(By.CssSelector("[data-e2e=\"h1-city\"]")).Text, "h1 city");
            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "name city");

            VerifyAreEqual("Найдено записей: 71", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all city");

            driver.FindElement(By.CssSelector("[data-e2e=\"GoToRegion\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("TestCountry1 - Список регионов", driver.FindElement(By.CssSelector("[data-e2e=\"h1-region\"]")).Text, "h1 region 2");

            VerifyFinally(testname);
        }
        
    }

    [TestFixture]
    public class SettingsRegionSelectDeleteTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
             "data\\Admin\\Settings\\Countries\\Region\\RegionDelete\\Customers.Country.csv",
               "data\\Admin\\Settings\\Countries\\Region\\RegionDelete\\Customers.Region.csv",
              "data\\Admin\\Settings\\Countries\\Region\\RegionDelete\\Customers.City.csv",
            "data\\Admin\\Settings\\Countries\\Region\\RegionDelete\\Customers.Customer.csv",
           "data\\Admin\\Settings\\Countries\\Region\\RegionDelete\\Customers.CustomerGroup.csv"

           );

            Init();
        }
        
        [Test]
        public void RegionzSelectDelete()
        {
            testname = "RegionszSelectDelete";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);

            //check delete cancel 
            GetGridCell(0, "_serviceColumn", "Region").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestRegion1", GetGridCell(0, "Name", "Region").Text, "1 grid canсel delete");

            //check delete
            GetGridCell(0, "_serviceColumn", "Region").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestRegion2", GetGridCell(0, "Name", "Region").Text, "1 grid delete");

            //check select 
            GetGridCell(0, "selectionRowHeaderCol", "Region").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol", "Region").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol", "Region").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol", "Region").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 1 grid");
            VerifyIsTrue(GetGridCell(1, "selectionRowHeaderCol", "Region").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 2 grid");
            VerifyIsTrue(GetGridCell(2, "selectionRowHeaderCol", "Region").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 3 grid");
            VerifyAreEqual("3", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count selected");

            //check delete selected items
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("TestRegion5", GetGridCell(0, "Name", "Region").Text, "selected 1 grid delete");
            VerifyAreEqual("TestRegion6", GetGridCell(1, "Name", "Region").Text, "selected 2 grid delete");
            VerifyAreEqual("TestRegion7", GetGridCell(2, "Name", "Region").Text, "selected 3 grid delete");

            //check select all on page
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol", "Region").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 1 grid");
            VerifyIsTrue(GetGridCell(9, "selectionRowHeaderCol", "Region").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 10 grid");

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("TestRegion15", GetGridCell(0, "Name", "Region").Text, "selected all on page 1 grid delete");
            VerifyAreEqual("TestRegion24", GetGridCell(9, "Name", "Region").Text, "selected all on page 10 grid delete");

            //check select all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("87", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count all selected after deleting");

            //check deselect all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(!GetGridCell(0, "selectionRowHeaderCol", "Region").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 1 grid");
            VerifyIsTrue(!GetGridCell(9, "selectionRowHeaderCol", "Region").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 10 grid");

            //check delete all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete all");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after deleting");

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete all 2");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after deleting 2");

            VerifyFinally(testname);
        }
    }
}
