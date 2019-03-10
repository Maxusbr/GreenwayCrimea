using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
namespace AdvantShop.Web.Site.Selenium.Test.Admin.CMS.StaticBlock
{
    [TestFixture]
    public class CMSStaticBlockTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog | ClearType.CMS);
            InitializeService.LoadData(
           "data\\Admin\\CMS\\StaticBlock\\Catalog.Product.csv",
            "data\\Admin\\CMS\\StaticBlock\\Catalog.Offer.csv",
            "data\\Admin\\CMS\\StaticBlock\\Catalog.Category.csv",
            "data\\Admin\\CMS\\StaticBlock\\Catalog.ProductCategories.csv", 
           "data\\Admin\\CMS\\StaticBlock\\CMS.StaticBlock.csv",
               "data\\Admin\\CMS\\StaticBlock\\CMS.StaticPage.csv"

           );

            Init();
        }

        [Test]
        public void CheckGrid()
        {
           GoToAdmin("staticblock");

            //check admin
            Assert.AreEqual("bannerDetails", GetGridCell(0, "Key").Text);
            Assert.AreEqual("Баннер, единый для всех товаров", GetGridCell(0, "InnerName").Text);
            Assert.AreEqual("13.08.2012 10:59", GetGridCell(0, "AddedFormatted").Text);
            Assert.AreEqual("13.08.2016 11:09", GetGridCell(0, "ModifiedFormatted").Text);
            Assert.IsTrue(GetGridCell(0, "Enabled").FindElement(By.TagName("input")).Selected);
            Assert.AreEqual("Найдено записей: 150", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            //check client
            GoToClient("products/test-product1");
            Assert.IsTrue(driver.FindElement(By.CssSelector(".details-aside")).FindElement(By.CssSelector(".static-block")).Displayed);
            Assert.IsTrue(driver.FindElement(By.CssSelector(".details-aside")).FindElement(By.CssSelector(".static-block")).Text.Contains("баннер, единый для всех товаров содержание тест"));
        }

        [Test]
        public void GridInplace()
        {
            GoToAdmin("staticblock");
            
            Assert.AreEqual("bannerDetails", GetGridCell(0, "Key").Text);
            Assert.IsTrue(GetGridCell(0, "Enabled").FindElement(By.TagName("input")).Selected);

            GetGridCell(0, "Enabled").Click();
            Thread.Sleep(500);

            //check admin
            GoToAdmin("staticblock");

            Assert.AreEqual("bannerDetails", GetGridCell(0, "Key").Text);
            Assert.IsFalse(GetGridCell(0, "Enabled").FindElement(By.TagName("input")).Selected);

            //check client
            GoToClient("products/test-product1");
            Assert.IsFalse(driver.FindElement(By.CssSelector(".details-aside")).FindElements(By.CssSelector(".static-block")).Count > 0);
            Assert.IsFalse(driver.PageSource.Contains("баннер, единый для всех товаров содержание тест"));
        }

        [Test]
        public void GridSelectEnabled()
        {
            GoToAdmin("staticblock");
            
            //do not active
            Assert.IsTrue(GetGridCell(2, "Enabled").FindElement(By.TagName("input")).Selected);
            Assert.IsTrue(GetGridCell(3, "Enabled").FindElement(By.TagName("input")).Selected);

            GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(3, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"2\"]")).Click();
            Thread.Sleep(1000);

            GoToAdmin("staticblock");

            Assert.IsFalse(GetGridCell(2, "Enabled").FindElement(By.TagName("input")).Selected);
            Assert.IsFalse(GetGridCell(3, "Enabled").FindElement(By.TagName("input")).Selected);

            //do active
            GoToAdmin("staticblock");

            Assert.IsFalse(GetGridCell(2, "Enabled").FindElement(By.TagName("input")).Selected);
            Assert.IsFalse(GetGridCell(3, "Enabled").FindElement(By.TagName("input")).Selected);

            GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(3, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();

            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"1\"]")).Click();
            Thread.Sleep(1000);

            GoToAdmin("staticblock");

            Assert.IsTrue(GetGridCell(2, "Enabled").FindElement(By.TagName("input")).Selected);
            Assert.IsTrue(GetGridCell(3, "Enabled").FindElement(By.TagName("input")).Selected);

        }

        [Test]
        public void GridSearch()
        {
             GoToAdmin("staticblock");

            //search by exist inner name
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("inner name 81");
            DropFocus("h1");
            WaitForAjax();

            Assert.IsTrue(GetGridCell(0, "InnerName").Text.Contains("inner name 81"));
            Assert.AreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            //search by exist key
            GoToAdmin("staticblock");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("staticblockkey112");
            DropFocus("h1");
            WaitForAjax();

            Assert.IsTrue(GetGridCell(0, "Key").Text.Contains("staticblockkey112"));
            Assert.AreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            //search by not exist name
            GoToAdmin("staticblock");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("inner name 5000");
            DropFocus("h1");
            WaitForAjax();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
            
            //search too much symbols
            GoToAdmin("staticblock");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            DropFocus("h1");
            WaitForAjax();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //search invalid symbols
            GoToAdmin("staticblock");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            DropFocus("h1");
            WaitForAjax();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }
        
        [Test]
        public void PresentSelectOnPage()
        {
            GoToAdmin("staticblock");
            
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Functions.GridPaginationSelect10(driver, baseURL);
            Assert.AreEqual("bannerDetails", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey106", GetGridCell(9, "Key").Text);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.TagName("input")).Selected);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.TagName("input")).Selected);

            GoToAdmin("staticblock");
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect20(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Assert.AreEqual("bannerDetails", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey115", GetGridCell(19, "Key").Text);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.TagName("input")).Selected);
            Assert.IsTrue(GetGridCell(19, "selectionRowHeaderCol").FindElement(By.TagName("input")).Selected);
        }

        [Test]
        public void zSelectDelete()
        {
            GoToAdmin("staticblock");
            gridReturnDefaultView10();
            //check delete cancel 
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("bannerDetails", GetGridCell(0, "Key").Text);

            //check delete
            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-times.link-invert")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            Assert.AreNotEqual("bannerDetails", GetGridCell(0, "Key").Text);

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
            Assert.AreNotEqual("staticblockkey1", GetGridCell(0, "Key").Text);
            Assert.AreNotEqual("staticblockkey10", GetGridCell(1, "Key").Text);
            Assert.AreNotEqual("staticblockkey100", GetGridCell(2, "Key").Text);

            //check select all on page
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
            Assert.AreEqual("staticblockkey110", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey119", GetGridCell(9, "Key").Text);

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

            GoToAdmin("staticblock");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }

        [Test]
        public void aSort()
        {
            GoToAdmin("staticblock");
            
            //check sort by key
            GetGridCell(-1, "Key").Click();
            WaitForAjax();
            Assert.AreEqual("bannerDetails", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey106", GetGridCell(9, "Key").Text);

            GetGridCell(-1, "Key").Click();
            WaitForAjax();
            Assert.AreEqual("staticblockkey99", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey90", GetGridCell(9, "Key").Text);

            //sort by inner name
            GetGridCell(-1, "InnerName").Click();
            WaitForAjax();
            Assert.AreEqual("inner name 1", GetGridCell(0, "InnerName").Text);
            Assert.AreEqual("inner name 107", GetGridCell(9, "InnerName").Text);

            GetGridCell(-1, "InnerName").Click();
            WaitForAjax();
            Assert.AreEqual("Баннер, единый для всех товаров", GetGridCell(0, "InnerName").Text);
            Assert.AreEqual("inner name 91", GetGridCell(9, "InnerName").Text);

            //sort by add date
            GetGridCell(-1, "AddedFormatted").Click();
            WaitForAjax();
            Assert.AreEqual("13.08.2012 10:59", GetGridCell(0, "AddedFormatted").Text);
            Assert.AreEqual("21.09.2013 11:11", GetGridCell(9, "AddedFormatted").Text);

            GetGridCell(-1, "AddedFormatted").Click();
            WaitForAjax();
            Assert.AreEqual("08.02.2014 11:11", GetGridCell(0, "AddedFormatted").Text);
            Assert.AreEqual("30.01.2014 11:11", GetGridCell(9, "AddedFormatted").Text);
            
            //sort by ModifiedFormatted date
            GetGridCell(-1, "ModifiedFormatted").Click();
            WaitForAjax();
            Assert.AreEqual("13.08.2016 11:09", GetGridCell(0, "ModifiedFormatted").Text);
            Assert.AreEqual("22.08.2016 11:09", GetGridCell(9, "ModifiedFormatted").Text);

            GetGridCell(-1, "ModifiedFormatted").Click();
            WaitForAjax();
            Assert.AreEqual("09.01.2017 11:09", GetGridCell(0, "ModifiedFormatted").Text);
            Assert.AreEqual("31.12.2016 11:09", GetGridCell(9, "ModifiedFormatted").Text);

            //sort by enabled
            GetGridCell(-1, "Enabled").Click();
            WaitForAjax();
            Assert.IsFalse(GetGridCell(0, "Enabled").FindElement(By.TagName("input")).Selected);
            Assert.IsFalse(GetGridCell(1, "Enabled").FindElement(By.TagName("input")).Selected);
            Assert.IsFalse(GetGridCell(2, "Enabled").FindElement(By.TagName("input")).Selected);

            GetGridCell(-1, "Enabled").Click();
            WaitForAjax();
            Assert.IsTrue(GetGridCell(0, "Enabled").FindElement(By.TagName("input")).Selected);
            Assert.IsTrue(GetGridCell(1, "Enabled").FindElement(By.TagName("input")).Selected);
            Assert.IsTrue(GetGridCell(2, "Enabled").FindElement(By.TagName("input")).Selected);
            Assert.IsTrue(GetGridCell(9, "Enabled").FindElement(By.TagName("input")).Selected);
        }

        [Test]
        public void aView()
        {
            GoToAdmin("staticblock");

            Assert.AreEqual("bannerDetails", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey106", GetGridCell(9, "Key").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect20(driver, baseURL);
            Assert.AreEqual("bannerDetails", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey115", GetGridCell(19, "Key").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect50(driver, baseURL);
            Assert.AreEqual("bannerDetails", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey142", GetGridCell(49, "Key").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect100(driver, baseURL);
            Assert.AreEqual("bannerDetails", GetGridCell(0, "Key").Text);
            Assert.AreEqual("staticblockkey53", GetGridCell(99, "Key").Text);
        }
    }
}