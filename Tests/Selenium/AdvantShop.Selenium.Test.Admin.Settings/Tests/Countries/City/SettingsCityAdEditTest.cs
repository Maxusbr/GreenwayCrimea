using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.Settings.SystemSettings.Countries.City
{
    [TestFixture]
    public class SettingsCityAdEditTest : BaseSeleniumTest
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
        public void CitiOpenEdit()
        {
            testname = "CitiOpenEdit";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");

            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            GetGridCell(0, "Name", "Region").Click();
            Thread.Sleep(2000);

            GetGridCell(0, "_serviceColumn", "City").FindElement(By.TagName("ui-modal-trigger")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("Редактирование города", driver.FindElement(By.TagName("h2")).Text, "pop up h2");
            VerifyAreEqual("TestCity1", driver.FindElement(By.CssSelector("[data-e2e=\"CityName\"]")).GetAttribute("value"), "pop up name");
            VerifyAreEqual("111111", driver.FindElement(By.CssSelector("[data-e2e=\"CityPhone\"]")).GetAttribute("value"), "pop up CityPhone");
            VerifyAreEqual("999999", driver.FindElement(By.CssSelector("[data-e2e=\"CityMobilePhone\"]")).GetAttribute("value"), "pop up CityMobilePhone");

            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"CityMain\"] input")).Selected, "pop up CityDisplay");
            VerifyAreEqual("1", driver.FindElement(By.CssSelector("[data-e2e=\"CitySort\"]")).GetAttribute("value"), "pop up CitySortOrder");

            IWebElement selectElem1 = driver.FindElement(By.CssSelector("[data-e2e=\"CityRegion\"]"));
            SelectElement select3 = new SelectElement(selectElem1);
            Assert.IsTrue(select3.SelectedOption.Text.Contains("TestRegion1"));
            VerifyFinally(testname);
        }

        [Test]
        public void CityEdit()
        {
            testname = "CityEdit";
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

            GetGridCell(0, "_serviceColumn", "City").FindElement(By.TagName("ui-modal-trigger")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.CssSelector(".modal-content"));

            driver.FindElement(By.CssSelector("[data-e2e=\"CityName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CityName\"]")).SendKeys("NewName");
            driver.FindElement(By.CssSelector("[data-e2e=\"CityPhone\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CityPhone\"]")).SendKeys("789789");
            driver.FindElement(By.CssSelector("[data-e2e=\"CityMobilePhone\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CityMobilePhone\"]")).SendKeys("987987");

            driver.FindElement(By.CssSelector("[data-e2e=\"CityMain\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"CitySort\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CitySort\"]")).SendKeys("888");

            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search not exist discount price range");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("NewName");
            WaitForAjax();

            VerifyAreEqual("NewName", GetGridCell(0, "Name", "City").Text, "grid name");
            VerifyAreEqual("789789", GetGridCell(0, "PhoneNumber", "City").FindElement(By.TagName("input")).GetAttribute("value"), "grid CityPhone");
            VerifyAreEqual("987987", GetGridCell(0, "MobilePhoneNumber", "City").FindElement(By.TagName("input")).GetAttribute("value"), "grid Iso3");
            VerifyAreEqual("888", GetGridCell(0, "CitySort", "City").FindElement(By.TagName("input")).GetAttribute("value"), "grid SortOrder");
            VerifyIsTrue(GetGridCell(0, "DisplayInPopup", "City").FindElement(By.TagName("input")).Selected, "grid DisplayInPopup");

            GetGridCell(0, "_serviceColumn", "City").FindElement(By.TagName("ui-modal-trigger")).Click();
            Thread.Sleep(4000);
            VerifyAreEqual("Редактирование города", driver.FindElement(By.TagName("h2")).Text, "pop up h2");
            VerifyAreEqual("NewName", driver.FindElement(By.CssSelector("[data-e2e=\"CityName\"]")).GetAttribute("value"), "npop up ame");
            VerifyAreEqual("789789", driver.FindElement(By.CssSelector("[data-e2e=\"CityPhone\"]")).GetAttribute("value"), "pop up CityPhone");
            VerifyAreEqual("987987", driver.FindElement(By.CssSelector("[data-e2e=\"CityMobilePhone\"]")).GetAttribute("value"), "pop up CityMobilePhone");

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"CityMain\"] input")).Selected, "CityDisplay");
            VerifyAreEqual("888", driver.FindElement(By.CssSelector("[data-e2e=\"CitySort\"]")).GetAttribute("value"), "pop up CitySortOrder");

            VerifyFinally(testname);
        }

        [Test]
        public void CityOpenAdd()
        {
            testname = "CityOpenAdd";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            GetGridCell(0, "Name", "Region").Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"AddCity\"]")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("Добавление города", driver.FindElement(By.TagName("h2")).Text, "pop up h2");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"CityName\"]")).GetAttribute("value"), "name");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"CityPhone\"]")).GetAttribute("value"), "CityPhone");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"CityMobilePhone\"]")).GetAttribute("value"), "CityMobilePhone");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"CityMain\"] input")).Selected, "CityDisplay");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"CitySort\"]")).GetAttribute("value"), "CitySortOrder");

            driver.FindElement(By.CssSelector("[data-e2e=\"CityName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CityName\"]")).SendKeys("NewCity");
            driver.FindElement(By.CssSelector("[data-e2e=\"CityPhone\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CityPhone\"]")).SendKeys("789789");
            driver.FindElement(By.CssSelector("[data-e2e=\"CityMobilePhone\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CityMobilePhone\"]")).SendKeys("987987");

            driver.FindElement(By.CssSelector("[data-e2e=\"CityMain\"]")).Click();
            
            driver.FindElement(By.CssSelector("[data-e2e=\"CitySort\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CitySort\"]")).SendKeys("789");

            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("NewCity");
            WaitForAjax();

            VerifyAreEqual("NewCity", GetGridCell(0, "Name", "City").Text, "grid name");
            VerifyAreEqual("789789", GetGridCell(0, "PhoneNumber", "City").FindElement(By.TagName("input")).GetAttribute("value"), "grid CityPhone");
            VerifyAreEqual("987987", GetGridCell(0, "MobilePhoneNumber", "City").FindElement(By.TagName("input")).GetAttribute("value"), "grid MobilePhoneNumber");
            VerifyAreEqual("789", GetGridCell(0, "CitySort", "City").FindElement(By.TagName("input")).GetAttribute("value"), "grid SortOrder");
            VerifyIsTrue(GetGridCell(0, "DisplayInPopup", "City").FindElement(By.TagName("input")).Selected, "grid DisplayInPopup");

            GetGridCell(0, "_serviceColumn", "City").FindElement(By.TagName("ui-modal-trigger")).Click();
            Thread.Sleep(4000);
            VerifyAreEqual("Редактирование города", driver.FindElement(By.TagName("h2")).Text, "pop up h2");
            VerifyAreEqual("NewCity", driver.FindElement(By.CssSelector("[data-e2e=\"CityName\"]")).GetAttribute("value"), "pop up name");
            VerifyAreEqual("789789", driver.FindElement(By.CssSelector("[data-e2e=\"CityPhone\"]")).GetAttribute("value"), "pop up CityPhone");
            VerifyAreEqual("987987", driver.FindElement(By.CssSelector("[data-e2e=\"CityMobilePhone\"]")).GetAttribute("value"), "pop up CityMobilePhone");

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"CityMain\"] input")).Selected, "pop up CityDisplay");
            VerifyAreEqual("789", driver.FindElement(By.CssSelector("[data-e2e=\"CitySort\"]")).GetAttribute("value"), "pop up CitySortOrder");


            VerifyFinally(testname);
        }
        [Test]
        public void CityOpenAddRegion()
        {
            testname = "CityOpenAddRegion";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            GetGridCell(0, "Name", "Region").Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"AddCity\"]")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.CssSelector(".modal-content"));

            driver.FindElement(By.CssSelector("[data-e2e=\"CityName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CityName\"]")).SendKeys("NewCityRegion");
            driver.FindElement(By.CssSelector("[data-e2e=\"CityPhone\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CityPhone\"]")).SendKeys("777777");
            driver.FindElement(By.CssSelector("[data-e2e=\"CityMobilePhone\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CityMobilePhone\"]")).SendKeys("888888");
            

            driver.FindElement(By.CssSelector("[data-e2e=\"CitySort\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CitySort\"]")).SendKeys("10");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"CityRegion\"]")))).SelectByText("TestRegion100");
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("NewCityRegion");
            WaitForAjax();

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search not exist");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            driver.FindElement(By.CssSelector("[data-e2e=\"GoToRegion\"]")).Click();
            Thread.Sleep(1000);
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestRegion100");
            WaitForAjax();

            VerifyAreEqual("TestRegion100", GetGridCell(0, "Name", "Region").Text, "grid name");
            GetGridCell(0, "Name", "Region").Click();
            Thread.Sleep(1000);

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("NewCityRegion");
            WaitForAjax();

            VerifyAreEqual("NewCityRegion", GetGridCell(0, "Name", "City").Text, "grid name");
            VerifyAreEqual("777777", GetGridCell(0, "PhoneNumber", "City").FindElement(By.TagName("input")).GetAttribute("value"), "grid CityPhone");
            VerifyAreEqual("888888", GetGridCell(0, "MobilePhoneNumber", "City").FindElement(By.TagName("input")).GetAttribute("value"), "grid MobilePhoneNumber");
            VerifyAreEqual("10", GetGridCell(0, "CitySort", "City").FindElement(By.TagName("input")).GetAttribute("value"), "grid SortOrder");
            VerifyIsFalse(GetGridCell(0, "DisplayInPopup", "City").FindElement(By.TagName("input")).Selected, "grid DisplayInPopup");

            GetGridCell(0, "_serviceColumn", "City").FindElement(By.TagName("ui-modal-trigger")).Click();
            Thread.Sleep(4000);
            VerifyAreEqual("Редактирование города", driver.FindElement(By.TagName("h2")).Text, "pop up h2");
            VerifyAreEqual("NewCityRegion", driver.FindElement(By.CssSelector("[data-e2e=\"CityName\"]")).GetAttribute("value"), "pop up name");
            VerifyAreEqual("777777", driver.FindElement(By.CssSelector("[data-e2e=\"CityPhone\"]")).GetAttribute("value"), "pop up CityPhone");
            VerifyAreEqual("888888", driver.FindElement(By.CssSelector("[data-e2e=\"CityMobilePhone\"]")).GetAttribute("value"), "pop up CityMobilePhone");

            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"CityMain\"] input")).Selected, "pop up CityDisplay");
            VerifyAreEqual("10", driver.FindElement(By.CssSelector("[data-e2e=\"CitySort\"]")).GetAttribute("value"), "pop up CitySortOrder");


            VerifyFinally(testname);
        }
    }
}
