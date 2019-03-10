using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Settings.SystemSettings.Countries.Country
{
    [TestFixture]
    public class CountriesPageTest : BaseSeleniumTest
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
        public void PageCounties()
        {
            testname = "PageCounties";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            gridReturnDefaultView10();
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "line 1");
            VerifyAreEqual("TestCountry10", GetGridCell(9, "Name", "Country").Text, "line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCountry11", GetGridCell(0, "Name", "Country").Text, "line 11");
            VerifyAreEqual("TestCountry20", GetGridCell(9, "Name", "Country").Text, "line 20");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCountry21", GetGridCell(0, "Name", "Country").Text, "line 21");
            VerifyAreEqual("TestCountry30", GetGridCell(9, "Name", "Country").Text, "line 30");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCountry31", GetGridCell(0, "Name", "Country").Text, "line 31");
            VerifyAreEqual("TestCountry40", GetGridCell(9, "Name", "Country").Text, "line 40");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCountry41", GetGridCell(0, "Name", "Country").Text, "line 41");
            VerifyAreEqual("TestCountry50", GetGridCell(9, "Name", "Country").Text, "line 50");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCountry51", GetGridCell(0, "Name", "Country").Text, "line 51");
            VerifyAreEqual("TestCountry60", GetGridCell(9, "Name", "Country").Text, "line 60");

            VerifyFinally(testname);
        }

        [Test]
        public void PageCountiesToBegin()
        {
            testname = "PageCountiesToBegin";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            gridReturnDefaultView10();
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "line 1");
            VerifyAreEqual("TestCountry10", GetGridCell(9, "Name", "Country").Text, "line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCountry11", GetGridCell(0, "Name", "Country").Text, "line 11");
            VerifyAreEqual("TestCountry20", GetGridCell(9, "Name", "Country").Text, "line 20");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCountry21", GetGridCell(0, "Name", "Country").Text, "line 21");
            VerifyAreEqual("TestCountry30", GetGridCell(9, "Name", "Country").Text, "line 30");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCountry31", GetGridCell(0, "Name", "Country").Text, "line 31");
            VerifyAreEqual("TestCountry40", GetGridCell(9, "Name", "Country").Text, "line 40");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCountry41", GetGridCell(0, "Name", "Country").Text, "line 41");
            VerifyAreEqual("TestCountry50", GetGridCell(9, "Name", "Country").Text, "line 50");

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "line 1");
            VerifyAreEqual("TestCountry10", GetGridCell(9, "Name", "Country").Text, "line 10");
            VerifyFinally(testname);
        }

        [Test]
        public void PageCountiesToEnd()
        {
            testname = "PageCountiesToEnd";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            gridReturnDefaultView10();
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "line 1");
            VerifyAreEqual("TestCountry10", GetGridCell(9, "Name", "Country").Text, "line 10");

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCountry101", GetGridCell(0, "Name", "Country").Text, "line 101");
            VerifyFinally(testname);
        }

        [Test]
        public void PageCountiesToNext()
        {
            testname = "PageCountiesToNext";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            gridReturnDefaultView10();
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "line 1");
            VerifyAreEqual("TestCountry10", GetGridCell(9, "Name", "Country").Text, "line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCountry11", GetGridCell(0, "Name", "Country").Text, "line 11");
            VerifyAreEqual("TestCountry20", GetGridCell(9, "Name", "Country").Text, "line 20");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCountry21", GetGridCell(0, "Name", "Country").Text, "line 21");
            VerifyAreEqual("TestCountry30", GetGridCell(9, "Name", "Country").Text, "line 30");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCountry31", GetGridCell(0, "Name", "Country").Text, "line 31");
            VerifyAreEqual("TestCountry40", GetGridCell(9, "Name", "Country").Text, "line 40");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCountry41", GetGridCell(0, "Name", "Country").Text, "line 41");
            VerifyAreEqual("TestCountry50", GetGridCell(9, "Name", "Country").Text, "line 50");
            VerifyFinally(testname);
        }

        [Test]
        public void PageCountiesToPrevious()
        {

            testname = "PageCountiesToPrevious";
            VerifyBegin(testname);
            GoToAdmin("settingssystem#?systemTab=countries");
            gridReturnDefaultView10();
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "line 1");
            VerifyAreEqual("TestCountry10", GetGridCell(9, "Name", "Country").Text, "line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCountry11", GetGridCell(0, "Name", "Country").Text, "line 11");
            VerifyAreEqual("TestCountry20", GetGridCell(9, "Name", "Country").Text, "line 20");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCountry21", GetGridCell(0, "Name", "Country").Text, "line 21");
            VerifyAreEqual("TestCountry30", GetGridCell(9, "Name", "Country").Text, "line 30");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCountry11", GetGridCell(0, "Name", "Country").Text, "line 11");
            VerifyAreEqual("TestCountry20", GetGridCell(9, "Name", "Country").Text, "line 20");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "line 1");
            VerifyAreEqual("TestCountry10", GetGridCell(9, "Name", "Country").Text, "line 10");
            VerifyFinally(testname);
        }
        [Test]
        public void CountiesPresent()
        {
            testname = "CountiesPresent";
            VerifyBegin(testname);

            GoToAdmin("settingssystem#?systemTab=countries");
            Functions.GridPaginationSelect10(driver, baseURL);
            VerifyAreEqual("Найдено записей: 101", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "find elem 101");
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "line 1 10");
            VerifyAreEqual("TestCountry10", GetGridCell(9, "Name", "Country").Text, "line 10");

            Functions.GridPaginationSelect20(driver, baseURL);
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "line 1 20");
            VerifyAreEqual("TestCountry20", GetGridCell(19, "Name", "Country").Text, "line 20");

            Functions.GridPaginationSelect50(driver, baseURL);
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "line 1 50 ");
            VerifyAreEqual("TestCountry50", GetGridCell(49, "Name", "Country").Text, "line 50");

            Functions.GridPaginationSelect100(driver, baseURL);
            VerifyAreEqual("TestCountry1", GetGridCell(0, "Name", "Country").Text, "line 1 100");
            VerifyAreEqual("TestCountry100", GetGridCell(99, "Name", "Country").Text, "line 100");
            VerifyFinally(testname);
        }
    }
}
