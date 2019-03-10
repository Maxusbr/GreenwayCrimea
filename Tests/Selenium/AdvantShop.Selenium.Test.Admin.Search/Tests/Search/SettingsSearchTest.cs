using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using System.Collections.ObjectModel;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.SettingsSearch
{
    [TestFixture]
    public class SettingsSearchTest : BaseSeleniumTest
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
            
            GoToAdmin();

            driver.FindElement(By.Id("header-bottom")).FindElement(By.TagName("input")).Click();
            driver.FindElement(By.Id("header-bottom")).FindElement(By.TagName("input")).SendKeys("301 редирект");
            Thread.Sleep(5000);
            WaitForElem(By.XPath("//span[contains(text(), 'test title 1')]"));
            MouseFocus(driver, By.XPath("//span[contains(text(), 'test title 1')]"));
            driver.FindElement(By.XPath("//span[contains(text(), 'test title 1')]")).Click();
            Thread.Sleep(5000);
        }

         

        [Test]
        public void SettingsSearchGridTest()
        {
            testname = "SettingsSearchTest";
            VerifyBegin(testname);

            GoToAdmin("settingssearch");

            VerifyAreEqual("Поиск настроек", driver.FindElement(By.TagName("h1")).Text, "h1 settings search page");

            VerifyAreEqual("test title 1", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "Title");
            VerifyAreEqual("settingsseo#?seoTab=seo301", GetGridCell(0, "Link").Text, "Link");
            VerifyAreEqual("301 редирект", GetGridCell(0, "KeyWords").FindElement(By.TagName("input")).GetAttribute("value"), "KeyWords");
            VerifyAreEqual("10", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"), "SortOrder");

            VerifyAreEqual("Найдено записей: 150", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");

            VerifyFinally(testname);
        }

      
        [Test]
        public void SettingsSearchInplaceTitle()
        {
            testname = "SettingsSearchInplaceTitle";
            VerifyBegin(testname);

            GoToAdmin("settingssearch");

            VerifyAreEqual("test title 1", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "inplace Title before editing");

            GetGridCell(0, "Title").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "Title").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "Title").FindElement(By.TagName("input")).SendKeys("Edited title 1");
            Blur();
            DropFocus("h1");
            
            //check admin
            GoToAdmin("settingssearch");

            VerifyAreEqual("Edited title 1", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "inplace Title");

            GetGridCell(0, "Title").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "Title").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "Title").FindElement(By.TagName("input")).SendKeys("test title 1");
            Blur();
            DropFocus("h1");
           
            VerifyFinally(testname);
        }

        [Test]
        public void SettingsSearchInplaceKeywords()
        {
            testname = "SettingsSearchInplaceKeywords";
            VerifyBegin(testname);

            GoToAdmin("settingssearch");

            VerifyAreEqual("301 редирект", GetGridCell(0, "KeyWords").FindElement(By.TagName("input")).GetAttribute("value"), "inplace KeyWords before editing");

            GetGridCell(0, "KeyWords").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "KeyWords").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "KeyWords").FindElement(By.TagName("input")).SendKeys("Edited KeyWords 1");
            Blur();
            DropFocus("h1");
            
            //check admin
            GoToAdmin("settingssearch");

            VerifyAreEqual("Edited KeyWords 1", GetGridCell(0, "KeyWords").FindElement(By.TagName("input")).GetAttribute("value"), "inplace KeyWords");

            //check search
            GoToAdmin();

            driver.FindElement(By.CssSelector(".search-input")).Click();
            driver.FindElement(By.CssSelector(".search-input")).SendKeys("Edited KeyWords 1");
            WaitForElem(By.XPath("//span[contains(text(), 'test title 1')]"));
            MouseFocus(driver, By.XPath("//span[contains(text(), 'test title 1')]"));
            driver.FindElement(By.XPath("//span[contains(text(), 'test title 1')]")).Click();
            Thread.Sleep(5000);
            WaitForElem(By.XPath("//span[contains(text(), 'Настройки')]"));

            VerifyIsTrue(driver.Url.Contains("seoTab=seo301"), "check url from search");
            VerifyIsTrue(driver.PageSource.Contains("301 Редиректы"), "page from search");
           
            GoToAdmin("settingssearch");

            GetGridCell(0, "KeyWords").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "KeyWords").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "KeyWords").FindElement(By.TagName("input")).SendKeys("301 редирект");
            Blur();
            DropFocus("h1");
            
            VerifyFinally(testname);
        }

        [Test]
        public void SettingsSearchInplaceSort()
        {
            testname = "SettingsSearchInplaceSort";
            VerifyBegin(testname);

            GoToAdmin("settingssearch");

            VerifyAreEqual("10", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"), "inplace Sort before editing");

            GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).SendKeys("200");
            DropFocus("h1");
            Blur();

            //check admin
            GoToAdmin("settingssearch");

            VerifyAreEqual("200", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"), "inplace Sort");

            GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).SendKeys("10");
            DropFocus("h1");
            Blur();

            VerifyFinally(testname);
        }

        
        [Test]
       public void SettingsSearch()
       {
            testname = "SettingsSearch";
            VerifyBegin(testname);

            GoToAdmin();
            
            driver.FindElement(By.Id("header-bottom")).FindElement(By.TagName("input")).Click();
            driver.FindElement(By.Id("header-bottom")).FindElement(By.TagName("input")).SendKeys("301 редирект");
            Thread.Sleep(5000);
            WaitForElem(By.XPath("//span[contains(text(), 'test title 1')]"));
            MouseFocus(driver, By.XPath("//span[contains(text(), 'test title 1')]"));
            driver.FindElement(By.XPath("//span[contains(text(), 'test title 1')]")).Click();
            Thread.Sleep(5000);
            WaitForElem(By.XPath("//span[contains(text(), 'Настройки')]"));

            VerifyIsTrue(driver.Url.Contains("seoTab=seo301"), "check url from search");
            VerifyIsTrue(driver.PageSource.Contains("301 Редиректы"), "page from search");

            VerifyFinally(testname);
        }

    [Test]
        public void SettingsSearchGoToLink()
        {
            testname = "SettingsSearchGoToLink";
            VerifyBegin(testname);

            GoToAdmin("settingssearch");

            VerifyAreEqual("settingsseo#?seoTab=seo301", GetGridCell(0, "Link").Text, "Link");

            GetGridCell(0, "Link").Click();

            Functions.OpenNewTab(driver, baseURL);
            VerifyIsTrue(driver.WindowHandles.Count.Equals(2), "count tabs");
            
            VerifyIsTrue(driver.Url.Contains("seoTab=seo301"), "check url from settings search grid");
            VerifyIsTrue(driver.PageSource.Contains("301 Редиректы"), "page from settings search grid");

            Functions.CloseTab(driver, baseURL);

            VerifyFinally(testname);
        }
        
        [Test]
          public void SettingsSearchzSelectDelete()
          {
            GoToAdmin("settingssearch");

            testname = "SettingsSearchSelectDelete";
            VerifyBegin(testname);

            //check delete cancel 
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
              Thread.Sleep(1000);
              driver.FindElement(By.ClassName("swal2-cancel")).Click();
              Thread.Sleep(2000);
              VerifyAreEqual("test title 1", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "1 grid canсel delete");

            //check delete
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
              Thread.Sleep(1000);
              driver.FindElement(By.ClassName("swal2-confirm")).Click();
              Thread.Sleep(2000);
             VerifyAreEqual("test title 10", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "1 grid delete");

            //check select 
             GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
              GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
              GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 1 grid");
            VerifyIsTrue(GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 2 grid");
            VerifyIsTrue(GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected 3 grid");
              VerifyAreEqual("3", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count selected");

              //check delete selected items
              Functions.GridDropdownDelete(driver, baseURL);
              VerifyAreEqual("test title 102", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "selected 1 grid delete");
              VerifyAreEqual("test title 103", GetGridCell(1, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "selected 2 grid delete");
              VerifyAreEqual("test title 104", GetGridCell(2, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "selected 3 grid delete");

            //check select all on page
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
              Thread.Sleep(1000);
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 1 grid");
            VerifyIsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "selected all on page 10 grid");

              //check delete all on page
              Functions.GridDropdownDelete(driver, baseURL);
              VerifyAreEqual("test title 111", GetGridCell(0, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "selected all on page 1 grid delete");
              VerifyAreEqual("test title 12", GetGridCell(9, "Title").FindElement(By.TagName("input")).GetAttribute("value"), "selected all on page 10 grid delete");

            //check select all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
              Thread.Sleep(1000);
              driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
              Thread.Sleep(1000);
              VerifyAreEqual("136", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text, "count all selected after deleting");

              //check deselect all 
              driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
              Thread.Sleep(1000);
            VerifyIsTrue(!GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 1 grid");
            VerifyIsTrue(!GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "deselect all 10 grid");

              //check delete all
              driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
              Thread.Sleep(1000);
              driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
              Functions.GridDropdownDelete(driver, baseURL);

              GoToAdmin("settingssearch");
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "delete all");
            VerifyAreEqual("Найдено записей: 0", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all after deleting");

            VerifyFinally(testname);
        }
    }
}