using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.ProductLists
{
    [TestFixture]
    public class Prod_ProdListAddFilterActivOffPageViewTest : BaseSeleniumTest
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
        public void AddProduct_ProductListsFilterActivityOffPage()
        {
            testname = "AddProduct_ProductListsFilterActivityOffPage";
            VerifyBegin(testname);

            Functions.AddProduct_ProductListsFilterSelect(driver, baseURL, filter: "Enabled", select: "Неактивные");

            VerifyAreEqual("TestProduct101", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 1 line 1");
            VerifyAreEqual("TestProduct110", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            VerifyAreEqual("TestProduct111", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 2 line 1");
            VerifyAreEqual("TestProduct120", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            VerifyAreEqual("TestProduct121", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 3 line 1");
            VerifyAreEqual("TestProduct130", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            VerifyAreEqual("TestProduct131", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 4 line 1");
            VerifyAreEqual("TestProduct140", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 4 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("TestProduct141", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 5 line 1");
            VerifyAreEqual("TestProduct150", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 5 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("TestProduct151", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 6 line 1");
            VerifyAreEqual("TestProduct160", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 6 line 10");

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            VerifyAreEqual("TestProduct101", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 1 line 1");
            VerifyAreEqual("TestProduct110", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 1 line 10");

            VerifyFinally(testname);
        }
        
        [Test]
        public void AddProduct_ProductListsFilterActivityOffPageToPrev()
        {
            testname = "AddProduct_ProductListsFilterActivityOffPageToPrev";
            VerifyBegin(testname);

            Functions.AddProduct_ProductListsFilterSelect(driver, baseURL, filter: "Enabled", select: "Неактивные");

            VerifyAreEqual("TestProduct101", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 1 line 1");
            VerifyAreEqual("TestProduct110", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("TestProduct111", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 2 line 1");
            VerifyAreEqual("TestProduct120", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("TestProduct121", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 3 line 1");
            VerifyAreEqual("TestProduct130", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("TestProduct111", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 2 line 1");
            VerifyAreEqual("TestProduct120", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("TestProduct101", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 1 line 1");
            VerifyAreEqual("TestProduct110", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 1 line 10");

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            VerifyAreEqual("TestProduct241", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "last page line 1");
            VerifyAreEqual("TestProduct250", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "last page line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void AddProduct_ProductListsFilterActivityOffView()
        {
            testname = "AddProduct_ProductListsFilterActivityOffView";
            VerifyBegin(testname);

            Functions.AddProduct_ProductListsFilterSelect(driver, baseURL, filter: "Enabled", select: "Неактивные");
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("10");
            VerifyAreEqual("TestProduct101", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "line 1");
            VerifyAreEqual("TestProduct110", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("20");
            VerifyAreEqual("TestProduct101", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "line 1");
            VerifyAreEqual("TestProduct120", GetGridCell(19, "Name", "ProductsSelectvizr").Text, "line 20");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("50");
            VerifyAreEqual("TestProduct101", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "line 1");
            VerifyAreEqual("TestProduct150", GetGridCell(49, "Name", "ProductsSelectvizr").Text, "line 50");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            PageSelectItems("100");
            VerifyAreEqual("TestProduct101", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "line 1");
            VerifyAreEqual("TestProduct200", GetGridCell(99, "Name", "ProductsSelectvizr").Text, "line 100");

            VerifyFinally(testname);
        }
    }
}