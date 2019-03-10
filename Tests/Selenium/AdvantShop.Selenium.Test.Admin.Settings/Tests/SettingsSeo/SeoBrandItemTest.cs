using System;
using NUnit.Framework;
using OpenQA.Selenium;
using AdvantShop.SeleniumTest.Core;
using System.Threading;

namespace AdvantShop.SeleniumTest.Admin.Settings.SettingsSEO
{
    [TestFixture]
    public class SettingsSeoBrandItemTest : BaseSeleniumTest
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
        public void DefaultSeoBrandsItemItem()
        {
            testname = "DefaultSeoBrandsItem";
            VerifyBegin(testname);

            GoToAdmin("brands/edit/24");
            ScrollTo(By.Id("URL"));
            if (!driver.FindElement(By.Id("DefaultMeta")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"DefaultMetaCheckbox\"]")).Click();
                ScrollTo(By.Id("header-top"));
                driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
                Thread.Sleep(2000);
            }

            GoToAdmin("settingsseo");
            ScrollTo(By.Id("BrandsDefaultMetaDescription"));

            driver.FindElement(By.Id("BrandItemDefaultTitle")).Click();
            driver.FindElement(By.Id("BrandItemDefaultTitle")).Clear();
            driver.FindElement(By.Id("BrandItemDefaultTitle")).SendKeys("New title BrandsItem");

            driver.FindElement(By.Id("BrandItemDefaultMetaKeywords")).Click();
            driver.FindElement(By.Id("BrandItemDefaultMetaKeywords")).Clear();
            driver.FindElement(By.Id("BrandItemDefaultMetaKeywords")).SendKeys("New meta keywords 1 BrandsItem, New meta keywords 2 BrandsItem, New meta keywords 3 BrandsItem");

            driver.FindElement(By.Id("BrandItemDefaultMetaDescription")).Click();
            driver.FindElement(By.Id("BrandItemDefaultMetaDescription")).Clear();
            driver.FindElement(By.Id("BrandItemDefaultMetaDescription")).SendKeys("New description BrandsItem");

            driver.FindElement(By.Id("BrandItemDefaultH1")).Click();
            driver.FindElement(By.Id("BrandItemDefaultH1")).Clear();
            driver.FindElement(By.Id("BrandItemDefaultH1")).SendKeys("New h1 BrandsItem");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"settingsSeoSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingsseo");

            VerifyAreEqual("New title BrandsItem", driver.FindElement(By.Id("BrandItemDefaultTitle")).GetAttribute("value"), "default seo BrandsItem title admin");
            VerifyAreEqual("New meta keywords 1 BrandsItem, New meta keywords 2 BrandsItem, New meta keywords 3 BrandsItem", driver.FindElement(By.Id("BrandItemDefaultMetaKeywords")).GetAttribute("value"), "default seo BrandsItem keywords admin");
            VerifyAreEqual("New description BrandsItem", driver.FindElement(By.Id("BrandItemDefaultMetaDescription")).GetAttribute("value"), "default seo BrandsItem description admin");
            VerifyAreEqual("New h1 BrandsItem", driver.FindElement(By.Id("BrandItemDefaultH1")).GetAttribute("value"), "default seo BrandsItem h1 admin");

            //check client
            GoToClient("manufacturers/armani");
            VerifyAreEqual("New title BrandsItem", driver.Title, "default seo BrandsItem title client");
            VerifyAreEqual("New meta keywords 1 BrandsItem, New meta keywords 2 BrandsItem, New meta keywords 3 BrandsItem", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "default seo BrandsItem keywords client");
            VerifyAreEqual("New description BrandsItem", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "default seo BrandsItem description client");
            VerifyAreEqual("New h1 BrandsItem", driver.FindElement(By.TagName("h1")).Text, "default seo BrandsItem h1 client");

            VerifyFinally(testname);
        }

        [Test]
        public void DefaultSeoBrandsItemVariables()
        {
            testname = "DefaultSeoBrandsItemVariables";
            VerifyBegin(testname);

            //set default meta
            GoToAdmin("brands/edit/25");
            ScrollTo(By.Id("URL"));
            if (!driver.FindElement(By.Id("DefaultMeta")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"DefaultMetaCheckbox\"]")).Click();
                ScrollTo(By.Id("header-top"));
                driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
                Thread.Sleep(2000);
            }

            GoToAdmin("settingsseo");
            ScrollTo(By.Id("BrandsDefaultMetaDescription"));

            driver.FindElement(By.Id("BrandItemDefaultTitle")).Click();
            driver.FindElement(By.Id("BrandItemDefaultTitle")).Clear();
            driver.FindElement(By.Id("BrandItemDefaultTitle")).SendKeys("#STORE_NAME# - #BRAND_NAME#");

            driver.FindElement(By.Id("BrandItemDefaultMetaKeywords")).Click();
            driver.FindElement(By.Id("BrandItemDefaultMetaKeywords")).Clear();
            driver.FindElement(By.Id("BrandItemDefaultMetaKeywords")).SendKeys("#STORE_NAME# - #BRAND_NAME#");

            driver.FindElement(By.Id("BrandItemDefaultMetaDescription")).Click();
            driver.FindElement(By.Id("BrandItemDefaultMetaDescription")).Clear();
            driver.FindElement(By.Id("BrandItemDefaultMetaDescription")).SendKeys("#STORE_NAME# - #BRAND_NAME#");

            driver.FindElement(By.Id("BrandItemDefaultH1")).Click();
            driver.FindElement(By.Id("BrandItemDefaultH1")).Clear();
            driver.FindElement(By.Id("BrandItemDefaultH1")).SendKeys("#STORE_NAME# - #BRAND_NAME#");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"settingsSeoSave\"]")).Click();
            Thread.Sleep(2000);

            //check admin
            GoToAdmin("settingsseo");

            VerifyAreEqual("#STORE_NAME# - #BRAND_NAME#", driver.FindElement(By.Id("BrandItemDefaultTitle")).GetAttribute("value"), "default seo BrandsItem title admin");
            VerifyAreEqual("#STORE_NAME# - #BRAND_NAME#", driver.FindElement(By.Id("BrandItemDefaultMetaKeywords")).GetAttribute("value"), "default seo BrandsItem keywords admin");
            VerifyAreEqual("#STORE_NAME# - #BRAND_NAME#", driver.FindElement(By.Id("BrandItemDefaultMetaDescription")).GetAttribute("value"), "default seo BrandsItem description admin");
            VerifyAreEqual("#STORE_NAME# - #BRAND_NAME#", driver.FindElement(By.Id("BrandItemDefaultH1")).GetAttribute("value"), "default seo BrandsItem h1 admin");

            //check client
            GoToClient("manufacturers/calvin_klein");
            VerifyAreEqual("Мой магазин - Calvin Klein", driver.Title, "default seo BrandsItem title client");
            VerifyAreEqual("Мой магазин - Calvin Klein", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "default seo BrandsItem keywords client");
            VerifyAreEqual("Мой магазин - Calvin Klein", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "default seo BrandsItem description client");
            VerifyAreEqual("Мой магазин - Calvin Klein", driver.FindElement(By.TagName("h1")).Text, "default seo BrandsItem h1 client");

            VerifyFinally(testname);
        }

        [Test]
        public void DefaultSeoBrandsItemReset()
        {
            testname = "DefaultSeoBrandsItemReset";
            VerifyBegin(testname);

            //admin set meta for BrandsItem
            GoToAdmin("brands/edit/26");
            ScrollTo(By.Id("URL"));
            if (driver.FindElement(By.Id("DefaultMeta")).Selected)
            {
                driver.FindElement(By.CssSelector("[data-e2e=\"DefaultMetaCheckbox\"]")).Click();
            }

            WaitForElem(By.Id("SeoTitle"));
            driver.FindElement(By.Id("SeoTitle")).Clear();
            driver.FindElement(By.Id("SeoTitle")).SendKeys("BrandsItem_Title");
            driver.FindElement(By.Id("SeoKeywords")).Clear();
            driver.FindElement(By.Id("SeoKeywords")).SendKeys("BrandsItem_SeoKeywords");
            driver.FindElement(By.Id("SeoDescription")).Clear();
            driver.FindElement(By.Id("SeoDescription")).SendKeys("BrandsItem_SeoDescription");
            driver.FindElement(By.Id("SeoH1")).Clear();
            driver.FindElement(By.Id("SeoH1")).SendKeys("BrandsItem_H1");
            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"btnSave\"]")).Click();
            Thread.Sleep(2000);

            //pre check client
            GoToClient("manufacturers/wrangler");
            VerifyAreEqual("BrandsItem_Title", driver.Title, "pre check seo BrandsItem title client");
            VerifyAreEqual("BrandsItem_H1", driver.FindElement(By.TagName("h1")).Text, "pre check seo BrandsItem h1 client");
            VerifyAreEqual("BrandsItem_SeoKeywords", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "pre check seo BrandsItem keywords client");
            VerifyAreEqual("BrandsItem_SeoDescription", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "pre check seo BrandsItem description client");

            //reset meta
            GoToAdmin("settingsseo");
            ScrollTo(By.Id("BrandsDefaultMetaDescription"));

            driver.FindElement(By.Id("BrandItemDefaultTitle")).Click();
            driver.FindElement(By.Id("BrandItemDefaultTitle")).Clear();
            driver.FindElement(By.Id("BrandItemDefaultTitle")).SendKeys("1");

            driver.FindElement(By.Id("BrandItemDefaultMetaKeywords")).Click();
            driver.FindElement(By.Id("BrandItemDefaultMetaKeywords")).Clear();
            driver.FindElement(By.Id("BrandItemDefaultMetaKeywords")).SendKeys("2");

            driver.FindElement(By.Id("BrandItemDefaultMetaDescription")).Click();
            driver.FindElement(By.Id("BrandItemDefaultMetaDescription")).Clear();
            driver.FindElement(By.Id("BrandItemDefaultMetaDescription")).SendKeys("3");

            driver.FindElement(By.Id("BrandItemDefaultH1")).Click();
            driver.FindElement(By.Id("BrandItemDefaultH1")).Clear();
            driver.FindElement(By.Id("BrandItemDefaultH1")).SendKeys("4");

            ScrollTo(By.Id("header-top"));
            driver.FindElement(By.CssSelector("[data-e2e=\"settingsSeoSave\"]")).Click();
            Thread.Sleep(2000);

            GoToAdmin("settingsseo");
            ScrollTo(By.Id("BrandItemDefaultMetaDescription"));

            driver.FindElement(By.LinkText("Сбросить мета информацию для всех брендов")).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.ClassName("swal2-confirm")).Click();
            Thread.Sleep(3000);

            //check client
            GoToClient("manufacturers/wrangler");
            VerifyAreEqual("1", driver.Title, "reset seo BrandsItem title client");
            VerifyAreEqual("2", driver.FindElement(By.CssSelector("[name=\"Keywords\"]")).GetAttribute("content"), "reset seo BrandsItem keywords client");
            VerifyAreEqual("3", driver.FindElement(By.CssSelector("[name=\"Description\"]")).GetAttribute("content"), "reset seo BrandsItem description client");
            VerifyAreEqual("4", driver.FindElement(By.TagName("h1")).Text, "reset seo BrandsItem h1 client");

            VerifyFinally(testname);
        }
    }
}