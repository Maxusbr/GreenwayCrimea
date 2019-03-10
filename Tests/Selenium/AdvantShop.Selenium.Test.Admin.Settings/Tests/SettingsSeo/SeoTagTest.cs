using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using System.Threading;

namespace AdvantShop.SeleniumTest.Admin.Settings.SettingsSEO
{
    [TestFixture]
    public class SettingsSeoTagTest : BaseSeleniumTest
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
        public void DefaultSeoTag()
        {
            testname = "DefaultSeoTag";
            VerifyBegin(testname);

            GoToAdmin("tags/edit/3");
            ScrollTo(By.Id("URL"));
            if (!driver.FindElement(By.Id("DefaultMeta")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"tagDefaultMeta\"]")).Click();
                ScrollTo(By.Id("header-top"));
                driver.FindElement(By.CssSelector("[data-e2e=\"SaveTag\"]")).Click();
                Thread.Sleep(2000);
            }

            GoToAdmin("settingsseo");
            ScrollTo(By.Id("CategoriesDefaultH1"));

            driver.FindElement(By.Id("TagsDefaultTitle")).Click();
            driver.FindElement(By.Id("TagsDefaultTitle")).Clear();
            driver.FindElement(By.Id("TagsDefaultTitle")).SendKeys("New title Tag");

            driver.FindElement(By.Id("TagsDefaultMetaKeywords")).Click();
            driver.FindElement(By.Id("TagsDefaultMetaKeywords")).Clear();
            driver.FindElement(By.Id("TagsDefaultMetaKeywords")).SendKeys("New meta keywords 1 Tag, New meta keywords 2 Tag, New meta keywords 3 Tag");

            driver.FindElement(By.Id("TagsDefaultMetaDescription")).Click();
            driver.FindElement(By.Id("TagsDefaultMetaDescription")).Clear();
            driver.FindElement(By.Id("TagsDefaultMetaDescription")).SendKeys("New description Tag");

            driver.FindElement(By.Id("TagsDefaultH1")).Click();
            driver.FindElement(By.Id("TagsDefaultH1")).Clear();
            driver.FindElement(By.Id("TagsDefaultH1")).SendKeys("New h1 Tag");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"settingsSeoSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingsseo");

            VerifyAreEqual("New title Tag", driver.FindElement(By.Id("TagsDefaultTitle")).GetAttribute("value"), "default seo product title admin");
            VerifyAreEqual("New meta keywords 1 Tag, New meta keywords 2 Tag, New meta keywords 3 Tag", driver.FindElement(By.Id("TagsDefaultMetaKeywords")).GetAttribute("value"), "default seo tag keywords admin");
            VerifyAreEqual("New description Tag", driver.FindElement(By.Id("TagsDefaultMetaDescription")).GetAttribute("value"), "default seo tag description admin");
            VerifyAreEqual("New h1 Tag", driver.FindElement(By.Id("TagsDefaultH1")).GetAttribute("value"), "default seo tag h1 admin");

            //check client
            GoToClient("categories/samsung-tv/tag/zhk");
            VerifyAreEqual("New title Tag", driver.Title, "default seo tag title client");
            VerifyAreEqual("New meta keywords 1 Tag, New meta keywords 2 Tag, New meta keywords 3 Tag", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "default seo tag keywords client");
            VerifyAreEqual("New description Tag", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "default seo tag description client");
            VerifyAreEqual("New h1 Tag", driver.FindElement(By.TagName("h1")).Text, "default seo tag h1 client");

            VerifyFinally(testname);
        }

        [Test]
        public void DefaultSeoTagVariables()
        {
            testname = "DefaultSeoTagVariables";
            VerifyBegin(testname);

            //set default meta
            GoToAdmin("tags/edit/1");
            ScrollTo(By.Id("URL"));
            if (!driver.FindElement(By.Id("DefaultMeta")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"tagDefaultMeta\"]")).Click();
                ScrollTo(By.Id("header-top"));
                driver.FindElement(By.CssSelector("[data-e2e=\"SaveTag\"]")).Click();
                Thread.Sleep(2000);
            }

            GoToAdmin("settingsseo");
            ScrollTo(By.Id("CategoriesDefaultH1"));

            driver.FindElement(By.Id("TagsDefaultTitle")).Click();
            driver.FindElement(By.Id("TagsDefaultTitle")).Clear();
            driver.FindElement(By.Id("TagsDefaultTitle")).SendKeys("#STORE_NAME# - #TAG_NAME#");

            driver.FindElement(By.Id("TagsDefaultMetaKeywords")).Click();
            driver.FindElement(By.Id("TagsDefaultMetaKeywords")).Clear();
            driver.FindElement(By.Id("TagsDefaultMetaKeywords")).SendKeys("#STORE_NAME# - #TAG_NAME#");

            driver.FindElement(By.Id("TagsDefaultMetaDescription")).Click();
            driver.FindElement(By.Id("TagsDefaultMetaDescription")).Clear();
            driver.FindElement(By.Id("TagsDefaultMetaDescription")).SendKeys("#STORE_NAME# - #TAG_NAME#");

            driver.FindElement(By.Id("TagsDefaultH1")).Click();
            driver.FindElement(By.Id("TagsDefaultH1")).Clear();
            driver.FindElement(By.Id("TagsDefaultH1")).SendKeys("#STORE_NAME# - #TAG_NAME#");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"settingsSeoSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingsseo");

            VerifyAreEqual("#STORE_NAME# - #TAG_NAME#", driver.FindElement(By.Id("TagsDefaultTitle")).GetAttribute("value"), "default seo tag title admin");
            VerifyAreEqual("#STORE_NAME# - #TAG_NAME#", driver.FindElement(By.Id("TagsDefaultMetaKeywords")).GetAttribute("value"), "default seo tag keywords admin");
            VerifyAreEqual("#STORE_NAME# - #TAG_NAME#", driver.FindElement(By.Id("TagsDefaultMetaDescription")).GetAttribute("value"), "default seo tag description admin");
            VerifyAreEqual("#STORE_NAME# - #TAG_NAME#", driver.FindElement(By.Id("TagsDefaultH1")).GetAttribute("value"), "default seo tag h1 admin");

            //check client
            GoToClient("categories/samsung-tv/tag/led");
            VerifyAreEqual("Мой магазин - LED", driver.Title, "default seo tag title client");
            VerifyAreEqual("Мой магазин -  LED", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "default seo tag keywords client");
            VerifyAreEqual("Мой магазин -  LED", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "default seo tag description client");
            VerifyAreEqual("Мой магазин - LED", driver.FindElement(By.TagName("h1")).Text, "default seo tag h1 client");

            VerifyFinally(testname);
        }

        [Test]
        public void DefaultSeoTagReset()
        {
            testname = "DefaultSeoTagReset";
            VerifyBegin(testname);

            //admin set meta for tag
            GoToAdmin("tags/edit/7");
            ScrollTo(By.Id("URL"));
            if (driver.FindElement(By.Id("DefaultMeta")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"tagDefaultMeta\"]")).Click();
            }

            WaitForElem(By.Id("SeoTitle"));
            driver.FindElement(By.Id("SeoTitle")).Clear();
            driver.FindElement(By.Id("SeoTitle")).SendKeys("Tag_Title");
            driver.FindElement(By.Id("SeoKeywords")).Clear();
            driver.FindElement(By.Id("SeoKeywords")).SendKeys("Tag_SeoKeywords");
            driver.FindElement(By.Id("SeoDescription")).Clear();
            driver.FindElement(By.Id("SeoDescription")).SendKeys("Tag_SeoDescription");
            driver.FindElement(By.Id("SeoH1")).Clear();
            driver.FindElement(By.Id("SeoH1")).SendKeys("Tag_H1");
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"SaveTag\"]")).Click();
            Thread.Sleep(2000);

            //pre check client
            GoToClient("categories/samsung-tv/tag/hdtv");
            VerifyAreEqual("Tag_Title", driver.Title, "pre check seo tag title client");
            VerifyAreEqual("Tag_H1", driver.FindElement(By.TagName("h1")).Text, "pre check seo tag h1 client");
            VerifyAreEqual("Tag_SeoKeywords", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "pre check seo tag keywords client");
            VerifyAreEqual("Tag_SeoDescription", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "pre check seo tag description client");

            //reset meta
            GoToAdmin("settingsseo");
            ScrollTo(By.Id("CategoriesDefaultMetaDescription"));

            driver.FindElement(By.Id("TagsDefaultTitle")).Click();
            driver.FindElement(By.Id("TagsDefaultTitle")).Clear();
            driver.FindElement(By.Id("TagsDefaultTitle")).SendKeys("1");

            driver.FindElement(By.Id("TagsDefaultMetaKeywords")).Click();
            driver.FindElement(By.Id("TagsDefaultMetaKeywords")).Clear();
            driver.FindElement(By.Id("TagsDefaultMetaKeywords")).SendKeys("2");

            driver.FindElement(By.Id("TagsDefaultMetaDescription")).Click();
            driver.FindElement(By.Id("TagsDefaultMetaDescription")).Clear();
            driver.FindElement(By.Id("TagsDefaultMetaDescription")).SendKeys("3");

            driver.FindElement(By.Id("TagsDefaultH1")).Click();
            driver.FindElement(By.Id("TagsDefaultH1")).Clear();
            driver.FindElement(By.Id("TagsDefaultH1")).SendKeys("4");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"settingsSeoSave\"]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("settingsseo");
            ScrollTo(By.Id("TagsDefaultMetaDescription"));

            driver.FindElement(By.LinkText("Сбросить мета информацию для всех тегов")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(3000);

            //check client
            GoToClient("categories/samsung-tv/tag/hdtv");
            VerifyAreEqual("1", driver.Title, "reset seo tag title client");
            VerifyAreEqual("2", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "reset seo tag keywords client");
            VerifyAreEqual("3", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "reset seo tag description client");
            VerifyAreEqual("4", driver.FindElement(By.TagName("h1")).Text, "reset seo tag h1 client");

            VerifyFinally(testname);
        }
    }
}