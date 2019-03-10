using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using AdvantShop.Selenium.Core.Infrastructure;
using System.Collections.ObjectModel;

namespace AdvantShop.SeleniumTest.Admin.CMS.News
{
    [TestFixture]
    public class CMSNewsEditTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CMS | ClearType.Catalog);
            InitializeService.LoadData(
                "data\\Admin\\CMS\\News\\Settings.News.csv",
                "data\\Admin\\CMS\\News\\Settings.NewsCategory.csv",
                "data\\Admin\\CMS\\News\\Catalog.Photo.csv"
           );

            Init();
        }

        [Test]
        public void NewsEdit()
        {
            //pre check
            GoToAdmin("news");
            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("title Test News 145");
            DropFocus("h1");

            Assert.AreEqual("title Test News 145", GetGridCell(0, "Title").Text);
            Assert.AreEqual("NewsCategory3", GetGridCell(0, "NewsCategory").Text);
            Assert.AreEqual("26.12.2014 10:40", GetGridCell(0, "AddingDateFormatted").Text);
            Assert.IsFalse(GetGridCell(0, "ShowOnMainPage").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.IsTrue(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            GetGridCell(0, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil")).Click();
            WaitForElem(By.Id("Title"));

            Assert.IsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Новость \"title Test News 145\""));

            driver.FindElement(By.Id("Title")).Click();
            driver.FindElement(By.Id("Title")).Clear();
            driver.FindElement(By.Id("Title")).SendKeys("News edited");

            //show on main page
            driver.FindElement(By.CssSelector("[data-e2e=\"ShowOnMainPage\"] span")).Click();

       (new SelectElement(driver.FindElement(By.Id("NewsCategoryId")))).SelectByText("NewsCategory99");

            driver.FindElements(By.CssSelector(".input-group-addon"))[0].Click();

            driver.FindElement(By.CssSelector("[colspan=\"5\"]")).Click();
            driver.FindElement(By.CssSelector("[colspan=\"5\"]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"2015\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"янв.\")]")).Click();
            driver.FindElement(By.XPath("//td[contains(text(),\"17\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"13:00\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"13:30\")]")).Click();

            SetCkText("Annotation edited", "TextAnnotation");
            SetCkText("News Text edited", "TextToPublication");

            //img from mashine
            ScrollTo(By.CssSelector("[data-e2e=\"imgAdd\"]"));
            driver.FindElement(By.XPath("(//input[@type='file'])[2]")).SendKeys(GetPicturePath("pic.png"));
            WaitForAjax();

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("news");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("title Test News 145");
            DropFocus("h1");
            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("News edited");
            DropFocus("h1");

            Assert.AreEqual("News edited", GetGridCell(0, "Title").Text);
            Assert.AreEqual("NewsCategory99", GetGridCell(0, "NewsCategory").Text);
            Assert.AreEqual("17.01.2015 13:30", GetGridCell(0, "AddingDateFormatted").Text);
            Assert.IsTrue(GetGridCell(0, "ShowOnMainPage").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);
            Assert.IsTrue(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            //check client
            GoToClient("news/test_news_145");

            Assert.IsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("News edited"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".news-categories.block")).Text.Contains("NewsCategory99"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".breads")).Text.Contains("NewsCategory99"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".breads")).Text.Contains("News edited"));

            Assert.AreEqual("Мой магазин - News edited", driver.Title);
            Assert.AreEqual("Мой магазин - News edited", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"));
            Assert.AreEqual("Мой магазин - News edited", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"));

            Assert.IsTrue(driver.PageSource.Contains("News Text edited"));
            Assert.IsFalse(driver.PageSource.Contains("Annotation edited"));
            Assert.IsTrue(driver.PageSource.Contains("17 января 2015"));

            GoToClient("newscategory/newscategory_url99");

            Assert.IsTrue(driver.PageSource.Contains("Annotation edited"));
            Assert.IsFalse(driver.PageSource.Contains("News Text edited"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".news-link-title")).Text.Contains("News edited"));
            Assert.IsFalse(driver.FindElement(By.CssSelector("[title=\"News edited\"]")).GetAttribute("src").Contains("nophoto"));

            GoToClient();

            Assert.IsTrue(driver.FindElement(By.CssSelector(".block.news-block")).Text.Contains("News edited"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".block.news-block")).Text.Contains("17 января 2015"));
        }

        [Test]
        public void NewsEditSeo()
        {
            //pre check 
            GoToClient("news/test_news_3");

            Assert.IsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("title Test News 3"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".news-categories.block")).Text.Contains("NewsCategory1"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".breads")).Text.Contains("NewsCategory1"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".breads")).Text.Contains("title Test News 3"));

            Assert.AreEqual("Мой магазин - title Test News 3", driver.Title);
            Assert.AreEqual("Мой магазин - title Test News 3", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"));
            Assert.AreEqual("Мой магазин - title Test News 3", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"));

            GoToClient();

            Assert.IsTrue(driver.FindElement(By.CssSelector(".block.news-block")).Text.Contains("title Test News 3"));

            GoToAdmin("news/edit/3");

            //not show on main page
            driver.FindElement(By.CssSelector("[data-e2e=\"ShowOnMainPage\"] span")).Click();

            //check delete img
            ScrollTo(By.CssSelector("[data-e2e=\"imgDel\"]"));
            driver.FindElement(By.CssSelector("[data-e2e=\"imgDel\"]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(1000);

            ScrollTo(By.Id("URL"));
            driver.FindElement(By.Id("URL")).Click();
            driver.FindElement(By.Id("URL")).Clear();
            driver.FindElement(By.Id("URL")).SendKeys("test_news_3_edited");

            //seo
            if (driver.FindElement(By.Id("DefaultMeta")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"DefaultMetaCheckbox\"]")).Click();
            }
            WaitForElem(By.Id("SeoTitle"));

            driver.FindElement(By.Id("SeoH1")).Click();
            driver.FindElement(By.Id("SeoH1")).Clear();
            driver.FindElement(By.Id("SeoH1")).SendKeys("News 3 Test SEO H1");

            driver.FindElement(By.Id("SeoTitle")).Click();
            driver.FindElement(By.Id("SeoTitle")).Clear();
            driver.FindElement(By.Id("SeoTitle")).SendKeys("News 3 Test SEO Title");

            driver.FindElement(By.Id("SeoKeywords")).Click();
            driver.FindElement(By.Id("SeoKeywords")).Clear();
            driver.FindElement(By.Id("SeoKeywords")).SendKeys("News 3 Test SEO Meta Keywords");

            driver.FindElement(By.Id("SeoDescription")).Click();
            driver.FindElement(By.Id("SeoDescription")).Clear();
            driver.FindElement(By.Id("SeoDescription")).SendKeys("News 3 Test SEO Meta Description");

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("news");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("title Test News 3");
            DropFocus("h1");

            Assert.AreEqual("title Test News 3", GetGridCell(5, "Title").Text);
            Assert.IsFalse(GetGridCell(5, "ShowOnMainPage").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            //check client
            GoToClient("news/test_news_3_edited");

            Assert.IsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("News 3 Test SEO H1"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".breads")).Text.Contains("title Test News 3"));
            Assert.IsFalse(driver.FindElement(By.CssSelector(".breads")).Text.Contains("News 3 Test SEO H1"));
            Assert.AreEqual("News 3 Test SEO Title", driver.Title);
            Assert.AreEqual("News 3 Test SEO Meta Keywords", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"));
            Assert.AreEqual("News 3 Test SEO Meta Description", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"));

            GoToClient("newscategory/newscategory_url1?page=0");

            Assert.IsTrue(driver.FindElements(By.CssSelector("[title=\"title Test News 3\"]")).Count == 1);

            GoToClient();

            Assert.IsFalse(driver.FindElement(By.CssSelector(".block.news-block")).Text.Contains("title Test News 3"));
        }

        [Test]
        public void NewsEditDelete()
        {
            //pre check
            Assert.IsFalse(Is404Page("news/test_news_139"));

            GoToAdmin("news/edit/139");
            
            driver.FindElement(By.XPath("//a[contains(text(),\"Удалить\")]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(1000);

            //check admin
            GoToAdmin("news");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("title Test News 139");
            DropFocus("h1");

            Assert.IsTrue(driver.PageSource.Contains("Ни одной записи не найдено"));

            //check client
            Assert.IsTrue(Is404Page("news/test_news_139"));
        }

        [Test]
        public void NewsEditView()
        {
            GoToAdmin("news/edit/123");

            Assert.IsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Новость \"title Test News 123\""));

            Assert.IsTrue(GetButton(eButtonType.Simple, "ViewNews").GetAttribute("href").Contains("/news/test_news_123"));

            GetButton(eButtonType.Simple, "ViewNews").Click();
            Thread.Sleep(1000);

            Functions.OpenNewTab(driver, baseURL);
            Assert.IsTrue(driver.WindowHandles.Count.Equals(2));
            
            Assert.IsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("title Test News 123"));

            Functions.CloseTab(driver, baseURL);
        }

        [Test]
        public void NewsEditEnabled()
        {
            //pre check
            Assert.IsTrue(Is404Page("news/test_news_55"));

            GoToAdmin("news/edit/55");

            Assert.IsFalse(driver.FindElement(By.CssSelector("[data-e2e=\"Enabled\"] input")).Selected);

            driver.FindElement(By.CssSelector("[data-e2e=\"Enabled\"] span")).Click();

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("news");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("title Test News 55");
            DropFocus("h1");

            Assert.IsTrue(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            GoToAdmin("news/edit/55");

            Assert.IsTrue(driver.FindElement(By.CssSelector("[data-e2e=\"Enabled\"] input")).Selected);

            //check client
            Assert.IsFalse(Is404Page("news/test_news_55"));
        }


    }
}
