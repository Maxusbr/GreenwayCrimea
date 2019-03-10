using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.SystemSettings.Countries.City
{
    [TestFixture]
    public class SettingsCityPageTest : BaseSeleniumTest
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
        public void PageCity()
        {
            testname = "PageCity";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            GetGridCell(0, "Name", "Region").Click();
            Thread.Sleep(2000);
            gridReturnDefaultView10();
            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "line 1");
            VerifyAreEqual("TestCity10", GetGridCell(9, "Name", "City").Text, "line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCity11", GetGridCell(0, "Name", "City").Text, "line 11");
            VerifyAreEqual("TestCity20", GetGridCell(9, "Name", "City").Text, "line 20");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCity21", GetGridCell(0, "Name", "City").Text, "line 21");
            VerifyAreEqual("TestCity30", GetGridCell(9, "Name", "City").Text, "line 30");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCity31", GetGridCell(0, "Name", "City").Text, "line 31");
            VerifyAreEqual("TestCity40", GetGridCell(9, "Name", "City").Text, "line 40");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCity41", GetGridCell(0, "Name", "City").Text, "line 41");
            VerifyAreEqual("TestCity50", GetGridCell(9, "Name", "City").Text, "line 50");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCity51", GetGridCell(0, "Name", "City").Text, "line 51");
            VerifyAreEqual("TestCity60", GetGridCell(9, "Name", "City").Text, "line 60");

            VerifyFinally(testname);
        }

        [Test]
        public void PageCityToBegin()
        {
            testname = "PageCityToBegin";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            GetGridCell(0, "Name", "Region").Click();
            Thread.Sleep(2000);
            gridReturnDefaultView10();
            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "line 1");
            VerifyAreEqual("TestCity10", GetGridCell(9, "Name", "City").Text, "line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCity11", GetGridCell(0, "Name", "City").Text, "line 11");
            VerifyAreEqual("TestCity20", GetGridCell(9, "Name", "City").Text, "line 20");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCity21", GetGridCell(0, "Name", "City").Text, "line 21");
            VerifyAreEqual("TestCity30", GetGridCell(9, "Name", "City").Text, "line 30");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCity31", GetGridCell(0, "Name", "City").Text, "line 31");
            VerifyAreEqual("TestCity40", GetGridCell(9, "Name", "City").Text, "line 40");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCity41", GetGridCell(0, "Name", "City").Text, "line 41");
            VerifyAreEqual("TestCity50", GetGridCell(9, "Name", "City").Text, "line 50");

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "line 1");
            VerifyAreEqual("TestCity10", GetGridCell(9, "Name", "City").Text, "line 10");
            VerifyFinally(testname);
        }

        [Test]
        public void PageCityToEnd()
        {
            testname = "PageCityToEnd";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            GetGridCell(0, "Name", "Region").Click();
            Thread.Sleep(2000);
            gridReturnDefaultView10();
            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "line 1");
            VerifyAreEqual("TestCity10", GetGridCell(9, "Name", "City").Text, "line 10");

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCity101", GetGridCell(0, "Name", "City").Text, "line 101");
            VerifyFinally(testname);
        }

        [Test]
        public void PageCityToNext()
        {
            testname = "PageCityToNext";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            GetGridCell(0, "Name", "Region").Click();
            Thread.Sleep(2000);
            gridReturnDefaultView10();
            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "line 1");
            VerifyAreEqual("TestCity10", GetGridCell(9, "Name", "City").Text, "line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCity11", GetGridCell(0, "Name", "City").Text, "line 11");
            VerifyAreEqual("TestCity20", GetGridCell(9, "Name", "City").Text, "line 20");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCity21", GetGridCell(0, "Name", "City").Text, "line 21");
            VerifyAreEqual("TestCity30", GetGridCell(9, "Name", "City").Text, "line 30");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCity31", GetGridCell(0, "Name", "City").Text, "line 31");
            VerifyAreEqual("TestCity40", GetGridCell(9, "Name", "City").Text, "line 40");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCity41", GetGridCell(0, "Name", "City").Text, "line 41");
            VerifyAreEqual("TestCity50", GetGridCell(9, "Name", "City").Text, "line 50");
            VerifyFinally(testname);
        }

        [Test]
        public void PageCityToPrevious()
        {

            testname = "PageCityToPrevious";
            VerifyBegin(testname);
            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            GetGridCell(0, "Name", "Region").Click();
            Thread.Sleep(2000);
            gridReturnDefaultView10();
            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "line 1");
            VerifyAreEqual("TestCity10", GetGridCell(9, "Name", "City").Text, "line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCity11", GetGridCell(0, "Name", "City").Text, "line 11");
            VerifyAreEqual("TestCity20", GetGridCell(9, "Name", "City").Text, "line 20");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCity21", GetGridCell(0, "Name", "City").Text, "line 21");
            VerifyAreEqual("TestCity30", GetGridCell(9, "Name", "City").Text, "line 30");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCity11", GetGridCell(0, "Name", "City").Text, "line 11");
            VerifyAreEqual("TestCity20", GetGridCell(9, "Name", "City").Text, "line 20");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "line 1");
            VerifyAreEqual("TestCity10", GetGridCell(9, "Name", "City").Text, "line 10");
            VerifyFinally(testname);
        }
        [Test]
        public void CityPresent()
        {
            testname = "CityPresent";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            GetGridCell(0, "Name", "Region").Click();
            Thread.Sleep(2000);
            Functions.GridPaginationSelect10(driver, baseURL);
            VerifyAreEqual("Найдено записей: 101", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "find elem 101");
            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "line 1 10");
            VerifyAreEqual("TestCity10", GetGridCell(9, "Name", "City").Text, "line 10");

            Functions.GridPaginationSelect20(driver, baseURL);
            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "line 1 20");
            VerifyAreEqual("TestCity20", GetGridCell(19, "Name", "City").Text, "line 20");

            Functions.GridPaginationSelect50(driver, baseURL);
            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "line 1 50 ");
            VerifyAreEqual("TestCity50", GetGridCell(49, "Name", "City").Text, "line 50");

            Functions.GridPaginationSelect100(driver, baseURL);
            VerifyAreEqual("TestCity1", GetGridCell(0, "Name", "City").Text, "line 1 100");
            VerifyAreEqual("TestCity100", GetGridCell(99, "Name", "City").Text, "line 100");
            VerifyFinally(testname);
        }
    }
}
