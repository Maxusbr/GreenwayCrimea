using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.ProductLists
{
    [TestFixture]
    public class ProductListsPageTest : BaseSeleniumTest
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
        public void PageProductLists()
        {
            GoToAdmin("productlists");
            Assert.AreEqual("ProductList1", GetGridCell(0, "Name", "ProductLists").Text);
            Assert.AreEqual("ProductList10", GetGridCell(9, "Name", "ProductLists").Text);

            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("ProductList11", GetGridCell(0, "Name", "ProductLists").Text);
            Assert.AreEqual("ProductList20", GetGridCell(9, "Name", "ProductLists").Text);

            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("ProductList21", GetGridCell(0, "Name", "ProductLists").Text);
            Assert.AreEqual("ProductList30", GetGridCell(9, "Name", "ProductLists").Text);

            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("ProductList31", GetGridCell(0, "Name", "ProductLists").Text);
            Assert.AreEqual("ProductList40", GetGridCell(9, "Name", "ProductLists").Text);

            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("ProductList41", GetGridCell(0, "Name", "ProductLists").Text);
            Assert.AreEqual("ProductList50", GetGridCell(9, "Name", "ProductLists").Text);

            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("ProductList51", GetGridCell(0, "Name", "ProductLists").Text);
            Assert.AreEqual("ProductList60", GetGridCell(9, "Name", "ProductLists").Text);
        }

        [Test]
        public void PageProductListsToBegin()
        {
            GoToAdmin("productlists");
            Assert.AreEqual("ProductList1", GetGridCell(0, "Name", "ProductLists").Text);
            Assert.AreEqual("ProductList10", GetGridCell(9, "Name", "ProductLists").Text);

            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("ProductList11", GetGridCell(0, "Name", "ProductLists").Text);
            Assert.AreEqual("ProductList20", GetGridCell(9, "Name", "ProductLists").Text);
            Thread.Sleep(2000);

            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("ProductList21", GetGridCell(0, "Name", "ProductLists").Text);
            Assert.AreEqual("ProductList30", GetGridCell(9, "Name", "ProductLists").Text);

            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("ProductList31", GetGridCell(0, "Name", "ProductLists").Text);
            Assert.AreEqual("ProductList40", GetGridCell(9, "Name", "ProductLists").Text);

            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("ProductList41", GetGridCell(0, "Name", "ProductLists").Text);
            Assert.AreEqual("ProductList50", GetGridCell(9, "Name", "ProductLists").Text);

            //to begin
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("ProductList1", GetGridCell(0, "Name", "ProductLists").Text);
            Assert.AreEqual("ProductList10", GetGridCell(9, "Name", "ProductLists").Text);
        }

        [Test]
        public void PageProductListsToEnd()
        {
            GoToAdmin("productlists");
            Assert.AreEqual("ProductList1", GetGridCell(0, "Name", "ProductLists").Text);
            Assert.AreEqual("ProductList10", GetGridCell(9, "Name", "ProductLists").Text);

            //to end
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("ProductList91", GetGridCell(0, "Name", "ProductLists").Text);
            Assert.AreEqual("ProductList100", GetGridCell(9, "Name", "ProductLists").Text);
        }

        [Test]
        public void PageProductListsToNext()
        {
            GoToAdmin("productlists");
            Assert.AreEqual("ProductList1", GetGridCell(0, "Name", "ProductLists").Text);
            Assert.AreEqual("ProductList10", GetGridCell(9, "Name", "ProductLists").Text);

            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("ProductList11", GetGridCell(0, "Name", "ProductLists").Text);
            Assert.AreEqual("ProductList20", GetGridCell(9, "Name", "ProductLists").Text);

            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("ProductList21", GetGridCell(0, "Name", "ProductLists").Text);
            Assert.AreEqual("ProductList30", GetGridCell(9, "Name", "ProductLists").Text);

            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("ProductList31", GetGridCell(0, "Name", "ProductLists").Text);
            Assert.AreEqual("ProductList40", GetGridCell(9, "Name", "ProductLists").Text);

            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("ProductList41", GetGridCell(0, "Name", "ProductLists").Text);
            Assert.AreEqual("ProductList50", GetGridCell(9, "Name", "ProductLists").Text);
        }

        [Test]
        public void PageProductListsToPrevious()
        {
            GoToAdmin("productlists");
            Assert.AreEqual("ProductList1", GetGridCell(0, "Name", "ProductLists").Text);
            Assert.AreEqual("ProductList10", GetGridCell(9, "Name", "ProductLists").Text);

            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("ProductList11", GetGridCell(0, "Name", "ProductLists").Text);
            Assert.AreEqual("ProductList20", GetGridCell(9, "Name", "ProductLists").Text);

            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("ProductList21", GetGridCell(0, "Name", "ProductLists").Text);
            Assert.AreEqual("ProductList30", GetGridCell(9, "Name", "ProductLists").Text);

            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("ProductList11", GetGridCell(0, "Name", "ProductLists").Text);
            Assert.AreEqual("ProductList20", GetGridCell(9, "Name", "ProductLists").Text);

            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("ProductList1", GetGridCell(0, "Name", "ProductLists").Text);
            Assert.AreEqual("ProductList10", GetGridCell(9, "Name", "ProductLists").Text);
        }

    }
}