using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.ProductLists
{
    [TestFixture]
    public class Prod_ProdListAddFilterArtNoPageViewTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\AddProd_ProdLists\\FilterPage\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\AddProd_ProdLists\\FilterPage\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\AddProd_ProdLists\\FilterPage\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\AddProd_ProdLists\\FilterPage\\Catalog.ProductCategories.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\AddProd_ProdLists\\FilterPage\\Catalog.ProductList.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\AddProd_ProdLists\\FilterPage\\Catalog.Product_ProductList.csv"
           );
             
            Init();
        }

         

        [Test]
        public void AddProduct_ProductListsFilterArtNoPage()
        {
            testname = "AddProduct_ProductListsFilterArtNoPage";
            VerifyBegin(testname);

            Functions.AddProduct_ProductListsFilter(driver, baseURL, filterName: "ProductArtNo");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("1");
            DropFocus("h2");

            VerifyAreEqual("1", GetGridCell(0, "ProductArtNo", "ProductsSelectvizr").Text, "page 1 line 1 art no");
            VerifyAreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 1 line 1 product name");
            VerifyAreEqual("107", GetGridCell(9, "ProductArtNo", "ProductsSelectvizr").Text, "page 1 line 10 art no");
            VerifyAreEqual("TestProduct107", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 1 line 10 product name");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            VerifyAreEqual("108", GetGridCell(0, "ProductArtNo", "ProductsSelectvizr").Text, "page 2 line 1 art no");
            VerifyAreEqual("TestProduct108", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 2 line 1 product name");
            VerifyAreEqual("116", GetGridCell(9, "ProductArtNo", "ProductsSelectvizr").Text, "page 2 line 10 art no");
            VerifyAreEqual("TestProduct116", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 2 line 10 product name");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            VerifyAreEqual("117", GetGridCell(0, "ProductArtNo", "ProductsSelectvizr").Text, "page 3 line 1 art no");
            VerifyAreEqual("TestProduct117", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 3 line 1 product name");
            VerifyAreEqual("125", GetGridCell(9, "ProductArtNo", "ProductsSelectvizr").Text, "page 3 line 10 art no");
            VerifyAreEqual("TestProduct125", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 3 line 10 product name");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            VerifyAreEqual("126", GetGridCell(0, "ProductArtNo", "ProductsSelectvizr").Text, "page 4 line 1 art no");
            VerifyAreEqual("TestProduct126", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 4 line 1 product name");
            VerifyAreEqual("134", GetGridCell(9, "ProductArtNo", "ProductsSelectvizr").Text, "page 4 line 10 art no");
            VerifyAreEqual("TestProduct134", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 4 line 10 product name");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("135", GetGridCell(0, "ProductArtNo", "ProductsSelectvizr").Text, "page 5 line 1 art no");
            VerifyAreEqual("TestProduct135", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 5 line 1 product name");
            VerifyAreEqual("143", GetGridCell(9, "ProductArtNo", "ProductsSelectvizr").Text, "page 5 line 10 art no");
            VerifyAreEqual("TestProduct143", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 5 line 10 product name");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("144", GetGridCell(0, "ProductArtNo", "ProductsSelectvizr").Text, "page 6 line 1 art no");
            VerifyAreEqual("TestProduct144", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 6 line 1 product name");
            VerifyAreEqual("152", GetGridCell(9, "ProductArtNo", "ProductsSelectvizr").Text, "page 6 line 10 art no");
            VerifyAreEqual("TestProduct152", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 6 line 10 product name");

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            VerifyAreEqual("1", GetGridCell(0, "ProductArtNo", "ProductsSelectvizr").Text, "page 1 line 1 art no");
            VerifyAreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 1 line 1 product name");
            VerifyAreEqual("107", GetGridCell(9, "ProductArtNo", "ProductsSelectvizr").Text, "page 1 line 10 art no");
            VerifyAreEqual("TestProduct107", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 1 line 10 product name");

            VerifyFinally(testname);
        }

        [Test]
        public void AddProduct_ProductListsFilterArtNoPageToPrev()
        {
            testname = "AddProduct_ProductListsFilterArtNoPageToPrev";
            VerifyBegin(testname);

            Functions.AddProduct_ProductListsFilter(driver, baseURL, filterName: "ProductArtNo");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("1");
            DropFocus("h2");

            VerifyAreEqual("1", GetGridCell(0, "ProductArtNo", "ProductsSelectvizr").Text, "page 1 line 1 art no");
            VerifyAreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 1 line 1 product name");
            VerifyAreEqual("107", GetGridCell(9, "ProductArtNo", "ProductsSelectvizr").Text, "page 1 line 10 art no");
            VerifyAreEqual("TestProduct107", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 1 line 10 product name");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("108", GetGridCell(0, "ProductArtNo", "ProductsSelectvizr").Text, "page 2 line 1 art no");
            VerifyAreEqual("TestProduct108", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 2 line 1 product name");
            VerifyAreEqual("116", GetGridCell(9, "ProductArtNo", "ProductsSelectvizr").Text, "page 2 line 10 art no");
            VerifyAreEqual("TestProduct116", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 2 line 10 product name");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("117", GetGridCell(0, "ProductArtNo", "ProductsSelectvizr").Text, "page 3 line 1 art no");
            VerifyAreEqual("TestProduct117", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 3 line 1 product name");
            VerifyAreEqual("125", GetGridCell(9, "ProductArtNo", "ProductsSelectvizr").Text, "page 3 line 10 art no");
            VerifyAreEqual("TestProduct125", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 3 line 10 product name");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("108", GetGridCell(0, "ProductArtNo", "ProductsSelectvizr").Text, "page 2 line 1 art no");
            VerifyAreEqual("TestProduct108", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 2 line 1 product name");
            VerifyAreEqual("116", GetGridCell(9, "ProductArtNo", "ProductsSelectvizr").Text, "page 2 line 10 art no");
            VerifyAreEqual("TestProduct116", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 2 line 10 product name");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("1", GetGridCell(0, "ProductArtNo", "ProductsSelectvizr").Text, "page 1 line 1 art no");
            VerifyAreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 1 line 1 product name");
            VerifyAreEqual("107", GetGridCell(9, "ProductArtNo", "ProductsSelectvizr").Text, "page 1 line 10 art no");
            VerifyAreEqual("TestProduct107", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 1 line 10 product name");

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            VerifyAreEqual("71", GetGridCell(0, "ProductArtNo", "ProductsSelectvizr").Text, "last page line 1 art no");
            VerifyAreEqual("TestProduct71", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "last page line 1 product name");
            VerifyAreEqual("91", GetGridCell(2, "ProductArtNo", "ProductsSelectvizr").Text, "last page line 3 art no");
            VerifyAreEqual("TestProduct91", GetGridCell(2, "Name", "ProductsSelectvizr").Text, "last page line 3 product name");

            VerifyFinally(testname);

        }

        [Test]
        public void AddProduct_ProductListsFilterArtNoView()
        {
            testname = "AddProduct_ProductListsFilterArtNoView";
            VerifyBegin(testname);

            Functions.AddProduct_ProductListsFilter(driver, baseURL, filterName: "ProductArtNo");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("1");
            DropFocus("h2");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("10");
            VerifyAreEqual("1", GetGridCell(0, "ProductArtNo", "ProductsSelectvizr").Text, "line 1 art no");
            VerifyAreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "line 1 product name");
            VerifyAreEqual("107", GetGridCell(9, "ProductArtNo", "ProductsSelectvizr").Text, "line 10 art no");
            VerifyAreEqual("TestProduct107", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "line 10 product name");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("20");
            VerifyAreEqual("1", GetGridCell(0, "ProductArtNo", "ProductsSelectvizr").Text, "line 1 art no");
            VerifyAreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "line 1 product name");
            VerifyAreEqual("116", GetGridCell(19, "ProductArtNo", "ProductsSelectvizr").Text, "line 20 art no");
            VerifyAreEqual("TestProduct116", GetGridCell(19, "Name", "ProductsSelectvizr").Text, "line 20 product name");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("50");
            VerifyAreEqual("1", GetGridCell(0, "ProductArtNo", "ProductsSelectvizr").Text, "line 1 art no");
            VerifyAreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "line 1 product name");
            VerifyAreEqual("143", GetGridCell(49, "ProductArtNo", "ProductsSelectvizr").Text, "line 50 art no");
            VerifyAreEqual("TestProduct143", GetGridCell(49, "Name", "ProductsSelectvizr").Text, "line 50 product name");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("100");
            VerifyAreEqual("1", GetGridCell(0, "ProductArtNo", "ProductsSelectvizr").Text, "line 1 art no");
            VerifyAreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "line 1 product name");
            VerifyAreEqual("189", GetGridCell(99, "ProductArtNo", "ProductsSelectvizr").Text, "line 100 art no");
            VerifyAreEqual("TestProduct189", GetGridCell(99, "Name", "ProductsSelectvizr").Text, "line 100 product name");

            VerifyFinally(testname);
        }
    }
}