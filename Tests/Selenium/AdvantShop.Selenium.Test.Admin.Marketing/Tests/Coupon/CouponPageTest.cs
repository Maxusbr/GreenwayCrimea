using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Marketing.Coupons
{
    [TestFixture]
    public class CouponPageTest: BaseSeleniumTest
    {
        [OneTimeSetUp]
    public void SetupTest()
    {
        InitializeService.RollBackDatabase();
        InitializeService.ClearData(ClearType.Catalog);
        InitializeService.LoadData(
        "data\\Admin\\Coupons\\ManyCoupon\\Catalog.Product.csv",
        "data\\Admin\\Coupons\\ManyCoupon\\Catalog.Photo.csv",
        "data\\Admin\\Coupons\\ManyCoupon\\Catalog.Offer.csv",
        "data\\Admin\\Coupons\\ManyCoupon\\Catalog.Category.csv",
        "data\\Admin\\Coupons\\ManyCoupon\\Catalog.ProductCategories.csv",
        "data\\Admin\\Coupons\\ManyCoupon\\Catalog.Brand.csv",
        "data\\Admin\\Coupons\\ManyCoupon\\Catalog.Color.csv",
        "data\\Admin\\Coupons\\ManyCoupon\\Catalog.Size.csv",
        "data\\Admin\\Coupons\\ManyCoupon\\Catalog.Coupon.csv",
        "data\\Admin\\Coupons\\ManyCoupon\\Catalog.CouponCategories.csv",
        "data\\Admin\\Coupons\\ManyCoupon\\Catalog.CouponProducts.csv"
       );

        Init();
    }

        [Test]
        public void PageCoupon()
        {
            testname = "PageCoupon";
            VerifyBegin(testname);
            GoToAdmin("coupons");
            gridReturnDefaultView10();
            VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 1");
            VerifyAreEqual("test10", GetGridCell(9, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("test11", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 11");
            VerifyAreEqual("test20", GetGridCell(9, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 20");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("test21", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 21");
            VerifyAreEqual("test30", GetGridCell(9, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 30");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("test31", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 31");
            VerifyAreEqual("test40", GetGridCell(9, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 40");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("test41", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 41");
            VerifyAreEqual("test50", GetGridCell(9, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 50");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("test51", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 51");
            VerifyAreEqual("test60", GetGridCell(9, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 60");
            VerifyFinally(testname);
        }

        [Test]
        public void PageCouponToBegin()
        {
            testname = "PageCouponToBegin";
            VerifyBegin(testname);
            GoToAdmin("coupons");
            gridReturnDefaultView10();
            VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 1");
            VerifyAreEqual("test10", GetGridCell(9, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("test11", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 11");
            VerifyAreEqual("test20", GetGridCell(9, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 20");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("test21", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 21");
            VerifyAreEqual("test30", GetGridCell(9, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 30");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("test31", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 31");
            VerifyAreEqual("test40", GetGridCell(9, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 40");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("test41", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 41");
            VerifyAreEqual("test50", GetGridCell(9, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 50");

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 1");
            VerifyAreEqual("test10", GetGridCell(9, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 10");
            VerifyFinally(testname);
        }

        [Test]
        public void PageCouponToEnd()
        {
            testname = "PageCouponToEnd";
            VerifyBegin(testname);
            GoToAdmin("coupons");
            gridReturnDefaultView10();
            VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 1");
            VerifyAreEqual("test10", GetGridCell(9, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 10");

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("test101", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 101");
            VerifyFinally(testname);
        }

        [Test]
        public void PageCouponToNext()
        {
            testname = "PageCouponToNext";
            VerifyBegin(testname);
            GoToAdmin("coupons");
            gridReturnDefaultView10();
            VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 1");
            VerifyAreEqual("test10", GetGridCell(9, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("test11", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 11");
            VerifyAreEqual("test20", GetGridCell(9, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 20");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("test21", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 21");
            VerifyAreEqual("test30", GetGridCell(9, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 30");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("test31", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 31");
            VerifyAreEqual("test40", GetGridCell(9, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 40");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("test41", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 41");
            VerifyAreEqual("test50", GetGridCell(9, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 50");
            VerifyFinally(testname);
        }

        [Test]
        public void PageCouponToPrevious()
        {
            testname = "PageCouponToPrevious";
            VerifyBegin(testname);
            GoToAdmin("coupons");
            gridReturnDefaultView10();
            VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 1");
            VerifyAreEqual("test10", GetGridCell(9, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("test11", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 11");
            VerifyAreEqual("test20", GetGridCell(9, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 20");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("test21", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 21");
            VerifyAreEqual("test30", GetGridCell(9, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 30");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("test11", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 11");
            VerifyAreEqual("test20", GetGridCell(9, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 20");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 1");
            VerifyAreEqual("test10", GetGridCell(9, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 10");
            VerifyFinally(testname);
        }
        [Test]
        public void CouponPresent()
        {
            testname = "CouponPresent";
            VerifyBegin(testname);
            GoToAdmin("coupons");
            Functions.GridPaginationSelect10(driver, baseURL);
            VerifyAreEqual("Найдено записей: 101", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "count all");
            VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 1");
            VerifyAreEqual("test10", GetGridCell(9, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 10");

            Functions.GridPaginationSelect20(driver, baseURL);
            VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 1");
            VerifyAreEqual("test20", GetGridCell(19, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 20");

            Functions.GridPaginationSelect50(driver, baseURL);
            VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 1");
            VerifyAreEqual("test50", GetGridCell(49, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 50");

            Functions.GridPaginationSelect100(driver, baseURL);
            VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 1");
            VerifyAreEqual("test100", GetGridCell(99, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "line 100");
            VerifyFinally(testname);
        }
        [Test]
        public void zSelectAndDelete()
        {
            testname = "SelectAndDelete";
            VerifyBegin(testname);
            GoToAdmin("coupons");
            gridReturnDefaultView10();
            //check delete cancel
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "cancel del");

            //check delete
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(3000);
            VerifyAreNotEqual("test1", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "del 1 elem");

            //check select 
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "Selected 1");
            VerifyIsTrue(GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "Selected 2");
            VerifyIsTrue(GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "Selected 3");
            VerifyAreEqual("3", driver.FindElement(By.ClassName("ui-grid-custom-selection-count")).Text);

            //check delete selected items
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreNotEqual("test2", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "after del  1");
            VerifyAreNotEqual("test3", GetGridCell(1, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "after del  2");
            VerifyAreNotEqual("test4", GetGridCell(2, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "after del  3");

            //check select all on page
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "Selected on page 1");
            VerifyIsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "Selected on page 10");

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("test15", GetGridCell(0, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "after del on page 1");
            VerifyAreEqual("test24", GetGridCell(9, "Code").FindElement(By.TagName("input")).GetAttribute("value"), "after del on page 10");

            //check select all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("87", driver.FindElement(By.ClassName("ui-grid-custom-selection-count")).Text, "count after del");

            //check deselect all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Thread.Sleep(2000);
            VerifyIsFalse(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "not Selected after del");
            VerifyIsFalse(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "not Selected after del");

            //check delete all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no element");

            Refresh();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no elem after refresh");
            VerifyFinally(testname);
        }
    }
}
