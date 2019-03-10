using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.ProductLists
{
    [TestFixture]
    public class Prod_ProdListAddFilterNameTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
       "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\AddProd_ProdLists\\FilterTest\\Catalog.Product.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\AddProd_ProdLists\\FilterTest\\Catalog.Offer.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\AddProd_ProdLists\\FilterTest\\Catalog.Category.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\AddProd_ProdLists\\FilterTest\\Catalog.ProductCategories.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\AddProd_ProdLists\\FilterTest\\Catalog.ProductList.csv",
           "data\\Admin\\Catalog\\ProductsOnMainPage\\ProductLists\\AddProd_ProdLists\\FilterTest\\Catalog.Product_ProductList.csv"
           );
             
            Init();
        }
        
        [Test]
        public void AddProduct_ProductListFilterNameExist()
        {
            Functions.AddProduct_ProductListsFilter(driver, baseURL, filterName: "Name");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("TestProduct80");
            WaitForAjax();
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct80", GetGridCell(0, "Name", "ProductsSelectvizr").Text);

            //close
            Functions.GridFilterClose(driver, baseURL, name: "Name");
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text);
        }

        [Test]
        public void AddProduct_ProductListFilterNameNotExist()
        {
            Functions.AddProduct_ProductListsFilter(driver, baseURL, filterName: "Name");

            //check not exist product
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("TestProductXZQWER");
            DropFocus("h2");
            WaitForAjax();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //check too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwwwqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq");
            DropFocus("h2");
            WaitForAjax();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //check invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            DropFocus("h2");
            WaitForAjax();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }
    }
}

