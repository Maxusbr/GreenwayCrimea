using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.GridProducts
{
    [TestFixture]
    public class GridProductsPaginationTest : BaseSeleniumTest
    {
        
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
           "data\\Admin\\Grid\\ProductsPageTest\\Catalog.Product.csv",
           "data\\Admin\\Grid\\ProductsPageTest\\Catalog.Offer.csv",
           "data\\Admin\\Grid\\ProductsPageTest\\Catalog.Category.csv",
           "data\\Admin\\Grid\\ProductsPageTest\\Catalog.ProductCategories.csv"
           );
             
            Init();
        }

        [Test]
        public void PageGridProducts()
        {
            GoToAdmin("catalog?categoryid=1");
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Assert.AreEqual("TestProduct11", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct20", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            Assert.AreEqual("TestProduct21", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct30", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            Assert.AreEqual("TestProduct31", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct40", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Assert.AreEqual("TestProduct41", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct50", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Assert.AreEqual("TestProduct51", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct60", GetGridCell(9, "Name").Text);
        }

        [Test]
        public void PageGridProductsToBegin()
        {
            GoToAdmin("catalog?categoryid=1");
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
           Assert.AreEqual("TestProduct11", GetGridCell(0, "Name").Text);
           Assert.AreEqual("TestProduct20", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("TestProduct21", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct30", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
           Assert.AreEqual("TestProduct31", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct40", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
             Assert.AreEqual("TestProduct41", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct50", GetGridCell(9, "Name").Text);

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
        }

        [Test]
        public void PageGridProductsToEnd()
        {
            GoToAdmin("catalog?categoryid=1");
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            //to end
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            Assert.AreEqual("TestProduct91", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct100", GetGridCell(9, "Name").Text);
        }

        [Test]
        public void PageGridProductsToNext()
        {
            GoToAdmin("catalog?categoryid=1");
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
             Assert.AreEqual("TestProduct11", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct20", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("TestProduct21", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct30", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
           Assert.AreEqual("TestProduct31", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct40", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("TestProduct41", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct50", GetGridCell(9, "Name").Text);
        }

        [Test]
        public void PageGridProductsToPrevious()
        {
            GoToAdmin("catalog?categoryid=1");
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("TestProduct11", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct20", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("TestProduct21", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct30", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Assert.AreEqual("TestProduct11", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct20", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPagination\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("TestProduct10", GetGridCell(9, "Name").Text);
        }
    }
}