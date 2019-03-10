using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using System.Threading;
using AdvantShop.Selenium.Core.Infrastructure;

namespace AdvantShop.SeleniumTest.Admin.Settings.SettingsSEO
{
    [TestFixture]
    public class SettingsSeoCategoryTest : BaseSeleniumTest
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
        public void DefaultSeoCategory()
        {
            testname = "DefaultSeoCategory";
            VerifyBegin(testname);

            GoToAdmin("settingsseo");
            ScrollTo(By.Id("DefaultMetaDescription"));

            driver.FindElement(By.Id("CategoriesDefaultTitle")).Click();
            driver.FindElement(By.Id("CategoriesDefaultTitle")).Clear();
            driver.FindElement(By.Id("CategoriesDefaultTitle")).SendKeys("New title Category");

            driver.FindElement(By.Id("CategoriesDefaultMetaKeywords")).Click();
            driver.FindElement(By.Id("CategoriesDefaultMetaKeywords")).Clear();
            driver.FindElement(By.Id("CategoriesDefaultMetaKeywords")).SendKeys("New meta keywords 1 Category, New meta keywords 2 Category, New meta keywords 3 Category");

            driver.FindElement(By.Id("CategoriesDefaultMetaDescription")).Click();
            driver.FindElement(By.Id("CategoriesDefaultMetaDescription")).Clear();
            driver.FindElement(By.Id("CategoriesDefaultMetaDescription")).SendKeys("New description Category");

            driver.FindElement(By.Id("CategoriesDefaultH1")).Click();
            driver.FindElement(By.Id("CategoriesDefaultH1")).Clear();
            driver.FindElement(By.Id("CategoriesDefaultH1")).SendKeys("New h1 Category");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"settingsSeoSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingsseo");

            VerifyAreEqual("New title Category", driver.FindElement(By.Id("CategoriesDefaultTitle")).GetAttribute("value"), "default seo category title admin");
            VerifyAreEqual("New meta keywords 1 Category, New meta keywords 2 Category, New meta keywords 3 Category", driver.FindElement(By.Id("CategoriesDefaultMetaKeywords")).GetAttribute("value"), "default seo category keywords admin");
            VerifyAreEqual("New description Category", driver.FindElement(By.Id("CategoriesDefaultMetaDescription")).GetAttribute("value"), "default seo category description admin");
            VerifyAreEqual("New h1 Category", driver.FindElement(By.Id("CategoriesDefaultH1")).GetAttribute("value"), "default seo category h1 admin");

            //check client
            GoToClient("categories/drinks");
            VerifyAreEqual("New title Category", driver.Title, "default seo category title client");
            VerifyAreEqual("New meta keywords 1 Category, New meta keywords 2 Category, New meta keywords 3 Category", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "default seo category keywords client");
            VerifyAreEqual("New description Category", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "default seo category description client");
            VerifyAreEqual("New h1 Category", driver.FindElement(By.TagName("h1")).Text, "default seo category h1 client");

            VerifyFinally(testname);
        }

        [Test]
        public void DefaultSeoCategoryVariables()
        {
            testname = "DefaultSeoCategoryVariables";
            VerifyBegin(testname);

            //set default meta 
            GoToAdmin("category/edit/5444");
            ScrollTo(By.CssSelector("[data-e2e=\"imgAdd\"]"));
            if (!driver.FindElement(By.CssSelector("[data-e2e=\"categoryDefaultMeta\"]")).FindElement(By.Id("DefaultMeta")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"categoryDefaultMeta\"]")).FindElement(By.TagName("span")).Click();
                ScrollTo(By.Id("header-top"));
                GetButton(eButtonType.Save).Click();
                Thread.Sleep(2000);
            }
        
            GoToAdmin("settingsseo");
            ScrollTo(By.Id("DefaultMetaDescription"));

            driver.FindElement(By.Id("CategoriesDefaultTitle")).Click();
            driver.FindElement(By.Id("CategoriesDefaultTitle")).Clear();
            driver.FindElement(By.Id("CategoriesDefaultTitle")).SendKeys("#STORE_NAME# - #CATEGORY_NAME##PAGE# - #TAGS#");

            driver.FindElement(By.Id("CategoriesDefaultMetaKeywords")).Click();
            driver.FindElement(By.Id("CategoriesDefaultMetaKeywords")).Clear();
            driver.FindElement(By.Id("CategoriesDefaultMetaKeywords")).SendKeys("#STORE_NAME# - #CATEGORY_NAME##PAGE# - #TAGS#");

            driver.FindElement(By.Id("CategoriesDefaultMetaDescription")).Click();
            driver.FindElement(By.Id("CategoriesDefaultMetaDescription")).Clear();
            driver.FindElement(By.Id("CategoriesDefaultMetaDescription")).SendKeys("#STORE_NAME# - #CATEGORY_NAME##PAGE# - #TAGS#");

            driver.FindElement(By.Id("CategoriesDefaultH1")).Click();
            driver.FindElement(By.Id("CategoriesDefaultH1")).Clear();
            driver.FindElement(By.Id("CategoriesDefaultH1")).SendKeys("#STORE_NAME# - #CATEGORY_NAME##PAGE# - #TAGS#");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"settingsSeoSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingsseo");

            VerifyAreEqual("#STORE_NAME# - #CATEGORY_NAME##PAGE# - #TAGS#", driver.FindElement(By.Id("CategoriesDefaultTitle")).GetAttribute("value"), "default seo category title admin");
            VerifyAreEqual("#STORE_NAME# - #CATEGORY_NAME##PAGE# - #TAGS#", driver.FindElement(By.Id("CategoriesDefaultMetaKeywords")).GetAttribute("value"), "default seo category keywords admin");
            VerifyAreEqual("#STORE_NAME# - #CATEGORY_NAME##PAGE# - #TAGS#", driver.FindElement(By.Id("CategoriesDefaultMetaDescription")).GetAttribute("value"), "default seo category description admin");
            VerifyAreEqual("#STORE_NAME# - #CATEGORY_NAME##PAGE# - #TAGS#", driver.FindElement(By.Id("CategoriesDefaultH1")).GetAttribute("value"), "default seo category h1 admin");

            //check client
            GoToClient("categories/samsung-tv");
            VerifyAreEqual("Мой магазин - Samsung - LED 3D  ЖК HDTV", driver.Title, "default seo category title client page 1");
            VerifyAreEqual("Мой магазин - Samsung -  LED 3D  ЖК HDTV", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "default seo category keywords client page 1");
            VerifyAreEqual("Мой магазин - Samsung -  LED 3D  ЖК HDTV", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "default seo category description client page 1");
            VerifyAreEqual("Мой магазин - Samsung - LED 3D  ЖК HDTV", driver.FindElement(By.TagName("h1")).Text, "default seo category h1 client page 1");

            GoToClient("categories/samsung-tv?page=2");
            VerifyAreEqual("Мой магазин - Samsung, страница № 2 - LED 3D  ЖК HDTV", driver.Title, "default seo category title client page 2");
            VerifyAreEqual("Мой магазин - Samsung, страница № 2 -  LED 3D  ЖК HDTV", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "default seo category keywords client page 2");
            VerifyAreEqual("Мой магазин - Samsung, страница № 2 -  LED 3D  ЖК HDTV", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "default seo category description client page 2");
            VerifyAreEqual("Мой магазин - Samsung, страница № 2 - LED 3D  ЖК HDTV", driver.FindElement(By.TagName("h1")).Text, "default seo category h1 client page 2");

            VerifyFinally(testname);
        }

        [Test]
        public void DefaultSeoCategoryReset()
        {
            testname = "DefaultSeoCategoryReset";
            VerifyBegin(testname);

            //admin set meta for category
            GoToAdmin("category/edit/434");
            ScrollTo(By.Id("UrlPath"));
            if (driver.FindElement(By.CssSelector("[data-e2e=\"categoryDefaultMeta\"]")).FindElement(By.Id("DefaultMeta")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"categoryDefaultMeta\"]")).FindElement(By.TagName("span")).Click();
            }
            WaitForElem(By.Id("SeoTitle"));
            driver.FindElement(By.Id("SeoTitle")).Clear();
            driver.FindElement(By.Id("SeoTitle")).SendKeys("Category_Title");
            driver.FindElement(By.Id("SeoKeywords")).Clear();
            driver.FindElement(By.Id("SeoKeywords")).SendKeys("Category_SeoKeywords");
            driver.FindElement(By.Id("SeoDescription")).Clear();
            driver.FindElement(By.Id("SeoDescription")).SendKeys("Category_SeoDescription");
            driver.FindElement(By.Id("SeoH1")).Clear();
            driver.FindElement(By.Id("SeoH1")).SendKeys("Category_H1");
            ScrollTo(By.Id("header-top"));
            GetButton(eButtonType.Save).Click();
            Thread.Sleep(2000);

            //pre check client
            GoToClient("categories/sea");
            VerifyAreEqual("Category_Title", driver.Title, "pre check seo category title client");
            VerifyAreEqual("Category_H1", driver.FindElement(By.TagName("h1")).Text, "pre check seo category h1 client");
            VerifyAreEqual("Category_SeoKeywords", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "pre check seo category keywords client");
            VerifyAreEqual("Category_SeoDescription", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "pre check seo category description client");

            //reset meta
            GoToAdmin("settingsseo");
            ScrollTo(By.Id("DefaultMetaDescription"));

            driver.FindElement(By.Id("CategoriesDefaultTitle")).Click();
            driver.FindElement(By.Id("CategoriesDefaultTitle")).Clear();
            driver.FindElement(By.Id("CategoriesDefaultTitle")).SendKeys("1");

            driver.FindElement(By.Id("CategoriesDefaultMetaKeywords")).Click();
            driver.FindElement(By.Id("CategoriesDefaultMetaKeywords")).Clear();
            driver.FindElement(By.Id("CategoriesDefaultMetaKeywords")).SendKeys("2");

            driver.FindElement(By.Id("CategoriesDefaultMetaDescription")).Click();
            driver.FindElement(By.Id("CategoriesDefaultMetaDescription")).Clear();
            driver.FindElement(By.Id("CategoriesDefaultMetaDescription")).SendKeys("3");

            driver.FindElement(By.Id("CategoriesDefaultH1")).Click();
            driver.FindElement(By.Id("CategoriesDefaultH1")).Clear();
            driver.FindElement(By.Id("CategoriesDefaultH1")).SendKeys("4");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"settingsSeoSave\"]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("settingsseo");
            ScrollTo(By.Id("CategoriesDefaultTitle"));

            driver.FindElement(By.LinkText("Сбросить мета информацию для всех категорий")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(3000);

            //check client
            GoToClient("categories/sea");
            VerifyAreEqual("1", driver.Title, "reset seo category title client");
            VerifyAreEqual("2", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "reset seo category keywords client");
            VerifyAreEqual("3", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "reset seo category description client");
            VerifyAreEqual("4", driver.FindElement(By.TagName("h1")).Text, "reset seo category h1 client");

            VerifyFinally(testname);
        }
    }
}