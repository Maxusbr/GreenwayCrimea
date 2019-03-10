using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;

namespace AdvantShop.SeleniumTest.Admin.Marketing.Certificates
{
    [TestFixture]
    public class CertificatePageTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog|ClearType.Orders);
            InitializeService.LoadData(
            "data\\Admin\\Certificates\\ManyCertificates\\Catalog.Product.csv",
            "data\\Admin\\Certificates\\ManyCertificates\\Catalog.Photo.csv",
            "data\\Admin\\Certificates\\ManyCertificates\\Catalog.Offer.csv",
            "data\\Admin\\Certificates\\ManyCertificates\\Catalog.Category.csv",
            "data\\Admin\\Certificates\\ManyCertificates\\Catalog.ProductCategories.csv",
            "data\\Admin\\Certificates\\ManyCertificates\\Catalog.Brand.csv",
            "data\\Admin\\Certificates\\ManyCertificates\\Catalog.Color.csv",
            "data\\Admin\\Certificates\\ManyCertificates\\Catalog.Size.csv",
            "data\\Admin\\Certificates\\ManyCertificates\\Catalog.Coupon.csv",
            "data\\Admin\\Certificates\\ManyCertificates\\Catalog.CouponCategories.csv",
            "data\\Admin\\Certificates\\ManyCertificates\\Catalog.CouponProducts.csv",
             "data\\Admin\\Certificates\\ManyCertificates\\[Order].Certificate.csv",
            "data\\Admin\\Certificates\\ManyCertificates\\[Order].OrderContact.csv",
            "data\\Admin\\Certificates\\ManyCertificates\\[Order].OrderCurrency.csv",
             "data\\Admin\\Certificates\\ManyCertificates\\[Order].OrderItems.csv",
             "data\\Admin\\Certificates\\ManyCertificates\\[Order].OrderStatus.csv",
             "data\\Admin\\Certificates\\ManyCertificates\\[Order].[Order].csv",
             "Data\\Admin\\Certificates\\ManyCertificates\\[Order].OrderSource.csv"
           );

            Init();
        }

        [Test]
        public void PageCertificate()
        {
            testname = "PageCertificate";
            VerifyBegin(testname);

            GoToAdmin("certificates");
            gridReturnDefaultView10();
            VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "line 1");
            VerifyAreEqual("Certificate10", GetGridCell(9, "CertificateCode").Text, "line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Certificate11", GetGridCell(0, "CertificateCode").Text, "line 11");
            VerifyAreEqual("Certificate20", GetGridCell(9, "CertificateCode").Text, "line 20");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Certificate21", GetGridCell(0, "CertificateCode").Text, "line 21");
            VerifyAreEqual("Certificate30", GetGridCell(9, "CertificateCode").Text, "line 30");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Certificate31", GetGridCell(0, "CertificateCode").Text, "line 31");
            VerifyAreEqual("Certificate40", GetGridCell(9, "CertificateCode").Text, "line 40");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Certificate41", GetGridCell(0, "CertificateCode").Text, "line 41");
            VerifyAreEqual("Certificate50", GetGridCell(9, "CertificateCode").Text, "line 50");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Certificate51", GetGridCell(0, "CertificateCode").Text, "line 51");
            VerifyAreEqual("Certificate60", GetGridCell(9, "CertificateCode").Text, "line 60");

            VerifyFinally(testname);
        }

        [Test]
        public void PageCertificateToBegin()
        {
            testname = "PageCertificateToBegin";
            VerifyBegin(testname);

            GoToAdmin("certificates");
            gridReturnDefaultView10();
            VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "line 1");
            VerifyAreEqual("Certificate10", GetGridCell(9, "CertificateCode").Text, "line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Certificate11", GetGridCell(0, "CertificateCode").Text, "line 11");
            VerifyAreEqual("Certificate20", GetGridCell(9, "CertificateCode").Text, "line 20");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Certificate21", GetGridCell(0, "CertificateCode").Text, "line 21");
            VerifyAreEqual("Certificate30", GetGridCell(9, "CertificateCode").Text, "line 30");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Certificate31", GetGridCell(0, "CertificateCode").Text, "line 31");
            VerifyAreEqual("Certificate40", GetGridCell(9, "CertificateCode").Text, "line 40");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Certificate41", GetGridCell(0, "CertificateCode").Text, "line 41");
            VerifyAreEqual("Certificate50", GetGridCell(9, "CertificateCode").Text, "line 50");

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "line 1");
            VerifyAreEqual("Certificate10", GetGridCell(9, "CertificateCode").Text, "line 10");
            VerifyFinally(testname);
        }

        [Test]
        public void PageCertificateToEnd()
        {
            testname = "PageCertificateToEnd";
            VerifyBegin(testname);

            GoToAdmin("certificates");
            gridReturnDefaultView10();
            VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "line 1");
            VerifyAreEqual("Certificate10", GetGridCell(9, "CertificateCode").Text, "line 10");

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Certificate101", GetGridCell(0, "CertificateCode").Text, "line 101");
            VerifyFinally(testname);
        }

        [Test]
        public void PageCertificateToNext()
        {
            testname = "PageCertificateToNext";
            VerifyBegin(testname);

            GoToAdmin("certificates");
            gridReturnDefaultView10();
            VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "line 1");
            VerifyAreEqual("Certificate10", GetGridCell(9, "CertificateCode").Text, "line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Certificate11", GetGridCell(0, "CertificateCode").Text, "line 11");
            VerifyAreEqual("Certificate20", GetGridCell(9, "CertificateCode").Text, "line 20");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Certificate21", GetGridCell(0, "CertificateCode").Text, "line 21");
            VerifyAreEqual("Certificate30", GetGridCell(9, "CertificateCode").Text, "line 30");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Certificate31", GetGridCell(0, "CertificateCode").Text, "line 31");
            VerifyAreEqual("Certificate40", GetGridCell(9, "CertificateCode").Text, "line 40");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Certificate41", GetGridCell(0, "CertificateCode").Text, "line 41");
            VerifyAreEqual("Certificate50", GetGridCell(9, "CertificateCode").Text, "line 50");
            VerifyFinally(testname);
        }

        [Test]
        public void PageCertificateToPrevious()
        {

            testname = "PageCertificateToPrevious";
            VerifyBegin(testname);
            GoToAdmin("certificates");
            gridReturnDefaultView10();
            VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "line 1");
            VerifyAreEqual("Certificate10", GetGridCell(9, "CertificateCode").Text, "line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Certificate11", GetGridCell(0, "CertificateCode").Text, "line 11");
            VerifyAreEqual("Certificate20", GetGridCell(9, "CertificateCode").Text, "line 20");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Certificate21", GetGridCell(0, "CertificateCode").Text, "line 21");
            VerifyAreEqual("Certificate30", GetGridCell(9, "CertificateCode").Text, "line 30");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Certificate11", GetGridCell(0, "CertificateCode").Text, "line 11");
            VerifyAreEqual("Certificate20", GetGridCell(9, "CertificateCode").Text, "line 20");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "line 1");
            VerifyAreEqual("Certificate10", GetGridCell(9, "CertificateCode").Text, "line 10");
            VerifyFinally(testname);
        }
        [Test]
        public void CertificatePresent()
        {
            testname = "CertificatePresent";
            VerifyBegin(testname);

            GoToAdmin("certificates");
            Functions.GridPaginationSelect10(driver, baseURL);
            VerifyAreEqual("Найдено записей: 101", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text, "find elem 101");
            VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "line 1");
            VerifyAreEqual("Certificate10", GetGridCell(9, "CertificateCode").Text, "line 10");

            Functions.GridPaginationSelect20(driver, baseURL);
            VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "line 1");
            VerifyAreEqual("Certificate20", GetGridCell(19, "CertificateCode").Text, "line 20");

            Functions.GridPaginationSelect50(driver, baseURL);
            VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "line 1");
            VerifyAreEqual("Certificate50", GetGridCell(49, "CertificateCode").Text, "line 50");

            Functions.GridPaginationSelect100(driver, baseURL);
            VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "line 1");
            VerifyAreEqual("Certificate100", GetGridCell(99, "CertificateCode").Text, "line 100");
            VerifyFinally(testname);
        }
        [Test]
        public void zSelectAndDelete()
        {
            testname = "SelectAndDelete";
            VerifyBegin(testname);

            GoToAdmin("certificates");
            gridReturnDefaultView10();
            //check delete cancel
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, "cancel del 1 element");

            //check delete
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(3000);
            Assert.AreNotEqual("Certificate1", GetGridCell(0, "CertificateCode").Text, " del 1 element");

            //check select 
            GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "Selected 1");
            VerifyIsTrue(GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "Selected 2");
            VerifyIsTrue(GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "Selected 3");
            VerifyAreEqual("3", driver.FindElement(By.ClassName("ui-grid-custom-selection-count")).Text, " count Selected");

            //check delete selected items
            Functions.GridDropdownDelete(driver, baseURL);
            Assert.AreNotEqual("Certificate2", GetGridCell(0, "CertificateCode").Text, "del 2 element");
            Assert.AreNotEqual("Certificate3", GetGridCell(1, "CertificateCode").Text, "del 3 element");
            Assert.AreNotEqual("Certificate4", GetGridCell(2, "CertificateCode").Text, "del 4 element");

            //check select all on page
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            VerifyIsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "Selected page 1");
            VerifyIsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "Selected pege 10");

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyAreEqual("Certificate15", GetGridCell(0, "CertificateCode").Text, "del Selected page 1");
            VerifyAreEqual("Certificate24", GetGridCell(9, "CertificateCode").Text, "gel Selected page 1");

            //check select all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(2000);
            VerifyAreEqual("87", driver.FindElement(By.ClassName("ui-grid-custom-selection-count")).Text, "after del on page");

            //check deselect all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsFalse(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "Selected all 1");
            Assert.IsFalse(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected, "Selected all 10");

            //check delete all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "no element");

            Refresh();
            VerifyIsTrue(driver.PageSource.Contains("Ни одной записи не найдено"), "after refresh");
            VerifyFinally(testname);
        }
    }
}
