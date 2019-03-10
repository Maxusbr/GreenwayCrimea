using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.SystemSettings.Countries.City
{
    [TestFixture]
    public class SettingsCityTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Customers);
            InitializeService.LoadData(
             "data\\Admin\\Settings\\Countries\\City\\Customers.Country.csv",
               "data\\Admin\\Settings\\Countries\\City\\Customers.Region.csv",
              "data\\Admin\\Settings\\Countries\\City\\Customers.City.csv",
            "data\\Admin\\Settings\\Countries\\City\\Customers.Customer.csv",
           "data\\Admin\\Settings\\Countries\\City\\Customers.CustomerGroup.csv"

           );

            Init();
        }
        [Test]
        public void CityGrid()
        {
            testname = "CityGrid";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            GetGridCell(0, "Name", "Region").Click();
            Thread.Sleep(2000);

            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "name");
            VerifyAreEqual("111111", GetGridCell(0, "PhoneNumber", "City").FindElement(By.TagName("input")).GetAttribute("value"), "phone");
            VerifyAreEqual("999999", GetGridCell(0, "MobilePhoneNumber", "City").FindElement(By.TagName("input")).GetAttribute("value"), "mobile phone");
            VerifyAreEqual("1", GetGridCell(0, "CitySort", "City").FindElement(By.TagName("input")).GetAttribute("value"), "SortOrder");
            VerifyIsFalse(GetGridCell(0, "DisplayInPopup", "City").FindElement(By.TagName("input")).Selected, "DisplayInPopup");

            VerifyAreEqual("Найдено записей: 101", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

        [Test]
        public void CityInplaceDisplay()
        {
            testname = "CityInplaceDisplay";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            GetGridCell(0, "Name", "Region").Click();
            Thread.Sleep(2000);

            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "name");
            VerifyIsFalse(GetGridCell(0, "DisplayInPopup", "City").FindElement(By.TagName("input")).Selected, "DisplayInPopup");

            GetGridCell(0, "DisplayInPopup", "City").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(GetGridCell(0, "DisplayInPopup", "City").FindElement(By.TagName("input")).Selected, "inplace DisplayInPopup 1");

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            GetGridCell(0, "Name", "Region").Click();
            Thread.Sleep(2000);

            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "name");
            VerifyIsTrue(GetGridCell(0, "DisplayInPopup", "City").FindElement(By.TagName("input")).Selected, "inplace DisplayInPopup 2");

            VerifyFinally(testname);
        }
        [Test]
        public void CityInplacePhone()
        {
            testname = "CityInplacePhone";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            GetGridCell(0, "Name", "Region").Click();
            Thread.Sleep(2000);

            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "name");
            VerifyAreEqual("111111", GetGridCell(0, "PhoneNumber", "City").FindElement(By.TagName("input")).GetAttribute("value"), "phone");
            VerifyAreEqual("999999", GetGridCell(0, "MobilePhoneNumber", "City").FindElement(By.TagName("input")).GetAttribute("value"), "mobile phone");

            GetGridCell(0, "PhoneNumber", "City").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "PhoneNumber", "City").FindElement(By.TagName("input")).SendKeys("123123");

            GetGridCell(0, "MobilePhoneNumber", "City").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "MobilePhoneNumber", "City").FindElement(By.TagName("input")).SendKeys("321321");
            Thread.Sleep(2000);
            XPathContainsText("h1", "Список городов");
            VerifyAreEqual("123123", GetGridCell(0, "PhoneNumber", "City").FindElement(By.TagName("input")).GetAttribute("value"), "inplace phone");
            VerifyAreEqual("321321", GetGridCell(0, "MobilePhoneNumber", "City").FindElement(By.TagName("input")).GetAttribute("value"), "inplace mobile phone");

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            GetGridCell(0, "Name", "Region").Click();
            Thread.Sleep(2000);

            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "name");
            VerifyAreEqual("123123", GetGridCell(0, "PhoneNumber", "City").FindElement(By.TagName("input")).GetAttribute("value"), "inplace phone 2");
            VerifyAreEqual("321321", GetGridCell(0, "MobilePhoneNumber", "City").FindElement(By.TagName("input")).GetAttribute("value"), "inplace mobile phone 2");


            VerifyFinally(testname);
        }
        [Test]
        public void CityInplacesCod()
        {
            testname = "CityInplaSortCod";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            GetGridCell(0, "Name", "Region").Click();
            Thread.Sleep(2000);
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestCity100");
            WaitForAjax();
            XPathContainsText("h1", "Список городов");
            WaitForAjax();
            VerifyAreEqual("TestCity100", GetGridCell(0, "Name", "City").Text, "name");
            VerifyAreEqual("100", GetGridCell(0, "CitySort", "City").FindElement(By.TagName("input")).GetAttribute("value"), "SortOrder");
            

            GetGridCell(0, "CitySort", "City").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "CitySort", "City").FindElement(By.TagName("input")).SendKeys("1000");
            Thread.Sleep(2000);
            XPathContainsText("h1", "Список городов");
            VerifyAreEqual("1000", GetGridCell(0, "CitySort", "City").FindElement(By.TagName("input")).GetAttribute("value"), "inplace SortOrder1");

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            GetGridCell(0, "Name", "Region").Click();
            Thread.Sleep(2000);
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestCity100");
            XPathContainsText("h1", "Список городов");
            WaitForAjax();
            VerifyAreEqual("TestCity100", GetGridCell(0, "Name", "City").Text, "name");
            VerifyAreEqual("1000", GetGridCell(0, "CitySort", "City").FindElement(By.TagName("input")).GetAttribute("value"), "inplace SortOrder2");

            VerifyFinally(testname);
        }

     
        [Test]
        public void CityzSelectDelete()
        {
            testname = "CityzSelectDelete";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            GetGridCell(0, "Name", "Region").Click();
            Thread.Sleep(2000);

            //check delete cancel 
            GetGridCell(0, "_serviceColumn", "City").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "1 grid canсel delete");

            //check delete
            GetGridCell(0, "_serviceColumn", "City").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCity2", GetGridCell(0, "Name", "City").Text, "1 grid delete");

            //check select 
            GetGridCell(0, "selectionRowHeaderCol", "City").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol", "City").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol", "City").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol", "City").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 1 grid");
            VerifyIsTrue(GetGridCell(1, "selectionRowHeaderCol", "City").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 2 grid");
            VerifyIsTrue(GetGridCell(2, "selectionRowHeaderCol", "City").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 3 grid");
            VerifyAreEqual("3", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count selected");

            //check delete selected items
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("TestCity5", GetGridCell(0, "Name", "City").Text, "selected 1 grid delete");
            VerifyAreEqual("TestCity6", GetGridCell(1, "Name", "City").Text, "selected 2 grid delete");
            VerifyAreEqual("TestCity7", GetGridCell(2, "Name", "City").Text, "selected 3 grid delete");

            //check select all on page
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol", "City").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 1 grid");
            VerifyIsTrue(GetGridCell(9, "selectionRowHeaderCol", "City").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 10 grid");

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("TestCity15", GetGridCell(0, "Name", "City").Text, "selected all on page 1 grid delete");
            VerifyAreEqual("TestCity24", GetGridCell(9, "Name", "City").Text, "selected all on page 10 grid delete");

            //check select all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(1000);
            VerifyAreEqual("87", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count all selected after deleting");

            //check deselect all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Thread.Sleep(1000);
            VerifyIsTrue(!GetGridCell(0, "selectionRowHeaderCol", "City").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 1 grid");
            VerifyIsTrue(!GetGridCell(9, "selectionRowHeaderCol", "City").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 10 grid");

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
            GetGridCell(0, "Name", "Region").Click();
            Thread.Sleep(2000);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete all 2");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after deleting 2");

            VerifyFinally(testname);
        }
    }
}
