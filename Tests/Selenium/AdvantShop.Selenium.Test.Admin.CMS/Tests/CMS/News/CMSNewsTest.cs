using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.CMS.News
{
    [TestFixture]
    public class CMSNewsTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CMS | ClearType.Catalog);
            InitializeService.LoadData(
                "data\\Admin\\CMS\\News\\Settings.News.csv",
                "data\\Admin\\CMS\\News\\Settings.NewsCategory.csv",
                "data\\Admin\\CMS\\News\\Catalog.Photo.csv"
           );

            Init();
        }

        [Test]
        public void NewsOpen()
        {
            GoToAdmin("news");

            //check admin
            Assert.AreEqual("Найдено записей: 150", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            Assert.IsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Новости"));

            Assert.AreEqual("title Test News 145", GetGridCell(0, "Title").Text);
            Assert.AreEqual("NewsCategory3", GetGridCell(0, "NewsCategory").Text);
            Assert.AreEqual("26.12.2014 10:40", GetGridCell(0, "AddingDateFormatted").Text);
            Assert.IsFalse(GetGridCell(0, "ShowOnMainPage").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.IsTrue(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            //check client
            GoToClient("news/test_news_145");

            Assert.IsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("title Test News 145"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".news-categories.block")).Text.Contains("NewsCategory3"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".breads")).Text.Contains("NewsCategory3"));

            Assert.IsTrue(driver.PageSource.Contains("Текст новости 145"));
            Assert.IsFalse(driver.PageSource.Contains("Текст аннотации 145"));
            Assert.IsTrue(driver.PageSource.Contains("26 декабря 2014"));

            GoToClient("newscategory/newscategory_url3");

            Assert.IsFalse(driver.FindElement(By.CssSelector("[alt=\"title Test News 145\"]")).GetAttribute("src").Contains("nophoto"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".news-link-title")).Text.Contains("title Test News 145"));
            Assert.IsFalse(driver.PageSource.Contains("Текст новости 145"));
            Assert.IsTrue(driver.PageSource.Contains("Текст аннотации 145"));

            GoToClient();

            Assert.IsFalse(driver.FindElement(By.CssSelector(".block.news-block")).Text.Contains("title Test News 145"));
        }

        [Test]
        public void NewsView()
        {
            GoToAdmin("news");

            Assert.AreEqual("title Test News 145", GetGridCell(0, "Title").Text);
            Assert.AreEqual("title Test News 135", GetGridCell(9, "Title").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect20(driver, baseURL);
            Assert.AreEqual("title Test News 145", GetGridCell(0, "Title").Text);
            Assert.AreEqual("title Test News 111", GetGridCell(19, "Title").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect50(driver, baseURL);
            Assert.AreEqual("title Test News 145", GetGridCell(0, "Title").Text);
            Assert.AreEqual("title Test News 55", GetGridCell(49, "Title").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect100(driver, baseURL);
            Assert.AreEqual("title Test News 145", GetGridCell(0, "Title").Text);
            Assert.AreEqual("title Test News 104", GetGridCell(99, "Title").Text);
        }

        [Test]
        public void NewzSelectDelete()
        {
            GoToAdmin("news");
            gridReturnDefaultView10();
            //check delete cancel 
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("title Test News 145", GetGridCell(0, "Title").Text);

            //check delete
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            Assert.AreNotEqual("title Test News 145", GetGridCell(0, "Title").Text);

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
            Assert.AreEqual("title Test News 141", GetGridCell(0, "Title").Text);
            Assert.AreEqual("title Test News 143", GetGridCell(1, "Title").Text);
            Assert.AreEqual("title Test News 137", GetGridCell(2, "Title").Text);

            //check select all on page
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
            Assert.AreEqual("title Test News 119", GetGridCell(0, "Title").Text);
            Assert.AreEqual("title Test News 99", GetGridCell(9, "Title").Text);
            Assert.AreEqual("Найдено записей: 136", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            //check select all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("136", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);

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

            GoToAdmin("news");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }

        [Test]
        public void NewsSearch()
        {
            GoToAdmin("news");

            //search by exist name
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("title Test News 114");
            DropFocus("h1");
            Blur();
            Assert.IsTrue(GetGridCell(0, "Title").Text.Contains("title Test News 114"));

            //search by not exist name
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("title Tests News 1142312313");
            DropFocus("h1");
            Blur();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //search too much symbols
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            DropFocus("h1");
            Blur();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //search invalid symbols
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            DropFocus("h1");
            Blur();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }

        [Test]
        public void NewsInplaceShowOnMainPage()
        {
            //pre check
            GoToClient();

            Assert.IsFalse(driver.FindElement(By.CssSelector(".block.news-block")).Text.Contains("title Test News 147"));

            GoToAdmin("news");

            Assert.AreEqual("title Test News 147", GetGridCell(1, "Title").Text);
            Assert.IsFalse(GetGridCell(1, "ShowOnMainPage").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            GetGridCell(1, "ShowOnMainPage").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();
            WaitForAjax();

            Assert.AreEqual("title Test News 147", GetGridCell(1, "Title").Text);
            Assert.IsTrue(GetGridCell(1, "ShowOnMainPage").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            //check after refreshing
            GoToAdmin("news");

            Assert.AreEqual("title Test News 147", GetGridCell(1, "Title").Text);
            Assert.IsTrue(GetGridCell(1, "ShowOnMainPage").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            //check client
            GoToClient();

            Assert.IsTrue(driver.FindElement(By.CssSelector(".block.news-block")).Text.Contains("title Test News 147"));
        }

        [Test]
        public void NewsInplaceEnabled()
        {
            //pre check
            Assert.IsFalse(Is404Page("news/test_news_135"));

            GoToAdmin("news");

            Assert.AreEqual("title Test News 135", GetGridCell(9, "Title").Text);
            Assert.IsTrue(GetGridCell(9, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            GetGridCell(9, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();
            WaitForAjax();

            Assert.AreEqual("title Test News 135", GetGridCell(9, "Title").Text);
            Assert.IsFalse(GetGridCell(9, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            //check after refreshing
            GoToAdmin("news");

            Assert.AreEqual("title Test News 135", GetGridCell(9, "Title").Text);
            Assert.IsFalse(GetGridCell(9, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            //check client
            Assert.IsTrue(Is404Page("news/test_news_135"));
        }
    }

    [TestFixture]
    public class CMSNewsSortTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CMS | ClearType.Catalog);
            InitializeService.LoadData(
                "data\\Admin\\CMS\\News\\Settings.News.csv",
                "data\\Admin\\CMS\\News\\Settings.NewsCategory.csv",
                "data\\Admin\\CMS\\News\\Catalog.Photo.csv"
           );

            Init();
        }


        [Test]
        public void NewsSort()
        {
            GoToAdmin("news");

            //check sort by name
            GetGridCell(-1, "Title").Click();
            WaitForAjax();
            Assert.IsTrue(GetGridCell(0, "Title").Text.Contains("title Test News 1"));
            Assert.IsTrue(GetGridCell(9, "Title").Text.Contains("title Test News 107"));

            GetGridCell(-1, "Title").Click();
            WaitForAjax();
            Assert.IsTrue(GetGridCell(0, "Title").Text.Contains("title Test News 99"));
            Assert.IsTrue(GetGridCell(9, "Title").Text.Contains("title Test News 90"));

            //sort by news category
            GetGridCell(-1, "NewsCategory").Click();
            WaitForAjax();
            Assert.AreEqual("NewsCategory1", GetGridCell(0, "NewsCategory").Text);
            Assert.AreEqual("NewsCategory1", GetGridCell(9, "NewsCategory").Text);

            GetGridCell(-1, "NewsCategory").Click();
            WaitForAjax();
            Assert.AreEqual("NewsCategory3", GetGridCell(0, "NewsCategory").Text);
            Assert.AreEqual("NewsCategory3", GetGridCell(9, "NewsCategory").Text);

            //sort by adding date
            GetGridCell(-1, "AddingDateFormatted").Click();
            WaitForAjax();
            Assert.AreEqual("03.10.2012 00:00", GetGridCell(0, "AddingDateFormatted").Text);
            Assert.AreEqual("03.10.2012 00:00", GetGridCell(9, "AddingDateFormatted").Text);

            GetGridCell(-1, "AddingDateFormatted").Click();
            WaitForAjax();
            Assert.AreEqual("26.12.2014 10:40", GetGridCell(0, "AddingDateFormatted").Text);
            Assert.AreEqual("26.12.2014 10:40", GetGridCell(9, "AddingDateFormatted").Text);

            //sort by show on main page
            GetGridCell(-1, "ShowOnMainPage").Click();
            WaitForAjax();
            Assert.IsFalse(GetGridCell(0, "ShowOnMainPage").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.IsFalse(GetGridCell(9, "ShowOnMainPage").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            GetGridCell(-1, "ShowOnMainPage").Click();
            WaitForAjax();
            Assert.IsTrue(GetGridCell(0, "ShowOnMainPage").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.IsTrue(GetGridCell(1, "ShowOnMainPage").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.IsTrue(GetGridCell(2, "ShowOnMainPage").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.IsFalse(GetGridCell(9, "ShowOnMainPage").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            //sort by enabled
            GetGridCell(-1, "Enabled").Click();
            WaitForAjax();
            Assert.IsFalse(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.IsFalse(GetGridCell(9, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            GetGridCell(-1, "Enabled").Click();
            WaitForAjax();
            Assert.IsTrue(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
        }
    }
}
