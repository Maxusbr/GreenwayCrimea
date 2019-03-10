using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using System.Collections.ObjectModel;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.SettingsSearch
{
    [TestFixture]
    public class SettingsSearchGridSearchTest : BaseSeleniumTest
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
        }

         

        [Test]
        public void SettingsSearchGridSearchExist()
        {
            testname = "SettingsSearchGridSearchExist";
            VerifyBegin(testname);

            GoToAdmin("settingssearch");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("test title 111");
            DropFocus("h1");
            WaitForAjax();

            VerifyAreEqual("test title 111", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "search exist settings");
            VerifyAreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");
            
            VerifyFinally(testname);
        }

        [Test]
        public void SettingsSearchGridSearchNotExist()
        {
            testname = "SettingsSearchGridSearchNotExist";
            VerifyBegin(testname);

            GoToAdmin("settingssearch");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("name settings");
            DropFocus("h1");
            WaitForAjax();

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search not exist");
            
            VerifyFinally(testname);
        }

        [Test]
        public void SettingsSearchGridSearchMuchSymbols()
        {
            testname = "SettingsSearchGridSearchMuchSymbols";
            VerifyBegin(testname);

            GoToAdmin("settingssearch");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            DropFocus("h1");
            WaitForAjax();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search too much symbols");
            
            VerifyFinally(testname);
        }

        [Test]
        public void SettingsSearchGridSearchInvalidSymbols()
        {
            testname = "SettingsSearchGridSearchInvalidSymbols";
            VerifyBegin(testname);

            GoToAdmin("settingssearch");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            DropFocus("h1");
            WaitForAjax();

            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "search invalid symbols");
            
            VerifyFinally(testname);
        }
        
    }
}