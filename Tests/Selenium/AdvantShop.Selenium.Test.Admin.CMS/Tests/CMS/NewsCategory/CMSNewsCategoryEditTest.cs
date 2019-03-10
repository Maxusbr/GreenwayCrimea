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
    public class CMSNewsCategoryEditTest : BaseSeleniumTest
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
        public void NewsCategoryEditAddSeo()
        {
            //pre check client
            GoToClient("newscategory/newscategory_url11");

            Assert.IsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Новости"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".breads")).Text.Contains("NewsCategory11"));
            Assert.AreEqual("Новости - Мой магазин", driver.Title);
            Assert.AreEqual("", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"));
            Assert.AreEqual("", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"));

            GoToAdmin("newscategory");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("NewsCategory11");
            DropFocus("h1");
            WaitForAjax();
            Assert.AreEqual("NewsCategory11", GetGridCell(0, "Name").Text);
            Assert.AreEqual("newscategory_url11", GetGridCell(0, "UrlPath").Text);
            Assert.AreEqual("11", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));

            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil.news-category-pointer")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));

            Assert.IsTrue(driver.FindElement(By.TagName("h2")).Text.Contains("Редактировать категорию новостей"));

            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryName\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryName\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryName\"]")).SendKeys("11 NewsCategory edited name");

            driver.FindElement(By.Id("UrlPath")).Click();
            driver.FindElement(By.Id("UrlPath")).Clear();
            driver.FindElement(By.Id("UrlPath")).SendKeys("changed_url_11");

            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategorySort\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategorySort\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategorySort\"]")).SendKeys("1");
            
            //add seo
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryH1\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryH1\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryH1\"]")).SendKeys("CategorySEO H1");

            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryTitle\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryTitle\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryTitle\"]")).SendKeys("CategorySEO Title");

            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryMetaKey\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryMetaKey\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryMetaKey\"]")).SendKeys("CategorySEO Meta Keywords");

            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryMetaDesc\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryMetaDesc\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryMetaDesc\"]")).SendKeys("CategorySEO Meta Description");

            driver.FindElement(By.CssSelector("[data-e2e=\"BtnSave\"]")).Click();
            Thread.Sleep(1000);

            //check admin
            GoToAdmin("newscategory");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("NewsCategory11");
            DropFocus("h1");
            WaitForAjax();
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("11 NewsCategory edited name");
            DropFocus("h1");
            WaitForAjax();

            Assert.AreEqual("11 NewsCategory edited name", GetGridCell(0, "Name").Text);
            Assert.AreEqual("changed_url_11", GetGridCell(0, "UrlPath").Text);
            Assert.AreEqual("1", GetGridCell(0, "SortOrder").FindElement(By.TagName("input")).GetAttribute("value"));

            //check client
            GoToClient("newscategory/changed_url_11");
            
            Assert.IsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("CategorySEO H1"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".breads")).Text.Contains("11 NewsCategory edited name"));
            Assert.AreEqual("CategorySEO Title", driver.Title);
            Assert.AreEqual("CategorySEO Meta Keywords", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"));
            Assert.AreEqual("CategorySEO Meta Description", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"));
        }

        [Test]
        public void NewsCategoryEditDelSeo()
        {

            GoToAdmin("newscategory");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("NewsCategory20");
            DropFocus("h1");
            WaitForAjax();
            Assert.AreEqual("NewsCategory20", GetGridCell(0, "Name").Text);

            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil.news-category-pointer")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));
            
            //add seo
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryH1\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryH1\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryH1\"]")).SendKeys("CategorySEO H1");

            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryTitle\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryTitle\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryTitle\"]")).SendKeys("CategorySEO Title");

            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryMetaKey\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryMetaKey\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryMetaKey\"]")).SendKeys("CategorySEO Meta Keywords");

            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryMetaDesc\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryMetaDesc\"]")).Clear();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryMetaDesc\"]")).SendKeys("CategorySEO Meta Description");

            driver.FindElement(By.CssSelector("[data-e2e=\"BtnSave\"]")).Click();
            Thread.Sleep(1000);

            //pre check client
            GoToClient("newscategory/newscategory_url20");

            Assert.IsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("CategorySEO H1"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".breads")).Text.Contains("NewsCategory20"));
            Assert.AreEqual("CategorySEO Title", driver.Title);
            Assert.AreEqual("CategorySEO Meta Keywords", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"));
            Assert.AreEqual("CategorySEO Meta Description", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"));

            //check admin
            GoToAdmin("newscategory");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("NewsCategory20");
            DropFocus("h1");
            WaitForAjax();

            Assert.AreEqual("NewsCategory20", GetGridCell(0, "Name").Text);

            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil.news-category-pointer")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));

            //delete seo
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryH1\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryH1\"]")).Clear();

            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryTitle\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryTitle\"]")).Clear();

            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryMetaKey\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryMetaKey\"]")).Clear();

            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryMetaDesc\"]")).Click();
            driver.FindElement(By.CssSelector("[data-e2e=\"NewsCategoryMetaDesc\"]")).Clear();

            driver.FindElement(By.CssSelector("[data-e2e=\"BtnSave\"]")).Click();
            Thread.Sleep(1000);

            //check client
            GoToClient("newscategory/newscategory_url20");

            Assert.IsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Новости"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".breads")).Text.Contains("NewsCategory20"));
            Assert.AreEqual("Новости - Мой магазин", driver.Title);
            Assert.AreEqual("", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"));
            Assert.AreEqual("", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"));
        }
    }
}
