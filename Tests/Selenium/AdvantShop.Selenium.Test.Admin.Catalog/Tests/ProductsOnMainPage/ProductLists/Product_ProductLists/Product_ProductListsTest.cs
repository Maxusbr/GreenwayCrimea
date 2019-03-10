using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.ProductLists
{
    [TestFixture]
    public class Product_ProductListsTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\Catalog.ProductCategories.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\Catalog.ProductList.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\Catalog.Product_ProductList.csv"
           );
             
            Init();
        }
        
        [Test]
        public void Product_ProductListPresent()
        {
            GoToAdmin("productlists");
            GetGridCell(0, "Name", "ProductLists").Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name", "Products").Text);

            Functions.GridPaginationSelect10(driver, baseURL);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name", "Products").Text);
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name", "Products").Text);

            Functions.GridPaginationSelect20(driver, baseURL);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name", "Products").Text);
            Assert.AreEqual("TestProduct20", GetGridCell(19, "Name", "Products").Text);

            Functions.GridPaginationSelect50(driver, baseURL);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name", "Products").Text);
            Assert.AreEqual("TestProduct50", GetGridCell(49, "Name", "Products").Text);

            Functions.GridPaginationSelect100(driver, baseURL);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name", "Products").Text);
            Assert.AreEqual("TestProduct100", GetGridCell(99, "Name", "Products").Text);

            //check go back 
            // driver.FindElement(By.XPath("//a[contains(text(), 'Вернуться назад')]")).Click();
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.LinkText("Вернуться назад")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("ProductList1", GetGridCell(0, "Name", "ProductLists").Text);

            //check no products in list
            GetGridCell(9, "Name", "ProductLists").Click();
            Thread.Sleep(2000);
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //client
            GoToClient();
            Assert.AreEqual("TestProduct1", driver.FindElements(By.CssSelector(".products-view.products-view-tile.row"))[0].FindElements(By.CssSelector(".products-view-name.products-view-name-default a"))[0].Text);
        }

        [Test]
        public void Product_ProductListaSort()
        {
            GoToAdmin("productlists");
            GetGridCell(0, "Name", "ProductLists").Click();
            Thread.Sleep(2000);

            //check sort by name
            GetGridCell(-1, "Name", "Products").Click();
            Thread.Sleep(1000);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name", "Products").Text);
            Assert.AreEqual("TestProduct17", GetGridCell(9, "Name", "Products").Text);

            GetGridCell(-1, "Name", "Products").Click();
            Thread.Sleep(1000);
            Assert.AreEqual("TestProduct99", GetGridCell(0, "Name", "Products").Text);
            Assert.AreEqual("TestProduct90", GetGridCell(9, "Name", "Products").Text);

            //check sort by name sort order
            GetGridCell(-1, "SortOrder", "Products").Click();
            Thread.Sleep(1000);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name", "Products").Text);
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name", "Products").Text);
            Assert.AreEqual("1", GetGridCell(0, "SortOrder", "Products").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("10", GetGridCell(9, "SortOrder", "Products").FindElement(By.TagName("input")).GetAttribute("value"));

            GetGridCell(-1, "SortOrder", "Products").Click();
            Thread.Sleep(1000);
            Assert.AreEqual("TestProduct100", GetGridCell(0, "Name", "Products").Text);
            Assert.AreEqual("TestProduct91", GetGridCell(9, "Name", "Products").Text);
            Assert.AreEqual("100", GetGridCell(0, "SortOrder", "Products").FindElement(By.TagName("input")).GetAttribute("value"));
            Assert.AreEqual("91", GetGridCell(9, "SortOrder", "Products").FindElement(By.TagName("input")).GetAttribute("value"));

            //check sort by name ArtNo
            GetGridCell(-1, "ProductArtNo", "Products").Click();
            Thread.Sleep(1000);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name", "Products").Text);
            Assert.AreEqual("TestProduct17", GetGridCell(9, "Name", "Products").Text);
            Assert.AreEqual("1", GetGridCell(0, "ProductArtNo", "Products").Text);
            Assert.AreEqual("17", GetGridCell(9, "ProductArtNo", "Products").Text);

            GetGridCell(-1, "ProductArtNo", "Products").Click();
            Thread.Sleep(1000);
            Assert.AreEqual("TestProduct99", GetGridCell(0, "Name", "Products").Text);
            Assert.AreEqual("TestProduct90", GetGridCell(9, "Name", "Products").Text);
            Assert.AreEqual("99", GetGridCell(0, "ProductArtNo", "Products").Text);
            Assert.AreEqual("90", GetGridCell(9, "ProductArtNo", "Products").Text);
        }
       
        [Test]
        public void Product_ProductListSearchAndInplace()
        {
            GoToAdmin("productlists");

            //check search exist product
            GetGridCell(0, "Name", "ProductLists").Click();
            Thread.Sleep(2000);
            GetGridFilter().SendKeys("TestProduct70");
            DropFocus("h2");
            WaitForAjax();
            Assert.AreEqual("TestProduct70", GetGridCell(0, "Name", "Products").Text);

            //check sort order inplace edit 
            GetGridCell(0, "SortOrder", "Products").FindElement(By.TagName("input")).Click();
            GetGridCell(0, "SortOrder", "Products").FindElement(By.TagName("input")).Clear();
            GetGridCell(0, "SortOrder", "Products").FindElement(By.TagName("input")).SendKeys("200");
            DropFocus("h2");
            WaitForAjax();

            GoToAdmin("productlists");
            GetGridCell(0, "Name", "ProductLists").Click();
            Thread.Sleep(2000);
            GetGridFilter().SendKeys("TestProduct70");
            DropFocus("h2");
            WaitForAjax();
            Assert.AreEqual("TestProduct70", GetGridCell(0, "Name", "Products").Text);
            Assert.AreEqual("200", GetGridCell(0, "SortOrder", "Products").FindElement(By.TagName("input")).GetAttribute("value"));

            //check search not exist product
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct110");
            DropFocus("h2");
            WaitForAjax();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //check search too much symbols
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            DropFocus("h2");
            WaitForAjax();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //check search invalid symbols
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            DropFocus("h2");
            WaitForAjax();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }
      
        [Test]
        public void Product_ProductListSelectAndDelete()
        {
            GoToAdmin("productlists");
            GetGridCell(0, "Name", "ProductLists").Click();
            Thread.Sleep(2000);
            gridReturnDefaultView10();

            //check delete cancel
            GetGridCell(0, "_serviceColumn", "Products").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-cancel")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name", "Products").Text);

            //check delete
            GetGridCell(0, "_serviceColumn", "Products").FindElement(By.TagName("ui-grid-custom-delete")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(2000);
            Assert.AreNotEqual("TestProduct1", GetGridCell(0, "Name", "Products").Text);

            //check select 
            GetGridCell(0, "selectionRowHeaderCol", "Products").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(1, "selectionRowHeaderCol", "Products").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            GetGridCell(2, "selectionRowHeaderCol", "Products").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxWrapSelect\"]")).Click();
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol", "Products").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(1, "selectionRowHeaderCol", "Products").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(2, "selectionRowHeaderCol", "Products").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("3", driver.FindElement(By.ClassName("ui-grid-custom-selection-count")).Text);

            //check delete selected items
            Functions.GridDropdownDelete(driver, baseURL);
            Assert.AreNotEqual("TestProduct2", GetGridCell(0, "Name", "Products").Text);
            Assert.AreNotEqual("TestProduct3", GetGridCell(1, "Name", "Products").Text);
            Assert.AreNotEqual("TestProduct4", GetGridCell(2, "Name", "Products").Text);

            //check select all on page
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol", "Products").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol", "Products").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);

            //check delete all on page
            Functions.GridDropdownDelete(driver, baseURL);
            Assert.AreEqual("TestProduct15", GetGridCell(0, "Name", "Products").Text);
            Assert.AreEqual("TestProduct24", GetGridCell(9, "Name", "Products").Text);

            //check select all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("86", driver.FindElement(By.ClassName("ui-grid-custom-selection-count")).Text);

            //check deselect all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsFalse(GetGridCell(0, "selectionRowHeaderCol", "Products").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsFalse(GetGridCell(9, "selectionRowHeaderCol", "Products").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);

            //check delete all
            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Functions.GridDropdownDelete(driver, baseURL);

            Refresh();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }
    }
}