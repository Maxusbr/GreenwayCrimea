﻿using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;

namespace AdvantShop.SeleniumTest.Admin.ProductLists
{
    [TestFixture]
    public class Prod_ProdListAddFilterAmountTest : BaseSeleniumTest
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
        public void AddProduct_ProductListFilterAmountMinMax()
        {
            Functions.AddProduct_ProductListsFilterFromTo(driver, baseURL, filterName: "Amount");

            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("200");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("250");
            DropFocus("h2");
            WaitForAjax();
            Assert.AreEqual("Найдено записей: 51", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
            Assert.AreEqual("TestProduct200", GetGridCell(0, "Name", "ProductsSelectvizr").Text);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-last a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct250", GetGridCell(0, "Name", "ProductsSelectvizr").Text);

            //close
            Functions.GridFilterClose(driver, baseURL, name: "Amount");
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text);
        }
      
        [Test]
        public void AddProduct_ProductListFilterAmountMinMaxNotExist()
        {
            Functions.AddProduct_ProductListsFilterFromTo(driver, baseURL, filterName: "Amount");

            //check min not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("500");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            DropFocus("h2");
            WaitForAjax();

            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //check max not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();

            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("2000");
            DropFocus("h2");
            WaitForAjax();
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect100(driver, baseURL);
            Assert.AreEqual("TestProduct189", GetGridCell(99, "Name", "ProductsSelectvizr").Text);
            
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct19", GetGridCell(0, "Name", "ProductsSelectvizr").Text);
            Assert.AreEqual("TestProduct53", GetGridCell(99, "Name", "ProductsSelectvizr").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct54", GetGridCell(0, "Name", "ProductsSelectvizr").Text);
            Assert.AreEqual("TestProduct99", GetGridCell(49, "Name", "ProductsSelectvizr").Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".pagination-next.disabled")).Count == 1);

            //check min and max not exist
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("500");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            DropFocus("h2");
            WaitForAjax();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }

        [Test]
        public void AddProduct_ProductListFilterAmountMinMaxInvalidSymbols()
        {
            Functions.AddProduct_ProductListsFilterFromTo(driver, baseURL, filterName: "Amount");

            //check min invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("########@@@@@@@@&&&&&&&******");
            DropFocus("h2");
            WaitForAjax();
            Assert.AreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Text);

            //check max invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("########@@@@@@@@&&&&&&&******");
            DropFocus("h2");
            WaitForAjax();
            Assert.AreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Text);

            //check min and max invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("########@@@@@@@@&&&&&&&******");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            DropFocus("h2");
            WaitForAjax();
            Assert.AreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Text);
            Assert.AreEqual("", driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Text);
        }

        [Test]
        public void AddProduct_ProductListFilterAmountMinMaxTooMuchSymbols()
        {
            Functions.AddProduct_ProductListsFilterFromTo(driver, baseURL, filterName: "Amount");

            //check min too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("1111111111");
            DropFocus("h2");
            WaitForAjax();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //check max too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).SendKeys("1111111111");
            DropFocus("h2");
            WaitForAjax();
            Assert.AreEqual("TestProduct1", GetGridCell(0, "Name", "ProductsSelectvizr").Text);
            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            Functions.GridPaginationSelect100(driver, baseURL);
            Assert.AreEqual("TestProduct189", GetGridCell(99, "Name", "ProductsSelectvizr").Text);

            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct19", GetGridCell(0, "Name", "ProductsSelectvizr").Text);
            Assert.AreEqual("TestProduct53", GetGridCell(99, "Name", "ProductsSelectvizr").Text);

            ScrollTo(By.CssSelector("[data-e2e=\"gridPaginationSelect\"]"));
            driver.FindElement(By.CssSelector(".pagination-next a")).Click();
            Thread.Sleep(2000);
            Assert.AreEqual("TestProduct54", GetGridCell(0, "Name", "ProductsSelectvizr").Text);
            Assert.AreEqual("TestProduct99", GetGridCell(49, "Name", "ProductsSelectvizr").Text);
            Assert.IsTrue(driver.FindElements(By.CssSelector(".pagination-next.disabled")).Count == 1);

            //check min and max too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeFrom\"]")).SendKeys("1111111111");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemRangeTo\"]")).Click();
            DropFocus("h2");
            WaitForAjax();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));
        }
    }
}

