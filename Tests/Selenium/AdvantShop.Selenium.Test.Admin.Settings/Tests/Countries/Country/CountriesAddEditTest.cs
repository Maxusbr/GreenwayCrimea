using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.SystemSettings.Countries.Country
{
    [TestFixture]
    public class CountriesAddEditTest : BaseSeleniumTest
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
        public void CountrieOpenEdit()
        {
            testname = "CountrieOpenEdit";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");

            GetGridCell(0, "_serviceColumn", "Country").FindElement(By.TagName("ui-modal-trigger")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("Редактирование страны", driver.FindElement(By.TagName("h2")).Text, "pop up h2");
            VerifyAreEqual("TestCountry1", driver.FindElement(By.CssSelector("[data-e2e=\"CountryName\"]")).GetAttribute("value"), "pop up name");
            VerifyAreEqual("AA", driver.FindElement(By.CssSelector("[data-e2e=\"CountryIso2\"]")).GetAttribute("value"), "pop up CountryIso2");
            VerifyAreEqual("AA1", driver.FindElement(By.CssSelector("[data-e2e=\"CountryIso3\"]")).GetAttribute("value"), "pop up CountryIso3");

            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"CountryDisplay\"] input")).Selected, "pop up CountryDisplay");
            VerifyAreEqual("111", driver.FindElement(By.CssSelector("[data-e2e=\"CountryCode\"]")).GetAttribute("value"), "pop up CountryCode");
            VerifyAreEqual("1", driver.FindElement(By.CssSelector("[data-e2e=\"CountrySortOrder\"]")).GetAttribute("value"), "pop up CountrySortOrder");

            VerifyFinally(testname);
        }
        [Test]
        public void CountriesEdit()
        {
            testname = "CountriesEdit";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
           
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestCountry100");
            WaitForAjax();

            GetGridCell(0, "_serviceColumn", "Country").FindElement(By.TagName("ui-modal-trigger")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.CssSelector(".modal-content"));

            driver.FindElement(By.CssSelector("[data-e2e=\"CountryName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CountryName\"]")).SendKeys("NewName");
           driver.FindElement(By.CssSelector("[data-e2e=\"CountryIso2\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CountryIso2\"]")).SendKeys("QQ");
            driver.FindElement(By.CssSelector("[data-e2e=\"CountryIso3\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CountryIso3\"]")).SendKeys("QQQ");

            driver.FindElement(By.CssSelector("[data-e2e=\"CountryDisplay\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"CountryCode\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CountryCode\"]")).SendKeys("999");
            driver.FindElement(By.CssSelector("[data-e2e=\"CountrySortOrder\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CountrySortOrder\"]")).SendKeys("888");

            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search not exist discount price range");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("NewName");
            WaitForAjax();

            VerifyAreEqual("NewName", GetGridCell(0, "Name", "Country").Text, "grid name");
            VerifyAreEqual("QQ", GetGridCell(0, "Iso2", "Country").FindElement(By.TagName("input")).GetAttribute("value"), "grid Iso2");
            VerifyAreEqual("QQQ", GetGridCell(0, "Iso3", "Country").FindElement(By.TagName("input")).GetAttribute("value"), "grid Iso3");
            VerifyAreEqual("999", GetGridCell(0, "DialCode", "Country").FindElement(By.TagName("input")).GetAttribute("value"), "grid DialCode");
            VerifyAreEqual("888", GetGridCell(0, "SortOrder", "Country").FindElement(By.TagName("input")).GetAttribute("value"), "grid SortOrder");
            VerifyIsTrue(GetGridCell(0, "DisplayInPopup", "Country").FindElement(By.TagName("input")).Selected, "grid DisplayInPopup");

            GetGridCell(0, "_serviceColumn", "Country").FindElement(By.TagName("ui-modal-trigger")).Click();
            Thread.Sleep(4000);
            VerifyAreEqual("Редактирование страны", driver.FindElement(By.TagName("h2")).Text, "pop up h2");
            VerifyAreEqual("NewName", driver.FindElement(By.CssSelector("[data-e2e=\"CountryName\"]")).GetAttribute("value"), "npop up ame");
            VerifyAreEqual("QQ", driver.FindElement(By.CssSelector("[data-e2e=\"CountryIso2\"]")).GetAttribute("value"), "pop up CountryIso2");
            VerifyAreEqual("QQQ", driver.FindElement(By.CssSelector("[data-e2e=\"CountryIso3\"]")).GetAttribute("value"), "pop up CountryIso3");

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"CountryDisplay\"] input")).Selected, "CountryDisplay");
            VerifyAreEqual("999", driver.FindElement(By.CssSelector("[data-e2e=\"CountryCode\"]")).GetAttribute("value"), "pop up CountryCode");
            VerifyAreEqual("888", driver.FindElement(By.CssSelector("[data-e2e=\"CountrySortOrder\"]")).GetAttribute("value"), "pop up CountrySortOrder");

            VerifyFinally(testname);
        }

        [Test]
        public void CountriesOpenAdd()
        {
            testname = "CountriesOpenAdd";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");

            driver.FindElement(By.CssSelector("[data-e2e=\"AddCountry\"]")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("Добавление страны", driver.FindElement(By.TagName("h2")).Text, "pop up h2");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"CountryName\"]")).GetAttribute("value"), "name");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"CountryIso2\"]")).GetAttribute("value"), "CountryIso2");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"CountryIso3\"]")).GetAttribute("value"), "CountryIso3");
            VerifyIsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"CountryDisplay\"] input")).Selected, "CountryDisplay");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"CountryCode\"]")).GetAttribute("value"), "CountryCode");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"CountrySortOrder\"]")).GetAttribute("value"), "CountrySortOrder");

            driver.FindElement(By.CssSelector("[data-e2e=\"CountryName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CountryName\"]")).SendKeys("NewCountry");
            driver.FindElement(By.CssSelector("[data-e2e=\"CountryIso2\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CountryIso2\"]")).SendKeys("WW");
            driver.FindElement(By.CssSelector("[data-e2e=\"CountryIso3\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CountryIso3\"]")).SendKeys("WWW");

            driver.FindElement(By.CssSelector("[data-e2e=\"CountryDisplay\"]")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"CountryCode\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CountryCode\"]")).SendKeys("987");
            driver.FindElement(By.CssSelector("[data-e2e=\"CountrySortOrder\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"CountrySortOrder\"]")).SendKeys("789");

            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("NewCountry");
            WaitForAjax();

            VerifyAreEqual("NewCountry", GetGridCell(0, "Name", "Country").Text, "grid name");
            VerifyAreEqual("WW", GetGridCell(0, "Iso2", "Country").FindElement(By.TagName("input")).GetAttribute("value"), "grid Iso2");
            VerifyAreEqual("WWW", GetGridCell(0, "Iso3", "Country").FindElement(By.TagName("input")).GetAttribute("value"), "grid Iso3");
            VerifyAreEqual("987", GetGridCell(0, "DialCode", "Country").FindElement(By.TagName("input")).GetAttribute("value"), "grid DialCode");
            VerifyAreEqual("789", GetGridCell(0, "SortOrder", "Country").FindElement(By.TagName("input")).GetAttribute("value"), "grid SortOrder");
            VerifyIsTrue(GetGridCell(0, "DisplayInPopup", "Country").FindElement(By.TagName("input")).Selected, "grid DisplayInPopup");

            GetGridCell(0, "_serviceColumn", "Country").FindElement(By.TagName("ui-modal-trigger")).Click();
            Thread.Sleep(4000);
            VerifyAreEqual("Редактирование страны", driver.FindElement(By.TagName("h2")).Text, "pop up h2");
            VerifyAreEqual("NewCountry", driver.FindElement(By.CssSelector("[data-e2e=\"CountryName\"]")).GetAttribute("value"), "pop up name");
            VerifyAreEqual("WW", driver.FindElement(By.CssSelector("[data-e2e=\"CountryIso2\"]")).GetAttribute("value"), "pop up CountryIso2");
            VerifyAreEqual("WWW", driver.FindElement(By.CssSelector("[data-e2e=\"CountryIso3\"]")).GetAttribute("value"), "pop up CountryIso3");

            VerifyIsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"CountryDisplay\"] input")).Selected, "pop up CountryDisplay");
            VerifyAreEqual("987", driver.FindElement(By.CssSelector("[data-e2e=\"CountryCode\"]")).GetAttribute("value"), "pop up CountryCode");
            VerifyAreEqual("789", driver.FindElement(By.CssSelector("[data-e2e=\"CountrySortOrder\"]")).GetAttribute("value"), "pop up CountrySortOrder");


            VerifyFinally(testname);
        }
    }
}
