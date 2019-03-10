﻿using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace AdvantShop.SeleniumTest.Admin.CMS.NewsCategory
{
    [TestFixture]
    public class CMSNewsCategoryFilterUrlPathTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CMS);
            InitializeService.LoadData(
                "data\\Admin\\CMS\\NewsCategory\\Settings.News.csv",
                "data\\Admin\\CMS\\NewsCategory\\Settings.NewsCategory.csv"
           );

            Init();
        }

        [Test]
        public void NewsCategoryFilterUrlPath()
        {
            GoToAdmin("newscategory");

            //search by exist name 
            Functions.GridFilterSet(driver, baseURL, name: "UrlPath");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("newscategory_url22");
            DropFocus("h1");
            Assert.AreEqual("newscategory_url22", GetGridCell(0, "UrlPath").Text);
            Assert.AreEqual("Найдено записей: 1", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            //search by not exist name
            GoToAdmin("newscategory");

            Functions.GridFilterSet(driver, baseURL, name: "UrlPath");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("2212312news_category");
            DropFocus("h1");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //search too much symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("1111111111222222222222222222333333333333334444444444rrrrrrrrrrrreeeeeeeeewwwwwwwwwwwwwwwwwww");
            DropFocus("h1");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //search invalid symbols
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("########@@@@@@@@&&&&&&&******,,,,..");
            DropFocus("h1");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //check delete filter
            Functions.GridFilterClose(driver, baseURL, name: "UrlPath");
            Assert.AreEqual("Найдено записей: 102", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
        }


        [Test]
        public void NewzCategoryFilterUrlPathDelete()
        {
            GoToAdmin("newscategory");

            Functions.GridFilterSet(driver, baseURL, name: "UrlPath");
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"gridFilterItemInput\"]")).SendKeys("newscategory_url2");
            DropFocus("h1");
            Assert.AreEqual("newscategory_url2", GetGridCell(0, "UrlPath").Text);
            Assert.AreEqual("Найдено записей: 11", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);

            driver.FindElement(By.CssSelector("[data-e2e=\"gridHeaderCheckboxWrapSelectAll\"]")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.CssSelector("[data-e2e=\"gridSelectionSelectAllFn\"]")).Click();
            Thread.Sleep(500);
            Functions.GridDropdownDelete(driver, baseURL);

            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            Functions.GridFilterClose(driver, baseURL, name: "UrlPath");

            GoToAdmin("newscategory");

            Assert.AreEqual("Найдено записей: 91", driver.FindElement(By.CssSelector(".input-group-addon.ui-grid-custom-filter-total")).Text);
        }
    }
}