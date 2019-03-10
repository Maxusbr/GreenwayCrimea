using System;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using AdvantShop.SeleniumTest.Core;
using OpenQA.Selenium.Support.UI;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.SeleniumTest.Admin.CMS.News
{
    [TestFixture]
    public class CMSNewsAddTest : BaseSeleniumTest
    {
        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.CMS);
            InitializeService.LoadData(
                "data\\Admin\\CMS\\News\\Settings.News.csv",
                "data\\Admin\\CMS\\News\\Settings.NewsCategory.csv"
           );

            Init();
        }

        [Test]
        public void NewsAdd()
        {
            GoToAdmin("news");
            
            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            WaitForElem(By.Id("Title"));

            Assert.IsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Новая новость"));

            driver.FindElement(By.Id("Title")).Click();
            driver.FindElement(By.Id("Title")).Clear();
            driver.FindElement(By.Id("Title")).SendKeys("Added News Test");

            //show on main page
            if (!driver.FindElement(By.Id("ShowOnMainPage")).Selected)

            {
                driver.FindElement(By.CssSelector("[data-e2e=\"ShowOnMainPage\"] span")).Click();
            }

       (new SelectElement(driver.FindElement(By.Id("NewsCategoryId")))).SelectByText("NewsCategory100");

            driver.FindElements(By.CssSelector(".input-group-addon"))[0].Click();

            driver.FindElement(By.CssSelector("[colspan=\"5\"]")).Click();
            driver.FindElement(By.CssSelector("[colspan=\"5\"]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"2015\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"янв.\")]")).Click();
            driver.FindElement(By.XPath("//td[contains(text(),\"17\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"13:00\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"13:30\")]")).Click();

            SetCkText("Annotation Text", "TextAnnotation");
            SetCkText("News Text Test", "TextToPublication");

            //img from mashine
            ScrollTo(By.CssSelector("[data-e2e=\"imgAdd\"]"));
            driver.FindElement(By.XPath("(//input[@type='file'])[2]")).SendKeys(GetPicturePath("pic.png"));
            Thread.Sleep(2000);

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("news");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Added News Test");
            DropFocus("h1");

            Assert.AreEqual("Added News Test", GetGridCell(0, "Title").Text);
            Assert.AreEqual("NewsCategory100", GetGridCell(0, "NewsCategory").Text);
            Assert.AreEqual("17.01.2015 13:30", GetGridCell(0, "AddingDateFormatted").Text);
            Assert.IsTrue(GetGridCell(0, "ShowOnMainPage").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            //check client
            GoToClient("news/added-news-test");

            Assert.IsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Added News Test"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".news-categories.block")).Text.Contains("NewsCategory100"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".breads")).Text.Contains("NewsCategory100"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".breads")).Text.Contains("Added News Test"));

            Assert.AreEqual("Мой магазин - Added News Test", driver.Title);
            Assert.AreEqual("Мой магазин - Added News Test", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"));
            Assert.AreEqual("Мой магазин - Added News Test", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"));

            Assert.IsTrue(driver.PageSource.Contains("News Text Test"));
            Assert.IsFalse(driver.PageSource.Contains("Annotation Text"));
            Assert.IsTrue(driver.PageSource.Contains("17 января 2015"));

            GoToClient("newscategory/newscategory_url100");

            Assert.IsTrue(driver.PageSource.Contains("Annotation Text"));
            Assert.IsFalse(driver.PageSource.Contains("News Text Test"));
            Assert.IsFalse(driver.FindElement(By.CssSelector("[title=\"Added News Test\"]")).GetAttribute("src").Contains("nophoto"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".news-link-title")).Text.Contains("Added News Test"));
            
            GoToClient();

            Assert.IsTrue(driver.FindElement(By.CssSelector(".block.news-block")).Text.Contains("Added News Test"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".block.news-block")).Text.Contains("17 января 2015"));
        }

        [Test]
        public void NewsAddSeo()
        {
            GoToAdmin("news/add");
            
            driver.FindElement(By.Id("Title")).Click();
            driver.FindElement(By.Id("Title")).Clear();
            driver.FindElement(By.Id("Title")).SendKeys("Added News Test SEO");

            //not show on main page
            if (driver.FindElement(By.Id("ShowOnMainPage")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"ShowOnMainPage\"] span")).Click();
            }

     (new SelectElement(driver.FindElement(By.Id("NewsCategoryId")))).SelectByText("NewsCategory10");

            driver.FindElements(By.CssSelector(".input-group-addon"))[0].Click();

            driver.FindElement(By.CssSelector("[colspan=\"5\"]")).Click();
            driver.FindElement(By.CssSelector("[colspan=\"5\"]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"2015\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"янв.\")]")).Click();
            driver.FindElement(By.XPath("//td[contains(text(),\"17\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"13:00\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"13:30\")]")).Click();

            SetCkText("Annotation Text SEO", "TextAnnotation");
            SetCkText("News Text Test SEO", "TextToPublication");

            //img by href
            ScrollTo(By.CssSelector("[data-e2e=\"imgByHref\"]"));
            driver.FindElement(By.CssSelector("[data-e2e=\"imgByHref\"]")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.CssSelector("[data-e2e=\"imgByHrefLinkText\"]")).SendKeys("https://upload.wikimedia.org/wikipedia/en/thumb/3/34/Mandaue_Cebu.png/80px-Mandaue_Cebu.png");
            driver.FindElement(By.CssSelector("[data-e2e=\"imgByHrefBtnSave\"]")).Click();
            Thread.Sleep(2000);

            driver.FindElement(By.Id("URL")).Click();
            driver.FindElement(By.Id("URL")).Clear();
            driver.FindElement(By.Id("URL")).SendKeys("news_added_seo");

            //seo
            ScrollTo(By.Name("DefaultMeta"));
            if (driver.FindElement(By.Id("DefaultMeta")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"DefaultMetaCheckbox\"]")).Click();
            }
            WaitForElem(By.Id("SeoTitle"));
            
            driver.FindElement(By.Id("SeoH1")).Click();
            driver.FindElement(By.Id("SeoH1")).Clear();
            driver.FindElement(By.Id("SeoH1")).SendKeys("Added News Test SEO H1");

            driver.FindElement(By.Id("SeoTitle")).Click();
            driver.FindElement(By.Id("SeoTitle")).Clear();
            driver.FindElement(By.Id("SeoTitle")).SendKeys("Added News Test SEO Title");

            driver.FindElement(By.Id("SeoKeywords")).Click();
            driver.FindElement(By.Id("SeoKeywords")).Clear();
            driver.FindElement(By.Id("SeoKeywords")).SendKeys("Added News Test SEO Meta Keywords");

            driver.FindElement(By.Id("SeoDescription")).Click();
            driver.FindElement(By.Id("SeoDescription")).Clear();
            driver.FindElement(By.Id("SeoDescription")).SendKeys("Added News Test SEO Meta Description");

            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("news");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Added News Test SEO");
            DropFocus("h1");

            Assert.AreEqual("Added News Test SEO", GetGridCell(0, "Title").Text);
            Assert.AreEqual("NewsCategory10", GetGridCell(0, "NewsCategory").Text);
            Assert.AreEqual("17.01.2015 13:30", GetGridCell(0, "AddingDateFormatted").Text);
            Assert.IsFalse(GetGridCell(0, "ShowOnMainPage").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            //check client
            GoToClient("news/news_added_seo");

            Assert.IsTrue(driver.FindElement(By.TagName("h1")).Text.Contains("Added News Test SEO H1"));
            Assert.IsTrue(driver.FindElement(By.CssSelector(".breads")).Text.Contains("Added News Test SEO"));
            Assert.IsFalse(driver.FindElement(By.CssSelector(".breads")).Text.Contains("Added News Test SEO H1"));
            Assert.AreEqual("Added News Test SEO Title", driver.Title);
            Assert.AreEqual("Added News Test SEO Meta Keywords", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"));
            Assert.AreEqual("Added News Test SEO Meta Description", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"));

            GoToClient("newscategory/newscategory_url10");
            
            Assert.IsFalse(driver.FindElement(By.CssSelector("[title=\"Added News Test SEO\"]")).GetAttribute("src").Contains("nophoto"));

            GoToClient();

            Assert.IsFalse(driver.FindElement(By.CssSelector(".block.news-block")).Text.Contains("Added News Test SEO"));
        }

        [Test]
        public void NewsAddDisabled()
        {
            GoToAdmin("news");

            driver.FindElement(By.CssSelector(".btn.btn-sm.btn-success.btn--margin-left")).Click();
            WaitForElem(By.Id("Title"));
            
            driver.FindElement(By.Id("Title")).Click();
            driver.FindElement(By.Id("Title")).Clear();
            driver.FindElement(By.Id("Title")).SendKeys("Added News Disabled");

            if (driver.FindElement(By.CssSelector("[data-e2e=\"Enabled\"] input")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"Enabled\"] span")).Click();
            }
            
     (new SelectElement(driver.FindElement(By.Id("NewsCategoryId")))).SelectByText("NewsCategory100");

            driver.FindElements(By.CssSelector(".input-group-addon"))[0].Click();

            driver.FindElement(By.CssSelector("[colspan=\"5\"]")).Click();
            driver.FindElement(By.CssSelector("[colspan=\"5\"]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"2015\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"янв.\")]")).Click();
            driver.FindElement(By.XPath("//td[contains(text(),\"17\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"13:00\")]")).Click();
            driver.FindElement(By.XPath("//span[contains(text(),\"13:30\")]")).Click();

            SetCkText("Annotation Text", "TextAnnotation");
            SetCkText("News Text Test", "TextToPublication");
            
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            string urlPath = driver.FindElement(By.Id("URL")).GetAttribute("value");

            //check admin
            GoToAdmin("news");

            GetGridFilter().Click();
            GetGridFilter().Clear();
            GetGridFilter().SendKeys("Added News Disabled");
            DropFocus("h1");

            Assert.AreEqual("Added News Disabled", GetGridCell(0, "Title").Text);
            Assert.IsFalse(GetGridCell(0, "Enabled").FindElement(By.CssSelector("[data-e2e=\"switchOnOffInput\"]")).Selected);

            //check client
            Assert.IsTrue(Is404Page("news/"+ urlPath));

            GoToClient();

            Assert.IsFalse(driver.FindElement(By.CssSelector(".block.news-block")).Text.Contains("Added News Disabled"));
        }
        
    }
}
