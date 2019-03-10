using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.ProductLists
{
    [TestFixture]
    public class Product_ProductListsPageTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\PaginationProduct_ProductListsTest\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\PaginationProduct_ProductListsTest\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\PaginationProduct_ProductListsTest\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\PaginationProduct_ProductListsTest\\Catalog.ProductCategories.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\PaginationProduct_ProductListsTest\\Catalog.ProductList.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\PaginationProduct_ProductListsTest\\Catalog.Product_ProductList.csv"
           );
             
            Init();
        }

         

        [Test]
        public void Product_ProductListsPage()
        {
            testname = "Product_ProductListsPage";
            VerifyBegin(testname);

            GoToAdmin("productlists");
            GetGridCell(0, "Name", "ProductLists").Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestProduct1", GetGridCell(0, "Name", "Products").Text, "page 1 line 1");
            VerifyAreEqual("TestProduct10", GetGridCell(9, "Name", "Products").Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            VerifyAreEqual("TestProduct11", GetGridCell(0, "Name", "Products").Text, "page 2 line 1");
            VerifyAreEqual("TestProduct20", GetGridCell(9, "Name", "Products").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            VerifyAreEqual("TestProduct21", GetGridCell(0, "Name", "Products").Text, "page 3 line 1");
            VerifyAreEqual("TestProduct30", GetGridCell(9, "Name", "Products").Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            VerifyAreEqual("TestProduct31", GetGridCell(0, "Name", "Products").Text, "page 4 line 1");
            VerifyAreEqual("TestProduct40", GetGridCell(9, "Name", "Products").Text, "page 4 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("TestProduct41", GetGridCell(0, "Name", "Products").Text, "page 5 line 1");
            VerifyAreEqual("TestProduct50", GetGridCell(9, "Name", "Products").Text, "page 5 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            VerifyAreEqual("TestProduct51", GetGridCell(0, "Name", "Products").Text, "page 6 line 1");
            VerifyAreEqual("TestProduct60", GetGridCell(9, "Name", "Products").Text, "page 6 line 10");

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            VerifyAreEqual("TestProduct1", GetGridCell(0, "Name", "Products").Text, "page 1 line 1");
            VerifyAreEqual("TestProduct10", GetGridCell(9, "Name", "Products").Text, "page 1 line 10");

            VerifyFinally(testname);
        }

        [Test]
        public void Product_ProductListsPageToPrev()
        {
            testname = "Product_ProductListsPageToPrev";
            VerifyBegin(testname);

            GoToAdmin("productlists");
            GetGridCell(0, "Name", "ProductLists").Click();
            Thread.Sleep(2000);
            VerifyAreEqual("TestProduct1", GetGridCell(0, "Name", "Products").Text, "page 1 line 1");
            VerifyAreEqual("TestProduct10", GetGridCell(9, "Name", "Products").Text, "page 1 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("TestProduct11", GetGridCell(0, "Name", "Products").Text, "page 2 line 1");
            VerifyAreEqual("TestProduct20", GetGridCell(9, "Name", "Products").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            VerifyAreEqual("TestProduct21", GetGridCell(0, "Name", "Products").Text, "page 3 line 1");
            VerifyAreEqual("TestProduct30", GetGridCell(9, "Name", "Products").Text, "page 3 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("TestProduct11", GetGridCell(0, "Name", "Products").Text, "page 2 line 1");
            VerifyAreEqual("TestProduct20", GetGridCell(9, "Name", "Products").Text, "page 2 line 10");

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            VerifyAreEqual("TestProduct1", GetGridCell(0, "Name", "Products").Text, "page 1 line 1");
            VerifyAreEqual("TestProduct10", GetGridCell(9, "Name", "Products").Text, "page 1 line 10");

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            VerifyAreEqual("TestProduct91", GetGridCell(0, "Name", "Products").Text, "last page line 1");
            VerifyAreEqual("TestProduct100", GetGridCell(9, "Name", "Products").Text, "last page line 10");

            VerifyFinally(testname);
        }
    }
}