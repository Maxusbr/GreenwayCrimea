using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace AdvantShop.SeleniumTest.Admin.CMS.NewsCategory
{
    [TestFixture]
    public class CMSNewsCategoryTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CMS);
            InitializeService.LoadData(
                "data\\Admin\\CMS\\NewsCategory\\Settings.News.csv",
                "data\\Admin\\CMS\\NewsCategory\\Settings.NewsCategory.csv"
           );

            Init();
        }

        [Test]
        public void NewsCategoryOpen()
        {
            GoToAdmin("newscategory");
            
            //check admin
            Assert.AreEqual("Найдено записей: 102", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            Assert.IsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Категории новостей"));

            Assert.AreEqual("NewsCategory1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("newscategory_url1", GetGridCell(0, "UrlPath").Text);
            Assert.AreEqual("1", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));

            //check client
            GoToClient("newscategory/newscategory_url1");

            Assert.IsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Новости"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".news-categories.block")).Text.Contains("NewsCategory1"));

            GoToClient("news");
            
            Assert.IsTrue(driver.FindElement(By.CssSelector(".news-categories.block")).Text.Contains("NewsCategory1"));
        }

        [Test]
        public void NewsCategoriesView()
        {
            GoToAdmin("newscategory");
            
            Assert.AreEqual("NewsCategory1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("NewsCategory10", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect20(driver, baseURL);
            Assert.AreEqual("NewsCategory1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("NewsCategory20", GetGridCell(19, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect50(driver, baseURL);
            Assert.AreEqual("NewsCategory1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("NewsCategory50", GetGridCell(49, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect100(driver, baseURL);
            Assert.AreEqual("NewsCategory1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("NewsCategory100", GetGridCell(99, "Name").Text);
        }

        [Test]
        public void NewzCategorySelectDelete()
        {
            GoToAdmin("newscategory");
            gridReturnDefaultView10();
            //check delete cancel 
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("NewsCategory1", GetGridCell(0, "Name").Text);

            //check delete
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            Assert.AreNotEqual("NewsCategory1", GetGridCell(0, "Name").Text);

            //check select 
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("3", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);

            //check delete selected items
            Functions.GridDropdownDelete(driver, baseURL);
            Assert.AreEqual("NewsCategory5", GetGridCell(0, "Name").Text);
            Assert.AreEqual("NewsCategory6", GetGridCell(1, "Name").Text);
            Assert.AreEqual("NewsCategory7", GetGridCell(2, "Name").Text);

            //check select all on page
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
            Assert.AreEqual("NewsCategory15", GetGridCell(0, "Name").Text);
            Assert.AreEqual("NewsCategory24", GetGridCell(9, "Name").Text);

            //check select all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("88", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);

            //check deselect all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsFalse(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsFalse(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);

            //check delete all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            GoToAdmin("newscategory");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }

        [Test]
        public void NewsCategorySearch()
        {
            GoToAdmin("newscategory");

            //search by exist name
            //GetGridFilter().Click();
            //GetGridFilter().Clear();
            GetGridFilter().SendKeys("NewsCategory33");
            DropFocus("h1");

            Assert.IsTrue(GetGridCell(0, "Name").Text.Contains("NewsCategory33"));

            //search by not exist name
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("NewsCategory4532");
             DropFocus("h1");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //search too much symbols
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            DropFocus("h1");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //search invalid symbols
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            DropFocus("h1");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }

        [Test]
        public void NewsCategoryInPlaceEdit()
        {
            GoToAdmin("newscategory");

            //GetGridFilter().Click();
            //GetGridFilter().Clear();
            GetGridFilter().SendKeys("NewsCategory55");
            DropFocus("h1");
            Assert.AreEqual("NewsCategory55", GetGridCell(0, "Name").Text);
            Assert.AreEqual("55", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));

            GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).SendKeys("44");
            DropFocus("h1");

            //check
            GoToAdmin("newscategory");

            //GetGridFilter().Click();
            //GetGridFilter().Clear();
            GetGridFilter().SendKeys("NewsCategory55");
             DropFocus("h1");
            Assert.AreEqual("NewsCategory55", GetGridCell(0, "Name").Text);
            Assert.AreEqual("44", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));
        }

        [Test]
        public void aNewsCategorySort()
        {
            GoToAdmin("newscategory");

            //check sort by name
            GetGridCell(-1, "Name").Click();
            WaitForAjax();
            Assert.IsTrue(GetGridCell(0, "Name").Text.Contains("NewsCategory1"));
            Assert.IsTrue(GetGridCell(9, "Name").Text.Contains("NewsCategory15"));

            GetGridCell(-1, "Name").Click();
            WaitForAjax();
            Assert.IsTrue(GetGridCell(0, "Name").Text.Contains("NewsCategory99"));
            Assert.IsTrue(GetGridCell(9, "Name").Text.Contains("NewsCategory90"));

            //sort by url path
            GetGridCell(-1, "UrlPath").Click();
            WaitForAjax();
            Assert.AreEqual("newscategory_url1", GetGridCell(0, "UrlPath").Text);
            Assert.AreEqual("newscategory_url15", GetGridCell(9, "UrlPath").Text);

            GetGridCell(-1, "UrlPath").Click();
            WaitForAjax();
            Assert.AreEqual("newscategory_url99", GetGridCell(0, "UrlPath").Text);
            Assert.AreEqual("newscategory_url90", GetGridCell(9, "UrlPath").Text);

            //sort by sort order
            GetGridCell(-1, "SortOrder").Click();
            WaitForAjax();
            Assert.AreEqual("1", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("10", GetGridCell(9, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));

            GetGridCell(-1, "SortOrder").Click();
            WaitForAjax();
            Assert.AreEqual("102", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("93", GetGridCell(9, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));
        }
    }
}
