using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using System.Collections.ObjectModel;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.SettingsSearch
{
    [TestFixture]
    public class SettingsSearchPageTest : BaseSeleniumTest
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
        public void SettingsSearchPage()
        {
            testname = "SettingsSearchPage";
            VerifyBegin(testname);

            VerifyAreEqual("test title 1", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 1");
            VerifyAreEqual("test title 107", GetGridCell(9, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            //Thread.Sleep(2000);
             VerifyAreEqual("test title 108", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "page 2 line 1");
            VerifyAreEqual("test title 116", GetGridCell(9, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            //Thread.Sleep(2000);
           VerifyAreEqual("test title 117", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "page 3 line 1");
            VerifyAreEqual("test title 125", GetGridCell(9, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            //Thread.Sleep(2000);
            VerifyAreEqual("test title 126", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "page 4 line 1");
            VerifyAreEqual("test title 134", GetGridCell(9, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "page 4 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            //Thread.Sleep(2000);
            VerifyAreEqual("test title 135", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "page 5 line 1");
           VerifyAreEqual("test title 143", GetGridCell(9, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "page 5 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            //Thread.Sleep(2000);
            VerifyAreEqual("test title 144", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "page 6 line 1");
            VerifyAreEqual("test title 17", GetGridCell(9, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "page 6 line 10");
            
            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            //Thread.Sleep(2000);
            VerifyAreEqual("test title 1", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 1");
            VerifyAreEqual("test title 107", GetGridCell(9, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsSearchPageToPrevious()
        {
            testname = "SettingsSearchPageToPrevious";
            VerifyBegin(testname);

            VerifyAreEqual("test title 1", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 1");
            VerifyAreEqual("test title 107", GetGridCell(9, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            //Thread.Sleep(2000);
             VerifyAreEqual("test title 108", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "page 2 line 1");
            VerifyAreEqual("test title 116", GetGridCell(9, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            //Thread.Sleep(2000);
           VerifyAreEqual("test title 117", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "page 3 line 1");
            VerifyAreEqual("test title 125", GetGridCell(9, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            //Thread.Sleep(2000);
             VerifyAreEqual("test title 108", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "page 2 line 1");
            VerifyAreEqual("test title 116", GetGridCell(9, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            //Thread.Sleep(2000);
            VerifyAreEqual("test title 1", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 1");
            VerifyAreEqual("test title 107", GetGridCell(9, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 10");

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            //Thread.Sleep(2000);
            VerifyAreEqual("test title 90", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "last page line 1");
            VerifyAreEqual("test title 99", GetGridCell(9, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "last page line 10");

            VerifyFinally(testname);
        }
    }
}