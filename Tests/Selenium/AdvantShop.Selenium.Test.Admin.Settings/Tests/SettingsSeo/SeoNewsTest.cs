using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using System.Threading;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.SeleniumTest.Admin.Settings.SettingsSEO
{
    [TestFixture]
    public class SettingsSeoNewsTest : BaseSeleniumTest
    {

        [OneTimeSetUp]
        public void SetupTest()
        {
            InitializeService.RollBackDatabase();
            InitializeService.ClearData(ClearType.SettingsProductsPerPage);
            InitializeService.LoadData(

           "data\\Admin\\Settings\\SettingsSeo\\Settings.Settings.csv"

           );

            Init();
        }


        [Test]
        public void DefaultSeoNews()
        {
            testname = "DefaultSeoNews";
            VerifyBegin(testname);

            GoToAdmin("news/edit/7");
            ScrollTo(By.Id("URL"));
            if (!driver.FindElement(By.Id("DefaultMeta")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"DefaultMetaCheckbox\"]")).Click();
                ScrollTo(By.Id("header-top"));
                driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
                Thread.Sleep(2000);
            }

            GoToAdmin("settingsseo");
            ScrollTo(By.Id("CategoryNewsDefaultH1"));

            driver.FindElement(By.Id("NewsDefaultTitle")).Click();
            driver.FindElement(By.Id("NewsDefaultTitle")).Clear();
            driver.FindElement(By.Id("NewsDefaultTitle")).SendKeys("New title News");

            driver.FindElement(By.Id("NewsDefaultMetaKeywords")).Click();
            driver.FindElement(By.Id("NewsDefaultMetaKeywords")).Clear();
            driver.FindElement(By.Id("NewsDefaultMetaKeywords")).SendKeys("New meta keywords 1 News, New meta keywords 2 News, New meta keywords 3 News");

            driver.FindElement(By.Id("NewsDefaultMetaDescription")).Click();
            driver.FindElement(By.Id("NewsDefaultMetaDescription")).Clear();
            driver.FindElement(By.Id("NewsDefaultMetaDescription")).SendKeys("New description News");

            driver.FindElement(By.Id("NewsDefaultH1")).Click();
            driver.FindElement(By.Id("NewsDefaultH1")).Clear();
            driver.FindElement(By.Id("NewsDefaultH1")).SendKeys("New h1 News");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"settingsSeoSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingsseo");

            VerifyAreEqual("New title News", driver.FindElement(By.Id("NewsDefaultTitle")).GetAttribute("value"), "default seo news title admin");
            VerifyAreEqual("New meta keywords 1 News, New meta keywords 2 News, New meta keywords 3 News", driver.FindElement(By.Id("NewsDefaultMetaKeywords")).GetAttribute("value"), "default seo news keywords admin");
            VerifyAreEqual("New description News", driver.FindElement(By.Id("NewsDefaultMetaDescription")).GetAttribute("value"), "default seo news description admin");
            VerifyAreEqual("New h1 News", driver.FindElement(By.Id("NewsDefaultH1")).GetAttribute("value"), "default seo news h1 admin");

            //check client
            GoToClient("news/fox");
            VerifyAreEqual("New title News", driver.Title, "default seo news title client");
            VerifyAreEqual("New meta keywords 1 News, New meta keywords 2 News, New meta keywords 3 News", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "default seo news keywords client");
            VerifyAreEqual("New description News", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "default seo news description client");
            VerifyAreEqual("New h1 News", driver.FindElement(By.TagName("h1")).Text, "default seo news h1 client");

            VerifyFinally(testname);
        }

        [Test]
        public void DefaultSeoNewsVariables()
        {
            testname = "DefaultSeoNewsVariables";
            VerifyBegin(testname);

            //set default meta
            GoToAdmin("news/edit/6");
            ScrollTo(By.Id("URL"));
            if (!driver.FindElement(By.Id("DefaultMeta")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"DefaultMetaCheckbox\"]")).Click();
                ScrollTo(By.Id("header-top"));
                driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
                Thread.Sleep(2000);
            }

            GoToAdmin("settingsseo");
            ScrollTo(By.Id("CategoryNewsDefaultH1"));

            driver.FindElement(By.Id("NewsDefaultTitle")).Click();
            driver.FindElement(By.Id("NewsDefaultTitle")).Clear();
            driver.FindElement(By.Id("NewsDefaultTitle")).SendKeys("#STORE_NAME# - #NEWS_NAME#");

            driver.FindElement(By.Id("NewsDefaultMetaKeywords")).Click();
            driver.FindElement(By.Id("NewsDefaultMetaKeywords")).Clear();
            driver.FindElement(By.Id("NewsDefaultMetaKeywords")).SendKeys("#STORE_NAME# - #NEWS_NAME#");

            driver.FindElement(By.Id("NewsDefaultMetaDescription")).Click();
            driver.FindElement(By.Id("NewsDefaultMetaDescription")).Clear();
            driver.FindElement(By.Id("NewsDefaultMetaDescription")).SendKeys("#STORE_NAME# - #NEWS_NAME#");

            driver.FindElement(By.Id("NewsDefaultH1")).Click();
            driver.FindElement(By.Id("NewsDefaultH1")).Clear();
            driver.FindElement(By.Id("NewsDefaultH1")).SendKeys("#STORE_NAME# - #NEWS_NAME#");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"settingsSeoSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingsseo");

            VerifyAreEqual("#STORE_NAME# - #NEWS_NAME#", driver.FindElement(By.Id("NewsDefaultTitle")).GetAttribute("value"), "default seo news title admin");
            VerifyAreEqual("#STORE_NAME# - #NEWS_NAME#", driver.FindElement(By.Id("NewsDefaultMetaKeywords")).GetAttribute("value"), "default seo news keywords admin");
            VerifyAreEqual("#STORE_NAME# - #NEWS_NAME#", driver.FindElement(By.Id("NewsDefaultMetaDescription")).GetAttribute("value"), "default seo news description admin");
            VerifyAreEqual("#STORE_NAME# - #NEWS_NAME#", driver.FindElement(By.Id("NewsDefaultH1")).GetAttribute("value"), "default seo news h1 admin");

            //check client
            GoToClient("news/armani");
            VerifyAreEqual("Мой магазин - Состоялся показ Armani Prive на Неделе высокой моды в Париже", driver.Title, "default seo news title client");
            VerifyAreEqual("Мой магазин - Состоялся показ Armani Prive на Неделе высокой моды в Париже", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "default seo news keywords client");
            VerifyAreEqual("Мой магазин - Состоялся показ Armani Prive на Неделе высокой моды в Париже", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "default seo news description client");
            VerifyAreEqual("Мой магазин - Состоялся показ Armani Prive на Неделе высокой моды в Париже", driver.FindElement(By.TagName("h1")).Text, "default seo news h1 client");

            VerifyFinally(testname);
        }

        [Test]
        public void DefaultSeoNewsReset()
        {
            testname = "DefaultSeoNewsReset";
            VerifyBegin(testname);

            //admin set meta for news
            GoToAdmin("news/edit/2");
            ScrollTo(By.Id("URL"));
            if (driver.FindElement(By.Id("DefaultMeta")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"DefaultMetaCheckbox\"]")).Click();
            }

            WaitForElem(By.Id("SeoTitle"));
            driver.FindElement(By.Id("SeoTitle")).Clear();
            driver.FindElement(By.Id("SeoTitle")).SendKeys("News_Title");
            driver.FindElement(By.Id("SeoKeywords")).Clear();
            driver.FindElement(By.Id("SeoKeywords")).SendKeys("News_SeoKeywords");
            driver.FindElement(By.Id("SeoDescription")).Clear();
            driver.FindElement(By.Id("SeoDescription")).SendKeys("News_SeoDescription");
            driver.FindElement(By.Id("SeoH1")).Clear();
            driver.FindElement(By.Id("SeoH1")).SendKeys("News_H1");
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);

            //pre check client
            GoToClient("news/news3");
            VerifyAreEqual("News_Title", driver.Title, "pre check seo news title client");
            VerifyAreEqual("News_H1", driver.FindElement(By.TagName("h1")).Text, "pre check seo news h1 client");
            VerifyAreEqual("News_SeoKeywords", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "pre check seo news keywords client");
            VerifyAreEqual("News_SeoDescription", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "pre check seo news description client");

            //reset meta
            GoToAdmin("settingsseo");
            ScrollTo(By.Id("CategoryNewsDefaultH1"));

            driver.FindElement(By.Id("NewsDefaultTitle")).Click();
            driver.FindElement(By.Id("NewsDefaultTitle")).Clear();
            driver.FindElement(By.Id("NewsDefaultTitle")).SendKeys("1");

            driver.FindElement(By.Id("NewsDefaultMetaKeywords")).Click();
            driver.FindElement(By.Id("NewsDefaultMetaKeywords")).Clear();
            driver.FindElement(By.Id("NewsDefaultMetaKeywords")).SendKeys("2");

            driver.FindElement(By.Id("NewsDefaultMetaDescription")).Click();
            driver.FindElement(By.Id("NewsDefaultMetaDescription")).Clear();
            driver.FindElement(By.Id("NewsDefaultMetaDescription")).SendKeys("3");

            driver.FindElement(By.Id("NewsDefaultH1")).Click();
            driver.FindElement(By.Id("NewsDefaultH1")).Clear();
            driver.FindElement(By.Id("NewsDefaultH1")).SendKeys("4");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"settingsSeoSave\"]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("settingsseo");
            ScrollTo(By.Id("NewsDefaultMetaDescription"));

            driver.FindElement(By.LinkText("Сбросить мета информацию для всех новостей")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(3000);

            //check client
            GoToClient("news/news3");
            VerifyAreEqual("1", driver.Title, "reset seo news title client");
            VerifyAreEqual("2", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "reset seo news keywords client");
            VerifyAreEqual("3", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "reset seo news description client");
            VerifyAreEqual("4", driver.FindElement(By.TagName("h1")).Text, "reset seo news h1 client");

            VerifyFinally(testname);
        }
    }
}