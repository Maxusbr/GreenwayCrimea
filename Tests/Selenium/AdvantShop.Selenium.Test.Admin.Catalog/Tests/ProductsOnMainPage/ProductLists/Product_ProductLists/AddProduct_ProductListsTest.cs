using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.ProductLists
{
    [TestFixture]
    public class Product_ProductListAddTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\AddProd_ProdLists\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\AddProd_ProdLists\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\AddProd_ProdLists\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\AddProd_ProdLists\\Catalog.ProductCategories.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\AddProd_ProdLists\\Catalog.ProductList.csv"
           );
             
            Init();
        }
        
        [Test]
        public void AddProduct_ProductListviaSearch()
        {
            GoToAdmin("productlists/products/1");

            //check whether a product is in product list
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct110");
            DropFocus("h2");
            Blur();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
            GoToAdmin("productlists/products/1");

            driver.FindElement(By.CssSelector("[data-e2e=\"product_ProductListAdd\"]")).Click();
            Thread.Sleep(5000);
            
            driver.FindElement(By.Id("4")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            Thread.Sleep(5000);
            WaitForElem(By.XPath("//span[contains(text(), 'TestCategory5')]"));
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory5')]")).Click();
            Thread.Sleep(5000);
            GetGridFilter().SendKeys("TestProduct110");
            // DropFocus("h2");
            XPathContainsText("h2", "Выбор товара");
            WaitForAjax();
            GetGridCell(0, "Name", "ProductsSelectvizr").Click();
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("//button[contains(text(), 'Выбрать')]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("productlists/products/1");
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct110");
            DropFocus("h2");
            Blur();
            Assert.AreEqual("TestProduct110", GetGridCell(0, "Name", "Products").Text);
        }

        [Test]
        public void AddProduct_ProductListviaCategory()
        {
            GoToAdmin("productlists/products/2");

            //check whether a product is in product list
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct155");
            DropFocus("h2");
            Blur();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
            GoToAdmin("productlists/products/2");

            driver.FindElement(By.CssSelector("[data-e2e=\"product_ProductListAdd\"]")).Click();
            Thread.Sleep(5000);
           
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory6')]")).Click();
            Thread.Sleep(5000);
            GetGridCell(4, "Name", "ProductsSelectvizr").Click();
            Thread.Sleep(2000);

            ScrollTo(By.XPath("//button[contains(text(), 'Выбрать')]"));

            driver.FindElement(By.XPath("//button[contains(text(), 'Выбрать')]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("productlists/products/2");
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct155");
            DropFocus("h2");
            Blur();
            Assert.AreEqual("TestProduct155", GetGridCell(0, "Name", "Products").Text);
        }

        [Test]
        public void AddProduct_ProductListviaSubCategory()
        {
            GoToAdmin("productlists/products/3");

            //check whether a product is in product list
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct101");
            DropFocus("h2");
            Blur();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
            GoToAdmin("productlists/products/3");

            driver.FindElement(By.CssSelector("[data-e2e=\"product_ProductListAdd\"]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.Id("4")).FindElement(By.CssSelector(".jstree-icon.jstree-ocl")).Click();
            Thread.Sleep(5000);
            WaitForElem(By.XPath("//span[contains(text(), 'TestCategory5')]"));
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory5')]")).Click();
            Thread.Sleep(2000);
            GetGridCell(0, "Name", "ProductsSelectvizr").Click();
            Thread.Sleep(2000);

            ScrollTo(By.XPath("//button[contains(text(), 'Выбрать')]"));

            driver.FindElement(By.XPath("//button[contains(text(), 'Выбрать')]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("productlists/products/3");
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct101");
            DropFocus("h2");
            Blur();
            Assert.AreEqual("TestProduct101", GetGridCell(0, "Name", "Products").Text);
        }
        
        [Test]
        public void AddNotExistProduct_ProductListviaSearch()
        {
            GoToAdmin("productlists/products/4");

            //check whether a product is in product list
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct504");
            DropFocus("h2");
            Blur();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
            GoToAdmin("productlists/products/4");

            driver.FindElement(By.CssSelector("[data-e2e=\"product_ProductListAdd\"]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory6')]")).Click();
            Thread.Sleep(5000);
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct504");
            DropFocus("h2");
            Blur();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            driver.FindElement(By.XPath("//button[contains(text(), 'Выбрать')]")).Click();
            Thread.Sleep(2000);
            GoToAdmin("productlists/products/4");
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct504");
            DropFocus("h2");
            Blur();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }

        [Test]
        public void AddProduct_ProductListUsingPagination()
        {
            GoToAdmin("productlists/products/5");

            //check whether a product is in product list
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct179");
            DropFocus("h2");
            Blur();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
            GoToAdmin("productlists/products/5");

            driver.FindElement(By.CssSelector("[data-e2e=\"product_ProductListAdd\"]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory6')]")).Click();
            Thread.Sleep(5000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            //  ScrollTo(By.XPath("//button[contains(text(), 'Выбрать')]"));

            GetGridCell(8, "Name", "ProductsSelectvizr").Click();
            Thread.Sleep(2000);
            
            driver.FindElement(By.XPath("//button[contains(text(), 'Выбрать')]")).Click();
            Thread.Sleep(3000);

            GoToAdmin("productlists/products/5");
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct179");
            DropFocus("h2");
            Blur();
            Assert.AreEqual("TestProduct179", GetGridCell(0, "Name", "Products").Text);
        }

        [Test]
        public void AddAProduct_ProductListView()
        {
            GoToAdmin("productlists/products/8");
            driver.FindElement(By.CssSelector("[data-e2e=\"product_ProductListAdd\"]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory6')]")).Click();
            Thread.Sleep(5000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect10(driver, baseURL);
            Assert.AreEqual("TestProduct151", GetGridCell(0, "Name", "ProductsSelectvizr").Text);
            Assert.AreEqual("TestProduct160", GetGridCell(9, "Name", "ProductsSelectvizr").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect20(driver, baseURL);
            Assert.AreEqual("TestProduct151", GetGridCell(0, "Name", "ProductsSelectvizr").Text);
            Assert.AreEqual("TestProduct170", GetGridCell(19, "Name", "ProductsSelectvizr").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect50(driver, baseURL);
            Assert.AreEqual("TestProduct151", GetGridCell(0, "Name", "ProductsSelectvizr").Text);
            Assert.AreEqual("TestProduct200", GetGridCell(49, "Name", "ProductsSelectvizr").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect100(driver, baseURL);
            Assert.AreEqual("TestProduct151", GetGridCell(0, "Name", "ProductsSelectvizr").Text);
            Assert.AreEqual("TestProduct250", GetGridCell(99, "Name", "ProductsSelectvizr").Text);

            //return default
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect10(driver, baseURL);
        }
        
        [Test]
        public void AddProduct_ProductListSelect()
        {
            GoToAdmin("productlists/products/9");

            //check whether a product is in product list
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct151");
            DropFocus("h2");
            Blur();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
            GoToAdmin("productlists/products/9");

            driver.FindElement(By.CssSelector("[data-e2e=\"product_ProductListAdd\"]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory6')]")).Click();
            Thread.Sleep(5000);
            GetGridCell(0, "Name", "ProductsSelectvizr").Click();
            Thread.Sleep(1000);
            GetGridCell(1, "Name", "ProductsSelectvizr").Click();
            Thread.Sleep(1000);

            //check items selected 
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol", "ProductsSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(1, "selectionRowHeaderCol", "ProductsSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.AreEqual("2", driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectedCount\"]")).Text);
            
            ScrollTo(By.XPath("//button[contains(text(), 'Выбрать')]"));

            driver.FindElement(By.XPath("//button[contains(text(), 'Выбрать')]")).Click();
            Thread.Sleep(2000);

            //check items selected add
            GoToAdmin("productlists/products/9");
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct151");
            DropFocus("h2");
            Blur();
            Assert.AreEqual("TestProduct151", GetGridCell(0, "Name", "Products").Text);
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct152");
            DropFocus("h2");
            Blur();
            Assert.AreEqual("TestProduct152", GetGridCell(0, "Name", "Products").Text);
        }
       
        [Test]
        public void AddProduct_ProductListSelectAllOnPage()
        {
            GoToAdmin("productlists/products/6");

            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            driver.FindElement(By.CssSelector("[data-e2e=\"product_ProductListAdd\"]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory6')]")).Click();
            Thread.Sleep(6000);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect50(driver, baseURL);

            //check items selected all on page
            ScrollTo(By.CssSelector("[data-e2e=\"gridFilterDropdownButton\"]"));
            GetGridCell(-1, "selectionRowHeaderCol", "ProductsSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol", "ProductsSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(49, "selectionRowHeaderCol", "ProductsSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            
            ScrollTo(By.XPath("//button[contains(text(), 'Выбрать')]"));

            driver.FindElement(By.XPath("//button[contains(text(), 'Выбрать')]")).Click();
            Thread.Sleep(2000);

            //check items selected add
            GoToAdmin("productlists/products/6");
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct169");
            DropFocus("h2");
            Blur();
            Assert.AreEqual("TestProduct169", GetGridCell(0, "Name", "Products").Text);
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("TestProduct152");
            DropFocus("h2");
            Blur();
            Assert.AreEqual("TestProduct152", GetGridCell(0, "Name", "Products").Text);
        }

        [Test]
        public void AddProduct_ProductListSelectAll()
        {
            GoToAdmin("productlists/products/7");

            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            driver.FindElement(By.CssSelector("[data-e2e=\"product_ProductListAdd\"]")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory6')]")).Click();
            Thread.Sleep(5000);

            //check items selected all 
            GetGridCell(-1, "selectionRowHeaderCol", "ProductsSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol", "ProductsSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol", "ProductsSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);

            //check items deselected all 
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionDeselectAllFn\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsFalse(GetGridCell(0, "selectionRowHeaderCol", "ProductsSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsFalse(GetGridCell(9, "selectionRowHeaderCol", "ProductsSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);

            GetGridCell(-1, "selectionRowHeaderCol", "ProductsSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(GetGridCell(0, "selectionRowHeaderCol", "ProductsSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);
            Assert.IsTrue(GetGridCell(9, "selectionRowHeaderCol", "ProductsSelectvizr").FindElement(By.CssSelector("[data-e2e=\"gridCheckboxSelect\"]")).Selected);

            ScrollTo(By.XPath("//button[contains(text(), 'Выбрать')]"));

            driver.FindElement(By.XPath("//button[contains(text(), 'Выбрать')]")).Click();
            Thread.Sleep(2000);

            //check all items add
            GoToAdmin("productlists/products/7");
            Assert.AreEqual("Найдено записей: 100", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
        }

        [Test]
        public void AddProduct_ProductListSort()
        {
            GoToAdmin("productlists/products/10");
            driver.FindElement(By.CssSelector("[data-e2e=\"product_ProductListAdd\"]")).Click();
            Thread.Sleep(5000);
            
            //check sort by name
            GetGridCell(-1, "Name", "ProductsSelectvizr").Click();
            WaitForAjax();
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text);
            Assert.AreEqual("TestProduct107", GetGridCell(9, "Name", "ProductsSelectvizr").Text);

            GetGridCell(-1, "Name", "ProductsSelectvizr").Click();
            WaitForAjax();
            Assert.AreEqual("TestProduct99", GetGridCell(0, "Name", "ProductsSelectvizr").Text);
           Assert.AreEqual("TestProduct90", GetGridCell(9, "Name", "ProductsSelectvizr").Text);

            //check sort by product art no
            GetGridCell(-1, "ProductArtNo", "ProductsSelectvizr").Click();
            WaitForAjax();
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text);
            Assert.AreEqual("TestProduct107", GetGridCell(9, "Name", "ProductsSelectvizr").Text);
            Assert.AreEqual("1", GetGridCell(0, "ProductArtNo", "ProductsSelectvizr").Text);
            Assert.AreEqual("107", GetGridCell(9, "ProductArtNo", "ProductsSelectvizr").Text);

            GetGridCell(-1, "ProductArtNo", "ProductsSelectvizr").Click();
            WaitForAjax();
            Assert.AreEqual("TestProduct99", GetGridCell(0, "Name", "ProductsSelectvizr").Text);
            Assert.AreEqual("TestProduct90", GetGridCell(9, "Name", "ProductsSelectvizr").Text);
            Assert.AreEqual("99", GetGridCell(0, "ProductArtNo", "ProductsSelectvizr").Text);
            Assert.AreEqual("90", GetGridCell(9, "ProductArtNo", "ProductsSelectvizr").Text);
        }
    }
}