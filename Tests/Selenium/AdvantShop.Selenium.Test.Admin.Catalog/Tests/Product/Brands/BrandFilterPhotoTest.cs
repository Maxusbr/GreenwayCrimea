using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.Web.Site.Selenium.Test.Admin.Brand
{
    [TestFixture]
    public class BrandFilterPhotoTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Catalog\\Brands\\BrandFilters\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\Brands\\BrandFilters\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\Brands\\BrandFilters\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\Brands\\BrandFilters\\Catalog.Brand.csv",
           "data\\Admin\\Catalog\\Brands\\BrandFilters\\Catalog.ProductCategories.csv",
             "data\\Admin\\Catalog\\Brands\\BrandFilters\\Catalog.Photo.csv"
           );

            Init();
        }


        [Test]
        public void BrandFilterPhoto()
        {
            GoToAdmin("brands");

            Assert.AreEqual("Найдено записей: 200", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            //check with photo
            Functions.GridFilterSet(driver, baseURL, name: "PhotoSrc");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("С фотографией");
            DropFocus("h1");
            WaitForAjax();
            Assert.IsFalse(GetGridCell(0, "PhotoSrc").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            Assert.IsFalse(GetGridCell(9, "PhotoSrc").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            Assert.AreEqual("Найдено записей: 100", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            //check without photo
            GoToAdmin("brands");

            Functions.GridFilterSet(driver, baseURL, name: "PhotoSrc");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Без фотографии");
            DropFocus("h1");
            WaitForAjax();
            Assert.IsTrue(GetGridCell(0, "PhotoSrc").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            Assert.IsTrue(GetGridCell(9, "PhotoSrc").FindElement(By.TagName("img")).GetAttribute("src").Contains("nophoto"));
            Assert.AreEqual("Найдено записей: 100", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

        }

        [Test]
        public void BrandFilterPhotoPresent()
        {
            GoToAdmin("brands");

            Functions.GridFilterSet(driver, baseURL, name: "PhotoSrc");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("С фотографией");
            DropFocus("h1");
            WaitForAjax();
            Functions.GridPaginationSelect10(driver, baseURL);
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName10", GetGridCell(9, "BrandName").Text);

            Functions.GridPaginationSelect20(driver, baseURL);
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName20", GetGridCell(19, "BrandName").Text);

            Functions.GridPaginationSelect50(driver, baseURL);
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName50", GetGridCell(49, "BrandName").Text);

            Functions.GridPaginationSelect100(driver, baseURL);
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName130", GetGridCell(99, "BrandName").Text);
        }

        [Test]
        public void BrandFilterPhotoSelectAndDelete()
        {
            GoToAdmin("brands");

            Functions.GridFilterSet(driver, baseURL, name: "PhotoSrc");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("С фотографией");
            DropFocus("h1");
            WaitForAjax();

            //check delete
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            GetGridCell(0, "_serviceColumn").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            Assert.AreNotEqual("BrandName1", GetGridCell(0, "BrandName").Text);

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
            Assert.AreNotEqual("BrandName2", GetGridCell(0, "BrandName").Text);
            Assert.AreNotEqual("BrandName3", GetGridCell(1, "BrandName").Text);
            Assert.AreNotEqual("BrandName4", GetGridCell(2, "BrandName").Text);

            //check select all on page
            Functions.GridPaginationSelect50(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(49, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
            Assert.AreEqual("BrandName55", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName130", GetGridCell(45, "BrandName").Text);
            Assert.AreEqual("Найдено записей: 46", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            //check select all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("46", driver.FindElement(By.ClassName("ui-grid-custom-selection-count")).Text);

            //check deselect all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsFalse(GetGridCell(0, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsFalse(GetGridCell(45, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);

            //check delete all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //close filter
            Functions.GridFilterClose(driver, baseURL, name: "PhotoSrc");
            Assert.AreEqual("BrandName71", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("Найдено записей: 100", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
        }

        [Test]
        public void BrandFilterPhotoDoNotActive()
        {
            GoToAdmin("brands");
            Functions.GridFilterSet(driver, baseURL, name: "PhotoSrc");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("Без фотографии");
            DropFocus("h1");
            WaitForAjax();
            Functions.GridPaginationSelect100(driver, baseURL);
            Assert.IsTrue(GetGridCell(99, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            GetGridCell(99, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();
            Refresh();
            Thread.Sleep(1000);
            Assert.IsFalse(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.AreEqual("BrandName200", GetGridCell(99, "BrandName").Text);

            //do selected items not active 
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            GetGridCell(98, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(97, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"2\"]")).Click();
            Thread.Sleep(1000);
            Refresh();
            Assert.AreEqual("BrandName199", GetGridCell(98, "BrandName").Text);
            Assert.AreEqual("BrandName198", GetGridCell(97, "BrandName").Text);
            Assert.IsFalse(GetGridCell(98, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.IsFalse(GetGridCell(97, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
        }

        [Test]
        public void BrandFilterPhotoDoActive()
        {
            GoToAdmin("brands");
            Functions.GridFilterSet(driver, baseURL, name: "PhotoSrc");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("С фотографией");
            DropFocus("h1");
            WaitForAjax();
            Assert.IsFalse(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffLabel\"]")).Click();
            Refresh();
            Assert.IsTrue(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            //do selected items active 
            GetGridCell(1, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownButton\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDropdownItem\"][data-e2e-grid-selection-dropdown-index=\"1\"]")).Click();
            Thread.Sleep(1000);
            Refresh();
            Assert.IsTrue(GetGridCell(1, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.IsTrue(GetGridCell(2, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
        }

        [Test]
        public void BrandFilterPhotoGoToEditAndBack()
        {
            GoToAdmin("brands");
            Functions.GridFilterSet(driver, baseURL, name: "PhotoSrc");
            (new SelectElement(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemSelect\"]")))).SelectByText("С фотографией");
            DropFocus("h1");
            WaitForAjax();

            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName10", GetGridCell(9, "BrandName").Text);

            GetGridCell(0, "BrandName").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("Производитель \"BrandName1\"", driver.FindElement(By.TagName("h1")).Text);
            Assert.AreEqual("BrandName1", driver.FindElement(By.Id("BrandName")).GetAttribute("value"));
            Assert.IsTrue(driver.Url.Contains("brands/edit/1"));

            GoBack();
            Thread.Sleep(3000);
            Assert.AreEqual("BrandName1", GetGridCell(0, "BrandName").Text);
            Assert.AreEqual("BrandName10", GetGridCell(9, "BrandName").Text);
            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"PhotoSrc\"]")).Displayed);
        }
    }
}