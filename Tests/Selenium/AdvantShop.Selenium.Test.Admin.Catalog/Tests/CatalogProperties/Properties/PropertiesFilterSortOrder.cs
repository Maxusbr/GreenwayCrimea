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
    public class PropertiesFilterSortOrder : BaseSeleniumTest
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
        public void BySortOrder()
        {
             GoToAdmin("properties");
            Functions.GridFilterSet(driver, baseURL, "SortOrder");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("10");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("50");
            DropFocus("h1");


            Assert.AreEqual("Property10", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property19", GetGridCell(9, "Name").Text);
            //close
            Functions.GridFilterClose(driver, baseURL, "SortOrder");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
        }
        [Test]
        public void BySortOrderPresent20()
        {
             GoToAdmin("properties");
            Functions.GridPaginationSelect20(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterSet(driver, baseURL, "SortOrder");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("10");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("50");
            DropFocus("h1");

            Assert.AreEqual("Property10", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property29", GetGridCell(19, "Name").Text);
            //close
            Functions.GridFilterClose(driver, baseURL, "SortOrder");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
        }
        [Test]
        public void BySortOrderPresent50()
        {
             GoToAdmin("properties");
            Functions.GridPaginationSelect50(driver, baseURL);
            ScrollTo(By.Id("header-top"));
            Functions.GridFilterSet(driver, baseURL, "SortOrder");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("10");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("50");
            DropFocus("h1");


            Assert.AreEqual("Property10", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property50", GetGridCell(40, "Name").Text);
            //close
            Functions.GridFilterClose(driver, baseURL, "SortOrder");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
        }
        [Test]
        public void BySortOrderPage()
        {
             GoToAdmin("properties");
            Functions.GridFilterSet(driver, baseURL, "SortOrder");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("10");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("50");
            DropFocus("h1");


            Assert.AreEqual("Property10", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property19", GetGridCell(9, "Name").Text);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(4) a")).Click();
            Assert.AreEqual("Property20", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property29", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(5) a")).Click();
            Assert.AreEqual("Property30", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property39", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(6) a")).Click();
            Assert.AreEqual("Property40", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property49", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-page:nth-child(7) a")).Click();
            Assert.AreEqual("Property50", GetGridCell(0, "Name").Text);
            ScrollTo(By.Id("header-top"));
            //close
            Functions.GridFilterClose(driver, baseURL, "SortOrder");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
        }

        [Test]
        public void BySortOrderPageToBegin()
        {
             GoToAdmin("properties");
            Functions.GridFilterSet(driver, baseURL, "SortOrder");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("10");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("50");
            DropFocus("h1");

            Assert.AreEqual("Property10", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property19", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("Property20", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property29", GetGridCell(9, "Name").Text);
           
            //to begin
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-first a")).Click();
            Assert.AreEqual("Property10", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property19", GetGridCell(9, "Name").Text);
            ScrollTo(By.Id("header-top"));
            //close
            Functions.GridFilterClose(driver, baseURL, "SortOrder");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
        }

        [Test]
        public void BySortOrderPageToEnd()
        {
             GoToAdmin("properties");
            Functions.GridFilterSet(driver, baseURL, "SortOrder");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("10");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("50");
            DropFocus("h1");


            Assert.AreEqual("Property10", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property19", GetGridCell(9, "Name").Text);

            //to end
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            Thread.Sleep(1000);
            Assert.AreEqual("Property50", GetGridCell(0, "Name").Text);
            ScrollTo(By.Id("header-top"));
            //close
            Functions.GridFilterClose(driver, baseURL, "SortOrder");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
        }

        [Test]
        public void BySortOrderPageToNext()
        {
             GoToAdmin("properties");
            Functions.GridFilterSet(driver, baseURL, "SortOrder");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("10");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("50");
            DropFocus("h1");

            Assert.AreEqual("Property10", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property19", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("Property20", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property29", GetGridCell(9, "Name").Text);
            ScrollTo(By.Id("header-top"));
            //Close
            Functions.GridFilterClose(driver, baseURL, "SortOrder");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
        }

        [Test]
        public void BySortOrderPageToPrevious()
        {
             GoToAdmin("properties");
            Functions.GridFilterSet(driver, baseURL, "SortOrder");
            DropFocus("h1");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("10");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterBlock\"][data-e2e-grid-filter-block-name=\"SortOrder\"] [data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("50");
            DropFocus("h1");

            Assert.AreEqual("Property10", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property19", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("Property20", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property29", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Assert.AreEqual("Property30", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property39", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Assert.AreEqual("Property20", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property29", GetGridCell(9, "Name").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-prev a")).Click();
            Assert.AreEqual("Property10", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property19", GetGridCell(9, "Name").Text);
            ScrollTo(By.Id("header-top"));
            //close
            Functions.GridFilterClose(driver, baseURL, "SortOrder");
            Assert.AreEqual("Property1", GetGridCell(0, "Name").Text);
            Assert.AreEqual("Property10", GetGridCell(9, "Name").Text);
        }
    }
}
