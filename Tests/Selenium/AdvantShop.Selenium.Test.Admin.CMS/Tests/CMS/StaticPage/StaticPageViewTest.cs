using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.CMS.StaticPage
{
    [TestFixture]
    public class StaticPageViewTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CMS);
            InitializeService.LoadData(
           "data\\Admin\\CMS\\StaticPage\\CMS.StaticPage.csv"
           );

            Init();
        }
       
        [Test]
        public void PageStaticPages()
        {
            GoToAdmin("staticpages");
            Assert.AreEqual("Page1", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page10", GetGridCell(9, "PageName").Text);

           ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Page11", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page20", GetGridCell(9, "PageName").Text);

           ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Page21", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page30", GetGridCell(9, "PageName").Text);

           ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Page31", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page40", GetGridCell(9, "PageName").Text);

           ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Page41", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page50", GetGridCell(9, "PageName").Text);

           ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Page51", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page60", GetGridCell(9, "PageName").Text);
        }

        [Test]
        public void PageStaticPagesToBegin()
        {
            GoToAdmin("staticpages");
            Assert.AreEqual("Page1", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page10", GetGridCell(9, "PageName").Text);

           ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Page11", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page20", GetGridCell(9, "PageName").Text);

           ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Page21", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page30", GetGridCell(9, "PageName").Text);

           ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Page31", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page40", GetGridCell(9, "PageName").Text);

           ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Page41", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page50", GetGridCell(9, "PageName").Text);

            //to begin
           ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Page1", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page10", GetGridCell(9, "PageName").Text);
        }

        [Test]
        public void PageStaticPagesToEnd()
        {
            GoToAdmin("staticpages");
            Assert.AreEqual("Page1", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page10", GetGridCell(9, "PageName").Text);

            //to end
           ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Page101", GetGridCell(0, "PageName").Text);
        }

        [Test]
        public void PageStaticPagesToNext()
        {
            GoToAdmin("staticpages");
            Assert.AreEqual("Page1", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page10", GetGridCell(9, "PageName").Text);

           ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Page11", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page20", GetGridCell(9, "PageName").Text);

           ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Page21", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page30", GetGridCell(9, "PageName").Text);

           ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Page31", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page40", GetGridCell(9, "PageName").Text);

           ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Page41", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page50", GetGridCell(9, "PageName").Text);
        }

        [Test]
        public void PageStaticPagesToPrevious()
        {
            GoToAdmin("staticpages");
            Assert.AreEqual("Page1", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page10", GetGridCell(9, "PageName").Text);

           ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Page11", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page20", GetGridCell(9, "PageName").Text);

           ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Page21", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page30", GetGridCell(9, "PageName").Text);

           ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Page11", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page20", GetGridCell(9, "PageName").Text);

           ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Page1", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page10", GetGridCell(9, "PageName").Text);
        }
        [Test]
        public void StaticPagesPresent()
        {
            GoToAdmin("staticpages");
            Functions.GridPaginationSelect10(driver, baseURL);
            Assert.AreEqual("Найдено записей: 101", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
            Assert.AreEqual("Page1", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page10", GetGridCell(9, "PageName").Text);

            Functions.GridPaginationSelect20(driver, baseURL);
            Assert.AreEqual("Page1", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page20",GetGridCell(19, "PageName").Text);

            Functions.GridPaginationSelect50(driver, baseURL);
            Assert.AreEqual("Page1", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page50", GetGridCell(49, "PageName").Text);

            Functions.GridPaginationSelect100(driver, baseURL);
            Assert.AreEqual("Page1", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page100", GetGridCell(99, "PageName").Text);
        }
        [Test]
        public void zSelectAndDelete()
        {
            GoToAdmin("staticpages");
            gridReturnDefaultView10();
            //check delete cancel
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Page1", GetGridCell(0, "PageName").Text);

            //check delete
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            Assert.AreNotEqual("Page1", GetGridCell(0, "PageName").Text);

            //check select 
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("3", driver.FindElement(By.ClassName("ui-grid-custom-selection-count")).Text);

            //check delete selected items
            Functions.GridDropdownDelete(driver, baseURL);
            Assert.AreNotEqual("Page2", GetGridCell(0, "PageName").Text);
            Assert.AreNotEqual("Page3", GetGridCell(1, "PageName").Text);
            Assert.AreNotEqual("Page4", GetGridCell(2, "PageName").Text);

            //check select all on page
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
            Assert.AreEqual("Page15", GetGridCell(0, "PageName").Text);
            Assert.AreEqual("Page24", GetGridCell(9, "PageName").Text);

            //check select all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("87", driver.FindElement(By.ClassName("ui-grid-custom-selection-count")).Text);

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
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            Refresh();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }
    }
}
