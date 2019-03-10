using System;
using NUnit.Framework;
using System.Threading;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace AdvantShop.SeleniumTest.Admin.Properties
{
    [TestFixture]
    public class PropertiesPaginationAndView : BaseSeleniumTest
    {
         
         

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.Catalog);
            InitializeService.LoadData(
                "Data\\Admin\\Properties\\Catalog.Category.csv",
                "data\\Admin\\Properties\\Catalog.Brand.csv",
                "Data\\Admin\\Properties\\Catalog.Property.csv",
                 "Data\\Admin\\Properties\\Catalog.PropertyValue.csv",
                 "Data\\Admin\\Properties\\Catalog.ProductPropertyValue.csv",
                "Data\\Admin\\Properties\\Catalog.Product.csv",
                "Data\\Admin\\Properties\\Catalog.Offer.csv",
                "Data\\Admin\\Properties\\Catalog.ProductCategories.csv",
                "Data\\Admin\\Properties\\Catalog.PropertyGroup.csv"
                );

             
            Init();

        }
        [Test]
        public void Present10Page()
        {
             GoToAdmin("properties");

            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Assert.AreEqual("Property11", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property20", GetGridCell(9, "Name").Text);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            Assert.AreEqual("Property21", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property30", GetGridCell(9, "Name").Text);
        }
        [Test]
        public void Present10PageToNext()
        {
             GoToAdmin("properties");

            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("Property11", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property20", GetGridCell(9, "Name").Text);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("Property21", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property30", GetGridCell(9, "Name").Text);
        }
        [Test]
        public void Present10PageToPrevious()
        {
             GoToAdmin("properties");

            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("Property11", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property20", GetGridCell(9, "Name").Text);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
        }
        [Test]
        public void Present10PageToEnd()
        {
             GoToAdmin("properties");

            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            Assert.AreEqual("Property101", GetGridCell(0, "Name").Text);
        }
        [Test]
        public void Present10PageToBegin()
        {
             GoToAdmin("properties");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("Property11", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property20", GetGridCell(9, "Name").Text);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("Property21", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property30", GetGridCell(9, "Name").Text);

            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
        }
    }
}