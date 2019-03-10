using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using System.Threading;

namespace AdvantShop.SeleniumTest.Admin.Settings.SettingsSEO
{
    [TestFixture]
    public class SettingsSeoStaticPageTest : BaseSeleniumTest
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
        public void DefaultSeoStaticPage()
        {
            testname = "DefaultSeoStaticPage";
            VerifyBegin(testname);

            GoToAdmin("staticpages/edit/129");
            ScrollTo(By.Id("URL"));
            if (!driver.FindElement(By.Id("DefaultMeta")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"DefaultMetaCheckbox\"]")).Click();
                ScrollTo(By.Id("header-top"));
                driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
                Thread.Sleep(2000);
            }

            GoToAdmin("settingsseo");
            ScrollTo(By.Id("ProductsDefaultAdditionalDescription"));

            driver.FindElement(By.Id("StaticPageDefaultTitle")).Click();
            driver.FindElement(By.Id("StaticPageDefaultTitle")).Clear();
            driver.FindElement(By.Id("StaticPageDefaultTitle")).SendKeys("New title StaticPage");

            driver.FindElement(By.Id("StaticPageDefaultMetaKeywords")).Click();
            driver.FindElement(By.Id("StaticPageDefaultMetaKeywords")).Clear();
            driver.FindElement(By.Id("StaticPageDefaultMetaKeywords")).SendKeys("New meta keywords 1 StaticPage, New meta keywords 2 StaticPage, New meta keywords 3 StaticPage");

            driver.FindElement(By.Id("StaticPageDefaultMetaDescription")).Click();
            driver.FindElement(By.Id("StaticPageDefaultMetaDescription")).Clear();
            driver.FindElement(By.Id("StaticPageDefaultMetaDescription")).SendKeys("New description StaticPage");

            driver.FindElement(By.Id("StaticPageDefaultH1")).Click();
            driver.FindElement(By.Id("StaticPageDefaultH1")).Clear();
            driver.FindElement(By.Id("StaticPageDefaultH1")).SendKeys("New h1 StaticPage");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"settingsSeoSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingsseo");

            VerifyAreEqual("New title StaticPage", driver.FindElement(By.Id("StaticPageDefaultTitle")).GetAttribute("value"), "default seo static page title admin");
            VerifyAreEqual("New meta keywords 1 StaticPage, New meta keywords 2 StaticPage, New meta keywords 3 StaticPage", driver.FindElement(By.Id("StaticPageDefaultMetaKeywords")).GetAttribute("value"), "default seo static page keywords admin");
            VerifyAreEqual("New description StaticPage", driver.FindElement(By.Id("StaticPageDefaultMetaDescription")).GetAttribute("value"), "default seo static page description admin");
            VerifyAreEqual("New h1 StaticPage", driver.FindElement(By.Id("StaticPageDefaultH1")).GetAttribute("value"), "default seo static page h1 admin");

            //check client
            GoToClient("pages/about");
            VerifyAreEqual("New title StaticPage", driver.Title, "default seo static page title client");
            VerifyAreEqual("New meta keywords 1 StaticPage, New meta keywords 2 StaticPage, New meta keywords 3 StaticPage", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "default seo static page keywords client");
            VerifyAreEqual("New description StaticPage", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "default seo static page description client");
            VerifyAreEqual("New h1 StaticPage", driver.FindElement(By.TagName("h1")).Text, "default seo static page h1 client");

            VerifyFinally(testname);
        }

        [Test]
        public void DefaultSeoStaticPageVariables()
        {
            testname = "DefaultSeoStaticPageVariables";
            VerifyBegin(testname);

            //set default meta
            GoToAdmin("staticpages/edit/132");
            ScrollTo(By.Id("URL"));
            if (!driver.FindElement(By.Id("DefaultMeta")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"DefaultMetaCheckbox\"]")).Click();
                ScrollTo(By.Id("header-top"));
                driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
                Thread.Sleep(2000);
            }

            GoToAdmin("settingsseo");
            ScrollTo(By.Id("ProductsDefaultAdditionalDescription"));

            driver.FindElement(By.Id("StaticPageDefaultTitle")).Click();
            driver.FindElement(By.Id("StaticPageDefaultTitle")).Clear();
            driver.FindElement(By.Id("StaticPageDefaultTitle")).SendKeys("#STORE_NAME# - #PAGE_NAME#");

            driver.FindElement(By.Id("StaticPageDefaultMetaKeywords")).Click();
            driver.FindElement(By.Id("StaticPageDefaultMetaKeywords")).Clear();
            driver.FindElement(By.Id("StaticPageDefaultMetaKeywords")).SendKeys("#STORE_NAME# - #PAGE_NAME#");

            driver.FindElement(By.Id("StaticPageDefaultMetaDescription")).Click();
            driver.FindElement(By.Id("StaticPageDefaultMetaDescription")).Clear();
            driver.FindElement(By.Id("StaticPageDefaultMetaDescription")).SendKeys("#STORE_NAME# - #PAGE_NAME#");

            driver.FindElement(By.Id("StaticPageDefaultH1")).Click();
            driver.FindElement(By.Id("StaticPageDefaultH1")).Clear();
            driver.FindElement(By.Id("StaticPageDefaultH1")).SendKeys("#STORE_NAME# - #PAGE_NAME#");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"settingsSeoSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingsseo");

            VerifyAreEqual("#STORE_NAME# - #PAGE_NAME#", driver.FindElement(By.Id("StaticPageDefaultTitle")).GetAttribute("value"), "default seo static page title admin");
            VerifyAreEqual("#STORE_NAME# - #PAGE_NAME#", driver.FindElement(By.Id("StaticPageDefaultMetaKeywords")).GetAttribute("value"), "default seo static page keywords admin");
            VerifyAreEqual("#STORE_NAME# - #PAGE_NAME#", driver.FindElement(By.Id("StaticPageDefaultMetaDescription")).GetAttribute("value"), "default seo static page description admin");
            VerifyAreEqual("#STORE_NAME# - #PAGE_NAME#", driver.FindElement(By.Id("StaticPageDefaultH1")).GetAttribute("value"), "default seo static page h1 admin");

            //check client
            GoToClient("pages/contacts");
            VerifyAreEqual("Мой магазин - Контакты", driver.Title, "default seo static page title client");
            VerifyAreEqual("Мой магазин - Контакты", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "default seo static page keywords client");
            VerifyAreEqual("Мой магазин - Контакты", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "default seo static page description client");
            VerifyAreEqual("Мой магазин - Контакты", driver.FindElement(By.TagName("h1")).Text, "default seo static page h1 client");

            VerifyFinally(testname);
        }

        [Test]
        public void DefaultSeoStaticPageReset()
        {
            testname = "DefaultSeoStaticPageReset";
            VerifyBegin(testname);

            //admin set meta for static page
            GoToAdmin("staticpages/edit/131");
            ScrollTo(By.Id("URL"));
            if (driver.FindElement(By.Id("DefaultMeta")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"DefaultMetaCheckbox\"]")).Click();
            }
            
            WaitForElem(By.Id("SeoTitle"));
            driver.FindElement(By.Id("SeoTitle")).Clear();
            driver.FindElement(By.Id("SeoTitle")).SendKeys("StaticPage_Title");
            driver.FindElement(By.Id("SeoKeywords")).Clear();
            driver.FindElement(By.Id("SeoKeywords")).SendKeys("StaticPage_SeoKeywords");
            driver.FindElement(By.Id("SeoDescription")).Clear();
            driver.FindElement(By.Id("SeoDescription")).SendKeys("StaticPage_SeoDescription");
            driver.FindElement(By.Id("SeoH1")).Clear();
            driver.FindElement(By.Id("SeoH1")).SendKeys("StaticPage_H1");
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);

            //pre check client
            GoToClient("pages/shipping");
            VerifyAreEqual("StaticPage_Title", driver.Title, "pre check seo static page title client");
            VerifyAreEqual("StaticPage_H1", driver.FindElement(By.TagName("h1")).Text, "pre check seo static page h1 client");
            VerifyAreEqual("StaticPage_SeoKeywords", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "pre check seo static page keywords client");
            VerifyAreEqual("StaticPage_SeoDescription", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "pre check seo static page description client");

            //reset meta
            GoToAdmin("settingsseo");
            ScrollTo(By.Id("ProductsDefaultAdditionalDescription"));

            driver.FindElement(By.Id("StaticPageDefaultTitle")).Click();
            driver.FindElement(By.Id("StaticPageDefaultTitle")).Clear();
            driver.FindElement(By.Id("StaticPageDefaultTitle")).SendKeys("1");

            driver.FindElement(By.Id("StaticPageDefaultMetaKeywords")).Click();
            driver.FindElement(By.Id("StaticPageDefaultMetaKeywords")).Clear();
            driver.FindElement(By.Id("StaticPageDefaultMetaKeywords")).SendKeys("2");

            driver.FindElement(By.Id("StaticPageDefaultMetaDescription")).Click();
            driver.FindElement(By.Id("StaticPageDefaultMetaDescription")).Clear();
            driver.FindElement(By.Id("StaticPageDefaultMetaDescription")).SendKeys("3");

            driver.FindElement(By.Id("StaticPageDefaultH1")).Click();
            driver.FindElement(By.Id("StaticPageDefaultH1")).Clear();
            driver.FindElement(By.Id("StaticPageDefaultH1")).SendKeys("4");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"settingsSeoSave\"]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("settingsseo");
            ScrollTo(By.Id("StaticPageDefaultMetaDescription"));

            driver.FindElement(By.LinkText("Сбросить мета информацию для всех статических страниц")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(3000);

            //check client
            GoToClient("pages/shipping");
            VerifyAreEqual("1", driver.Title, "reset seo static page title client");
            VerifyAreEqual("2", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "reset seo static page keywords client");
            VerifyAreEqual("3", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "reset seo static page description client");
            VerifyAreEqual("4", driver.FindElement(By.TagName("h1")).Text, "reset seo static page h1 client");

            VerifyFinally(testname);
        }
    }
}