using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.SystemSettings.Countries.Region
{
    [TestFixture]
    public class SettingsRegionAddEditTest : BaseSeleniumTest
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
        public void RegionOpenEdit()
        {
            testname = "RegionOpenEdit";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);

            GetGridCell(0, "_serviceColumn", "Region").FindElement(By.TagName("ui-modal-trigger")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("Редактирование региона", driver.FindElement(By.TagName("h2")).Text, "pop up h2");
            VerifyAreEqual("TestRegion1", driver.FindElement(By.CssSelector("[data-e2e=\"RegionName\"]")).GetAttribute("value"), "pop up name");
            VerifyAreEqual("11", driver.FindElement(By.CssSelector("[data-e2e=\"RegionCode\"]")).GetAttribute("value"), "pop up RegionCode");
            VerifyAreEqual("1", driver.FindElement(By.CssSelector("[data-e2e=\"RegionSort\"]")).GetAttribute("value"), "pop up RegionSort");

            VerifyFinally(testname);
        }
        [Test]
        public void RegionEdit()
        {
            testname = "RegionEdit";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestRegion100");
            WaitForAjax();

            GetGridCell(0, "_serviceColumn", "Region").FindElement(By.TagName("ui-modal-trigger")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.CssSelector(".modal-content"));

            driver.FindElement(By.CssSelector("[data-e2e=\"RegionName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"RegionName\"]")).SendKeys("NewName");
            driver.FindElement(By.CssSelector("[data-e2e=\"RegionCode\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"RegionCode\"]")).SendKeys("999");
            driver.FindElement(By.CssSelector("[data-e2e=\"RegionSort\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"RegionSort\"]")).SendKeys("888");

            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search not exist discount price range");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("NewName");
            WaitForAjax();

            VerifyAreEqual("NewName", GetGridCell(0, "Name", "Region").Text, "grid name");
            VerifyAreEqual("999", GetGridCell(0, "RegionCode", "Region").FindElement(By.TagName("input")).GetAttribute("value"), "grid RegionCode");
            VerifyAreEqual("888", GetGridCell(0, "SortOrder", "Region").FindElement(By.TagName("input")).GetAttribute("value"), "grid SortOrder");

            GetGridCell(0, "_serviceColumn", "Region").FindElement(By.TagName("ui-modal-trigger")).Click();
            Thread.Sleep(4000);
            VerifyAreEqual("Редактирование региона", driver.FindElement(By.TagName("h2")).Text, "pop up h2");
            VerifyAreEqual("NewName", driver.FindElement(By.CssSelector("[data-e2e=\"RegionName\"]")).GetAttribute("value"), "npop up ame");
            VerifyAreEqual("999", driver.FindElement(By.CssSelector("[data-e2e=\"RegionCode\"]")).GetAttribute("value"), "pop up RegionCode");
            VerifyAreEqual("888", driver.FindElement(By.CssSelector("[data-e2e=\"RegionSort\"]")).GetAttribute("value"), "pop up RegionSort");

            VerifyFinally(testname);
        }

        [Test]
        public void RegionOpenAdd()
        {
            testname = "RegionOpenAdd";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector("[data-e2e=\"AddRegion\"]")).Click();
            Thread.Sleep(4000);
            WaitForElem(By.CssSelector(".modal-content"));

            VerifyAreEqual("Добавление региона", driver.FindElement(By.TagName("h2")).Text, "pop up h2");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"RegionName\"]")).GetAttribute("value"), "name");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"RegionCode\"]")).GetAttribute("value"), "RegionCode");
            VerifyAreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"RegionSort\"]")).GetAttribute("value"), "RegionSort");

            driver.FindElement(By.CssSelector("[data-e2e=\"RegionName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"RegionName\"]")).SendKeys("NewRegion");
            driver.FindElement(By.CssSelector("[data-e2e=\"RegionCode\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"RegionCode\"]")).SendKeys("987");
            driver.FindElement(By.CssSelector("[data-e2e=\"RegionSort\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"RegionSort\"]")).SendKeys("789");

            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("NewRegion");
            WaitForAjax();

            VerifyAreEqual("NewRegion", GetGridCell(0, "Name", "Region").Text, "grid name");
            VerifyAreEqual("987", GetGridCell(0, "RegionCode", "Region").FindElement(By.TagName("input")).GetAttribute("value"), "grid RegionCode");
            VerifyAreEqual("789", GetGridCell(0, "SortOrder", "Region").FindElement(By.TagName("input")).GetAttribute("value"), "grid SortOrder");

            GetGridCell(0, "_serviceColumn", "Region").FindElement(By.TagName("ui-modal-trigger")).Click();
            Thread.Sleep(4000);
            VerifyAreEqual("Редактирование региона", driver.FindElement(By.TagName("h2")).Text, "pop up h2");
            VerifyAreEqual("NewRegion", driver.FindElement(By.CssSelector("[data-e2e=\"RegionName\"]")).GetAttribute("value"), "pop up name");
            VerifyAreEqual("987", driver.FindElement(By.CssSelector("[data-e2e=\"RegionCode\"]")).GetAttribute("value"), "pop up RegionCode");
            VerifyAreEqual("789", driver.FindElement(By.CssSelector("[data-e2e=\"RegionSort\"]")).GetAttribute("value"), "pop up RegionSort");


            VerifyFinally(testname);
        }
    }
}
