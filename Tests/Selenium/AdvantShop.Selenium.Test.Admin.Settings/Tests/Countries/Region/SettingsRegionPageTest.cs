using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.SystemSettings.Countries.Region
{
    [TestFixture]
    public class SettingsRegionPageTest : BaseSeleniumTest
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
        public void PageRegion()
        {
            testname = "PageRegion";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestRegion1", GetGridCell(0, "Name", "Region").Text, "line 1");
            VerifyAreEqual("TestRegion10", GetGridCell(9, "Name", "Region").Text, "line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestRegion11", GetGridCell(0, "Name", "Region").Text, "line 11");
            VerifyAreEqual("TestRegion20", GetGridCell(9, "Name", "Region").Text, "line 20");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestRegion21", GetGridCell(0, "Name", "Region").Text, "line 21");
            VerifyAreEqual("TestRegion30", GetGridCell(9, "Name", "Region").Text, "line 30");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestRegion31", GetGridCell(0, "Name", "Region").Text, "line 31");
            VerifyAreEqual("TestRegion40", GetGridCell(9, "Name", "Region").Text, "line 40");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestRegion41", GetGridCell(0, "Name", "Region").Text, "line 41");
            VerifyAreEqual("TestRegion50", GetGridCell(9, "Name", "Region").Text, "line 50");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestRegion51", GetGridCell(0, "Name", "Region").Text, "line 51");
            VerifyAreEqual("TestRegion60", GetGridCell(9, "Name", "Region").Text, "line 60");

            VerifyFinally(testname);
        }

        [Test]
        public void PageRegionToBegin()
        {
            testname = "PageRegionToBegin";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestRegion1", GetGridCell(0, "Name", "Region").Text, "line 1");
            VerifyAreEqual("TestRegion10", GetGridCell(9, "Name", "Region").Text, "line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestRegion11", GetGridCell(0, "Name", "Region").Text, "line 11");
            VerifyAreEqual("TestRegion20", GetGridCell(9, "Name", "Region").Text, "line 20");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestRegion21", GetGridCell(0, "Name", "Region").Text, "line 21");
            VerifyAreEqual("TestRegion30", GetGridCell(9, "Name", "Region").Text, "line 30");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestRegion31", GetGridCell(0, "Name", "Region").Text, "line 31");
            VerifyAreEqual("TestRegion40", GetGridCell(9, "Name", "Region").Text, "line 40");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestRegion41", GetGridCell(0, "Name", "Region").Text, "line 41");
            VerifyAreEqual("TestRegion50", GetGridCell(9, "Name", "Region").Text, "line 50");

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestRegion1", GetGridCell(0, "Name", "Region").Text, "line 1");
            VerifyAreEqual("TestRegion10", GetGridCell(9, "Name", "Region").Text, "line 10");
            VerifyFinally(testname);
        }

        [Test]
        public void PageRegionToEnd()
        {
            testname = "PageRegionToEnd";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestRegion1", GetGridCell(0, "Name", "Region").Text, "line 1");
            VerifyAreEqual("TestRegion10", GetGridCell(9, "Name", "Region").Text, "line 10");

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestRegion101", GetGridCell(0, "Name", "Region").Text, "line 101");
            VerifyFinally(testname);
        }

        [Test]
        public void PageRegionToNext()
        {
            testname = "PageRegionToNext";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestRegion1", GetGridCell(0, "Name", "Region").Text, "line 1");
            VerifyAreEqual("TestRegion10", GetGridCell(9, "Name", "Region").Text, "line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestRegion11", GetGridCell(0, "Name", "Region").Text, "line 11");
            VerifyAreEqual("TestRegion20", GetGridCell(9, "Name", "Region").Text, "line 20");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestRegion21", GetGridCell(0, "Name", "Region").Text, "line 21");
            VerifyAreEqual("TestRegion30", GetGridCell(9, "Name", "Region").Text, "line 30");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestRegion31", GetGridCell(0, "Name", "Region").Text, "line 31");
            VerifyAreEqual("TestRegion40", GetGridCell(9, "Name", "Region").Text, "line 40");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestRegion41", GetGridCell(0, "Name", "Region").Text, "line 41");
            VerifyAreEqual("TestRegion50", GetGridCell(9, "Name", "Region").Text, "line 50");
            VerifyFinally(testname);
        }

        [Test]
        public void PageRegionToPrevious()
        {

            testname = "PageRegionToPrevious";
            VerifyBegin(testname);
            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestRegion1", GetGridCell(0, "Name", "Region").Text, "line 1");
            VerifyAreEqual("TestRegion10", GetGridCell(9, "Name", "Region").Text, "line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestRegion11", GetGridCell(0, "Name", "Region").Text, "line 11");
            VerifyAreEqual("TestRegion20", GetGridCell(9, "Name", "Region").Text, "line 20");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestRegion21", GetGridCell(0, "Name", "Region").Text, "line 21");
            VerifyAreEqual("TestRegion30", GetGridCell(9, "Name", "Region").Text, "line 30");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestRegion11", GetGridCell(0, "Name", "Region").Text, "line 11");
            VerifyAreEqual("TestRegion20", GetGridCell(9, "Name", "Region").Text, "line 20");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestRegion1", GetGridCell(0, "Name", "Region").Text, "line 1");
            VerifyAreEqual("TestRegion10", GetGridCell(9, "Name", "Region").Text, "line 10");
            VerifyFinally(testname);
        }
        [Test]
        public void RegionPresent()
        {
            testname = "RegionPresent";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            GetGridCell(0, "Name", "Country").Click();
            Thread.Sleep(2000);
            Functions.GridPaginationSelect10(driver, baseURL);
            VerifyAreEqual("Найдено записей: 101", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "find elem 101");
            VerifyAreEqual("TestRegion1", GetGridCell(0, "Name", "Region").Text, "line 1 10");
            VerifyAreEqual("TestRegion10", GetGridCell(9, "Name", "Region").Text, "line 10");

            Functions.GridPaginationSelect20(driver, baseURL);
            VerifyAreEqual("TestRegion1", GetGridCell(0, "Name", "Region").Text, "line 1 20");
            VerifyAreEqual("TestRegion20", GetGridCell(19, "Name", "Region").Text, "line 20");

            Functions.GridPaginationSelect50(driver, baseURL);
            VerifyAreEqual("TestRegion1", GetGridCell(0, "Name", "Region").Text, "line 1 50 ");
            VerifyAreEqual("TestRegion50", GetGridCell(49, "Name", "Region").Text, "line 50");

            Functions.GridPaginationSelect100(driver, baseURL);
            VerifyAreEqual("TestRegion1", GetGridCell(0, "Name", "Region").Text, "line 1 100");
            VerifyAreEqual("TestRegion100", GetGridCell(99, "Name", "Region").Text, "line 100");
            VerifyFinally(testname);
        }
    }
}
