using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.ProductLists
{
    [TestFixture]
    public class Prod_ProdListAddFilterPricePageViewTest : BaseSeleniumTest
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
        public void AddProduct_ProductListsFilterPricePage()
        {
            testname = "AddProduct_ProductListsFilterPricePage";
            VerifyBegin(testname);

            Functions.AddProduct_ProductListsFilterFromTo(driver, baseURL, filterName: "Price");
            Functions.FilterPageFromTo(driver, baseURL, "h2");
            
            VerifyAreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 1 line 1");
            VerifyAreEqual("TestProduct17", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            VerifyAreEqual("TestProduct18", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 2 line 1");
            VerifyAreEqual("TestProduct26", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            VerifyAreEqual("TestProduct27", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 3 line 1");
            VerifyAreEqual("TestProduct35", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            VerifyAreEqual("TestProduct36", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 4 line 1");
            VerifyAreEqual("TestProduct44", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 4 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("TestProduct45", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 5 line 1");
            VerifyAreEqual("TestProduct53", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 5 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("TestProduct54", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 6 line 1");
            VerifyAreEqual("TestProduct62", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 6 line 10");

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            VerifyAreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 1 line 1");
            VerifyAreEqual("TestProduct17", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 1 line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void AddProduct_ProductListsFilterPricePageToPrev()
        {
            testname = "AddProduct_ProductListsFilterPricePageToPrev";
            VerifyBegin(testname);

            Functions.AddProduct_ProductListsFilterFromTo(driver, baseURL, filterName: "Price");
            Functions.FilterPageFromTo(driver, baseURL, "h2");
            
            VerifyAreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 1 line 1");
            VerifyAreEqual("TestProduct17", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("TestProduct18", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 2 line 1");
            VerifyAreEqual("TestProduct26", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("TestProduct27", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 3 line 1");
            VerifyAreEqual("TestProduct35", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("TestProduct18", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 2 line 1");
            VerifyAreEqual("TestProduct26", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 1 line 1");
            VerifyAreEqual("TestProduct17", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 1 line 10");

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            VerifyAreEqual("TestProduct90", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "last page line 1");
            VerifyAreEqual("TestProduct99", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "last page line 10");

            VerifyFinally(testname);

        }

        [Test]
        public void AddProduct_ProductListsFilterPriceView()
        {
            testname = "AddProduct_ProductListsFilterPriceView";
            VerifyBegin(testname);

            Functions.AddProduct_ProductListsFilterFromTo(driver, baseURL, filterName: "Price");
            Functions.FilterPageFromTo(driver, baseURL, "h2");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("10");
            VerifyAreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "line 1");
            VerifyAreEqual("TestProduct17", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("20");
            VerifyAreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "line 1");
            VerifyAreEqual("TestProduct26", GetGridCell(19, "Name", "ProductsSelectvizr").Text, "line 20");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("50");
            VerifyAreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "line 1");
            VerifyAreEqual("TestProduct53", GetGridCell(49, "Name", "ProductsSelectvizr").Text, "line 50");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("100");
            VerifyAreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "line 1");
            VerifyAreEqual("TestProduct99", GetGridCell(99, "Name", "ProductsSelectvizr").Text, "line 100");

            VerifyFinally(testname);
        }
    }
}