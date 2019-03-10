using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace AdvantShop.SeleniumTest.Admin.CMS.NewsCategory
{
    [TestFixture]
    public class CMSNewsCategoryAddTest : BaseSeleniumTest
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
        public void NewsCategoryAdd()
        {
            GoToAdmin("newscategory");

            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));

            Assert.IsTrue(driver.FindElement(By.TagName("h2")).Text.Contains("Добавить категорию новостей"));

            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryName\"]")).SendKeys("NewCategoryAdd");

            driver.FindElement(By.Id("UrlPath")).Click();
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("news_category_added");

            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategorySort\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategorySort\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategorySort\"]")).SendKeys("10");

            driver.FindElement(By.CssSelector("[data-e2e=\"BtnSave\"]")).Click();
            Thread.Sleep(1000);

            //check admin
            GoToAdmin("newscategory");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("NewCategoryAdd");
            DropFocus("h1");
            WaitForAjax();

            Assert.AreEqual("NewCategoryAdd", GetGridCell(0, "Name").Text);
            Assert.AreEqual("news_category_added", GetGridCell(0, "UrlPath").Text);
            Assert.AreEqual("10", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));

            //check client
            GoToClient("newscategory/news_category_added");

            Assert.IsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Новости"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".breads")).Text.Contains("NewCategoryAdd"));
            Assert.AreEqual("Новости - Мой магазин", driver.Title);
            Assert.AreEqual("", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"));
            Assert.AreEqual("", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"));
        }

        [Test]
        public void NewsCategoryAddSeo()
        {
            GoToAdmin("newscategory");

            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));
            
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryName\"]")).SendKeys("NewCategorySEO");

            driver.FindElement(By.Id("UrlPath")).Click();
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("news_category_added_seo");

            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryH1\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryH1\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryH1\"]")).SendKeys("NewCategorySEO H1");

            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryTitle\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryTitle\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryTitle\"]")).SendKeys("NewCategorySEO Title");

            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryMetaKey\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryMetaKey\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryMetaKey\"]")).SendKeys("NewCategorySEO Meta Keywords");

            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryMetaDesc\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryMetaDesc\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryMetaDesc\"]")).SendKeys("NewCategorySEO Meta Description");

            driver.FindElement(By.CssSelector("[data-e2e=\"BtnSave\"]")).Click();
            Thread.Sleep(1000);

            //check admin
            GoToAdmin("newscategory");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("NewCategorySEO");
            DropFocus("h1");
            WaitForAjax();

            Assert.AreEqual("NewCategorySEO", GetGridCell(0, "Name").Text);
            Assert.AreEqual("0", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));

            //check client
            GoToClient("newscategory/news_category_added_seo");

            Assert.IsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("NewCategorySEO H1"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".breads")).Text.Contains("NewCategorySEO"));
            Assert.AreEqual("NewCategorySEO Title", driver.Title);
            Assert.AreEqual("NewCategorySEO Meta Keywords", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"));
            Assert.AreEqual("NewCategorySEO Meta Description", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"));
        }
    }
}
