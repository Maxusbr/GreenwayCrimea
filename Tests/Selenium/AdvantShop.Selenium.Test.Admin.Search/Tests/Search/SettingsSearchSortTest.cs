using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using System.Collections.ObjectModel;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.SettingsSearch
{
    [TestFixture]
    public class SettingsSearchSortTest : BaseSeleniumTest
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
        public void SettingsSearchSortTitle()
        {
            testname = "SettingsSearchSortTitle";
            VerifyBegin(testname);

            GetGridCell(-1, "Title").Click();
            WaitForAjax();
            VerifyAreEqual("test title 1", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "sort title 1 asc");
            VerifyAreEqual("test title 107", GetGridCell(9, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "sort title 10 asc");

            GetGridCell(-1, "Title").Click();
            WaitForAjax();
            VerifyAreEqual("test title 99", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "sort title 1 desc");
            VerifyAreEqual("test title 90", GetGridCell(9, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "sort title 10 desc");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsSearchSortLink()
        {
            testname = "SettingsSearchSortLink";
            VerifyBegin(testname);

            GetGridCell(-1, "Link").Click();
            WaitForAjax();
            VerifyAreEqual("link1", GetGridCell(0, "Link").Text, "sort link 1 asc");
            VerifyAreEqual("link107", GetGridCell(9, "Link").Text, "sort link 10 asc");

            GetGridCell(-1, "Link").Click();
            WaitForAjax();
            VerifyAreEqual("settingsseo#?seoTab=seo301", GetGridCell(0, "Link").Text, "sort link 1 desc");
            VerifyAreEqual("link91", GetGridCell(9, "Link").Text, "sort link 10 desc");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsSearchSortKeywords()
        {
            testname = "SettingsSearchSortKeywords";
            VerifyBegin(testname);

            GetGridCell(-1, "KeyWords").Click();
            WaitForAjax();
            VerifyAreEqual("301 редирект", GetGridCell(0, "KeyWords").FindElement(By.TagName("input")).GetAttribute("value"), "sort KeyWords 1 desc");
            VerifyAreEqual("keywords 106", GetGridCell(9, "KeyWords").FindElement(By.TagName("input")).GetAttribute("value"), "sort KeyWords 10 desc");

            GetGridCell(-1, "KeyWords").Click();
            WaitForAjax();
            VerifyAreEqual("keywords 99", GetGridCell(0, "KeyWords").FindElement(By.TagName("input")).GetAttribute("value"), "sort KeyWords 1 asc");
            VerifyAreEqual("keywords 90", GetGridCell(9, "KeyWords").FindElement(By.TagName("input")).GetAttribute("value"), "sort KeyWords 10 asc");

            VerifyFinally(testname);
        }

        [Test]
        public void SettingsSearchSortSortOrder()
        {
            testname = "SettingsSearchSortSortOrder";
            VerifyBegin(testname);

            GetGridCell(-1, "SortOrder").Click();
            WaitForAjax();
            VerifyAreEqual("10", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"), "sort SortOrder 1 asc");
            VerifyAreEqual("100", GetGridCell(9, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"), "sort SortOrder 10 asc");

            GetGridCell(-1, "SortOrder").Click();
            WaitForAjax();
            VerifyAreEqual("1500", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"), "sort SortOrder 1 desc");
            VerifyAreEqual("1410", GetGridCell(9, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"), "sort SortOrder 10 desc");

            VerifyFinally(testname);
        }

    }
}