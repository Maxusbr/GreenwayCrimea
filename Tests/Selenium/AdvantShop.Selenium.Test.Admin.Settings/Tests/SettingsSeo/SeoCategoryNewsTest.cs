using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using System.Threading;

namespace AdvantShop.SeleniumTest.Admin.Settings.SettingsSEO
{
    [TestFixture]
    public class SettingsSeoCategoryNewsTest : BaseSeleniumTest
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
        public void DefaultSeoCategoryNews()
        {
            testname = "DefaultSeoCategoryNews";
            VerifyBegin(testname);
            
            GoToAdmin("settingsseo");
            ScrollTo(By.Id("StaticPageDefaultMetaDescription"));

            driver.FindElement(By.Id("CategoryNewsDefaultTitle")).Click();
            driver.FindElement(By.Id("CategoryNewsDefaultTitle")).Clear();
            driver.FindElement(By.Id("CategoryNewsDefaultTitle")).SendKeys("New title CategoryNews");

            driver.FindElement(By.Id("CategoryNewsDefaultMetaKeywords")).Click();
            driver.FindElement(By.Id("CategoryNewsDefaultMetaKeywords")).Clear();
            driver.FindElement(By.Id("CategoryNewsDefaultMetaKeywords")).SendKeys("New meta keywords 1 CategoryNews, New meta keywords 2 CategoryNews, New meta keywords 3 CategoryNews");

            driver.FindElement(By.Id("CategoryNewsDefaultMetaDescription")).Click();
            driver.FindElement(By.Id("CategoryNewsDefaultMetaDescription")).Clear();
            driver.FindElement(By.Id("CategoryNewsDefaultMetaDescription")).SendKeys("New description CategoryNews");

            driver.FindElement(By.Id("CategoryNewsDefaultH1")).Click();
            driver.FindElement(By.Id("CategoryNewsDefaultH1")).Clear();
            driver.FindElement(By.Id("CategoryNewsDefaultH1")).SendKeys("New h1 CategoryNews");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"settingsSeoSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingsseo");

            VerifyAreEqual("New title CategoryNews", driver.FindElement(By.Id("CategoryNewsDefaultTitle")).GetAttribute("value"), "default seo category news title admin");
            VerifyAreEqual("New meta keywords 1 CategoryNews, New meta keywords 2 CategoryNews, New meta keywords 3 CategoryNews", driver.FindElement(By.Id("CategoryNewsDefaultMetaKeywords")).GetAttribute("value"), "default seo category news keywords admin");
            VerifyAreEqual("New description CategoryNews", driver.FindElement(By.Id("CategoryNewsDefaultMetaDescription")).GetAttribute("value"), "default seo category news description admin");
            VerifyAreEqual("New h1 CategoryNews", driver.FindElement(By.Id("CategoryNewsDefaultH1")).GetAttribute("value"), "default seo category news h1 admin");

            //check client
            GoToClient("newscategory/common");
            VerifyAreEqual("New title CategoryNews", driver.Title, "default seo category news title client");
            VerifyAreEqual("New meta keywords 1 CategoryNews, New meta keywords 2 CategoryNews, New meta keywords 3 CategoryNews", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "default seo category news keywords client");
            VerifyAreEqual("New description CategoryNews", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "default seo category news description client");
            VerifyAreEqual("New h1 CategoryNews", driver.FindElement(By.TagName("h1")).Text, "default seo category news h1 client");

            VerifyFinally(testname);
        }

        [Test]
        public void DefaultSeoCategoryNewsVariables()
        {
            testname = "DefaultSeoCategoryNewsVariables";
            VerifyBegin(testname);
            
            GoToAdmin("settingsseo");
            ScrollTo(By.Id("StaticPageDefaultMetaDescription"));

            driver.FindElement(By.Id("CategoryNewsDefaultTitle")).Click();
            driver.FindElement(By.Id("CategoryNewsDefaultTitle")).Clear();
            driver.FindElement(By.Id("CategoryNewsDefaultTitle")).SendKeys("#STORE_NAME# - #NEWSCATEGORY_NAME#");

            driver.FindElement(By.Id("CategoryNewsDefaultMetaKeywords")).Click();
            driver.FindElement(By.Id("CategoryNewsDefaultMetaKeywords")).Clear();
            driver.FindElement(By.Id("CategoryNewsDefaultMetaKeywords")).SendKeys("#STORE_NAME# - #NEWSCATEGORY_NAME#");

            driver.FindElement(By.Id("CategoryNewsDefaultMetaDescription")).Click();
            driver.FindElement(By.Id("CategoryNewsDefaultMetaDescription")).Clear();
            driver.FindElement(By.Id("CategoryNewsDefaultMetaDescription")).SendKeys("#STORE_NAME# - #NEWSCATEGORY_NAME#");

            driver.FindElement(By.Id("CategoryNewsDefaultH1")).Click();
            driver.FindElement(By.Id("CategoryNewsDefaultH1")).Clear();
            driver.FindElement(By.Id("CategoryNewsDefaultH1")).SendKeys("#STORE_NAME# - #NEWSCATEGORY_NAME#");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"settingsSeoSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingsseo");

            VerifyAreEqual("#STORE_NAME# - #NEWSCATEGORY_NAME#", driver.FindElement(By.Id("CategoryNewsDefaultTitle")).GetAttribute("value"), "default seo category news title admin");
            VerifyAreEqual("#STORE_NAME# - #NEWSCATEGORY_NAME#", driver.FindElement(By.Id("CategoryNewsDefaultMetaKeywords")).GetAttribute("value"), "default seo category news keywords admin");
            VerifyAreEqual("#STORE_NAME# - #NEWSCATEGORY_NAME#", driver.FindElement(By.Id("CategoryNewsDefaultMetaDescription")).GetAttribute("value"), "default seo category news description admin");
            VerifyAreEqual("#STORE_NAME# - #NEWSCATEGORY_NAME#", driver.FindElement(By.Id("CategoryNewsDefaultH1")).GetAttribute("value"), "default seo category news h1 admin");

            //check client
            GoToClient("newscategory/computer");
            VerifyAreEqual("Мой магазин - Компьютеры", driver.Title, "default seo category news title client");
            VerifyAreEqual("Мой магазин - Компьютеры", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "default seo category news keywords client");
            VerifyAreEqual("Мой магазин - Компьютеры", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "default seo category news description client");
            VerifyAreEqual("Мой магазин - Компьютеры", driver.FindElement(By.TagName("h1")).Text, "default seo category news h1 client");

            VerifyFinally(testname);
        }

        [Test]
        public void DefaultSeoCategoryNewsReset()
        {
            testname = "DefaultSeoCategoryNewsReset";
            VerifyBegin(testname);

            //admin set meta for category news
            GoToAdmin("newscategory");
            GetGridCell(2, "_serviceColumn").FindElement(By.CssSelector(".ui-grid-custom-service-icon.fa.fa-pencil.news-category-pointer")).Click();
            Thread.Sleep(2000);
            WaitForElem(By.CssSelector(".modal-content"));

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
            Thread.Sleep(2000);

            //pre check client
            GoToClient("newscategory/fashion");
            VerifyAreEqual("NewCategorySEO Title", driver.Title, "pre check seo category news title client");
            VerifyAreEqual("NewCategorySEO H1", driver.FindElement(By.TagName("h1")).Text, "pre check seo category news h1 client");
            VerifyAreEqual("NewCategorySEO Meta Keywords", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "pre check seo category news keywords client");
            VerifyAreEqual("NewCategorySEO Meta Description", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "pre check seo category news description client");

            //reset meta
            GoToAdmin("settingsseo");
            ScrollTo(By.Id("StaticPageDefaultMetaDescription"));

            driver.FindElement(By.Id("CategoryNewsDefaultTitle")).Click();
            driver.FindElement(By.Id("CategoryNewsDefaultTitle")).Clear();
            driver.FindElement(By.Id("CategoryNewsDefaultTitle")).SendKeys("1");

            driver.FindElement(By.Id("CategoryNewsDefaultMetaKeywords")).Click();
            driver.FindElement(By.Id("CategoryNewsDefaultMetaKeywords")).Clear();
            driver.FindElement(By.Id("CategoryNewsDefaultMetaKeywords")).SendKeys("2");

            driver.FindElement(By.Id("CategoryNewsDefaultMetaDescription")).Click();
            driver.FindElement(By.Id("CategoryNewsDefaultMetaDescription")).Clear();
            driver.FindElement(By.Id("CategoryNewsDefaultMetaDescription")).SendKeys("3");

            driver.FindElement(By.Id("CategoryNewsDefaultH1")).Click();
            driver.FindElement(By.Id("CategoryNewsDefaultH1")).Clear();
            driver.FindElement(By.Id("CategoryNewsDefaultH1")).SendKeys("4");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"settingsSeoSave\"]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("settingsseo");
            ScrollTo(By.Id("CategoryNewsDefaultTitle"));

            driver.FindElement(By.LinkText("Сбросить мета информацию для всех категорий новостей")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(3000);

            //check client
            GoToClient("newscategory/fashion");
            VerifyAreEqual("1", driver.Title, "reset seo category news title client");
            VerifyAreEqual("2", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "reset seo category news keywords client");
            VerifyAreEqual("3", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "reset seo category news description client");
            VerifyAreEqual("4", driver.FindElement(By.TagName("h1")).Text, "reset seo category news h1 client");

            VerifyFinally(testname);
        }
    }
}