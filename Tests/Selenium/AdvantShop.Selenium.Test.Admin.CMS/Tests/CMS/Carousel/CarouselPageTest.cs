using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.CMS.Carousel
{
    [TestFixture]
    public class CarouselPageTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CMS | ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\CMS\\Carousel\\Catalog.Product.csv",
           "data\\Admin\\CMS\\Carousel\\Catalog.Offer.csv",
           "data\\Admin\\CMS\\Carousel\\Catalog.Category.csv",
           "data\\Admin\\CMS\\Carousel\\Catalog.ProductCategories.csv",
           "data\\Admin\\CMS\\Carousel\\CMS.Carousel.csv"
           );

            Init();
        }
        [Test]
        public void CarouselPage()
        {
            testname = "CarouselPage";
            VerifyBegin(testname);

            GoToAdmin("carousel");
            VerifyAreEqual("products/test-product1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 1");
            VerifyAreEqual("products/test-product10", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("products/test-product11", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"), "page 2 line 1");
            VerifyAreEqual("products/test-product20", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"), "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("products/test-product21", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"), "page 3 line 1");
            VerifyAreEqual("products/test-product30", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"), "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("products/test-product31", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"), "page 4 line 1");
            VerifyAreEqual("products/test-product40", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"), "page 4 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("products/test-product41", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"), "page 5 line 1");
            VerifyAreEqual("products/test-product50", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"), "page 5 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("products/test-product51", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"), "page 6 line 1");
            VerifyAreEqual("products/test-product60", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"), "page 6 line 10");
            
            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("products/test-product1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 1");
            VerifyAreEqual("products/test-product10", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void CarouselPageToPrevious()
        {
            testname = "CarouselPageToPrevious";
            VerifyBegin(testname);

            GoToAdmin("carousel");
            VerifyAreEqual("products/test-product1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 1");
            VerifyAreEqual("products/test-product10", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("products/test-product11", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"), "page 2 line 1");
            VerifyAreEqual("products/test-product20", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"), "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("products/test-product21", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"), "page 3 line 1");
            VerifyAreEqual("products/test-product30", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"), "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("products/test-product11", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"), "page 2 line 1");
            VerifyAreEqual("products/test-product20", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"), "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("products/test-product1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 1");
            VerifyAreEqual("products/test-product10", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"), "page 1 line 10");

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("products/test-product101", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"), "last page line 1");

            VerifyFinally(testname);
        }
      
        [Test]
        public void CarouselPresent()
        {
            GoToAdmin("carousel");
            Functions.GridPaginationSelect10(driver, baseURL);
            VerifyAreEqual("Найдено записей: 101", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
            VerifyAreEqual("products/test-product1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            VerifyAreEqual("products/test-product10", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect20(driver, baseURL);
            VerifyAreEqual("products/test-product1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            VerifyAreEqual("products/test-product20", GetGridCell(19, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect50(driver, baseURL);
            VerifyAreEqual("products/test-product1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            VerifyAreEqual("products/test-product50", GetGridCell(49, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect100(driver, baseURL);
            VerifyAreEqual("products/test-product1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            VerifyAreEqual("products/test-product100", GetGridCell(99, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            //back default
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect10(driver, baseURL);
        }
        [Test]
        public void zSelectAndDelete()
        {
            GoToAdmin("carousel");

            //check delete cancel
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("products/test-product1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            //check delete
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            Assert.AreNotEqual("test1", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            //check select 
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            VerifyAreEqual("3", driver.FindElement(By.ClassName("ui-grid-custom-selection-count")).Text);

            //check delete selected items
            Functions.GridDropdownDelete(driver, baseURL);
            Assert.AreNotEqual("products/test-product2", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreNotEqual("products/test-product3", GetGridCell(1, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreNotEqual("products/test-product4", GetGridCell(2, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            //check select all on page
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("products/test-product15", GetGridCell(0, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));
            VerifyAreEqual("products/test-product24", GetGridCell(9, "CaruselUrl").FindElement(By.TagName("input")).GetAttribute("value"));

            //check select all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("87", driver.FindElement(By.ClassName("ui-grid-custom-selection-count")).Text);

            //check deselect all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsFalse(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsFalse(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);

            //check delete all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            Refresh();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }
      
    }
}
