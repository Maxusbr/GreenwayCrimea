using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using System.Collections.ObjectModel;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.SettingsSearch
{
    [TestFixture]
    public class SettingsSearchPresentTest : BaseSeleniumTest
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
        public void SettingsSearchPresent10()
        {
            testname = "SettingsSearchPresent10";
            VerifyBegin(testname);

            //Functions.GridPaginationSelect10(driver, baseURL);
            PageSelectItems("10");
            VerifyAreEqual("test title 1", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "present line 1");
            VerifyAreEqual("test title 107", GetGridCell(9, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "present line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsSearchPresent20()
        {
            testname = "SettingsSearchPresent20";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("20");
            VerifyAreEqual("test title 1", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "present line 1");
            VerifyAreEqual("test title 116", GetGridCell(19, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "present line 20");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsSearchPresent50()
        {
            testname = "SettingsSearchPresent50";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("50");
            VerifyAreEqual("test title 1", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "present line 1");
            VerifyAreEqual("test title 143", GetGridCell(49, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "present line 50");
            
            VerifyFinally(testname);
        }

        [Test]
        public void SettingsSearchPresent100()
        {
            testname = "SettingsSearchPresent100";
            VerifyBegin(testname);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("100");
            VerifyAreEqual("test title 1", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "present line 1");
            VerifyAreEqual("test title 53", GetGridCell(99, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "present line 100");

            VerifyFinally(testname);
        }

    }
}