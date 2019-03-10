using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.ProductLists
{
    [TestFixture]
    public class Product_ProductListAddPageTest : BaseSeleniumTest
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
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\AddProd_ProdLists\\Catalog.ProductList.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\AddProd_ProdLists\\Catalog.Product_ProductList.csv"
           );
             
            Init();
        }

         

        [Test]
        public void PageAddProduct_ProductLists()
        {
            testname = "PageAddProduct_ProductLists";
            VerifyBegin(testname);
            
            GoToAdmin("productlists/products/2");
            driver.FindElement(By.CssSelector("[data-e2e=\"product_ProductListAdd\"]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory6')]")).Click();
            VerifyAreEqual("18/20", driver.FindElement(By.Id("1_anchor")).FindElements(By.TagName("span"))[1].Text, "products in categories count");
            VerifyAreEqual("TestProduct151", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 1 line 1");
            VerifyAreEqual("TestProduct160", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            VerifyAreEqual("TestProduct161", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 2 line 1");           
            VerifyAreEqual("TestProduct170", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            VerifyAreEqual("TestProduct171", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 3 line 1");           
            VerifyAreEqual("TestProduct180", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            VerifyAreEqual("TestProduct181", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 4 line 1");           
            VerifyAreEqual("TestProduct190", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 4 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
           VerifyAreEqual("TestProduct191", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 5 line 1");           
            VerifyAreEqual("TestProduct200", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 5 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("TestProduct201", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 6 line 1");           
            VerifyAreEqual("TestProduct210", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 6 line 10");
            
            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            VerifyAreEqual("TestProduct151", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 1 line 1");
            VerifyAreEqual("TestProduct160", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 1 line 10");
            
            VerifyFinally(testname);
        }

        [Test]
        public void PageAddProduct_ProductListsToPrevious()
        {
            testname = "PageAddProduct_ProductListsToPrevious";
            VerifyBegin(testname);

            GoToAdmin("productlists/products/2");
            driver.FindElement(By.CssSelector("[data-e2e=\"product_ProductListAdd\"]")).Click();
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//span[contains(text(), 'TestCategory6')]")).Click();
            Thread.Sleep(3000);
            VerifyAreEqual("TestProduct151", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 1 line 1");
            VerifyAreEqual("TestProduct160", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("TestProduct161", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 2 line 1");
            VerifyAreEqual("TestProduct170", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("TestProduct171", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 3 line 1");
            VerifyAreEqual("TestProduct180", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("TestProduct161", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 2 line 1");
            VerifyAreEqual("TestProduct170", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("TestProduct151", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "page 1 line 1");
            VerifyAreEqual("TestProduct160", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "page 1 line 10");

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            VerifyAreEqual("TestProduct241", GetGridCell(0, "Name", "ProductsSelectvizr").Text, "last page line 1");
            VerifyAreEqual("TestProduct250", GetGridCell(9, "Name", "ProductsSelectvizr").Text, "last page line 10");

            VerifyFinally(testname);

        }
    }
}